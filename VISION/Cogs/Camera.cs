using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Camera
    {
        private CogAcqFifoTool camTool = new CogAcqFifoTool();
        public Camera(int Toolnumber = 0)
        {
            camTool = new CogAcqFifoTool();
            camTool.Name = "cam - " + Toolnumber.ToString();
        }
        public bool Loadtool(string path)
        {
            camTool = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(path);

            return true;
        }

        public  CogImage8Grey Run()
        {
            this.camTool.Run();
            CogImage8Grey Image = (CogImage8Grey)camTool.OutputImage;
            return Image;
        }
    }
}
