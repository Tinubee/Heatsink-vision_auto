using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Caliper
    {
        private Cognex.VisionPro.Caliper.CogCaliperTool Tool;
        public Caliper(int Toolnumber)
        {
            Tool = new Cognex.VisionPro.Caliper.CogCaliperTool();
            Tool.Name = "Caliper - " + Toolnumber.ToString();
        }

        public string ToolName()
        {
            return this.Tool.Name;
        }


        private bool NewTool()
        {
            Tool.RunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.Pair;

            Cognex.VisionPro.CogRectangleAffine Region = new Cognex.VisionPro.CogRectangleAffine();

            Region.CenterX = 400;
            Region.CenterY = 300;

            Region.SideXLength = 100;
            Region.SideYLength = 100;

            Region.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            Region.LineWidthInScreenPixels = 10;
            Region.Color = Cognex.VisionPro.CogColorConstants.Green;

            Region.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDot;
            Region.SelectedLineWidthInScreenPixels = 10;
            Region.SelectedColor = Cognex.VisionPro.CogColorConstants.Cyan;

            Region.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dot;
            Region.DragLineWidthInScreenPixels = 10;
            Region.DragColor = Cognex.VisionPro.CogColorConstants.Yellow;

            Region.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
            Region.Interactive = true;

            Tool.Region = Region;

            return true;
        }

        /// <summary>
        /// 파일에서 툴을 불러 옴. 파일이 있는 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">파일이 있는  폴더의 경로</param>
        /// <returns></returns>
        public bool Loadtool(string path)
        {
            string Savepath = path;

            if (System.IO.Directory.Exists(Savepath) == false)
            {
                NewTool();
                return true;
            }

            Savepath = Savepath + "\\" + Tool.Name + ".vpp";

            if (System.IO.File.Exists(Savepath) == false)
            {
                NewTool();
                return false;
            }

            Tool = (Cognex.VisionPro.Caliper.CogCaliperTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Savepath);

            return true;
        }

        /// <summary>
        /// 파일에 툴의 정보를 씀. 대상 파일이 위치 할 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">저장 할 대상 폴더의 경로</param>
        /// <returns></returns>
        public bool Savetool(string path)
        {
            string Savepath = path;

            if (System.IO.Directory.Exists(Savepath) == false)
            {
                return false;
            }

            Savepath = Savepath + "\\" + Tool.Name + ".vpp";

            Cognex.VisionPro.CogSerializer.SaveObjectToFile(this.Tool, Savepath);

            return true;
        }

        /// <summary>
        /// 툴에 이미지 입력
        /// </summary>
        /// <param name="image">툴에 입력 할 이미지</param>
        /// <returns></returns>
        public bool InputImage(Cognex.VisionPro.CogImage8Grey image)
        {
            if (image == null)
            {
                return false;
            }

            Tool.InputImage = image;
            return true;
        }

        public bool Run(Cognex.VisionPro.CogImage8Grey image)
        {
            if (!InputImage(image))
            {
                return false;
            }

            this.Tool.Run();

            if (this.Tool.Results == null)
            {
                return false;
            }

            return true;
        }

        //public Cognex.VisionPro.CogLine Line()
        //{
        //    return Tool.Results.GetLine();
        //}

        //public Cognex.VisionPro.CogLineSegment Segment()
        //{
        //    return Tool.Results.GetLineSegment();
        //}

        public void Area(ref Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogImage8Grey Image, string ImageSpace)
        {
            if (InputImage(Image) == true)
            {
                Display.InteractiveGraphics.Add(Tool.Region, null, false);
            }
        }

        //public double Threshold()
        //{
        //    return this.Tool.RunParams.ContrastThreshold;
        //}

        public void Threshold(double threshold)
        {
            //this.Tool.RunParams.ContrastThreshold = threshold;
        }

        public void Halfpixel(int halfpixel)
        {

        }

        public void Length(double Length)
        {

        }
      
        public double Result_Rotation()
        {
            double Result = 0;
            return Result;
        }
        public int Result_LineCount()
        {
            int Result = 0;
            Result = Tool.Results.Count;
            return Result;
        }
        public double Result_Corner_X()
        {
            double Result = 0.0;
            Result = Tool.Results[0].Edge0.PositionX;
            return Result;
        }
        public double Result_Corner_Y()
        {
            double Result = 0.0;
            Result = Tool.Results[0].Edge0.PositionY;
            return Result;
        }
        public void ResultAllDisplay(Cognex.VisionPro.CogGraphicCollection Collection)
        {
            //if (Tool.Results == null || Tool.Results.GetCircle() == null)
            //{
            //    return;
            //}
            //if (Tool.Results.Count < 1)
            //{
            //    return;
            //}
            Collection.Add(Tool.Results[0].CreateResultGraphics(Cognex.VisionPro.Caliper.CogCaliperResultGraphicConstants.All));
            //Collection.Add(Tool.Results.LineResultsB.GetLine());
        }
        public void ResultDisplay(ref Cognex.VisionPro.Display.CogDisplay display)
        {
            if (Tool.Results == null)
            {
                return;
            }
            display.InteractiveGraphics.Add(Tool.Results[0].CreateResultGraphics(Cognex.VisionPro.Caliper.CogCaliperResultGraphicConstants.All), null, false);
            //display.InteractiveGraphics.Add(this.Tool.Results.(), null, false);
        }
        /// <summary>
        /// 검사 툴 전체 셋업 화면을 화면에 표시
        /// </summary>
        public void ToolSetup()
        {
            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            Cognex.VisionPro.Caliper.CogCaliperEditV2 Edit = new Cognex.VisionPro.Caliper.CogCaliperEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.Show(); // 폼 실행
        }
    }
}
