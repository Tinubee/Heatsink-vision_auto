using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public class 코그넥스 : Dictionary<CameraType, 코그넥스툴>
    {
        public 코그넥스툴 카메라1툴정보 = null;
        public 코그넥스툴 카메라2툴정보 = null;
        public 코그넥스툴 카메라3툴정보 = null;
        public 코그넥스툴 카메라4툴정보 = null;
        public 코그넥스툴 카메라5툴정보 = null;
        public 코그넥스툴 카메라6툴정보 = null;
        public 코그넥스툴 카메라7툴정보 = null;
        public 코그넥스툴 카메라8툴정보 = null;

        public Boolean Init()
        {
            base.Clear();
            this.카메라1툴정보 = new 코그넥스툴(CameraType.Cam01,"");
            this.카메라2툴정보 = new 코그넥스툴(CameraType.Cam02, "");
            this.카메라3툴정보 = new 코그넥스툴(CameraType.Cam03, "");
            this.카메라4툴정보 = new 코그넥스툴(CameraType.Cam04, "");
            this.카메라5툴정보 = new 코그넥스툴(CameraType.Cam05, "");
            this.카메라6툴정보 = new 코그넥스툴(CameraType.Cam06, "");
            this.카메라7툴정보 = new 코그넥스툴(CameraType.Cam07, "");
            this.카메라8툴정보 = new 코그넥스툴(CameraType.Cam08, "");

            base.Add(CameraType.Cam01, this.카메라1툴정보);
            base.Add(CameraType.Cam02, this.카메라2툴정보);
            base.Add(CameraType.Cam03, this.카메라3툴정보);
            base.Add(CameraType.Cam04, this.카메라4툴정보);
            base.Add(CameraType.Cam05, this.카메라5툴정보);
            base.Add(CameraType.Cam06, this.카메라6툴정보);
            base.Add(CameraType.Cam07, this.카메라7툴정보);
            base.Add(CameraType.Cam08, this.카메라8툴정보);

            return true;
        }
    }

    public class 코그넥스툴
    {
        [JsonProperty("Camera")]
        public virtual CameraType 카메라 { get; set; } = CameraType.None;
        [JsonProperty("ToolCount")]
        public virtual Int32 툴개수 { get; set; } = 30;
        [JsonProperty("PatternTool")]
        public virtual List<CogPMAlignMultiTool> 패턴툴 { get; set; }
        [JsonProperty("BlobTool")]
        public virtual List<CogBlobTool> 블롭툴 { get; set; }
        [JsonProperty("PLCAddress")]
        public String PLC결과어드레스;
        public 코그넥스툴(CameraType 카메라, String plcAddress)
        {
            this.카메라 = 카메라;
            this.PLC결과어드레스 = plcAddress;
            this.Init();
        }

        public void Init() => Load();

        public void Load()
        {
            string patternToolPath = Path.Combine(Global.환경설정.도구경로, this.카메라.ToString());
            string blobToolPath = Path.Combine(Global.환경설정.도구경로, this.카메라.ToString());

            for (int lop = 0; lop < this.툴개수; lop++)
            {
                this.패턴툴.Add((CogPMAlignMultiTool)CogSerializer.LoadObjectFromFile($"{patternToolPath}\\PMAlign-{lop}.vpp"));
                this.블롭툴.Add((CogBlobTool)CogSerializer.LoadObjectFromFile($"{blobToolPath}\\Blob-{lop}.vpp"));
            }
        }
    }
}
