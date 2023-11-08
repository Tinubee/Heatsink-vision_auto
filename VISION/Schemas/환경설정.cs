using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public class 환경설정
    {
        [Description("설정저장 기본경로"), JsonProperty("ConfigSavePath")]
        public string 기본경로 { get; set; } = @"C:\HKC\HeatSink\Config";
        [Description("이미지 저장위치"), JsonProperty("ImageSavePath")]
        public string 이미지저장경로 { get; set; } = @"C:\HKC\HeatSink\SaveImage";
        [Description("결과 저장위치"), JsonProperty("DocumentSavePath")]
        public string 자료저장경로 { get; set; } = @"C:\HKC\HeatSink\SaveData"; 
        [Description("결과 저장위치"), JsonProperty("ModelSavePath")]
        public string 모델저장경로 { get; set; } = @"C:\HKC\HeatSink\Models";
        [Description("OK 이미지 저장"), JsonProperty("SaveOK")]
        public Boolean 사진저장OK { get; set; } = false;
        [Description("NG 이미지 저장"), JsonProperty("SaveNG")]
        public Boolean 사진저장NG { get; set; } = false;
        [Description("현재 검사모델"), JsonProperty("CurrentModel")]
        public Int32 선택모델 { get; set; } = 0;
        [Description("검사결과 보관일수"), JsonProperty("DaysToKeepResults")]
        public int 결과보관 { get; set; } = 180;
        [Description("로그 보관일수"), JsonProperty("DaysToKeepLogs")]
        public int 로그보관 { get; set; } = 120;


        [JsonIgnore]
        private string 저장파일 { get { return Path.Combine(this.기본경로, "환경설정.json"); } }
        [JsonIgnore]
        public String OK이미지저장경로 { get { return Path.Combine(this.이미지저장경로, "OK"); } }
        [JsonIgnore]
        public String NG이미지저장경로 { get { return Path.Combine(this.이미지저장경로, "NG"); } }
    
        public bool Init()
        {
            return this.Load();
        }

        public bool Load()
        {
            if (!Utils.Common.DirectoryExists(기본경로, true))
            {
                //Global.오류로그(로그영역, "기본경로", "기본경로 디렉토리를 생성할 수 없습니다.", true);
                return false;
            }

            //if (!Utils.Common.DirectoryExists(로그경로, true))
            //{
            //    Global.오류로그(로그영역, "로그경로", "로그 디렉토리를 생성할 수 없습니다.", true);
            //    return false;
            //}

            if (File.Exists(저장파일))
            {
                try
                {
                    환경설정 설정 = JsonConvert.DeserializeObject<환경설정>(File.ReadAllText(저장파일, Encoding.UTF8));
                    foreach (PropertyInfo p in 설정.GetType().GetProperties())
                    {
                        if (!p.CanWrite) continue;
                        Object v = p.GetValue(설정);
                        if (v == null) continue;
                        p.SetValue(this, v);
                    }
                }
                catch (Exception ex)
                {
                    //Global.오류로그(로그영역, "환경설정 로드", ex.Message, true);
                }
            }

            else
            {
                this.Save();
                //Global.정보로그(로그영역, "환경설정 로드", "저장된 설정파일이 없습니다.", false);
            }

            if (!Utils.Common.DirectoryExists(이미지저장경로, true))
            {
                //Global.오류로그(로그영역, "환경설정 로드", "이미지 저장경로를 생성할 수 없습니다.", true);
                return false;
            }
            if (!Utils.Common.DirectoryExists(자료저장경로, true))
            {
                //Global.오류로그(로그영역, "환경설정 로드", "자료 저장경로를 생성할 수 없습니다.", true);
                return false;
            }
            if (!Utils.Common.DirectoryExists(모델저장경로, true))
            {
                //Global.오류로그(로그영역, "환경설정 로드", "모델 저장경로를 생성할 수 없습니다.", true);
                return false;
            }
          
            return true;
        }

        public void Save()
        {
            if (!MvUtils.Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this, MvUtils.Utils.JsonSetting())))
            {
                //Global.오류로그(로그영역.GetString(), "환경설정 저장", "환경설정 저장에 실패하였습니다.", true);
            }
        }

    }
}
