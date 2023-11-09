using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace VISION.Schemas
{
    public enum 모델구분
    {
        [Bindable(false)]
        None = 0,
        [Description("0023")]
        HeatSink0023 = 1,
        [Description("0024")]
        HeatSink0024 = 2,
        [Description("0026")]
        HeatSink0026 = 3,
        [Description("0028")]
        HeatSink0028 = 4,
        [Description("0033")]
        HeatSink0033 = 5,
    }

    public enum 노멀미러
    {
        [ListBindable(false)]
        None = 0,
        Normal = 1,
        Mirror = 2,
    }

    public class 모델정보
    {
        [JsonProperty("type"), Translation("Model", "모델")]
        public 모델구분 모델구분 { get; set; } = 모델구분.None;
        [JsonProperty("desc"), Translation("Description", "설명")]
        public String 모델설명 { get; set; } = String.Empty;

        [JsonProperty("date"), Translation("Date", "일자")]
        public DateTime 양산일자 { get; set; } = DateTime.Today;
        [JsonProperty("OK"), Translation("OK", "양품")]
        public Int32 양품갯수 { get; set; } = 0;
        [JsonProperty("NG"), Translation("NG", "불량")]
        public Int32 불량갯수 { get; set; } = 0;
        [JsonIgnore, Translation("Total", "전체")]
        public Int32 전체갯수 { get { return 양품갯수 + 불량갯수; } }
        [JsonIgnore, Translation("Yield", "양품율")]
        public Double 양품수율 { get { return 전체갯수 > 0 ? (Double)양품갯수 / (Double)전체갯수 * (Double)100 : (Double)100; } }
        [JsonIgnore, Translation("OK", "양품")]
        public String 양품갯수표현 { get { return MvUtils.Utils.FormatNumeric(양품갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("NG", "불량")]
        public String 불량갯수표현 { get { return MvUtils.Utils.FormatNumeric(불량갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("Total", "전체")]
        public String 전체갯수표현 { get { return MvUtils.Utils.FormatNumeric(전체갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("Yield", "양품율")]
        public String 양품수율표현 { get { return MvUtils.Utils.FormatNumeric(양품수율, "{0:#,0}%"); } }
        [JsonIgnore]
        public Int32 모델번호 { get { return (Int32)this.모델구분; } }
        [JsonIgnore]
        public String 모델코드 { get { return MvUtils.Utils.GetDescription(this.모델구분); } }
        [JsonIgnore]
        public String 모델사진 { get { return Path.Combine(Global.환경설정.사진경로, 모델번호.ToString("d2") + ".png"); } }

        [JsonIgnore]
        public 검사설정자료 검사설정 = null;

        public Image 마스터이미지()
        {
            if (!File.Exists(this.모델사진)) return null;
            return Image.FromFile(this.모델사진);
        }

        public 모델정보() { }
        public 모델정보(모델구분 구분)
        {
            this.모델구분 = 구분;
            this.모델설명 = GetModelDescription(구분);
            this.Init();
        }

        public void Init()
        {
            this.검사설정 = new 검사설정자료(this);
        }

        public void Close() { }

        public static String GetModelDescription(모델구분 구분)
        {
            DXDescriptionAttribute d = MvUtils.Utils.GetAttribute<DXDescriptionAttribute>(구분);
            if (d == null) return String.Empty;
            return d.Description;
        }

        public void 수량리셋()
        {
            this.양품갯수 = 0;
            this.불량갯수 = 0;
            Global.모델자료.Save();
        }

        public void 수량추가(결과구분 구분)
        {
            if (구분 == 결과구분.PS) return;
            if (구분 == 결과구분.OK) this.양품갯수++;
            else this.불량갯수++;
        }
    }

    public class 모델자료 : BindingList<모델정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Models", "모델관리");
        private String 저장파일 { get { return Path.Combine(Global.환경설정.모델저장경로, $"Models.json"); } }
        public 모델정보 선택모델 { get { return this.GetItem(Global.환경설정.선택모델); } }
        public event Global.BaseEvent 검사수량변경;

        public void Init()
        {
            this.Load();
            this.BaseModel();
        }

        public void Close()
        {
            this.Save();
            foreach (모델정보 모델 in this)
                모델.Close();
        }

        private void Load()
        {
            if (!File.Exists(저장파일))
            {
                //Global.정보로그(로그영역.GetString(), "자료로드", "저장파일이 없습니다.", false);
                return;
            }
            try
            {
                List<모델정보> 자료 = JsonConvert.DeserializeObject<List<모델정보>>(File.ReadAllText(저장파일));
                if (자료 == null) return;
                자료.Sort((a, b) => a.모델번호.CompareTo(b.모델번호));
                자료.ForEach(e => this.Add(e));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역.GetString(), "자료로드", ex.Message, false);
            }

            if (this.GetItem(Global.환경설정.선택모델) == null)
                if (this.Count > 0) Global.환경설정.선택모델 = this[0].모델구분;

            foreach (모델정보 정보 in this)
            {
                if (정보.양산일자 == DateTime.Today) continue;
                정보.양산일자 = DateTime.Today;
                정보.수량리셋();
            }
            SettingLoad();
        }

        public 모델정보 GetItem(모델구분 모델코드)
        {
            return this.Where(e => e.모델구분 == 모델코드).FirstOrDefault();
        }

        private void BaseModel()
        {
            foreach (모델구분 구분 in typeof(모델구분).GetEnumValues())
            {
                if (구분 == 모델구분.None) continue;
                모델정보 모델 = this.GetItem(구분);
                if (모델 == null) this.Add(new 모델정보(구분));
                //else 모델.모델설명 = 모델정보.GetModelDescription(구분);
            }
            if (this.선택모델 == null) Global.환경설정.선택모델 = 모델구분.None;
        }

        public void Save()
        {
            File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public Boolean 모델삭제(모델정보 정보)
        {
            if (정보.모델구분 != Global.환경설정.선택모델) return this.Remove(정보);
            //Global.경고로그(로그영역.GetString(), "모델삭제", "현재 선택된 모델이므로 삭제하실 수 없습니다.", true);
            return false;
        }

        public void 수량리셋()
        {
            this.선택모델.수량리셋();
            this.검사수량변경?.Invoke();
        }

        public void 수량추가(모델구분 모델, 결과구분 결과)
        {
            this.GetItem(모델)?.수량추가(결과);
            this.검사수량변경?.Invoke();
        }

        public void SettingLoad()
        {
            foreach (모델정보 정보 in this)
                SettingLoad(정보);
        }
        public void SettingLoad(모델구분 모델)
        {
            SettingLoad(this.GetItem(모델));
        }
        public void SettingLoad(모델정보 정보)
        {
            if (정보 == null) return;
            if (정보.검사설정 == null) 정보.Init();
            정보.검사설정.Load();
        }

        public void SettingSave()
        {
            foreach (모델정보 정보 in this)
                SettingSave(정보);
        }
        public void SettingSave(모델구분 모델)
        {
            SettingSave(this.GetItem(모델));
        }
        public void SettingSave(모델정보 정보)
        {
            정보?.검사설정?.Save();
        }
    }

    public class 검사설정자료 : BindingList<검사정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Inspection Settings", "검사설정");
        private 모델정보 모델정보;
        private 모델구분 모델구분 { get { return 모델정보.모델구분; } }
        private Int32 모델번호 { get { return 모델정보.모델번호; } }
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, $"Model.{모델구분}.json"); } }
        public 검사설정자료(모델정보 모델) { this.모델정보 = 모델; }

        public void Init() { this.Load(); }

        public void Load()
        {
            this.Clear();

            if (!File.Exists(저장파일))
            {
                //Global.정보로그(로그영역.GetString(), "자료로드", $"[{Utils.GetDescription(모델구분)}] 검사설정 파일이 없습니다.", false);
                foreach (검사항목 항목 in typeof(검사항목).GetEnumValues())
                {
                    if (항목 == 검사항목.None) continue;
                    ResultAttribute a = MvUtils.Utils.GetAttribute<ResultAttribute>(항목);
                    this.Add(new 검사정보() { 검사항목 = 항목, 검사그룹 = a.검사그룹, 검사장치 = a.장치구분, 결과분류 = a.결과분류 });
                }
                this.Save();
                return;
            }
            try
            {
                List<검사정보> 자료 = JsonConvert.DeserializeObject<List<검사정보>>(File.ReadAllText(저장파일));
                자료.Sort((a, b) => a.검사항목.CompareTo(b.검사항목));
                if (자료 == null)
                {
                    //Global.정보로그(로그영역.GetString(), "자료로드", "저장 된 설정자료가 올바르지 않습니다.", false);
                    return;
                }

                자료.ForEach(e =>
                {
                    검사정보 정보 = new 검사정보(e);
                    this.Add(정보);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역.GetString(), "자료로드", ex.Message, false);
            }
        }

        public Boolean Save()
        {
            try
            {
                if (File.Exists(저장파일))
                {
                    String path = Path.Combine(Global.환경설정.기본경로, "backup");
                    if (Common.DirectoryExists(path, true))
                        File.Copy(저장파일, Path.Combine(path, $"검사설정.{모델번호.ToString("d2")}.{Utils.FormatDate(DateTime.Now, "{0:yymmddhhmmss}")}.json"));
                }

                File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));
                Debug.WriteLine(저장파일, "티칭저장");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //Global.오류로그(로그영역.GetString(), "자료저장", ex.Message, false);
                return false;
            }
        }

        public 검사정보 GetItem(검사정보 정보)
        {
            return this.Where(e => e.검사항목 == 정보.검사항목).FirstOrDefault();
            //return this.Where(e => e.검사번호 == 정보.검사번호 && e.장치구분 == 정보.장치구분 && e.검사그룹 == 정보.검사그룹).FirstOrDefault();
        }

        public void ResetItem(검사정보 설정) => this.ResetItem(this.IndexOf(설정));

        //public Dictionary<String, Object> GetRunPrms(검사정보 검사)
        //{
        //    List<검사정보> 자료 = new List<검사정보>();
        //    if (검사.검사유형 == 검사유형.원홀) 자료.Add(검사);
        //    else if (검사.검사유형 == 검사유형.타원W || 검사.검사유형 == 검사유형.타원H)
        //        자료 = this.Where(e => e.장치구분 == 검사.장치구분 && e.센서번호 == 검사.센서번호).ToList();
        //    if (자료.Count < 1) return null;
        //    return 검사결과.GetRunPrms(자료, 검사);
        //}
    }
}
