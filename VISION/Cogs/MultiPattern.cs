using Cognex.VisionPro.Dimensioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class MultiPMAlign
    {
        private Cognex.VisionPro.PMAlign.CogPMAlignMultiTool Tool;
        private Class_Common cm { get { return Program.cm; } }
        private PGgloble Glob = PGgloble.getInstance;
        public MultiPMAlign(int Toolnumber = 0)
        {
            Tool = new Cognex.VisionPro.PMAlign.CogPMAlignMultiTool();
            Tool.Name = "MultiPattern - " + Toolnumber.ToString();
        }

        public string ToolName()
        {
            return this.Tool.Name;
        }

        private bool NewTool()
        {
            Cognex.VisionPro.CogRectangleAffine SearchRegion = new Cognex.VisionPro.CogRectangleAffine();
            Cognex.VisionPro.CogRectangleAffine TrainRegion = new Cognex.VisionPro.CogRectangleAffine();

            SearchRegion.CenterX = 800;
            SearchRegion.CenterY = 600;

            SearchRegion.SideXLength = 200;
            SearchRegion.SideYLength = 200;

            SearchRegion.LineWidthInScreenPixels = 5;
            SearchRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            SearchRegion.Color = Cognex.VisionPro.CogColorConstants.Green;

            SearchRegion.DragLineWidthInScreenPixels = 2;
            SearchRegion.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDotDot;
            SearchRegion.DragColor = Cognex.VisionPro.CogColorConstants.Blue;

            SearchRegion.SelectedLineWidthInScreenPixels = 5;
            SearchRegion.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            SearchRegion.SelectedColor = Cognex.VisionPro.CogColorConstants.Red;

            SearchRegion.Interactive = true;
            SearchRegion.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;

            TrainRegion.CenterX = 800;
            TrainRegion.CenterY = 600;

            TrainRegion.SideXLength = 200;
            TrainRegion.SideYLength = 200;

            TrainRegion.LineWidthInScreenPixels = 5;
            TrainRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            TrainRegion.Color = Cognex.VisionPro.CogColorConstants.Green;

            TrainRegion.DragLineWidthInScreenPixels = 2;
            TrainRegion.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDotDot;
            TrainRegion.DragColor = Cognex.VisionPro.CogColorConstants.Blue;

            TrainRegion.SelectedLineWidthInScreenPixels = 5;
            TrainRegion.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            TrainRegion.SelectedColor = Cognex.VisionPro.CogColorConstants.Red;

            TrainRegion.Interactive = true;
            TrainRegion.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;

            SearchRegion.TipText = "Multi Pattern Search Area";
            TrainRegion.TipText = "Multi Pattern Train Area";

            Tool.SearchRegion = SearchRegion;
            Tool.RunParams.RuntimeMode = Cognex.VisionPro.PMAlign.CogPMAlignMultiRuntimeModeConstants.Exhaustive;
            Tool.RunParams.UseXYOverlapBetweenPatterns = false;

            return true;
        }

        public bool LoadTool(string path)
        {
            try
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

                Tool = (Cognex.VisionPro.PMAlign.CogPMAlignMultiTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Savepath);

                return true;
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
                return false;
            }
        }

        public bool SaveTool(string path)
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
        public void InputImage(Cognex.VisionPro.CogImage8Grey image)
        {
            Tool.InputImage = image;
        }
        public bool setImage(Cognex.VisionPro.CogImage8Grey image, int number, bool istrain = false)
        {
            if (image == null)
            {
                return false;
            }

            if (istrain == false)
            {
                Tool.InputImage = image;
            }
            else
            {
                Tool.Operator[number].Pattern.TrainImage = image;
            }

            return true;
        }
        public Cognex.VisionPro.CogImage8Grey TrainedImage(int number)
        {
            if (istrain(number) == false)
            {
                return null;
            }

            return (Cognex.VisionPro.CogImage8Grey)Tool.Operator[number].Pattern.GetTrainedPatternImage();
        }
        public bool istrain(int num)
        {
            if (Tool.Operator.Count() == 0)
            {
                return false;
            }
            else
                return Tool.Operator.Trained;
        }
        public int PatternCount()
        {
            return Tool.Operator.Count();
        }

        public bool Train(Cognex.VisionPro.CogImage8Grey image, int num)
        {
            try
            {
                if (setImage(image, num, true) == false)
                {
                    return false;
                }

                Cognex.VisionPro.CogRectangleAffine Area3 = (Cognex.VisionPro.CogRectangleAffine)Tool.Operator[num].Pattern.TrainRegion;
                Tool.Operator[num].Pattern.Origin.TranslationX = Area3.CenterX;
                Tool.Operator[num].Pattern.Origin.TranslationY = Area3.CenterY;

                Cognex.VisionPro.CogRectangleAffine Area = (Cognex.VisionPro.CogRectangleAffine)Tool.Operator[num].Pattern.TrainRegion;

                Tool.Operator[num].Pattern.Train();
                return istrain(num);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
                return istrain(num); ;
            }
        }
        public void SetSearchAreaPosition(ref Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogImage8Grey Image)
        {

        }
        public void SearchArea(ref Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogImage8Grey Image, int CameraNumber, int ToolNumber)
        {
            Cognex.VisionPro.CogRectangleAffine SearchRegion = new Cognex.VisionPro.CogRectangleAffine();
            
            SearchRegion = (Cognex.VisionPro.CogRectangleAffine)Tool.SearchRegion;
            if (SearchRegion == null) return;
            SearchRegion.LineWidthInScreenPixels = 5;
            SearchRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            SearchRegion.Color = Cognex.VisionPro.CogColorConstants.Green;

            SearchRegion.DragLineWidthInScreenPixels = 2;
            SearchRegion.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDotDot;
            SearchRegion.DragColor = Cognex.VisionPro.CogColorConstants.Blue;

            SearchRegion.SelectedLineWidthInScreenPixels = 5;
            SearchRegion.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            SearchRegion.SelectedColor = Cognex.VisionPro.CogColorConstants.Red;

            SearchRegion.Interactive = true;
            SearchRegion.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
            //if (ToolNumber == 0)
            //{
            //    SearchRegion.CenterX = 1500;
            //    SearchRegion.CenterY = 1000;
            //    SearchRegion.SideXLength = 2000;
            //    SearchRegion.SideYLength = 2000;
            //}
            //else
            //{
            //    SearchRegion.CenterX = Glob.StandPoint_X + Glob.SearchAreaPositionX[CameraNumber, ToolNumber];
            //    SearchRegion.CenterY = Glob.StandPoint_Y + Glob.SearchAreaPositionY[CameraNumber, ToolNumber];
            //    SearchRegion.SideXLength = Glob.SearchAreaGaroLength[CameraNumber, ToolNumber];
            //    SearchRegion.SideYLength = Glob.SearchAreaSeroLength[CameraNumber, ToolNumber];
            //    SearchRegion.Rotation = Glob.SearchAreaRotation[CameraNumber, ToolNumber];
            //}
            Tool.SearchRegion = SearchRegion;

            display.InteractiveGraphics.Add((Cognex.VisionPro.ICogGraphicInteractive)Tool.SearchRegion, null, false);
        }

        public bool Run(Cognex.VisionPro.CogImage8Grey image)
        {
            if (image == null)
            {
                return false;
            }
            Tool.InputImage = image;
            Tool.Run();
            if (Tool.Results == null)
            {
                return false;
            }
            if (Tool.Results.PMAlignResults.Count < 1)
            {
                return false;
            }
            return true;
        }
        public double Threshold()
        {
            return Tool.RunParams.PMAlignRunParams.AcceptThreshold;
        }
        public void Threshold(double threshold)
        {
            Tool.RunParams.PMAlignRunParams.AcceptThreshold = threshold;
        }
        public double ResultScore(int i)
        {
            if (Tool.Results == null || Tool.Results.PMAlignResults.Count < 1)
            {
                return 0;
            }
            else
            {
                return Tool.Results.PMAlignResults[i].Score * 100;
            }
        }
        public int ResultCount()
        {
            try
            {
                if (Tool.Results == null) return 0;
                return Tool.Results.PMAlignResults.Count;
            }
            catch
            {
                return 0;
            }
        }
        public double TransX(int num)
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                return 0;
            }

            return Tool.Results.PMAlignResults[num].GetPose().TranslationX;
        }
        public double TransY(int num)
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                return 0;
            }

            return Tool.Results.PMAlignResults[num].GetPose().TranslationY;
        }
        public double TransRotation(int num)
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                return 0;
            }

            return Tool.Results.PMAlignResults[num].GetPose().Rotation;
        }
        public Cognex.VisionPro.CogTransform2DLinear ResultPoint(int PatternNumber)
        {
            if (Tool.Results == null)
            {
                return null;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                return null;
            }

            return Tool.Results.PMAlignResults[PatternNumber].GetPose();
        }
        public int HighestResultToolNumber()
        {
            try
            {
                if (Tool.Results == null) return 0;

                double[] Result = new double[Tool.Results.PMAlignResults.Count];
                int HighestResultToolNumber = 0;
                if (Tool.Results == null)
                {
                    return -1;
                }
                for (int i = 0; i < Result.Count(); i++)
                {
                    Result[i] = Tool.Results.PMAlignResults[i].Score;
                }

                for (int i = 0; i < Result.Count() - 1; i++)
                {
                    if (Result[HighestResultToolNumber] < Result[i])
                    {
                        HighestResultToolNumber = i;
                    }
                }
                return HighestResultToolNumber;

            }
            catch
            {
                return 0;
            }
           
        }
        public void ResultDisplay(ref Cognex.VisionPro.Display.CogDisplay display, int PatternNumber)
        {
            if (Tool.Results == null)
            {
                return;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                Cognex.VisionPro.CogRectangleAffine NG = (Cognex.VisionPro.CogRectangleAffine)Tool.SearchRegion;
                NG.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
                NG.Color = Cognex.VisionPro.CogColorConstants.Red;
                NG.Interactive = false;
                display.InteractiveGraphics.Add(NG, null,false);
                return;
            }
            display.InteractiveGraphics.Add(Tool.Results.PMAlignResults[PatternNumber].CreateResultGraphics(Cognex.VisionPro.PMAlign.CogPMAlignResultGraphicConstants.All), null, false);
        }
        public void ResultDisplay(Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogGraphicCollection Collection, int Number, int ToolNumber)
        {
            CogCreateGraphicLabelTool lb_Score = new CogCreateGraphicLabelTool();
            lb_Score.InputImage = display.Image;
            lb_Score.SourceSelector = CogCreateGraphicLabelSourceSelectorConstants.InputDouble;

            if (Tool.Results == null || Tool.Results.PMAlignResults.Count < 1)
            {
                Cognex.VisionPro.CogRectangleAffine NG = (Cognex.VisionPro.CogRectangleAffine)Tool.SearchRegion;
                NG.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
                NG.Color = Cognex.VisionPro.CogColorConstants.Red;
                NG.Interactive = false;

                lb_Score.InputGraphicLabel.X = NG.CenterX;
                lb_Score.InputGraphicLabel.Y = NG.CenterY;
                lb_Score.OutputColor = Cognex.VisionPro.CogColorConstants.Red;
                lb_Score.InputDouble = ToolNumber;
                lb_Score.Run();

                display.InteractiveGraphics.Add(NG, null, false);
                display.StaticGraphics.Add(lb_Score.GetOutputGraphicLabel(), null);
            }
            else
            {
                lb_Score.InputGraphicLabel.X = Tool.Results.PMAlignResults[Number].GetPose().TranslationX;
                lb_Score.InputGraphicLabel.Y = Tool.Results.PMAlignResults[Number].GetPose().TranslationY;
                lb_Score.InputDouble = ToolNumber;//Convert.ToDouble((Tool.Results.PMAlignResults[Number].Score * 100).ToString("F2"));
                lb_Score.OutputColor = Cognex.VisionPro.CogColorConstants.Green;
                lb_Score.Run();
              
                display.InteractiveGraphics.Add(Tool.Results.PMAlignResults[Number].CreateResultGraphics(Cognex.VisionPro.PMAlign.CogPMAlignResultGraphicConstants.MatchRegion), null, false);
                display.InteractiveGraphics.Add(lb_Score.GetOutputGraphicLabel(), null, false);
            }
        }

        public void ToolSetup()
        {
            try
            {
                System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
                Cognex.VisionPro.PMAlign.CogPMAlignMultiEditV2 Edit = new Cognex.VisionPro.PMAlign.CogPMAlignMultiEditV2();

                Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
                Edit.Subject = Tool; // 에디트에 툴 정보 입력.
                Window.Controls.Add(Edit); // 폼에 에디트 추가.

                Window.Width = 800;
                Window.Height = 600;

                Window.ShowDialog(); // 폼 실행
            }
            catch(Exception)
            {
                
            }
        }
    }
}
