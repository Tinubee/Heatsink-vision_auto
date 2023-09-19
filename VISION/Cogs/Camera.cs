﻿using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using System.Diagnostics;

namespace VISION.Cogs
{
    public class Camera
    {
        //private PGgloble Glob = PGgloble.getInstance;
        //private CogJobManager manager = new CogJobManager();
        //private CogJob job = new CogJob();
        //private CogToolGroup toolGroup = new CogToolGroup();
        //private CogAcqFifoTool camTool = new CogAcqFifoTool();
        private PGgloble Glob = PGgloble.getInstance;
        private CogJobManager jobManager = new CogJobManager();
        private CogJob job = new CogJob();
        private CogAcqFifoTool camTool = new CogAcqFifoTool();

        public Camera(int Toolnumber = 0)
        {
            jobManager.GarbageCollection = true;
            jobManager.JobAdd(job);
            camTool = new CogAcqFifoTool();
            camTool.Name = "cam - " + Toolnumber.ToString();
            //this.manager = new CogJobManager() { GarbageCollection = true };
            //this.job = new CogJob();
            //this.manager.JobAdd(job);
            //this.job.VisionTool = this.toolGroup; //??
            //camTool.Name = "cam - " + Toolnumber.ToString();
        }

        public bool Loadtool(string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                CogSerializer.SaveObjectToFile(this.camTool, path);
            }
            camTool = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(path);
            CogToolGroup group = new CogToolGroup();
            job.VisionTool = group;
            group.Tools.Add(camTool);
            return true;
            //if (System.IO.File.Exists(path) == false)
            //{
            //    CogSerializer.SaveObjectToFile(this.camTool, path);
            //}
            //camTool = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(path);
            //this.toolGroup.Tools.Add(camTool);
            //return true;
        }

        public CogImage8Grey Run()
        {
            this.camTool.Run(); //Tool실행.
            CogImage8Grey Image = (CogImage8Grey)camTool.OutputImage; //Tool 출력이미지.
            return Image;
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
