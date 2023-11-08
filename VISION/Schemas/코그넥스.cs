using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public class 코그넥스 : Dictionary<CameraType, 코그넥스툴>
    {

    }

    public class 코그넥스툴
    {
        [JsonProperty("Camera")]
        public virtual CameraType 구분 { get; set; } = CameraType.None;
        [JsonProperty("PatternTool")]
        public virtual 패턴툴 패턴툴 { get; set; }
        [JsonProperty("BlobTool")]
        public virtual 블롭툴 블롭툴 { get; set; }

        public void Init() => Load();

        public void Load()
        {
            this.패턴툴.툴 = 패턴툴.Load();
        }
    }

    public class 패턴툴
    {
        [JsonProperty("Tool")]
        public virtual CogPMAlignMultiTool 툴 { get; set; }
        [JsonProperty("Number")]
        public virtual Int32 번호 { get; set; } = 0;
        [JsonProperty("Name")]
        public virtual String 이름 { get; set; } = string.Empty;

        public CogPMAlignMultiTool Load()
        {
            string Savepath = "";
            툴 = (CogPMAlignMultiTool)CogSerializer.LoadObjectFromFile(Savepath);

            return 툴;
        }
    }

    public class 블롭툴
    {
        [JsonProperty("Tool")]
        public virtual CogBlobTool 툴 { get; set; }
        [JsonProperty("Number")]
        public virtual Int32 번호 { get; set; } = 0;
        [JsonProperty("Name")]
        public virtual String 이름 { get; set; } = string.Empty;
    }
}
