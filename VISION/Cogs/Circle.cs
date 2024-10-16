using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Circle
    {
        private Cognex.VisionPro.Caliper.CogFindEllipseTool Tool;

        public Circle(int Toolnumber)
        {
            this.Tool = new Cognex.VisionPro.Caliper.CogFindEllipseTool();
            this.Tool.Name = "Circle - " + Toolnumber.ToString();
        }

        public string ToolName()
        {
            return this.Tool.Name;
        }

        private void NewTool()
        { // 툴의 가장 초기 상태 셋업
            Cognex.VisionPro.CogEllipticalArc Region = new Cognex.VisionPro.CogEllipticalArc();

            Region.CenterX = 800;
            Region.CenterY = 800;

            Region.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            Region.LineWidthInScreenPixels = 3;
            Region.Color = Cognex.VisionPro.CogColorConstants.Green;

            Region.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dash;
            Region.SelectedLineWidthInScreenPixels = 3;
            Region.SelectedColor = Cognex.VisionPro.CogColorConstants.Cyan;

            Region.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dot;
            Region.DragLineWidthInScreenPixels = 3;
            Region.DragColor = Cognex.VisionPro.CogColorConstants.Blue;

            Region.GraphicDOFEnable = CogEllipticalArcDOFConstants.All;
            Region.Interactive = true;

            this.Tool.RunParams.ExpectedEllipticalArc = Region;

            this.Tool.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge;
            this.Tool.RunParams.CaliperSearchDirection = Cognex.VisionPro.Caliper.CogFindEllipseSearchDirectionConstants.Inward;//CogFindCircleSearchDirectionConstants.Inward;
            this.Tool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight;
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

            Tool = (Cognex.VisionPro.Caliper.CogFindEllipseTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Savepath);

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

            Cognex.VisionPro.CogSerializer.SaveObjectToFile(Tool, Savepath);

            return true;
        }

        public bool InputImage(Cognex.VisionPro.CogImage8Grey Image)
        {
            if (Image == null)
            {
                return false;
            }
            else
            {
                this.Tool.InputImage = Image;
                return true;
            }
        }

        public bool Run(Cognex.VisionPro.CogImage8Grey Image)
        {
            if (!this.InputImage(Image))
            {
                return false;
            }

            this.Tool.Run();

            System.Threading.Thread.Sleep(50);

            if (this.Tool.Results == null)
            {
                return false;
            }

            if (this.Tool.Results.Count < 1)
            {
                return false;
            }

            return true;
        }

        public void Area(ref Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogImage8Grey Image, string ImageSpace)
        {
            if (this.InputImage(Image) == true)
            {
                Cognex.VisionPro.CogGraphicCollection cogRegion_Collection;
                Cognex.VisionPro.ICogRecord cogRect_FindRecord;
                Tool.CurrentRecordEnable = Cognex.VisionPro.Caliper.CogFindEllipseCurrentRecordConstants.All; //  CogFindCircleCurrentRecordConstants.All;
                cogRect_FindRecord = Tool.CreateCurrentRecord();

                cogRegion_Collection = (Cognex.VisionPro.CogGraphicCollection)cogRect_FindRecord.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

                //if (this.Tool.RunParams.ExpectedCircularArc == null)
                //{
                //    Region = new Cognex.VisionPro.CogLineSegment();
                //}
                //else
                //{
                //    Region = this.Tool.RunParams.cal;
                //}

                this.Tool.RunParams.ExpectedEllipticalArc.SelectedSpaceName = ImageSpace;
                //this.Tool.RunParams.ex = Region;

                Display.InteractiveGraphics.Add(this.Tool.RunParams.ExpectedEllipticalArc, null, false);
                //for (int i = 0; i < cogRegion_Collection.Count; i++)
                //{
                //    Display.InteractiveGraphics.Add((Cognex.VisionPro.ICogGraphicInteractive)cogRegion_Collection[i], "FindLine", true);
                //}
            }
        }

        //public void Area(ref Cognex.VisionPro.Display.CogDisplay Display)
        //{
        //    Display.InteractiveGraphics.Add(this.Tool.RunParams.ExpectedCircularArc, null, false);
        //}

        public double Threshold()
        {
            return this.Tool.RunParams.CaliperRunParams.ContrastThreshold;
        }

        public void Threshold(double threshold)
        {
            this.Tool.RunParams.CaliperRunParams.ContrastThreshold = threshold;
        }

        public int Halfpixel()
        {
            return this.Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels;
        }

        public void Halfpixel(int halfpixel)
        {
            this.Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = halfpixel;
        }

        public double[] ResultLocation()
        {
            double[] Result = { 0.0, 0.0 };

            if (this.Tool.Results == null)
            {
                return Result;
            }

            if (this.Tool.Results.Count < 1)
            {
                return Result;
            }

            Result[0] = this.Tool.Results.GetEllipse().CenterX;
            Result[1] = this.Tool.Results.GetEllipse().CenterY;

            return Result;
        }

        public double ResultLocation_X()
        {
            double Result = 0.0;

            if (this.Tool.Results == null)
            {
                return Result;
            }

            if (this.Tool.Results.Count == 0)
            {
                return Result;
            }

            Result = this.Tool.Results.GetEllipse().CenterX;

            return Result;
        }

        public double ResultLocation_Y()
        {
            double Result = 0.0;

            if (this.Tool.Results == null)
            {
                return Result;
            }

            if (this.Tool.Results.Count < 1)
            {
                return Result;
            }
            Result = this.Tool.Results.GetEllipse().CenterY;

            return Result;
        }
        public void ResultAllDisplay(Cognex.VisionPro.CogGraphicCollection Collection)
        {
            if (Tool.Results == null || Tool.Results.GetEllipse() == null)
            {
                return;
            }
            if (Tool.Results.Count < 1)
            {
                return;
            }
            Collection.Add(Tool.Results.GetEllipse());
            //Collection.Add(Tool.Results.LineResultsB.GetLine());
        }


        public void Area_Affine_Main1(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.RunParams.ExpectedEllipticalArc == null)
            {
                this.NewTool();
            }

            CogEllipticalArc area = (CogEllipticalArc)Tool.RunParams.ExpectedEllipticalArc;
            area.Interactive = true;
            area.GraphicDOFEnable = CogEllipticalArcDOFConstants.All; //CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.RunParams.ExpectedEllipticalArc = area;
        }


        public Cognex.VisionPro.CogEllipse GetCircle()
        {
            if (Tool.Results == null)
            {
                return null;
            }
            return Tool.Results.GetEllipse();
        }
        public void ResultDisplay(ref Cognex.VisionPro.Display.CogDisplay Display)
        {
            if (this.Tool.Results == null)
            {
                return;
            }

            if (this.Tool.Results.Count < 1)
            {
                return;
            }

            Cognex.VisionPro.CogEllipse Result = null;
            Result = this.Tool.Results.GetEllipse();
            if (Result == null)
            {
                return;
            }
            Result.LineWidthInScreenPixels = 5;
            Result.Color = Cognex.VisionPro.CogColorConstants.DarkGreen;

            Display.InteractiveGraphics.Add(Result, null, true);
        }

        public void ToolSetup()
        {
            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            Cognex.VisionPro.Caliper.CogFindEllipseEditV2 Edit = new Cognex.VisionPro.Caliper.CogFindEllipseEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.Show(); // 폼 실행
        }
    }
}
