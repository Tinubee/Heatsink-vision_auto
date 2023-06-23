using Cognex.VisionPro;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace VISION.Cogs
{
    public class Model
    {
        private PGgloble Glob = PGgloble.getInstance;
        // 모델 기초 자료
        private string Name; // 모델 명
        private int Number; // 모델 번호

        public const int BLOBTOOLMAX = 30;
        public const int LINETOOLMAX = 30;
        public const int OCRTOOLMAX = 30;
        public const int MULTIPATTERNMAX = 30;
        public const int DISTANCEMAX = 10;
        public const int CALIPERMAX = 10;
        public const int CIRCLETOOLMAX = 10;
        //Program.CameraList = CamList.LoadCamInfo();

        private Camera[] Camera = new Camera[Program.CameraList.Count()];
        private Mask[] Masks = new Mask[Program.CameraList.Count()];

        private Line[,] Lines = new Line[Program.CameraList.Count(), LINETOOLMAX];
        private bool[,] LineEnable = new bool[Program.CameraList.Count(), LINETOOLMAX];

        private Blob[,] Blobs = new Blob[Program.CameraList.Count(), BLOBTOOLMAX];
        private bool[,] BlobEnable = new bool[Program.CameraList.Count(), BLOBTOOLMAX];
        private bool[,] BlobNGOKChange = new bool[Program.CameraList.Count(), BLOBTOOLMAX];
        private int[,] BlobOKCount = new int[Program.CameraList.Count(), BLOBTOOLMAX];
        private int[,] BlobFixPatternNumber = new int[Program.CameraList.Count(), BLOBTOOLMAX];

        private MultiPMAlign[,] MultiPattern = new MultiPMAlign[Program.CameraList.Count(), MULTIPATTERNMAX];
        private bool[,] MultiPatternEnable = new bool[Program.CameraList.Count(), MULTIPATTERNMAX];
        private int[,] MultiPatternOrderNumber = new int[Program.CameraList.Count(), MULTIPATTERNMAX];

        private Circle[,] Circles = new Circle[Program.CameraList.Count(), CIRCLETOOLMAX];
        private bool[,] CircleEnable = new bool[Program.CameraList.Count(), CIRCLETOOLMAX];

        private Distance[,] Distances = new Distance[Program.CameraList.Count(), DISTANCEMAX];
        private bool[,] DistanceEnable = new bool[Program.CameraList.Count(), DISTANCEMAX];
        private string[,] Distance_UseTool1_Number = new string[Program.CameraList.Count(), DISTANCEMAX];
        private string[,] Distance_UseTool2_Number = new string[Program.CameraList.Count(), DISTANCEMAX];

        private double[,] Distance_CalibrationValue = new double[Program.CameraList.Count(), DISTANCEMAX];
        private double[,] Distance_LowValue = new double[Program.CameraList.Count(), DISTANCEMAX];
        private double[,] Distance_HighValue = new double[Program.CameraList.Count(), DISTANCEMAX];

        private Caliper[,] Calipers = new Caliper[Program.CameraList.Count(), CALIPERMAX];
        private bool[,] CaliperEnable = new bool[Program.CameraList.Count(), CALIPERMAX];

        public Model()
        { // 초기화
            Debug.WriteLine("Cognex Model 초기화 시작");
            int CircleMax = CIRCLETOOLMAX - 1;
            int BlobMax = BLOBTOOLMAX - 1;
            int LineMax = LINETOOLMAX - 1;
            int MultiPatternMax = MULTIPATTERNMAX - 1;
            int DistanceMax = DISTANCEMAX - 1;
            int CalipersMax = CALIPERMAX - 1;

            for (int lop = 0; lop < 6; lop++) //Glob.allCameraCount 에서 6으로 임시 변경.
            {
                Camera[lop] = new Camera(lop);
                Debug.WriteLine($"Camera{lop + 1} 초기화 완료.");

                Masks[lop] = new Mask(lop);
                Debug.WriteLine($"Mask{lop + 1} 초기화 완료.");
            }


            for (int i = 0; i < Program.CameraList.Count(); i++)
            {
                for (int lop = 0; lop <= MultiPatternMax; lop++)
                {
                    MultiPattern[i, lop] = new MultiPMAlign(lop);
                }
                for (int lop = 0; lop <= BlobMax; lop++)
                {
                    Blobs[i, lop] = new Blob(lop);
                }
                for (int lop = 0; lop <= LineMax; lop++)
                {
                    Lines[i, lop] = new Line(lop);
                }
                for (int lop = 0; lop < CircleMax; lop++)
                {
                    Circles[i, lop] = new Circle(lop);
                }
                for (int lop = 0; lop < DistanceMax; lop++)
                {
                    Distances[i, lop] = new Distance(lop);
                }
                for (int lop = 0; lop <= CalipersMax; lop++)
                {
                    Calipers[i, lop] = new Caliper(lop);
                }
            }
        }
        /// <summary>
        /// 모델 이름으로 모델 교체
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ModelRoot"></param>
        /// <returns></returns>
        public bool Loadmodel(string Name, string ModelRoot, int camnumber, bool isFirst = false)
        {
            INIControl Modellist;
            INIControl CFGFILE;

            PGgloble global = PGgloble.getInstance;

            Modellist = new INIControl(global.MODELLIST);
            CFGFILE = new INIControl(global.CONFIGFILE);

            string CHKName;
            int CHKNumber;

            CHKNumber = int.Parse(Modellist.ReadData("NAME", Name, true));
            CHKName = Modellist.ReadData("NUMBER", CHKNumber.ToString(), false);

            if (Directory.Exists(ModelRoot) == false)
            {
                return false;
            }

            ModelRoot += "\\" + CHKName + $"\\Cam{camnumber}"; //각 Camera별 Vision Tool .vpp 파일 경로.

            if (Directory.Exists(ModelRoot) == false)
            {
                Directory.CreateDirectory(ModelRoot);
                //eturn false;
            }

            if (Read(ModelRoot, camnumber) == false)
            {
                if (isFirst == false)
                {
                    return false;
                }
            }

            this.Name = CHKName;
            Number = CHKNumber;

            CFGFILE.WriteData("LASTMODEL", "NAME", this.Name);
            CFGFILE.WriteData("LASTMODEL", "NUMBER", Number.ToString());
            return true;
        }

        /// <summary>
        /// 검사 모델 파일 불러오기
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns></returns>
        private bool Read(string path, int cam)
        {
            GC.Collect();
            // 실제 모델 전환하는 메소드
            PGgloble glos = PGgloble.getInstance;
            INIControl Modelcfg = new INIControl(path + glos.MODELCONFIGFILE);

            int CircleMax = CIRCLETOOLMAX - 1;
            int BlobMax = BLOBTOOLMAX - 1;
            int LineMax = LINETOOLMAX - 1;
            int CalipersMax = CALIPERMAX - 1;
            int MultiPatternMax = MULTIPATTERNMAX - 1;
            int DistanceMax = DISTANCEMAX - 1;

            Camera[cam].Loadtool(path + $"\\cam - {cam.ToString()}.vpp");
            Masks[cam].Loadtool(path + $"\\Mask - {cam.ToString()}.vpp");

            for (int lop = 0; lop <= MultiPatternMax; lop++)
            {
                MultiPattern[cam, lop].LoadTool(path);
                MultiPatternEnable[cam, lop] = Modelcfg.ReadData("MULTI PATTERN - " + lop.ToString(), "Enable") == "1" ? true : false;
                if (Modelcfg.ReadData("MULTI PATTERN - " + lop.ToString(), "Order") == "")
                {
                    Modelcfg.WriteData("MULTI PATTERN - " + lop.ToString(), "Order", "1");
                }
                MultiPatternOrderNumber[cam, lop] = Convert.ToInt16(Modelcfg.ReadData("MULTI PATTERN - " + lop.ToString(), "Order"));
            }
            for (int lop = 0; lop <= BlobMax; lop++)
            {
                Blobs[cam, lop].Loadtool(path);
                BlobEnable[cam, lop] = Modelcfg.ReadData("BLOB - " + lop.ToString(), "Enable") == "1" ? true : false;
                if (Modelcfg.ReadData("BLOB - " + lop.ToString(), "NGOKChange") == "")
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "NGOKChange", "0");
                if (Modelcfg.ReadData("BLOB - " + lop.ToString(), "OKCount") == "")
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "OKCount", BlobOKCount[cam, lop].ToString());
                if (Modelcfg.ReadData("BLOB - " + lop.ToString(), "BlobFixPatternNumber") == "")
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "BlobFixPatternNumber", lop.ToString());

                BlobNGOKChange[cam, lop] = Modelcfg.ReadData("BLOB - " + lop.ToString(), "NGOKChange") == "1" ? true : false;
                BlobOKCount[cam, lop] = Convert.ToInt32(Modelcfg.ReadData("BLOB - " + lop.ToString(), "OKCount"));
                BlobFixPatternNumber[cam, lop] = Convert.ToInt32(Modelcfg.ReadData("BLOB - " + lop.ToString(), "BlobFixPatternNumber"));
            }
            for (int lop = 0; lop <= LineMax; lop++)
            {
                Lines[cam, lop].Loadtool(path);
                LineEnable[cam, lop] = Modelcfg.ReadData("LINE - " + lop.ToString(), "Enable") == "1" ? true : false;
            }
            for (int lop = 0; lop < CircleMax; lop++)
            {
                Circles[cam, lop].Loadtool(path);
                CircleEnable[cam, lop] = Modelcfg.ReadData("CIRCLE - " + lop.ToString(), "Enable") == "1" ? true : false;
            }
            for (int lop = 0; lop < DistanceMax; lop++)
            {
                Distances[cam, lop].Loadtool(path, lop);
                DistanceEnable[cam, lop] = Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Enable") == "1" ? true : false;
                if (Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_UseTool1_Number") == "")
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_UseTool1_Number", $"LINE + {lop.ToString()}");
                if (Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_UseTool2_Number") == "")
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_UseTool2_Number", $"LINE + {lop.ToString()}");
                if (Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_CalibrationValue") == "")
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_CalibrationValue", "1");
                if (Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_LowValue") == "")
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_LowValue", "1");
                if (Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_HighValue") == "")
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_HighValue", "1");

                Distance_UseTool1_Number[cam, lop] = Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_UseTool1_Number");
                Distance_UseTool2_Number[cam, lop] = Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_UseTool2_Number");

                Distance_CalibrationValue[cam, lop] = Convert.ToDouble(Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_CalibrationValue"));
                Distance_LowValue[cam, lop] = Convert.ToDouble(Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_LowValue"));
                Distance_HighValue[cam, lop] = Convert.ToDouble(Modelcfg.ReadData("DISTANCE - " + lop.ToString(), "Distance_HighValue"));
            }

            for (int lop = 0; lop < CalipersMax; lop++)
            {
                Calipers[cam, lop].Loadtool(path);
                CaliperEnable[cam, lop] = Modelcfg.ReadData("CALIPER - " + lop.ToString(), "Enable") == "1" ? true : false;
            }

            return true;
        }

        public Cognex.VisionPro.CogImage8Grey Imageconvert(Cognex.VisionPro.CogImage24PlanarColor image)
        {
            if (image == null)
            {
                return null;
            }

            Cognex.VisionPro.ImageProcessing.CogImageConvertTool Tool = new Cognex.VisionPro.ImageProcessing.CogImageConvertTool();
            Tool.RunParams.RunMode = Cognex.VisionPro.ImageProcessing.CogImageConvertRunModeConstants.Plane0;

            Tool.InputImage = image;
            Tool.Run();
            return (Cognex.VisionPro.CogImage8Grey)Tool.OutputImage;
        }

        /// <summary>
        /// 현재 모델을 파일에 저장
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns></returns>
        public bool SaveModel(string path, int cam)
        {
            PGgloble glos = PGgloble.getInstance;
            INIControl Modelcfg = new INIControl(path + glos.MODELCONFIGFILE);

            int CircleMax = CIRCLETOOLMAX - 1;
            int BlobMax = BLOBTOOLMAX - 1;
            int LineMax = LINETOOLMAX - 1;
            int MultiPatternMax = MULTIPATTERNMAX - 1;
            int DistanceMax = DISTANCEMAX - 1;
            int CalipersMax = CALIPERMAX - 1;

            Camera[cam].SaveTool(path , $"cam - {cam.ToString()}.vpp");
            Masks[cam].SaveTool(path, $"Mask - {cam.ToString()}.vpp");

            for (int lop = 0; lop <= MultiPatternMax; lop++)
            {
                MultiPattern[cam, lop].SaveTool(path);
                if (MultiPatternEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("MULTI PATTERN - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("MULTI PATTERN - " + lop.ToString(), "Enable", "0");
                }
                Modelcfg.WriteData("MULTI PATTERN - " + lop.ToString(), "Order", MultiPatternOrderNumber[cam, lop].ToString());
            }

            for (int lop = 0; lop <= BlobMax; lop++)
            {
                Blobs[cam, lop].Savetool(path);
                if (BlobEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "Enable", "0");
                }
                if (BlobNGOKChange[cam, lop] == true)
                {
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "NGOKChange", "1");
                }
                else
                {
                    Modelcfg.WriteData("BLOB - " + lop.ToString(), "NGOKChange", "0");
                }
                Modelcfg.WriteData("BLOB - " + lop.ToString(), "OKCount", BlobOKCount[cam, lop].ToString());
                Modelcfg.WriteData("BLOB - " + lop.ToString(), "BlobFixPatternNumber", BlobFixPatternNumber[cam, lop].ToString());
            }
            for (int lop = 0; lop < CircleMax; lop++)
            {
                Circles[cam, lop].Savetool(path);
                if (CircleEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("CIRCLE - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("CIRCLE - " + lop.ToString(), "Enable", "0");
                }
            }
            for (int lop = 0; lop <= LineMax; lop++)
            {
                Lines[cam, lop].Savetool(path);
                if (LineEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("LINE - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("LINE - " + lop.ToString(), "Enable", "0");
                }
            }

            for (int lop = 0; lop < DistanceMax; lop++)
            {
                Distances[cam, lop].Savetool(path, lop);
                if (DistanceEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Enable", "0");
                }
                Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_UseTool1_Number", Distance_UseTool1_Number[cam, lop].ToString());
                Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_UseTool2_Number", Distance_UseTool2_Number[cam, lop].ToString());

                Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_CalibrationValue", Distance_CalibrationValue[cam, lop].ToString());
                Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_LowValue", Distance_LowValue[cam, lop].ToString());
                Modelcfg.WriteData("DISTANCE - " + lop.ToString(), "Distance_HighValue", Distance_HighValue[cam, lop].ToString());
            }

            for (int lop = 0; lop <= CalipersMax; lop++)
            {
                Calipers[cam, lop].Savetool(path);
                if (CaliperEnable[cam, lop] == true)
                {
                    Modelcfg.WriteData("CALIPER - " + lop.ToString(), "Enable", "1");
                }
                else
                {
                    Modelcfg.WriteData("CALIPER - " + lop.ToString(), "Enable", "0");
                }
            }

            return true;
        }
        public string Modelname()
        {
            // 현재 모델 명
            return Name;
        }

        public int ModelNumber()
        {
            // 현재 모델 번호
            return this.Number;
        }
        public MultiPMAlign[,] MultiPatterns()
        {
            return MultiPattern;
        }
        public void MultiPatterns(MultiPMAlign[,] multipatts)
        {
            MultiPattern = multipatts;
        }
        public bool[,] MultiPatternEnables()
        {
            return MultiPatternEnable;
        }

        public void MultiPatternEnables(bool[,] multipatternenable)
        {
            MultiPatternEnable = multipatternenable;
        }

        public int[,] MultiPatternOrderNumbers()
        {
            return MultiPatternOrderNumber;
        }

        public void MultiPatternOrderNumbers(int[,] multipatternordernumber)
        {
            MultiPatternOrderNumber = multipatternordernumber;
        }

        public Distance[,] Distancess()
        {
            return Distances;
        }
        public void Distancess(Distance[,] distances)
        {
            Distances = distances;
        }
        public bool[,] DistanceEnables()
        {
            return DistanceEnable;
        }

        public void DistanceEnables(bool[,] distanceenable)
        {
            DistanceEnable = distanceenable;
        }
        public Circle[,] Circle()
        {
            return Circles;
        }

        public void Circle(Circle[,] circle)
        {
            Circles = circle;
        }
        public bool[,] CircleEnables()
        {
            return CircleEnable;
        }

        public void CircleEnables(bool[,] circleenable)
        {
            CircleEnable = circleenable;
        }
        public Line[,] Line()
        {
            return Lines;
        }

        public Mask[] MaskTool()
        {
            return Masks;
        }

        public void MaskTools(Mask[] masktools)
        {
            Masks = masktools;
        }

        public Camera[] Cam()
        {
            return Camera;
        }

        public void Cams(Camera[] cameras)
        {
            Camera = cameras;
        }

        public Caliper[,] Calipes()
        {
            return Calipers;
        }

        public void Calipes(Caliper[,] calips)
        {
            this.Calipers = calips;
        }

        public void Line(Line[,] line)
        {
            Lines = line;
        }

        public bool[,] CaliperEnables()
        {
            return CaliperEnable;
        }

        public void CaliperEnables(bool[,] caliperenable)
        {
            CaliperEnable = caliperenable;
        }

        public bool[,] LineEnables()
        {
            return LineEnable;
        }

        public void LineEnables(bool[,] lineenable)
        {
            LineEnable = lineenable;
        }
        public Blob[,] Blob()
        {
            return Blobs;
        }
        public void Blob(Blob[,] blob)
        {
            Blobs = blob;
        }

        public bool[,] BlobEnables()
        {
            return BlobEnable;
        }

        public void BlobEnables(bool[,] blobenable)
        {
            BlobEnable = blobenable;
        }

        public bool[,] BlobNGOKChanges()
        {
            return BlobNGOKChange;
        }

        public void BlobNGOKChanges(bool[,] blobngokchange)
        {
            BlobNGOKChange = blobngokchange;
        }


        public int[,] BlobOKCounts()
        {
            return BlobOKCount;
        }

        public void BlobOKCounts(int[,] OKcount)
        {
            BlobOKCount = OKcount;
        }

        public int[,] BlobFixPatternNumbers()
        {
            return BlobFixPatternNumber;
        }

        public void BlobFixPatternNumbers(int[,] PatternNumber)
        {
            BlobFixPatternNumber = PatternNumber;
        }

        public string[,] Distance_UseTool1_Numbers()
        {
            return Distance_UseTool1_Number;
        }

        public void Distance_UseTool1_Numbers(string[,] DistanceUseTool1Number)
        {
            Distance_UseTool1_Number = DistanceUseTool1Number;
        }
        public string[,] Distance_UseTool2_Numbers()
        {
            return Distance_UseTool2_Number;
        }

        public void Distance_UseTool2_Numbers(string[,] DistanceUseTool2Number)
        {
            Distance_UseTool2_Number = DistanceUseTool2Number;
        }

        public double[,] Distance_CalibrationValues()
        {
            return Distance_CalibrationValue;
        }

        public void Distance_CalibrationValues(double[,] distance_calibrationvalues)
        {
            Distance_CalibrationValue = distance_calibrationvalues;
        }

        public double[,] Distance_LowValues()
        {
            return Distance_LowValue;
        }

        public void Distance_LowValues(double[,] distance_lowvalue)
        {
            Distance_LowValue = distance_lowvalue;
        }

        public double[,] Distance_HighValues()
        {
            return Distance_HighValue;
        }

        public void Distance_HighValues(double[,] distance_highvalue)
        {
            Distance_HighValue = distance_highvalue;
        }

        public bool MultiPattern_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection, int shotNumber)
        {
            try
            {
                bool Result = true;
                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int MultiPatternMax = MULTIPATTERNMAX - 1;

                Display.Image = Image;
                Display.InteractiveGraphics.Clear();
                Display.StaticGraphics.Clear();

                for (int lop = 0; lop <= MultiPatternMax; lop++)
                {
                    if (MultiPatternEnable[CamNumber, lop] == true && (MultiPatternOrderNumber[CamNumber, lop] == Glob.InspectOrder || MultiPatternOrderNumber[CamNumber, lop] == shotNumber))
                    {
                        MultiPattern[CamNumber, lop].Run(Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }
                //검사 툴 결과 확인.
                for (int lop = 0; lop <= MultiPatternMax; lop++)
                {
                    if (MultiPatternEnable[CamNumber, lop] == true && (MultiPatternOrderNumber[CamNumber, lop] == Glob.InspectOrder || MultiPatternOrderNumber[CamNumber, lop] == shotNumber))
                    {
                        MultiPattern[CamNumber, lop].ResultDisplay(Display, Collection, MultiPattern[CamNumber, lop].HighestResultToolNumber(), lop);
                        //SCORE 표시.
                        Glob.MultiInsPat_Result[CamNumber, lop] = MultiPattern[CamNumber, lop].ResultScore(MultiPattern[CamNumber, lop].HighestResultToolNumber());
                    }
                    else
                    {
                        //ltiPattern[CamNumber, lop].ResultNGDisplay(Display, Collection);
                        Glob.MultiInsPat_Result[CamNumber, lop] = 0;
                        //Result = false;
                    }
                }
                return Result;
            }
            catch
            {
                return false;
            }
        }
        public bool Blob_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection,int shotNumber)
        {
            try
            {
                bool Result = true;
                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int BlobMax = BLOBTOOLMAX - 1;
                // 검사 툴 작동
                for (int lop = 0; lop <= BlobMax; lop++)
                {
                    int patternIndex = BlobFixPatternNumber[CamNumber, lop];
                    //pattern 4 
                    if (BlobEnable[CamNumber, lop] == true && (MultiPatternOrderNumber[CamNumber, patternIndex] == Glob.InspectOrder || MultiPatternOrderNumber[CamNumber, patternIndex] == shotNumber))
                    {
                        Blobs[CamNumber, lop].Run(Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }
                for (int lop = 0; lop <= BlobMax; lop++)
                {
                    int patternIndex = BlobFixPatternNumber[CamNumber, lop];
                    if (MultiPatternOrderNumber[CamNumber, patternIndex] == Glob.InspectOrder || MultiPatternOrderNumber[CamNumber, patternIndex] == shotNumber)
                    {
                        if (BlobEnable[CamNumber, lop] == true)
                        {
                            if (Blobs[CamNumber, lop].ResultBlobCount() != BlobOKCount[CamNumber, lop]) // - 검사결과 NG
                            {
                                if (BlobNGOKChange[CamNumber, lop])
                                {
                                    Blobs[CamNumber, lop].ResultAllBlobDisplayPLT(Collection, true);
                                }
                                else
                                {
                                    Result = false;
                                    Blobs[CamNumber, lop].ResultAllBlobDisplayPLT(Collection, false);
                                    ResultString[lop] = "NG";
                                }
                            }
                            else // - 검사결과 OK
                            {
                                if (BlobNGOKChange[CamNumber, lop])
                                {
                                    Result = false;
                                    Blobs[CamNumber, lop].ResultAllBlobDisplayPLT(Collection, false);
                                    ResultString[lop] = "NG";
                                }
                                else
                                {
                                    Blobs[CamNumber, lop].ResultAllBlobDisplayPLT(Collection, true);
                                }
                                   
                            }
                        }
                    }
                }
                //}
                return Result;
            }
            catch (Exception)
            {
                //MessageBox.Show(ee.Message);
                return false;
            }
        }

        public bool Dimension_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection)
        {
            try
            {
                bool Result = true;
                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int DistanceMax = DISTANCEMAX - 1;

                Display.Image = Image;

                for (int lop = 1; lop <= DistanceMax; lop++)
                {
                    if (DistanceEnable[CamNumber, lop] == true)
                    {
                        string Tool1_Name = Distance_UseTool1_Number[CamNumber, lop];
                        string Tool2_Name = Distance_UseTool2_Number[CamNumber, lop];

                        string[] splitTool1Name = Tool1_Name.Split('-');
                        string[] splitTool2Name = Tool2_Name.Split('-');

                        //기준선 라인 툴 실행.
                        Lines[CamNumber, Convert.ToInt32(splitTool1Name[1])].Run(Image);
                        Lines[CamNumber, Convert.ToInt32(splitTool1Name[1])].ResultDisplay(Display, Collection);
                        Distances[CamNumber, lop].InputLine(lop, Lines[CamNumber, Convert.ToInt32(splitTool1Name[1])].GetLine());

                        //포인트 툴 실행.
                        switch (splitTool2Name[0])
                        {
                            case "Line ":
                                Lines[CamNumber, Convert.ToInt32(splitTool2Name[1])].Run(Image);
                                Lines[CamNumber, Convert.ToInt32(splitTool2Name[1])].ResultDisplay(Display, Collection);
                                Distances[CamNumber, lop].InputXY(Lines[CamNumber, Convert.ToInt32(splitTool2Name[1])].Average_PointX(), Lines[CamNumber, Convert.ToInt32(splitTool2Name[1])].Average_PointY());
                                break;
                            case "Caliper ":
                                Calipers[CamNumber, Convert.ToInt32(splitTool2Name[1])].Run(Image);
                                Calipers[CamNumber, Convert.ToInt32(splitTool2Name[1])].ResultAllDisplay(Collection);
                                Distances[CamNumber, lop].InputXY(Calipers[CamNumber, Convert.ToInt32(splitTool2Name[1])].Result_Corner_X(), Calipers[CamNumber, Convert.ToInt32(splitTool2Name[1])].Result_Corner_Y());
                                break;
                            case "Circle ":
                                Circles[CamNumber, Convert.ToInt32(splitTool2Name[1])].Run(Image);
                                Circles[CamNumber, Convert.ToInt32(splitTool2Name[1])].ResultAllDisplay(Collection);
                                Distances[CamNumber, lop].InputCircle(Circles[CamNumber, Convert.ToInt32(splitTool2Name[1])].GetCircle());
                                break;
                        }

                        Distances[CamNumber, lop].Run(lop, Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }
                for (int lop = 1; lop <= DistanceMax; lop++)
                {
                    if (DistanceEnable[CamNumber, lop] == true)
                    {
                        Distances[CamNumber, lop].ResultDisplay(lop, Display, Collection);
                    }
                }

                return Result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Line_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection)
        {
            try
            {
                bool Result = true;
                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int LineMax = LINETOOLMAX - 1;

                Display.Image = Image;

                for (int lop = 1; lop <= LineMax; lop++)
                {
                    if (LineEnable[CamNumber, lop] == true)
                    {
                        Lines[CamNumber, lop].Run(Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }
                for (int lop = 1; lop <= LineMax; lop++)
                {
                    if (LineEnable[CamNumber, lop] == true)
                    {
                        Lines[CamNumber, lop].ResultDisplay(Display, Collection);
                    }
                }

                return Result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Circle_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection)
        {
            try
            {
                bool Result = true;

                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int CircleMax = CIRCLETOOLMAX - 1;

                for (int lop = 1; lop <= CircleMax; lop++)
                {
                    if (CircleEnable[CamNumber, lop] == true)
                    {
                        Circles[CamNumber, lop].Run(Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }

                for (int lop = 1; lop <= CircleMax; lop++)
                {
                    if (CircleEnable[CamNumber, lop] == true)
                    {
                        Circles[CamNumber, lop].ResultAllDisplay(Collection);
                    }
                }
                return Result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Caliper_Inspection(ref Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogImage8Grey Image, ref string[] ResultString, int CamNumber, CogGraphicCollection Collection)
        {
            try
            {
                bool Result = true;

                CogGraphicCollection CollectionNG = new CogGraphicCollection();
                int CaliperMax = CALIPERMAX - 1;

                for (int lop = 1; lop <= CaliperMax; lop++)
                {
                    if (CaliperEnable[CamNumber, lop] == true)
                    {
                        Calipers[CamNumber, lop].Run(Image);
                        ResultString[lop] = "OK";
                    }
                    else
                    {
                        ResultString[lop] = "NON";
                    }
                }

                for (int lop = 1; lop <= CaliperMax; lop++)
                {
                    if (CaliperEnable[CamNumber, lop] == true)
                    {
                        Calipers[CamNumber, lop].ResultAllDisplay(Collection);
                    }
                }
                return Result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public CogImage8Grey FixtureImage(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, out string ImageSpacename, int HighPatternNumber,int FixIndexNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, FixIndexNumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, FixIndexNumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, FixIndexNumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
        public CogImage8Grey Blob_FixtureImage(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (Cognex.VisionPro.CogImage8Grey)Fixture.OutputImage;
        }

        public CogImage8Grey Blob_FixtureImage1(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }

        public CogImage8Grey Blob_FixtureImage2(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
        public CogImage8Grey Blob_FixtureImage3(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
        public CogImage8Grey Blob_FixtureImage4(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
        public CogImage8Grey Blob_FixtureImage5(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
        public CogImage8Grey Blob_FixtureImage6(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }

        public CogImage8Grey LINE_FixtureImage(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }

        public CogImage8Grey Mask_FixtureImage1(CogImage8Grey OriImage, CogTransform2DLinear Fixtured, string SetName, int Camnumber, int toolnumber, out string ImageSpacename, int HighPatternNumber)
        {
            Cognex.VisionPro.CalibFix.CogFixtureTool Fixture = new Cognex.VisionPro.CalibFix.CogFixtureTool();

            Fixture.InputImage = OriImage;
            Fixture.RunParams.FixturedSpaceName = SetName;
            Fixture.RunParams.UnfixturedFromFixturedTransform = Fixtured;
            Fixture.RunParams.FixturedSpaceNameDuplicateHandling = Cognex.VisionPro.CalibFix.CogFixturedSpaceNameDuplicateHandlingConstants.Compatibility;
            //***************추가***************//
            Fixtured.TranslationX = MultiPattern[Camnumber, toolnumber].TransX(HighPatternNumber);
            Fixtured.TranslationY = MultiPattern[Camnumber, toolnumber].TransY(HighPatternNumber);
            Fixtured.Rotation = MultiPattern[Camnumber, toolnumber].TransRotation(HighPatternNumber);

            Fixture.Run();
            ImageSpacename = Fixture.OutputImage.SelectedSpaceName;
            return (CogImage8Grey)Fixture.OutputImage;
        }
    }

    public struct Points
    {
        public double X;
        public double Y;
        public int Threshold;
        public bool Enable;
    }
}
