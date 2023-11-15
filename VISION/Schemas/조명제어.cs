using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public enum 조명구분
    {
        상부표면검사조명 = 1,
        측면표면검사조명 = 2,
        너트검사조명 = 3,
        BACK조명 = 4,
    }

    public enum 조명포트
    {
        None,
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
        COM9
    }
    public enum 조명채널
    {
        CH1 = 1,
        CH2 = 2,
        CH3 = 3,
        CH4 = 4,
        CH5 = 5,
        CH6 = 6,
        CH7 = 7,
        CH8 = 8,
        CH9 = 9,
        CH10 = 10,
        CH11 = 11,
        CH12 = 12,
    }

    public abstract class 조명컨트롤러
    {
        public abstract String 로그영역 { get; set; }
        public abstract 조명포트 포트 { get; set; }
        public abstract Int32 통신속도 { get; set; }
        public abstract Int32 최대밝기 { get; }
        public abstract String STX { get; set; }
        public abstract String ETX { get; set; }
        public SerialPort 통신포트;

        public virtual void Init()
        {
            //if (Global.환경설정.동작구분 != 동작구분.Live) return;
            통신포트 = new SerialPort();
            통신포트.PortName = this.포트.ToString();
            통신포트.BaudRate = 통신속도;
            통신포트.DataBits = (Int32)8;
            통신포트.StopBits = StopBits.One;
            통신포트.Parity = Parity.None;
            통신포트.DataReceived += DataReceived;
            통신포트.ErrorReceived += ErrorReceived;
        }

        public virtual Boolean IsOpen() => 통신포트 != null && 통신포트.IsOpen;
        public virtual Boolean Open()
        {
            if (통신포트 == null) return false;
            try
            {
                통신포트.Open();
                return 통신포트.IsOpen;
            }
            catch (Exception ex)
            {
                통신포트.Dispose();
                통신포트 = null;
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역, "장치연결", "조명 제어 포트에 연결할 수 없습니다.\n" + ex.Message, true);
                return false;
            }
        }

        public virtual void Close()
        {
            if (통신포트 == null || !통신포트.IsOpen) return;
            통신포트.Close();
            통신포트.Dispose();
            통신포트 = null;
        }

        public virtual Int32 밝기변환(Int32 밝기) => Convert.ToInt32(Math.Round((Double)this.최대밝기 * 밝기 / 100));
        public abstract Boolean Set(조명정보 정보);
        public abstract Boolean Save(조명정보 정보);
        public abstract Boolean TurnOn(조명정보 정보);
        public abstract Boolean TurnOff(조명정보 정보);

        public virtual void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.WriteLine($"ErrorReceived 포트={this.포트}, {e.EventType}", this.로그영역);
            Debug.WriteLine(e.ToString());
        }
        public virtual void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String data = sp.ReadExisting();
            Debug.WriteLine($"DataReceived 포트={this.포트}, {data}", this.로그영역);
        }

        public virtual Boolean SendCommand(String 구분, String Command)
        {
            if (!IsOpen())
            {
                //Global.오류로그(로그영역, 구분, "조명컨트롤러 포트에 연결할 수 없습니다.", true);
                return false;
            }
            try
            {
                통신포트.Write($"{STX}{Command}{ETX}");
                //Debug.WriteLine($"{STX}{Command}{ETX}".Trim(), 구분);
                return true;
            }
            catch (Exception ex)
            {
                //Global.오류로그(로그영역, 구분, ex.Message, true);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }

    public class LCP_100DC : 조명컨트롤러
    {
        public override String 로그영역 { get; set; } = nameof(LCP_100DC);
        public override 조명포트 포트 { get; set; } = 조명포트.COM3;
        public override Int32 통신속도 { get; set; } = 9600;
        public override Int32 최대밝기 { get; } = 1023;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널 - 1}w{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널 - 1}o");//{this.밝기변환(정보.밝기).ToString("d4")}
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널 - 1}f");//0000
    }
    public class LCP24_150DC : LCP_100DC
    {
        public override String 로그영역 { get; set; } = nameof(LCP24_150DC);
        public override Int32 통신속도 { get; set; } = 19200;
        public override Int32 최대밝기 { get; } = 100;
        public override Boolean Set(조명정보 정보) => false;
        public override Boolean TurnOn(조명정보 정보) => SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널}o{this.밝기변환(정보.밝기).ToString("d4")}");
        public override Boolean TurnOff(조명정보 정보) => SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널}f0000");
    }


    public class 조명제어 : BindingList<조명정보>
    {
        [JsonIgnore]
        private string 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "조명설정.json"); } }
        [JsonIgnore]
        private LCP24_150DC 컨트롤러1;
        [JsonIgnore]
        private LCP24_150DC 컨트롤러2;
        [JsonIgnore]
        private LCP24_150DC 컨트롤러3;
        [JsonIgnore]
        private LCP_100DC 컨트롤러4;

        public void Init()
        {
            this.컨트롤러1 = new LCP24_150DC() { 포트 = 조명포트.COM6 };
            this.컨트롤러2 = new LCP24_150DC() { 포트 = 조명포트.COM7 };
            this.컨트롤러3 = new LCP24_150DC() { 포트 = 조명포트.COM8 };
            this.컨트롤러4 = new LCP_100DC() { 포트 = 조명포트.COM9 };

            this.컨트롤러1.Init();
            this.컨트롤러2.Init();
            this.컨트롤러3.Init();
            this.컨트롤러4.Init();

            // 컨트롤러 당 카메라 1대씩 연결
            this.Add(new 조명정보(CameraType.Cam01, 컨트롤러1) { 채널 = 조명채널.CH1, 밝기 = 70 });
            this.Add(new 조명정보(CameraType.Cam02, 컨트롤러1) { 채널 = 조명채널.CH2, 밝기 = 70 });
            this.Add(new 조명정보(CameraType.Cam03, 컨트롤러1) { 채널 = 조명채널.CH1, 밝기 = 70 });
            this.Add(new 조명정보(CameraType.Cam04, 컨트롤러2) { 채널 = 조명채널.CH2, 밝기 = 70 });
            this.Add(new 조명정보(CameraType.Cam05, 컨트롤러2) { 채널 = 조명채널.CH1, 밝기 = 90 });
            this.Add(new 조명정보(CameraType.Cam06, 컨트롤러3) { 채널 = 조명채널.CH2, 밝기 = 90 });

            this.Load();
            this.Open();
            this.Set();
        }
        public 조명정보 GetItem(CameraType 카메라)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라) return 조명;
            return null;
        }
        public 조명정보 GetItem(CameraType 카메라, 조명포트 포트, 조명채널 채널)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라 && 조명.포트 == 포트 && 조명.채널 == 채널) return 조명;
            return null;
        }
        public void Load()
        {
            if (!File.Exists(this.저장파일)) return;
            try
            {
                List<조명정보> 자료 = JsonConvert.DeserializeObject<List<조명정보>>(File.ReadAllText(this.저장파일), MvUtils.Utils.JsonSetting());
                foreach (조명정보 정보 in 자료)
                {
                    조명정보 조명 = this.GetItem(정보.카메라, 정보.포트, 정보.채널);
                    if (조명 == null) continue;
                    조명.Set(정보);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역, "조명 설정 로드", ex.Message, false);
            }
        }
        public void Save()
        {
            if (!MvUtils.Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this, MvUtils.Utils.JsonSetting())))
            {
                //Global.오류로그(로그영역, "조명설정 저장", "조명 설정 저장에 실패하였습니다.", true);
            }
        }
        public void Open()
        {
            if (!this.컨트롤러1.Open())
            {
                this.컨트롤러1.Close();
                //Global.오류로그(로그영역, "조명장치 연결", "조명 컨트롤러1에 연결할 수 없습니다.", true);
            }
            if (!this.컨트롤러2.Open())
            {
                this.컨트롤러2.Close();
                //Global.오류로그(로그영역, "조명장치 연결", "조명 컨트롤러2에 연결할 수 없습니다.", true);
            }
            if (!this.컨트롤러3.Open())
            {
                this.컨트롤러3.Close();
                //Global.오류로그(로그영역, "조명장치 연결", "조명 컨트롤러2에 연결할 수 없습니다.", true);
            }
            if (!this.컨트롤러4.Open())
            {
                this.컨트롤러4.Close();
                //Global.오류로그(로그영역, "조명장치 연결", "조명 컨트롤러2에 연결할 수 없습니다.", true);
            }
        }
        public void Close()
        {
            this.TurnOff();
            Task.Delay(100).Wait();
            this.컨트롤러1?.Close();
            this.컨트롤러2?.Close();
            this.컨트롤러3?.Close();
            this.컨트롤러4?.Close();
        }

        public void Set()
        {
            Task.Run(() =>
            {
                foreach (조명정보 조명 in this)
                {
                    if (!조명.Set()) 조명.TurnOn();
                    Task.Delay(200).Wait();
                    조명.TurnOff();
                    Task.Delay(200).Wait();
                }
            });
        }

        public void Set(CameraType 카메라, 조명포트 포트, Int32 밝기)
        {
            foreach (조명정보 정보 in this)
            {
                if (정보.카메라 == 카메라 && 정보.포트 == 포트)
                {
                    정보.밝기 = 밝기;
                    정보.Set();
                }
            }
        }

        public void TurnOn()
        {
            foreach (조명정보 정보 in this)
                정보.TurnOn();
        }

        public void TurnOnOff(CameraType 카메라, Boolean IsOn)
        {
            if (IsOn) this.TurnOn(카메라);
            else this.TurnOff(카메라);
        }

        public void TurnOn(CameraType 카메라)
        {
            foreach (조명정보 정보 in this)
                if (정보.카메라 == 카메라)
                    정보.TurnOn();
        }

        public void TurnOff(CameraType 카메라)
        {
            foreach (조명정보 정보 in this)
                if (정보.카메라 == 카메라)
                    정보.TurnOff();
        }

        public void TurnOff()
        {
            foreach (조명정보 정보 in this)
                정보.TurnOff();
        }
    }

    public class 조명정보
    {
        [JsonProperty("Camera")]
        public CameraType 카메라 { get; set; } = CameraType.None;
        [JsonProperty("Port")]
        public 조명포트 포트 { get; set; } = 조명포트.None;
        [JsonProperty("Channel")]
        public 조명채널 채널 { get; set; } = 조명채널.CH1;
        [JsonProperty("Brightness")]
        public Int32 밝기 { get; set; } = 100;
        [JsonProperty("Description")]
        public String 설명 { get; set; } = String.Empty;
        [JsonIgnore]
        public Boolean 켜짐 { get; set; } = false;
        [JsonIgnore]
        public 조명컨트롤러 컨트롤러;

        public 조명정보() { }
        public 조명정보(CameraType 카메라, 조명컨트롤러 컨트롤)
        {
            this.카메라 = 카메라;
            this.컨트롤러 = 컨트롤;
            this.포트 = 컨트롤.포트;
        }

        //public Boolean Get() { return this.컨트롤러.Get(this); }
        public Boolean Set()
        {
            this.켜짐 = this.컨트롤러.Set(this);
            return this.켜짐;
        }
        public Boolean TurnOn()
        {
            if (this.켜짐) return true;
            this.켜짐 = this.컨트롤러.TurnOn(this);
            return this.켜짐;
        }
        public Boolean TurnOff()
        {
            if (!this.켜짐) return true;
            this.켜짐 = false;
            return this.컨트롤러.TurnOff(this);
        }
        public Boolean OnOff()
        {
            if (this.켜짐) return this.TurnOff();
            else return this.TurnOn();
        }

        public void Set(조명정보 정보)
        {
            this.밝기 = 정보.밝기;
            this.설명 = 정보.설명;
        }
    }
}
