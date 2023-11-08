using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Diagnostics;

namespace VISION.Cogs
{
    public class Camera
    {
        private CogToolBlock toolBlock = null;

        //private CogAcqFifoTool camTool = new CogAcqFifoTool();
        private PGgloble Glob = PGgloble.getInstance;
        private CogJobManager jobManager = new CogJobManager();
        private CogJob job = new CogJob();
        private CogAcqFifoTool camTool = new CogAcqFifoTool();
        public string savePath = string.Empty;

        public int 카메라번호 { get; set; } = 0;
        public CogImage8Grey InputImage { get => this.GetInput<CogImage8Grey>("InputImage"); set => this.SetInput("InputImage", value); }
        public CogImage8Grey OutputImage { get { return GetOutput<CogImage8Grey>(this.toolBlock, "OutputImage"); } }

        public T GetInput<T>(String name) { return GetInput<T>(this.toolBlock, name); }
        public Boolean SetInput(String name, Object value) { return SetInput(this.toolBlock, name, value); }

        public T GetOutput<T>(String name) { return GetOutput<T>(this.toolBlock, name); }


        public static T GetInput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Inputs.Contains(name)) return (T)tool.Inputs[name].Value;
            return default(T);
        }
        public static Boolean SetInput(CogToolBlock tool, String name, Object value)
        {
            if (tool == null) return false;
            if (!tool.Inputs.Contains(name)) return false;
            tool.Inputs[name].Value = value;
            return true;
        }
        public static T GetOutput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Outputs.Contains(name)) return (T)tool.Outputs[name].Value;
            return default(T);
        }

        public void 패턴툴추가(CogPMAlignMultiTool Tool)
        {
            if (this.toolBlock.Tools.Contains(Tool)) return;

            //Tool.InputImage = this.InputImage;
            this.toolBlock.Tools.Add(Tool);
        }

        public void 블롭툴추가(CogBlobTool Tool)
        {
            if (this.toolBlock.Tools.Contains(Tool)) return;

            Tool.InputImage = this.InputImage;
            this.toolBlock.Tools.Add(Tool);
        }

        public CogToolBlock OpenToolBlock()
        {
            this.InputImage = this.InputImage;
            return this.toolBlock;
        }

        public CogJob JobFile()
        {
            return this.job;
        }

        public Camera(int Toolnumber = 0)
        {
            jobManager.GarbageCollection = true;
            jobManager.JobAdd(job);
            camTool = new CogAcqFifoTool();
            camTool.Name = "cam - " + Toolnumber.ToString();
        }

        public bool Loadtool(string path, string originPath, int camNumber)
        {
            this.카메라번호 = camNumber;
            if (System.IO.File.Exists(path) == false)
            {
                CogSerializer.SaveObjectToFile(this.camTool, path);
            }
            camTool = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(path);

            CogToolGroup group = new CogToolGroup();
            job.VisionTool = group;
            group.Tools.Add(camTool);

            //CogToolGroup group = new CogToolGroup() { Name = $"GroupTool-{this.카메라번호}" }; ;
            //this.toolBlock = new CogToolBlock();
            //this.toolBlock.Name = this.camTool.Name;
            //this.toolBlock.Tools.Add(camTool);
            //this.toolBlock.Inputs.Add(new CogToolBlockTerminal("InputImage", typeof(CogImage8Grey)));
            //this.toolBlock.Outputs.Add(new CogToolBlockTerminal("OutputImage", typeof(CogImage8Grey)));
            //group.Tools.Add(this.toolBlock);
            //this.job.VisionTool = group;
            //this.savePath = originPath + $"\\cam{this.카메라번호}-GroupTool.vpp";
            return true;
        }

        public CogImage8Grey Run()
        {
            this.camTool.Run(); //Tool실행.
            CogImage8Grey Image = (CogImage8Grey)camTool.OutputImage; //Tool 출력이미지.
            return Image;
        }

        public Boolean ToolBlockRun()
        {
            this.toolBlock.Run();
            return true;
        }

        public void SetBrightness(double value)
        {
            if (camTool.Operator == null) return;

            if (this.camTool.Operator.OwnedBrightnessParams == null) return;

            this.camTool.Operator.OwnedBrightnessParams.Brightness = value;
        }
        public bool SaveTool(string path, string camName)
        {
            string Savepath = path;

            if (System.IO.Directory.Exists(Savepath) == false)
            {
                return false;
            }

            Savepath = Savepath + "\\" + camName;

            CogSerializer.SaveObjectToFile(camTool, Savepath);

            return true;
        }


        public double GetBrightness()
        {
            if (this.camTool.Operator.OwnedBrightnessParams == null) return 0;

            return this.camTool.Operator.OwnedBrightnessParams.Brightness;
        }

        public void SetExposure(double value)
        {
            if (this.camTool.Operator == null) return;
            this.camTool.Operator.OwnedExposureParams.Exposure = value;
        }

        public double GetExposure()
        {
            return this.camTool.Operator.OwnedExposureParams.Exposure;
        }

        public void StartLive()
        {
            this.camTool.Operator.StartAcquire();
        }

        public void Close()
        {
            //this.camTool.Dispose();
            //this.camTool.Operator.FrameGrabber.Disconnect(false);
            this.job.Shutdown();
            this.jobManager.Shutdown();
        }

        public void ToolSetup()
        {
            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            CogAcqFifoEditV2 Edit = new CogAcqFifoEditV2();
            Edit.Dock = System.Windows.Forms.DockStyle.Fill;
            Edit.Subject = camTool;
            Window.Controls.Add(Edit);
            Window.Width = 800;
            Window.Height = 600;
            Window.Show();
        }
    }
}
