using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Line
    {
        private Cognex.VisionPro.Caliper.CogFindLineTool Tool;
        public Line(int Toolnumber)
        {
            Tool = new Cognex.VisionPro.Caliper.CogFindLineTool();
            Tool.Name = "Line - " + Toolnumber.ToString();
        }

        public string ToolName()
        {
            return this.Tool.Name;
        }

        private bool NewTool()
        {
            Tool.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge;
            Tool.RunParams.NumCalipers = 10;
            Tool.CurrentRecordEnable = Cognex.VisionPro.Caliper.CogFindLineCurrentRecordConstants.All;
            Cognex.VisionPro.CogLineSegment Region = new Cognex.VisionPro.CogLineSegment();

            Region.StartX = 50;
            Region.StartY = 50;

            Region.EndX = 250;
            Region.EndY = 50;

            Region.EndPointAdornment = Cognex.VisionPro.CogLineSegmentAdornmentConstants.None;
            Region.StartPointAdornment = Cognex.VisionPro.CogLineSegmentAdornmentConstants.None;

            Region.MouseCursor = Cognex.VisionPro.CogStandardCursorConstants.ManipulableGraphic;

            Region.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            Region.LineWidthInScreenPixels = 3;
            Region.Color = Cognex.VisionPro.CogColorConstants.Green;

            Region.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDot;
            Region.SelectedLineWidthInScreenPixels = 3;
            Region.SelectedColor = Cognex.VisionPro.CogColorConstants.Cyan;

            Region.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dot;
            Region.DragLineWidthInScreenPixels = 3;
            Region.DragColor = Cognex.VisionPro.CogColorConstants.Yellow;

            Region.GraphicDOFEnable = Cognex.VisionPro.CogLineSegmentDOFConstants.All;
            Region.Interactive = true;

            Tool.RunParams.ExpectedLineSegment = Region;
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

            Tool = (Cognex.VisionPro.Caliper.CogFindLineTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Savepath);

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

            this.Tool.InputImage = image;
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

            if (this.Tool.Results.GetLine() == null)
            {
                return false;
            }

            return true;
        }
        public double GetRotation()
        {
            try
            {
                return Tool.Results.GetLine().Rotation;
            }
            catch (Exception ee)
            {
                return 0;
            }

        }
        public Cognex.VisionPro.CogLine GetLine()
        {
            try
            {
                return Tool.Results.GetLine();
            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public Cognex.VisionPro.CogLineSegment Segment()
        {
            return Tool.Results.GetLineSegment();
        }

        public void Area(ref Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogImage8Grey Image, string ImageSpace)
        {
            if (InputImage(Image) == true)
            {
                Cognex.VisionPro.CogLineSegment Region;
                Cognex.VisionPro.CogGraphicCollection cogRegion_Collection;
                Cognex.VisionPro.ICogRecord cogRect_FindRecord;
                Tool.CurrentRecordEnable = Cognex.VisionPro.Caliper.CogFindLineCurrentRecordConstants.All;
                cogRect_FindRecord = Tool.CreateCurrentRecord();

                cogRegion_Collection = (Cognex.VisionPro.CogGraphicCollection)cogRect_FindRecord.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

                if (this.Tool.RunParams.ExpectedLineSegment == null)
                {
                    Region = new Cognex.VisionPro.CogLineSegment();
                }
                else
                {
                    Region = this.Tool.RunParams.ExpectedLineSegment;
                }

                this.Tool.RunParams.ExpectedLineSegment.SelectedSpaceName = ImageSpace;
                this.Tool.RunParams.ExpectedLineSegment = Region;

                Display.InteractiveGraphics.Add(Tool.RunParams.ExpectedLineSegment, null, false);
                for (int i = 0; i < cogRegion_Collection.Count; i++)
                {
                    Display.InteractiveGraphics.Add((Cognex.VisionPro.ICogGraphicInteractive)cogRegion_Collection[i], "FindLine", true);
                }
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

        public int Direction()
        {
            if (Tool.RunParams.CaliperSearchDirection == 1.5707963267949)
            {
                return 0;
            }
            else if (Tool.RunParams.CaliperSearchDirection == -1.5707963267949)
            {
                return 1;
            }
            else
                return 0;
        }
        public void Direction(int Direction)
        {
            switch (Direction)
            {
                case 0:
                    Tool.RunParams.CaliperSearchDirection = 1.5707963267949;
                    break;
                case 1:
                    Tool.RunParams.CaliperSearchDirection = -1.5707963267949;
                    break;
            }
        }

        public void Halfpixel(int halfpixel)
        {

        }

        public void Length(double Length)
        {

        }
        public int CaliperNumber()
        {
            return Tool.RunParams.NumCalipers;
        }
        public void CaliperNumber(int num)
        {
            Tool.RunParams.NumCalipers = num;
        }

        public int Polarity()
        {
            switch (Tool.RunParams.CaliperRunParams.Edge0Polarity)
            {
                case Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark:
                    return 0;
                case Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight:
                    return 1;
                default:
                    return 2;
            }
        }
        public void Polarity(int Type)
        {
            switch (Type)
            {
                case 0:
                    Tool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark;
                    break;
                case 1:
                    Tool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight;
                    break;
                default:
                    Tool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DontCare;
                    break;
            }
        }
        public double Average_PointX()
        {
            double Result = 0;
            if (Tool.Results == null)
            {
                return 0;
            }
            for (int i = 0; i < Tool.Results.Count; i++)
            {
                if (Tool.Results[i].Found == false)
                {
                    continue;
                }
                Result += Tool.Results[i].X;
            }
            Result = Result / Tool.Results.NumPointsFound;
            return Result;
        }
        public double Average_PointY()
        {
            double Result = 0;
            if (Tool.Results == null)
            {
                return 0;
            }
            for (int i = 0; i < Tool.Results.Count; i++)
            {
                if (Tool.Results[i].Found == false)
                {
                    continue;
                }
                Result += Tool.Results[i].Y;
            }
            Result = Result / Tool.Results.NumPointsFound;
            return Result;
        }
        public void ResultAllDisplay(ref Cognex.VisionPro.CogGraphicCollection Collection)
        {
            if (Tool.Results == null)
            {
                return;
            }

            if (this.Tool.Results.GetLine() == null)
            {
                return;
            }
            Collection.Add(Tool.Results.GetLine());
            Collection.Add(Tool.Results.GetLineSegment());
        }
        public void ResultDisplay(Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogGraphicCollection Collection)
        {
            try
            {
                if (this.Tool.Results == null || Tool.Results.GetLine() == null)
                {
                    return;
                }
                Collection.Add(Tool.Results.GetLine());
                display.StaticGraphics.AddList(Collection, "");
            }
            catch (Exception ee)
            {
                return;
            }
        }
        /// <summary>
        /// 검사 툴 전체 셋업 화면을 화면에 표시
        /// </summary>
        public void ToolSetup()
        {
            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            Cognex.VisionPro.Caliper.CogFindLineEditV2 Edit = new Cognex.VisionPro.Caliper.CogFindLineEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.Show(); // 폼 실행
        }
    }
}

