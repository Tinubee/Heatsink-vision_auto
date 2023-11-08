using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Display;
using VISION.Class;
using System.Diagnostics;

namespace VISION.Cogs
{
    public class MultiPMAlign
    {
        private CogPMAlignMultiTool Tool;
        private Class_Common cm { get { return Program.cm; } }
        private PGgloble Glob = PGgloble.getInstance;
        public MultiPMAlign(int Toolnumber = 0)
        {
            Tool = new CogPMAlignMultiTool();
            Tool.Name = "MultiPattern - " + Toolnumber.ToString();
        }
        public CogPMAlignMultiTool PMTool()
        {
            this.Tool.InputImage = null;
            return this.Tool;
        }
        public string ToolName()
        {
            return this.Tool.Name;
        }

        private bool NewTool()
        {
            CogRectangleAffine SearchRegion = new CogRectangleAffine();
            CogRectangleAffine TrainRegion = new CogRectangleAffine();

            SearchRegion.CenterX = 800;
            SearchRegion.CenterY = 600;

            SearchRegion.SideXLength = 200;
            SearchRegion.SideYLength = 200;

            SearchRegion.LineWidthInScreenPixels = 5;
            SearchRegion.LineStyle = CogGraphicLineStyleConstants.Solid;
            SearchRegion.Color = CogColorConstants.Green;

            SearchRegion.DragLineWidthInScreenPixels = 2;
            SearchRegion.DragLineStyle = CogGraphicLineStyleConstants.DashDotDot;
            SearchRegion.DragColor = CogColorConstants.Blue;

            SearchRegion.SelectedLineWidthInScreenPixels = 5;
            SearchRegion.SelectedLineStyle = CogGraphicLineStyleConstants.Solid;
            SearchRegion.SelectedColor = CogColorConstants.Red;

            SearchRegion.Interactive = true;
            SearchRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;

            TrainRegion.CenterX = 800;
            TrainRegion.CenterY = 600;

            TrainRegion.SideXLength = 200;
            TrainRegion.SideYLength = 200;

            TrainRegion.LineWidthInScreenPixels = 5;
            TrainRegion.LineStyle = CogGraphicLineStyleConstants.Solid;
            TrainRegion.Color = CogColorConstants.Green;

            TrainRegion.DragLineWidthInScreenPixels = 2;
            TrainRegion.DragLineStyle = CogGraphicLineStyleConstants.DashDotDot;
            TrainRegion.DragColor = CogColorConstants.Blue;

            TrainRegion.SelectedLineWidthInScreenPixels = 5;
            TrainRegion.SelectedLineStyle = CogGraphicLineStyleConstants.Solid;
            TrainRegion.SelectedColor = CogColorConstants.Red;

            TrainRegion.Interactive = true;
            TrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;

            SearchRegion.TipText = "Multi Pattern Search Area";
            TrainRegion.TipText = "Multi Pattern Train Area";

            Tool.SearchRegion = SearchRegion;
            Tool.RunParams.RuntimeMode = CogPMAlignMultiRuntimeModeConstants.Exhaustive;
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

                Tool = (CogPMAlignMultiTool)CogSerializer.LoadObjectFromFile(Savepath);

                return true;
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
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

            CogSerializer.SaveObjectToFile(Tool, Savepath);

            return true;
        }
        public void InputImage(CogImage8Grey image)
        {
            Tool.InputImage = image;
        }
        public bool setImage(CogImage8Grey image, int number, bool istrain = false)
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
        public CogImage8Grey TrainedImage(int number)
        {
            if (istrain(number) == false)
            {
                return null;
            }

            return (CogImage8Grey)Tool.Operator[number].Pattern.GetTrainedPatternImage();
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

        public bool Train(CogImage8Grey image, int num)
        {
            try
            {
                if (setImage(image, num, true) == false)
                {
                    return false;
                }

                CogRectangleAffine Area3 = (CogRectangleAffine)Tool.Operator[num].Pattern.TrainRegion;
                Tool.Operator[num].Pattern.Origin.TranslationX = Area3.CenterX;
                Tool.Operator[num].Pattern.Origin.TranslationY = Area3.CenterY;

                CogRectangleAffine Area = (CogRectangleAffine)Tool.Operator[num].Pattern.TrainRegion;

                Tool.Operator[num].Pattern.Train();
                return istrain(num);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
                return istrain(num); ;
            }
        }
        public void SetSearchAreaPosition(ref CogDisplay display, CogImage8Grey Image)
        {

        }
        public void SearchArea(ref CogDisplay display, CogImage8Grey Image, int CameraNumber, int ToolNumber)
        {
            CogRectangleAffine SearchRegion = new CogRectangleAffine();

            SearchRegion = (CogRectangleAffine)Tool.SearchRegion;
            if (SearchRegion == null) return;
            SearchRegion.LineWidthInScreenPixels = 5;
            SearchRegion.LineStyle = CogGraphicLineStyleConstants.Solid;
            SearchRegion.Color = CogColorConstants.Green;

            SearchRegion.DragLineWidthInScreenPixels = 2;
            SearchRegion.DragLineStyle = CogGraphicLineStyleConstants.DashDotDot;
            SearchRegion.DragColor = CogColorConstants.Blue;

            SearchRegion.SelectedLineWidthInScreenPixels = 5;
            SearchRegion.SelectedLineStyle = CogGraphicLineStyleConstants.Solid;
            SearchRegion.SelectedColor = CogColorConstants.Red;

            SearchRegion.Interactive = true;
            SearchRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
          
            Tool.SearchRegion = SearchRegion;

            display.InteractiveGraphics.Add((ICogGraphicInteractive)Tool.SearchRegion, null, false);
        }

        public bool Run(CogImage8Grey image)
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
        public CogTransform2DLinear ResultPoint(int PatternNumber)
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
        public void ResultDisplay(ref CogDisplay display, int PatternNumber)
        {
            if (Tool.Results == null)
            {
                return;
            }

            if (Tool.Results.PMAlignResults.Count < 1)
            {
                CogRectangleAffine NG = (CogRectangleAffine)Tool.SearchRegion;
                NG.LineStyle = CogGraphicLineStyleConstants.Solid;
                NG.Color = CogColorConstants.Red;
                NG.Interactive = false;
                display.InteractiveGraphics.Add(NG, null, false);
                return;
            }
            display.InteractiveGraphics.Add(Tool.Results.PMAlignResults[PatternNumber].CreateResultGraphics(CogPMAlignResultGraphicConstants.All), null, false);
        }
        public void ResultDisplay(CogDisplay display, CogGraphicCollection Collection, int Number, int ToolNumber, double 회전각도)
        {
            CogCreateGraphicLabelTool lb_Score = new CogCreateGraphicLabelTool();
            lb_Score.InputImage = display.Image;
            lb_Score.SourceSelector = CogCreateGraphicLabelSourceSelectorConstants.InputGraphicLabelText;

            if (Tool.Results == null || Tool.Results.PMAlignResults.Count < 1)
            {
                CogRectangleAffine NG = (CogRectangleAffine)Tool.SearchRegion;
                NG.LineStyle = CogGraphicLineStyleConstants.Solid;
                NG.Color = CogColorConstants.Red;
                NG.Interactive = false;

                lb_Score.InputGraphicLabel.X = NG.CenterX;
                lb_Score.InputGraphicLabel.Y = NG.CenterY;
                lb_Score.OutputColor = CogColorConstants.Red;
                lb_Score.InputGraphicLabel.Rotation = 회전각도;
                lb_Score.InputGraphicLabel.Text = $"{ToolNumber}번패턴 검색영역 내 없음";
                lb_Score.Run();

                //display.InteractiveGraphics.Add(NG, null, false);
                display.StaticGraphics.Add(lb_Score.GetOutputGraphicLabel(), null);
            }
            else
            {
                lb_Score.InputGraphicLabel.X = Tool.Results.PMAlignResults[Number].GetPose().TranslationX;
                lb_Score.InputGraphicLabel.Y = Tool.Results.PMAlignResults[Number].GetPose().TranslationY;
                lb_Score.InputGraphicLabel.Text = Tool.Results.PMAlignResults[Number].Score > Tool.RunParams.PMAlignRunParams.AcceptThreshold ? $"{ToolNumber}" : $"{ToolNumber} - {(Tool.Results.PMAlignResults[Number].Score * 100).ToString("F2")} / {Tool.RunParams.PMAlignRunParams.AcceptThreshold * 100}";
                lb_Score.InputGraphicLabel.Rotation = 회전각도;
                lb_Score.OutputColor = Tool.Results.PMAlignResults[Number].Score > Tool.RunParams.PMAlignRunParams.AcceptThreshold ? CogColorConstants.Green : CogColorConstants.Red;
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
                CogPMAlignMultiEditV2 Edit = new CogPMAlignMultiEditV2();

                Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
                Edit.Subject = Tool; // 에디트에 툴 정보 입력.
                Window.Controls.Add(Edit); // 폼에 에디트 추가.

                Window.Width = 800;
                Window.Height = 600;

                Window.ShowDialog(); // 폼 실행
            }
            catch (Exception)
            {

            }
        }
    }
}
