using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION.Cogs
{
    public class Blob
    {
        private Cognex.VisionPro.Blob.CogBlobTool Tool;
        public bool Singleblob;

        /// <summary>
        /// 툴의 기본 초기화를 담당 함. 입력 되는 이름은 툴 파일 저장과 읽어 오는 작업에 쓰임.
        /// </summary>
        /// <param name="Toolnumber">툴의 이름.</param>
        public Blob(int Toolnumber = 0)
        {
            Tool = new Cognex.VisionPro.Blob.CogBlobTool();
            Tool.Name = "Blob - " + Toolnumber.ToString();

            if (Toolnumber <= 4)
            {
                Singleblob = true;
            }
            else
            {
                Singleblob = false;
            }

            Tool.RunParams.SegmentationParams.Mode = Cognex.VisionPro.Blob.CogBlobSegmentationModeConstants.HardFixedThreshold;
            Tool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs;
            Tool.RunParams.SegmentationParams.HardFixedThreshold = 200;
        }

        private bool NewTool()
        { // 툴의 가장 초기 상태 셋업
            Cognex.VisionPro.CogPolygon Region = new Cognex.VisionPro.CogPolygon();

            Region.NumVertices = 4;
            Region.SetVertexX(0, 130);
            Region.SetVertexY(0, 60);
            Region.SetVertexX(1, 500);
            Region.SetVertexY(1, 60);
            Region.SetVertexX(2, 500);
            Region.SetVertexY(2, 450);
            Region.SetVertexX(3, 130);
            Region.SetVertexY(3, 450);

            //Region.GetVertexX(0);
            //Region.CenterY = 300;

            //Region.SideXLength = 200;
            //Region.SideYLength = 200;

            Region.LineWidthInScreenPixels = 1;
            Region.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;
            Region.Color = Cognex.VisionPro.CogColorConstants.Green;

            Region.SelectedLineWidthInScreenPixels = 1;
            Region.SelectedColor = Cognex.VisionPro.CogColorConstants.Cyan;
            Region.SelectedLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.DashDot;

            Region.DragLineWidthInScreenPixels = 1;
            Region.DragColor = Cognex.VisionPro.CogColorConstants.Blue;
            Region.DragLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dot;
            Region.TipText = "Search Area";

            Region.Interactive = true;
            Region.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;

            Tool.Region = Region;

            return true;
        }

        private bool NewTool0()
        {
            Cognex.VisionPro.CogCircularAnnulusSection Region = new Cognex.VisionPro.CogCircularAnnulusSection();

            Region.CenterX = 400;
            Region.CenterY = 300;

            Region.Interactive = true;
            Region.GraphicDOFEnable = Cognex.VisionPro.CogCircularAnnulusSectionDOFConstants.All;

            Tool.Region = Region;
            return true;
        }
        public string ToolName()
        {
            return this.Tool.Name;
        }

        /// <summary>
        /// 파일에서 툴을 불러 옴. 파일이 있는 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">파일이 있는  폴더의 경로</param>
        /// <returns></returns>
        public bool Loadtool(string path)
        {
            string Savepath = path;
            string TempName = this.Tool.Name;

            if (System.IO.Directory.Exists(Savepath) == false)
            {
                //if (this.Tool.Name == "Blob - 0" || this.Tool.Name == "Blob - 19")
                //{
                //    NewTool0();
                //}
                //else
                //{
                NewTool();
                //}
                return true;
            }

            Savepath = Savepath + "\\" + Tool.Name + ".vpp";

            if (System.IO.File.Exists(Savepath) == false)
            {
                //if (this.Tool.Name == "Blob - 0" || this.Tool.Name == "Blob - 19")
                //{
                //    NewTool0();
                //}
                //else
                //{
                NewTool();
                // }
                return false;
            }

            Tool = (Cognex.VisionPro.Blob.CogBlobTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Savepath);
            Tool.Name = TempName;
            Application.DoEvents();
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

            Tool.InputImage = image;
            return true;
        }

        /// <summary>
        /// 툴 동작. 검사를 수행함.
        /// </summary>
        /// <param name="image">툴에 입력 할 이미지.</param>
        /// <returns>결과</returns>
        public bool Run(Cognex.VisionPro.CogImage8Grey image)
        {
            if (InputImage(image) == false)
            {
                return false;
            }

            Cognex.VisionPro.CogPolygon Region = (Cognex.VisionPro.CogPolygon)this.Tool.Region;
            Region.Selected = false;

            Tool.Region = Region;

            Tool.Run();

            if (Tool.Results == null)
            {
                return false;
            }

            if (Tool.Results.GetBlobs().Count > 0)
            {
                return false;
            }

            //if (Singleblob == true) {
            //    if (Tool.Results.GetBlobs().Count > 1) {
            //        return false;
            //    }
            //}
            return true;
        }

        public bool Run(Cognex.VisionPro.CogImage8Grey image, Points Point, string SpaceName)
        {
            if (InputImage(image) == false)
            {
                return false;
            }

            Cognex.VisionPro.CogCircularAnnulusSection Region = (Cognex.VisionPro.CogCircularAnnulusSection)this.Tool.Region;

            Region.Selected = false;

            Region.CenterX = Point.X;
            Region.CenterY = Point.Y;

            Threshold(Point.Threshold);

            Tool.Region.SelectedSpaceName = SpaceName;
            Tool.Region = Region;

            Tool.Run();

            if (Tool.Results == null)
            {
                return false;
            }

            if (Tool.Results.GetBlobs().Count <= 0)
            {
                return false;
            }

            //if (Singleblob == true) {
            //    if (Tool.Results.GetBlobs().Count > 1) {
            //        return false;
            //    }
            //}
            return true;
        }

        public void AreatoPoint(Cogs.Points Point)
        {
            Cognex.VisionPro.CogCircularAnnulusSection Region = (Cognex.VisionPro.CogCircularAnnulusSection)this.Tool.Region;

            Region.CenterX = Point.X;
            Region.CenterY = Point.Y;

            this.Tool.Region = Region;
        }

        public void GetnowPoint(ref Points point)
        {
            Cognex.VisionPro.CogCircularAnnulusSection Region = (Cognex.VisionPro.CogCircularAnnulusSection)this.Tool.Region;

            point.X = Region.CenterX;
            point.Y = Region.CenterY;

        }

        public void Area_FixturedPoint(ref Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogImage8Grey Image, string ImageSpace, Points Point)
        {
            if (this.InputImage(Image) == true)
            {

                Cognex.VisionPro.CogCircularAnnulusSection Region;

                if (this.Tool.Region == null)
                {
                    Region = new Cognex.VisionPro.CogCircularAnnulusSection();
                }
                else
                {
                    Region = (Cognex.VisionPro.CogCircularAnnulusSection)this.Tool.Region;
                }

                Region.CenterX = Point.X;
                Region.CenterY = Point.Y;

                this.Tool.Region.SelectedSpaceName = ImageSpace;
                this.Tool.Region = Region;

                display.InteractiveGraphics.Add((Cognex.VisionPro.ICogGraphicInteractive)Tool.Region, null, false);
            }
        }

        /// <summary>
        /// 검사 영역을 화면에 표시
        /// </summary>
        /// <param name="display">표시 대상 디스플레이</param>
        /// <param name="image">대상 이미지</param>
        public void Area_Affine(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            area.LineWidthInScreenPixels = 1;
            Tool.Region = area;

            display.InteractiveGraphics.Add((ICogGraphicInteractive)Tool.Region, null, false);
        }

        public void Area_Eclips(ref Cognex.VisionPro.Display.CogDisplay display, Cognex.VisionPro.CogImage8Grey image)
        {
            if (InputImage(image) == false)
            {
                return;
            }

            display.InteractiveGraphics.Add((Cognex.VisionPro.ICogGraphicInteractive)Tool.Region, null, false);
        }

        /// <summary>
        /// 설정 된 임계치를 읽어옴.
        /// </summary>
        /// <returns></returns>
        public int Threshold()
        {
            return Tool.RunParams.SegmentationParams.HardFixedThreshold;
        }
        public int PointNumber()
        {
            Cognex.VisionPro.CogPolygon Region = (Cognex.VisionPro.CogPolygon)this.Tool.Region;
            return Region.NumVertices;
        }
        /// <summary>
        /// 임계치를 설정 함.
        /// </summary>
        /// <param name="threshold">설정 할 임계치. 0 ~ 255</param>
        public void Threshold(int threshold)
        {
            Tool.RunParams.SegmentationParams.HardFixedThreshold = threshold;
        }

        /// <summary>
        /// 최소 픽셀 넓이 제한을 읽어옴.
        /// </summary>
        /// <returns></returns>
        public int Minipixel()
        {
            return Tool.RunParams.ConnectivityMinPixels;
        }

        /// <summary>
        /// 최소 픽셀 넓이 제한 설정
        /// </summary>
        /// <param name="minipixel"></param>
        public void Minipixel(int minipixel)
        {
            Tool.RunParams.ConnectivityMinPixels = minipixel;
        }
        /// <summary>
        /// 검사 극성 설정 읽어 옴.
        /// </summary>
        /// <returns>검사 극성. 0 : 밝은 블롭   1 : 어두운 블롭</returns>
        public int Polarity()
        {
            switch (Tool.RunParams.SegmentationParams.Polarity)
            {
                case Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs:
                    return 0;
                case Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.DarkBlobs:
                    return 1;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// 검사 극성 설정
        /// </summary>
        /// <param name="polarty">검사 극성. 0 : 밝은 블롭   1 : 어두운 블롭</param>
        public void Polarity(int polarty)
        {
            switch (polarty)
            {
                case 0:
                    Tool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs;
                    break;

                case 1:
                    Tool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.DarkBlobs;
                    break;
            }
        }
        public void AreaPointNumber(int pointnumber)
        {
            Cognex.VisionPro.CogPolygon Region = (Cognex.VisionPro.CogPolygon)this.Tool.Region;
            Region.NumVertices = pointnumber;
            Region.SetVertexX(pointnumber - 1, (Region.GetVertexX(pointnumber - 1) + Region.GetVertexX(pointnumber - 2)) / 2);
            Region.SetVertexY(pointnumber - 1, (Region.GetVertexY(pointnumber - 1) + Region.GetVertexY(pointnumber - 2)) / 2);
            this.Tool.Region = Region;
            //return Tool.Region.
        }
        public double ResultPixelArea()
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            if (Tool.Results.GetBlobs().Count < 1)
            {
                return 0;
            }

            return Tool.Results.GetBlobs()[0].Area;
        }


        /// <summary>
        /// 검사 결과를 화면에 표시 용.
        /// </summary>
        /// <param name="display">검사 결과 표시 대상 디스플레이</param>
        public void ResultDisplay(ref Cognex.VisionPro.Display.CogDisplay display, ref Cognex.VisionPro.CogGraphicCollection Collection, bool isOK = true)
        {
            Cognex.VisionPro.CogColorConstants Resultcolor = new Cognex.VisionPro.CogColorConstants();
            Cognex.VisionPro.Dimensioning.CogCreateGraphicLabelTool Nameing = new Cognex.VisionPro.Dimensioning.CogCreateGraphicLabelTool();
            System.Drawing.Font Fonts = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15);

            Resultcolor = Cognex.VisionPro.CogColorConstants.Green;

            if (this.Tool.Results == null)
            {
                return;
            }

            if (this.Singleblob == true)
            {
                if (this.Tool.Results.GetBlobs().Count < 1)
                {
                    isOK = false;
                }
            }

            if (isOK == false)
            {
                Resultcolor = Cognex.VisionPro.CogColorConstants.Red;
            }

            Cognex.VisionPro.CogRectangleAffine ResultRegion;

            ResultRegion = (Cognex.VisionPro.CogRectangleAffine)Tool.Region;

            ResultRegion.Interactive = false;

            ResultRegion.Color = Resultcolor;
            ResultRegion.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.None;

            ResultRegion.LineWidthInScreenPixels = 5;
            ResultRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;

            display.InteractiveGraphics.Add(ResultRegion, null, false);

            Nameing.InputImage = Tool.InputImage;
            Nameing.InputGraphicLabel.Text = Tool.Name;

            Nameing.InputGraphicLabel.X = ResultRegion.CenterX;
            Nameing.InputGraphicLabel.Y = ResultRegion.CenterY - (ResultRegion.SideYLength / 4);

            Nameing.InputGraphicLabel.Font = Fonts;
            Nameing.OutputColor = Resultcolor;

            Nameing.Run();

            Collection.Add(ResultRegion);

            display.InteractiveGraphics.Add(Nameing.GetOutputGraphicLabel(), null, false);
        }

        public void ResultDisplay(ref Cognex.VisionPro.CogGraphicCollection Collection, bool isOK)
        {
            Cognex.VisionPro.CogColorConstants Resultcolor = new Cognex.VisionPro.CogColorConstants();

            System.Drawing.Font Fonts = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15);

            Resultcolor = Cognex.VisionPro.CogColorConstants.Green;

            Resultcolor = isOK == false ? Cognex.VisionPro.CogColorConstants.Red : Cognex.VisionPro.CogColorConstants.Green;
            //if (isOK == false)
            //{
            //    Resultcolor = Cognex.VisionPro.CogColorConstants.Red;
            //}
            Cognex.VisionPro.CogPolygon ResultRegion;

            ResultRegion = (Cognex.VisionPro.CogPolygon)Tool.Region;

            ResultRegion.Color = Resultcolor;
            ResultRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.None;

            ResultRegion.LineWidthInScreenPixels = 5;
            ResultRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;

            Collection.Add(ResultRegion);
        }

        public void ResultDisplay_Location(ref Cognex.VisionPro.Display.CogDisplay display, string Imagespace)
        {
            if (this.Tool.Results == null)
            {
                return;
            }

            if (this.Tool.Results.GetBlobs().Count < 1)
            {
                return;
            }

            Cognex.VisionPro.CogCircularAnnulusSection Result = new Cognex.VisionPro.CogCircularAnnulusSection();
            Result.CenterX = this.Tool.Results.GetBlobs()[0].CenterOfMassX;
            Result.CenterY = this.Tool.Results.GetBlobs()[0].CenterOfMassY;
            Result.LineWidthInScreenPixels = 10;
            Result.Color = Cognex.VisionPro.CogColorConstants.Green;

            display.InteractiveGraphics.Add(Result, null, false);
        }

        public void ResultDisplay_Location(ref Cognex.VisionPro.CogGraphicCollection Collecter, Points Point)
        {
            bool Result = true;
            if (this.Tool.Results == null)
            {
                Result = false;
            }

            if (this.Tool.Results.GetBlobs().Count < 1)
            {
                Result = false;
            }

            Cognex.VisionPro.CogCircularAnnulusSection Resultarea = new Cognex.VisionPro.CogCircularAnnulusSection();
            Resultarea.CenterX = this.Tool.Results.GetBlobs()[0].CenterOfMassX;
            Resultarea.CenterY = this.Tool.Results.GetBlobs()[0].CenterOfMassY;
            Resultarea.LineWidthInScreenPixels = 5;
            if (Result == true)
            {
                Resultarea.Color = Cognex.VisionPro.CogColorConstants.Green;
            }
            else
            {
                Resultarea.Color = Cognex.VisionPro.CogColorConstants.Red;
            }

            Collecter.Add(Resultarea);
        }

        public Cognex.VisionPro.ICogGraphic ResultNameDisplay(bool isOK = true)
        {
            Cognex.VisionPro.Dimensioning.CogCreateGraphicLabelTool Nameing = new Cognex.VisionPro.Dimensioning.CogCreateGraphicLabelTool();
            Cognex.VisionPro.CogColorConstants Resultcolor = new Cognex.VisionPro.CogColorConstants();
            System.Drawing.Font Fonts = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15);
            Resultcolor = Cognex.VisionPro.CogColorConstants.Green;

            if (isOK == false)
            {
                Resultcolor = Cognex.VisionPro.CogColorConstants.Red;
            }

            Cognex.VisionPro.CogRectangleAffine ResultRegion;

            ResultRegion = (Cognex.VisionPro.CogRectangleAffine)Tool.Region;

            ResultRegion.Color = Resultcolor;
            ResultRegion.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.None;

            ResultRegion.LineWidthInScreenPixels = 5;
            ResultRegion.LineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Solid;

            Nameing.InputImage = Tool.InputImage;
            Nameing.InputGraphicLabel.Text = Tool.Name;

            Nameing.InputGraphicLabel.X = ResultRegion.CenterX;
            Nameing.InputGraphicLabel.Y = ResultRegion.CenterY - (ResultRegion.SideYLength / 4);

            Nameing.InputGraphicLabel.Font = Fonts;
            Nameing.OutputColor = Resultcolor;

            Nameing.Run();

            return Nameing.GetOutputGraphicLabel();
        }

        /// <summary>
        /// 블롭툴의 결과 수를 받아 옴.
        /// </summary>
        /// <returns>작동 한 툴의 결과 블롭 수.</returns>
        public int ResultBlobCount()
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            return Tool.Results.GetBlobs().Count;
        }


        public void ResultBlobDisplay(ref Cognex.VisionPro.CogGraphicCollection Collection)
        {
            if (this.Tool.Results == null)
            {
                return;
            }

            int Count = this.Tool.Results.GetBlobs().Count;
            if (Count < 1)
            {
                return;
            }
            Collection.Add(this.Tool.Results.GetBlobs()[0].CreateResultGraphics(Cognex.VisionPro.Blob.CogBlobResultGraphicConstants.All));
        }

        public void ResultAllBlobDisplay(ref Cognex.VisionPro.CogGraphicCollection Collection)
        {
            if (Tool.Results == null)
            {
                return;
            }

            int Count = this.Tool.Results.GetBlobs().Count;
            if (Count < 1)
            {
                return;
            }

            Collection.Add(Tool.Results.GetBlobs()[0].CreateResultGraphics(Cognex.VisionPro.Blob.CogBlobResultGraphicConstants.All));
        }

        public void ResultAllBlobDisplayPLT(Cognex.VisionPro.CogGraphicCollection Collection, bool result)
        {
            if (Tool.Results == null)
            {
                return;
            }

            int Count = Tool.Results.GetBlobs(true).Count;
            if (result)
            {
                CogPolygon OKResultRegion;
                CogPolygon OKResultRegion2;
                if (Count > 0)
                {
                    for (int lop = 0; lop < Count; lop++)
                    {
                        OKResultRegion2 = Tool.Results.GetBlobs()[lop].GetBoundary();
                        OKResultRegion2.Color = CogColorConstants.Green;
                        OKResultRegion2.LineWidthInScreenPixels = 4;
                        Collection.Add(OKResultRegion2);
                    }
                }
                OKResultRegion = (CogPolygon)Tool.Region;
                OKResultRegion.Color = CogColorConstants.Yellow;
                OKResultRegion.LineWidthInScreenPixels = 5;
                Collection.Add(OKResultRegion);
                return;
            }

            Count--;
            CogPolygon ResultRegion;

            for (int lop = 0; lop <= Count; lop++)
            {
                ResultRegion = Tool.Results.GetBlobs()[lop].GetBoundary();
                ResultRegion.Color = result == false ? CogColorConstants.Red : CogColorConstants.Green;
                Collection.Add(ResultRegion);
            }
        }

        public void ResultNGInspPointDisplay(ref Cognex.VisionPro.CogGraphicCollection Collection, Cognex.VisionPro.CogImage8Grey image, string Selectimage, Points Point)
        {
            Cognex.VisionPro.Dimensioning.CogCreateCircleTool Result = new Cognex.VisionPro.Dimensioning.CogCreateCircleTool();

            Result.InputImage = image;
            Result.InputImage.SelectedSpaceName = Selectimage;
            Result.InputCircle.CenterX = Point.X;
            Result.InputCircle.CenterY = Point.Y;
            Result.InputCircle.Radius = 40;
            Result.OutputLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dash;
            Result.OutputColor = Cognex.VisionPro.CogColorConstants.Red;
            Result.OutputLineWidthInScreenPixels = 5;

            Result.Run();

            Collection.Add(Result.GetOutputCircle());

        }

        public void ResultNGInspPointDisplay_2(ref Cognex.VisionPro.CogGraphicCollection Collection, Cognex.VisionPro.CogImage8Grey image, string Selectimage, Points Point)
        {
            Cognex.VisionPro.Dimensioning.CogCreateCircleTool Result = new Cognex.VisionPro.Dimensioning.CogCreateCircleTool();

            Result.InputImage = image;
            Result.InputImage.SelectedSpaceName = Selectimage;
            Result.InputCircle.CenterX = Point.X;
            Result.InputCircle.CenterY = Point.Y;
            Result.InputCircle.Radius = 40;
            Result.OutputLineStyle = Cognex.VisionPro.CogGraphicLineStyleConstants.Dash;
            Result.OutputColor = Cognex.VisionPro.CogColorConstants.Red;
            Result.OutputLineWidthInScreenPixels = 5;

            Result.Run();

            Collection.Add(Result.GetOutputCircle());

        }
        public void Area_Affine_Main1(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public void Area_Affine_Main2(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public void Area_Affine_Main3(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public void Area_Affine_Main4(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public void Area_Affine_Main5(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public void Area_Affine_Main6(ref CogDisplay display, CogImage8Grey image, string ImageSpace)
        {
            ImageSpace = $"MultiPattern - {ImageSpace}";
            if (InputImage(image) == false)
            {
                return;
            }

            if (this.Tool.Region == null)
            {
                this.NewTool();
            }

            CogPolygon area = (CogPolygon)Tool.Region; //영역설정 CogRectangleAffine에서 CogPolygon으로 변경함 - 191230
            area.Interactive = true;
            area.GraphicDOFEnable = CogPolygonDOFConstants.All;
            area.SelectedSpaceName = ImageSpace;
            area.Color = CogColorConstants.Green;
            Tool.Region = area;
        }
        public double ResultAllBlobArea()
        {
            if (Tool.Results == null)
            {
                return 0;
            }

            int Count;

            if (Tool.RunParams.RunTimeMeasures.Count < 0)
            {
                Count = Tool.Results.GetBlobs(true).Count;
            }
            else
            {
                Count = Tool.Results.GetBlobs().Count;
            }

            double Result = 0;
            if (Count < 1)
            {
                return Result;
            }
            Count--;
            for (int lop = 0; lop <= Count; lop++)
            {
                Result += Tool.Results.GetBlobs()[lop].Area;
            }

            return Result;
        }
        public int SpaceName()
        {
            int Toolnumber;
            string spacename = Tool.Region.SelectedSpaceName;
            if (spacename == "@" || spacename == ".")
            {
                return 0;
            }
            string[] SplitSpacename = spacename.Split('-');
            Toolnumber = Convert.ToInt32(SplitSpacename[1]);

            return Toolnumber;
        }
        public ResultPoints[] ResultAllPoint()
        {
            ResultPoints[] Errresult = new ResultPoints[1];
            Errresult[0].X = 0.0;
            Errresult[0].Y = 0.0;

            if (Tool.Results.GetBlobs().Count <= 0)
            {
                return Errresult;
            }

            ResultPoints[] Result = new ResultPoints[Tool.Results.GetBlobs().Count];

            for (int lop = 0; lop <= Result.Length - 1; lop++)
            {
                Result[lop].X = Tool.Results.GetBlobs()[lop].CenterOfMassX;
                Result[lop].Y = Tool.Results.GetBlobs()[lop].CenterOfMassY;
            }

            return Result;
        }

        public double Result_MessCenter_X()
        {
            double Result = 0.0;

            if (this.Tool.Results == null)
            {
                return Result;
            }


            int Count = Tool.Results.GetBlobs().Count;
            if (Count < 1)
            {
                return Result;
            }

            Result = Tool.Results.GetBlobs()[0].CenterOfMassX;

            return Result;
        }

        public double Result_MessCenter_Y()
        {
            double Result = 0.0;
            int Count = Tool.Results.GetBlobs().Count;
            if (Count < 1)
            {
                return Result;
            }
            Result = Tool.Results.GetBlobs()[0].CenterOfMassY;

            return Result;
        }

        /// <summary>
        /// 검사 툴 전체 셋업 화면을 화면에 표시
        /// </summary>
        public void ToolSetup()
        {
            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            Cognex.VisionPro.Blob.CogBlobEditV2 Edit = new Cognex.VisionPro.Blob.CogBlobEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.Show(); // 폼 실행
        }
    }

    public struct ResultPoints
    {
        public double X;
        public double Y;
    }
}

