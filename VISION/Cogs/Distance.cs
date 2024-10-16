using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VISION.Cogs
{
    public class Distance
    {
        private Cognex.VisionPro.Dimensioning.CogDistancePointLineTool Tool;

        public Distance(int Toolnumber)
        {
            Tool = new Cognex.VisionPro.Dimensioning.CogDistancePointLineTool();

            Tool.Name = "Distance - " + Toolnumber.ToString();

        }

        public string ToolName(int toolnum)
        {

            return Tool.Name;
        }

        private bool NewTool()
        {
            return true;
        }

        /// <summary>
        /// 파일에서 툴을 불러 옴. 파일이 있는 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">파일이 있는  폴더의 경로</param>
        /// <returns></returns>
        public bool Loadtool(string path, int num)
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


                Tool = (Cognex.VisionPro.Dimensioning.CogDistancePointLineTool)CogSerializer.LoadObjectFromFile(Savepath);

                return true;
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee);
                return false;
            }
           
        }

        /// <summary>
        /// 파일에 툴의 정보를 씀. 대상 파일이 위치 할 폴더의 경로만 제공 하면 됨.
        /// </summary>
        /// <param name="path">저장 할 대상 폴더의 경로</param>
        /// <returns></returns>
        public bool Savetool(string path, int num)
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

        /// <summary>
        /// 툴에 이미지 입력
        /// </summary>
        /// <param name="image">툴에 입력 할 이미지</param>
        /// <returns></returns>
        public bool InputImage(int toolnum, CogImage8Grey image)
        {
            if (image == null)
            {
                return false;
            }

            Tool.InputImage = image;
            return true;
        }
        public void ResultDisplay(int toolnum, Cognex.VisionPro.Display.CogDisplay display, CogGraphicCollection Collection)
        {
            CogLineSegment segment;
            try
            {

                segment = (CogLineSegment)Tool.CreateLastRunRecord().SubRecords["InputImage"].SubRecords["Arrow"].Content;


                Collection.Add(segment);
                display.StaticGraphics.AddList(Collection, "");
            }
            catch
            {
                return;
            }
        }
        public double DistanceValue(int toolnum)
        {
            try
            {

                return Tool.Distance;
            }
            catch
            {
                return 0;
            }

        }
        public bool Run(int toolnum, CogImage8Grey image)
        {
            if (!InputImage(toolnum, image))
            {
                return false;
            }

            Tool.Run();

            if (Tool.Line == null)
            {
                return false;
            }

            return true;
        }

        public bool InputLine(int toolnum, CogLine Line)
        {

            Tool.Line = Line;
            return true;
        }
        public bool InputCircle(CogEllipse Circle)
        {
            if (Circle == null) return false;

            Tool.X = Circle.CenterX;
            Tool.Y = Circle.CenterY;

            return true;
        }
        public bool InputXY(double PointX, double PointY)
        {
            Tool.X = PointX;
            Tool.Y = PointY;
            return true;
        }
        public double GetX(int toolnum)
        {
            return Tool.X;
        }
        public double GetY(int toolnum)
        {
            return Tool.Y;

        }
        /// <summary>
        /// 검사 툴 전체 셋업 화면을 화면에 표시
        /// </summary>
        public void ToolSetup(int toolnum)
        {

            System.Windows.Forms.Form Window = new System.Windows.Forms.Form();
            Cognex.VisionPro.Dimensioning.CogDistancePointLineEditV2 Edit = new Cognex.VisionPro.Dimensioning.CogDistancePointLineEditV2();

            Edit.Dock = System.Windows.Forms.DockStyle.Fill; // 화면 채움
            Edit.Subject = Tool; // 에디트에 툴 정보 입력.
            Window.Controls.Add(Edit); // 폼에 에디트 추가.

            Window.Width = 800;
            Window.Height = 600;

            Window.Show(); // 폼 실행

        }
    }
}

