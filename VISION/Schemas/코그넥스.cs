using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Schemas
{
    public class 코그넥스 : List<코그넥스툴>
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
            this.카메라1툴정보 = new 코그넥스툴(CameraType.Cam01, "");
            this.카메라2툴정보 = new 코그넥스툴(CameraType.Cam02, "");
            this.카메라3툴정보 = new 코그넥스툴(CameraType.Cam03, "");
            this.카메라4툴정보 = new 코그넥스툴(CameraType.Cam04, "");
            this.카메라5툴정보 = new 코그넥스툴(CameraType.Cam05, "");
            this.카메라6툴정보 = new 코그넥스툴(CameraType.Cam06, "");
            this.카메라7툴정보 = new 코그넥스툴(CameraType.Cam07, "");
            this.카메라8툴정보 = new 코그넥스툴(CameraType.Cam08, "");

            base.Add(this.카메라1툴정보);
            base.Add(this.카메라2툴정보);
            base.Add(this.카메라3툴정보);
            base.Add(this.카메라4툴정보);
            base.Add(this.카메라5툴정보);
            base.Add(this.카메라6툴정보);
            base.Add(this.카메라7툴정보);
            base.Add(this.카메라8툴정보);

            return true;
        }
        public 코그넥스툴 GetItem(CameraType 구분)
        {
            foreach (코그넥스툴 코그넥스툴 in this)
                if ((int)코그넥스툴.카메라 == (int)구분) return 코그넥스툴;
            return null;
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
        [JsonProperty("PatternToolUsed")]
        public virtual List<Boolean> 패턴툴사용여부 { get; set; }
        [JsonProperty("BlobTool")]
        public virtual List<CogBlobTool> 블롭툴 { get; set; }
        [JsonProperty("BlobToolUsed")]
        public virtual List<Boolean> 블롭툴사용여부 { get; set; }
        [JsonProperty("PLCAddress")]
        public String PLC결과어드레스;
        [JsonProperty("Result")]
        public Boolean 결과값 { get; set; } = false;
        [JsonProperty("InputImage")]
        public CogImage8Grey 이미지 { get; set; }

        public 코그넥스툴(CameraType 카메라, String plcAddress)
        {
            this.카메라 = 카메라;
            this.PLC결과어드레스 = plcAddress;
            this.패턴툴 = new List<CogPMAlignMultiTool>();
            this.블롭툴 = new List<CogBlobTool>();
            this.패턴툴사용여부 = new List<bool>();
            this.블롭툴사용여부 = new List<bool>();
            this.이미지 = new CogImage8Grey();
            this.Init();
        }

        public void Init() => Load();

        public void Load()
        {
            string patternToolPath = Path.Combine(Global.환경설정.도구경로, Global.환경설정.선택모델.ToString());
            string blobToolPath = Path.Combine(Global.환경설정.도구경로, Global.환경설정.선택모델.ToString());

            patternToolPath = $"{patternToolPath}\\{this.카메라}";
            blobToolPath = $"{blobToolPath}\\{this.카메라}";

            if (!Common.DirectoryExists(patternToolPath, true))
                return;
            if (!Common.DirectoryExists(blobToolPath, true))
                return;

            for (int lop = 1; lop <= this.툴개수; lop++)
            {
                string patternToolvppPath = $"{patternToolPath}\\MultiPattern - {lop}.vpp";
                string blobToolvppPath = $"{blobToolPath}\\Blob - {lop}.vpp";

                if (File.Exists(patternToolvppPath)) this.패턴툴.Add((CogPMAlignMultiTool)CogSerializer.LoadObjectFromFile(patternToolvppPath));
                else
                {
                    기본패턴툴생성(patternToolvppPath);
                    this.패턴툴.Add((CogPMAlignMultiTool)CogSerializer.LoadObjectFromFile(patternToolvppPath));
                }

                if (File.Exists(blobToolvppPath)) this.블롭툴.Add((CogBlobTool)CogSerializer.LoadObjectFromFile(blobToolvppPath));
                else
                {
                    기본블롭툴생성(blobToolvppPath);
                    this.블롭툴.Add((CogBlobTool)CogSerializer.LoadObjectFromFile(blobToolvppPath));
                }

                this.패턴툴사용여부.Add(false);
                this.블롭툴사용여부.Add(false);
            }
        }

        private CogPMAlignMultiTool 기본패턴툴생성(string path)
        {
            CogPMAlignMultiTool tool = new CogPMAlignMultiTool();
            CogSerializer.SaveObjectToFile(tool, path);
            return tool;
        }

        private CogBlobTool 기본블롭툴생성(string path)
        {
            CogBlobTool tool = new CogBlobTool();
            CogSerializer.SaveObjectToFile(tool, path);
            return tool;
        }

        public void InputImage()
        {
            foreach (CogPMAlignMultiTool tool in this.패턴툴)
                tool.InputImage = 이미지;
            foreach (CogBlobTool tool in this.블롭툴)
                tool.InputImage = this.이미지;
        }

        public Boolean Run()
        {
            foreach (CogPMAlignMultiTool tool in this.패턴툴)
                tool.Run();
            foreach (CogBlobTool tool in this.블롭툴)
                tool.Run();


            return this.결과값;
        }
    }
}
