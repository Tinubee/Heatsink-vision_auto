using Cognex.VisionPro.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public readonly string LOADINGFROM = Application.StartupPath + "\\OtherForm\\LoadingForm\\LoadingForm_KHM\\bin\\Debug\\LoadingForm_KHM.exe"; //로딩창 경로
        public readonly string SAVEFROM = Application.StartupPath + "\\OtherForm\\SaveForm\\SaveForm_KHM\\bin\\Debug\\SaveForm_KHM.exe"; //저장창 경로
        public readonly string MODELCHANGEFROM = Application.StartupPath + "\\OtherForm\\ModelChange\\ModelChange_KHM\\bin\\Debug\\ModelChange_KHM.exe"; //변경창 경로

        public readonly string MODELROOT = Application.StartupPath + "\\Models"; //모델저장경로.
        public readonly string MODELLIST = Application.StartupPath + "\\ModelList.ini"; //모델리스트 ini파일

        public readonly string MODELCONFIGFILE = "\\Modelcfg.ini"; //모델 사용유무 저장.

        public readonly string CONFIGFILE = Application.StartupPath + "\\config.ini";
        public readonly string SETTING = Application.StartupPath + "\\setting.ini"; //setting값 저장

        public readonly string PROGRAM_VERSION = "1.0.0"; //Program Version
        #region "버전 관리 및 업데이트 내용"
        //1.0.0 - 현장투입 후 완성된 최종버전(주요기능 및 프로그램 구성 완료) - 날짜 / 이름
        //2.0.1 - 프로그램 로딩창, 저장창, 모델변경창 새로 추가.
        //       - 치수 측정 방식 변경(Line, Circle 등등 전부 없애고 Distance Tool 로 통합

        //*******추가 확인해야될 사항들*******(2022년 2월22일 김형민)
        //1. 모델 변경 시 Camera Exposure값 & Gain값 설정 되는 지 확인(각 모델별로)
        //2. Camera Setting 값 저장위치 확인(Model -> 각 Cam에 저장하도록 하기)
        //3. Setting Form  종료시 Cognex Model Load 안해도 되는지 확인(안해도 되면 빼버리기) - AutoRun 진행 시 변경된 Cognex Model로 Load되는지 확인.
        //4. PLC에서 Model Change 신호 시, 정상적으로 변경되는지 확인(Camera Setting 값 포함)
        #endregion

        // 시스템
        public Cogs.Model RunnModel = null;
        public CogIPOneImageTool[] FlipImageTool = new CogIPOneImageTool[6];
        public Frm_Main G_MainForm;

        public string CurruntModelName;
        public string ImageSaveRoot; // 이미지 저장 경로
        public int ImageSaveDay; //이미지 보관일수.
        public string DataSaveRoot; // 검사 결과 저장 경로
        public string LineName; // 프로그램 메인 화면 중앙 상단에 적힐 무언가.

        public string Camera_SerialNumber; //카메라 시리얼번호.
        public CamSets CameraOption; //카메라 옵션 클래스

        // Light controller
        public int LightControlNumber;
        public string[] PortName = new string[4]; // 포트 번호
        public string[] Parity = new string[4]; // 패리티 비트
        public string[] StopBits = new string[4]; // 스톱비트
        public string[] DataBit = new string[4]; // 데이터 비트
        public string[] BaudRate = new string[4]; // 보오 레이트
        public int[,] LightChAndValue = new int[4, 2]; //조명컨트롤러 채널(컨트롤번호, 채널번호) 조명값
        //public string PortName; // 포트 번호
        //public string Parity; // 패리티 비트
        //public string StopBits; // 스톱비트
        //public string DataBit; // 데이터 비트
        //public string BaudRate; // 보오 레이트

        public bool InspectUsed; //검사 사용
        //public string LightCH1; //조명컨트롤러 채널1
        //public string LightCH2; //조명컨트롤러 채널2

        public string ImageFilePath; //이미지파일경로.

        public int CamNumber; //사용카메라번호

        public int RegionAreaInfo; //패턴툴 영역 정보. - 패턴툴에서 영역을 가져오는게 있을껀데.. 못찾겠어서 임시로 만들어놨다.
        public int Line1_OK;
        public int Line1_NG;
        public int Line2_OK;
        public int Line2_NG;

        public bool OKImageSave = true;
        public bool NGImageSave = true;

        public double[,] InsPat_Result = new double[6, 30];
        public double[,] MultiInsPat_Result = new double[6, 30];

        public bool[] PatternResult = new bool[6];
        public bool[] BlobResult = new bool[6];
        public bool[] CaliperResult = new bool[6];
        public bool[] MeasureResult = new bool[6];

        public bool[] BlobResult4 = new bool[3];
        public bool[] BlobResult5 = new bool[2];

        public double StandPoint_X;
        public double StandPoint_Y;
   
        public double[,] MultiPatternResultData = new double[6, 30];
        public int allCameraCount;

        public bool[] scratchError = new bool[2]; //동시에 촬영 진행으로 인해, 앞 뒤 불량유형 나누어 놓았음.
        public bool[] noScratchError = new bool[2];

        public bool[] firstInspection = new bool[2]; //처음 시작할때 (2번째 결과값에 안넣어주기 위해 )
        public int InspectOrder;

        public bool statsOK = false;

        //마스크 툴 관련
        public CogMaskCreatorTool[] curruntMaskTool = new CogMaskCreatorTool[6];
        public string curruntMaskToolPath;

        public bool[] Inspect4= new bool[3];
        public bool[] Inspect5 = new bool[2];
    }
    public struct CamSets
    {
        public double[] Exposure; //조리개값
        public double[] Gain;
        public int DelayTime; //지연시간
    }
}
