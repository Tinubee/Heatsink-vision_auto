using Cognex.VisionPro.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.Cogs;
using VISION.Schemas;

namespace VISION
{
    public class PGgloble
    {
        #region "DO NOT TOUCH"
        private static PGgloble instance = null;
        private static readonly object Lock = new object();

        private PGgloble()
        {

        }

        public static PGgloble getInstance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new PGgloble();
                    }
                    return instance;
                }
            }
        }
        #endregion
        public readonly string PROGRAMROOT = Application.StartupPath;

        public readonly string MODELROOT = Application.StartupPath + "\\Models"; //모델저장경로.
        public readonly string MODELLIST = Application.StartupPath + "\\ModelList.ini"; //모델리스트 ini파일

        public readonly string MODELCONFIGFILE = "\\Modelcfg.ini"; //모델 사용유무 저장.

        public readonly string CONFIGFILE = Application.StartupPath + "\\config.ini";
        public readonly string SETTING = Application.StartupPath + "\\setting.ini"; //setting값 저장

        public readonly string PROGRAM_VERSION = "1.0.0"; //Program Version
        // 시스템
        public int CamCount = 8;
        //public List<string> availablePort = new List<string>();
        public Model RunnModel = null;
        public CogIPOneImageTool[] FlipImageTool = new CogIPOneImageTool[8];
        public Frm_Main G_MainForm;

        public string CurruntModelName;
        public string ImageSaveRoot; // 이미지 저장 경로
        public int ImageSaveDay; //이미지 보관일수.
        public string DataSaveRoot; // 검사 결과 저장 경로
        public string LineName; // 프로그램 메인 화면 중앙 상단에 적힐 무언가.

        public string Camera_SerialNumber; //카메라 시리얼번호.
        public LineCamSets[] LineCameraOption; //카메라 옵션 클래스
        public ModelType ModelTypes;

        // Light controller
        public int LightControlNumber;
        public string[] PortName = new string[4]; // 포트 번호
        public string[] Parity = new string[4]; // 패리티 비트
        public string[] StopBits = new string[4]; // 스톱비트
        public string[] DataBit = new string[4]; // 데이터 비트
        public string[] BaudRate = new string[4]; // 보오 레이트
        public int[,] LightChAndValue = new int[4, 2]; //조명컨트롤러 채널(컨트롤번호, 채널번호) 조명값
        public bool InspectUsed; //검사 사용
        public string ImageFilePath; //이미지파일경로.

        public int CamNumber; //사용카메라번호

        public int RegionAreaInfo; //패턴툴 영역 정보.
        public int Line1_OK;
        public int Line1_NG;
        public int Line2_OK;
        public int Line2_NG;

        public bool OKImageSave = true;
        public bool NGImageSave = true;
        public bool NGContainUIImageSave = false;

        public double[,] InsPat_Result = new double[8, 30];
        public double[,] MultiInsPat_Result = new double[8, 30];

        public bool[] PatternResult = new bool[8];
        public bool[] BlobResult = new bool[8];
        public bool[] CaliperResult = new bool[8];
        public bool[] MeasureResult = new bool[8];

        public bool[] BlobResult4 = new bool[3];
        public bool[] BlobResult5 = new bool[2];

        public double StandPoint_X;
        public double StandPoint_Y;

        public double[,] MultiPatternResultData = new double[8, 30];
        public int allCameraCount;

        public bool[] scratchError = new bool[2]; //동시에 촬영 진행으로 인해, 앞 뒤 불량유형 나누어 놓았음.
        public bool[] noScratchError = new bool[2];

        public bool[] firstInspection = new bool[2]; //처음 시작할때 (2번째 결과값에 안넣어주기 위해 )
        public int InspectOrder;

        public bool statsOK = false;

        //마스크 툴 관련
        public CogMaskCreatorTool[] curruntMaskTool = new CogMaskCreatorTool[8];
        public string curruntMaskToolPath;

        public bool[] Inspect4 = new bool[3];
        public bool[] Inspect5 = new bool[2];


        public string[] press1PinResult = new string[3];
        public string[] press2PinResult = new string[2];

        public 코그넥스파일 코그넥스파일;
        public static 그랩제어 그랩제어;
    }
    public struct LineCamSets
    {
        public string Port;
        public int CamNumber;
        public double Exposure; 
        public double Gain; 
        public int DelayTime;
    }

    public enum ModelType
    {
        shield,
        heatsink0023,
        heatsink0024,
        heatsink0026,
    }

    public struct 코그넥스파일
    {
        public Model 모델;
        public Camera[] 카메라;
        public Mask[,] 마스크툴;
        public Blob[,] 블롭툴;
        public MultiPMAlign[,] 패턴툴;
        public Line[,] 라인툴;
        public Circle[,] 써클툴;
        public Distance[,] 거리측정툴;
        public Caliper[,] 캘리퍼툴;

        public bool[,] 라인툴사용여부;
        public bool[,] 블롭툴사용여부;
        public bool[,] 블롭툴역검사;
        public bool[,] 패턴툴사용여부;
        public bool[,] 거리측정툴사용여부;
        public bool[,] 써클툴사용여부;
        public bool[,] 캘리퍼툴사용여부;

        public int[,] 블롭툴양품갯수;
        public int[,] 블롭툴픽스쳐번호;
        public int[,] 패턴툴검사순서번호;

        public double[,] 보정값;
        public double[,] 최소값;
        public double[,] 최대값;
    }
}
