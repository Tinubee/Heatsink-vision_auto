using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Camera
    {
        private CogJob job = new CogJob();
        private CogAcqFifoTool camTool = new CogAcqFifoTool();

        public Camera(int Toolnumber = 0)
        {
            camTool = new CogAcqFifoTool();
            camTool.Name = "cam - " + Toolnumber.ToString();
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
        }

        public CogImage8Grey Run()
        {
            Debug.WriteLine("6번카메라촬영 시작.");
            this.camTool.Run();
            CogImage8Grey Image = (CogImage8Grey)camTool.OutputImage;
            return Image;
        }

        public void SetBrightness(double value)
        {
            if(this.camTool.Operator != null)
            {
                this.camTool.Operator.OwnedBrightnessParams.Brightness = value;
            }
           
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
            if(this.camTool.Operator.OwnedBrightnessParams== null) return 0;

            return this.camTool.Operator.OwnedBrightnessParams.Brightness;
        }

        public void SetExposure(double value)
        {
            if(this.camTool.Operator == null) return;
            this.camTool.Operator.OwnedExposureParams.Exposure= value;
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
            this.job.Shutdown();
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
