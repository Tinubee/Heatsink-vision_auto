using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Cognex.VisionPro;
using System.Runtime.InteropServices;
using Cognex.VisionPro.Display;
using System.IO.Ports;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Dimensioning;
using System.Drawing.Imaging;
using System.Reflection;
using KimLib;
using Cognex.VisionPro.FGGigE;
using VISION.Cogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Net.Sockets;
using System.Collections;

namespace VISION
{
    public delegate void EventCallBack(Bitmap bmp);
    public partial class Frm_Main : Form
    {
        public Log log = new Log();
        private Class_Common cm { get { return Program.cm; } } //에러 메세지 보여주기.
        private CogImage24PlanarColor[] Colorimage; //컬러 이미지
        private CogImage8Grey[] Monoimage; //흑백이미지
        internal Frm_ToolSetUp frm_toolsetup; //툴셋업창 화면
        internal Frm_AnalyzeResult frm_analyzeresult;

        internal bool[] trigflag; //트리거 불함수
        private CogImage8Grey Fiximage; //PMAlign툴의 결과이미지(픽스쳐이미지)
        private string FimageSpace; //PMAlign툴 SpaceName(보정하기위해)

        private Cogs.Camera[] TempCam;
        private CogDisplay[] TempCogDisplay;

        private Cogs.Model TempModel; //모델
        private Cogs.Blob[,] TempBlobs; //블롭툴
        private Cogs.Line[,] TempLines; //라인툴
        private Cogs.Circle[,] TempCircles; //써클툴
        private Cogs.MultiPMAlign[,] TempMulti;
        private Cogs.Distance[,] TempDistance;
        private Cogs.Caliper[,] TempCaliper;

        private bool[,] TempLineEnable; //라인툴 사용여부
        private bool[,] TempBlobEnable;//블롭툴 사용여부
        private bool[,] TempCircleEnable; //써클툴 사용여부
        private bool[,] TempMultiEnable;
        private bool[,] TempDistanceEnable;
        private bool[,] TempCaliperEnable;

        private int[,] TempBlobOKCount;//블롭툴 설정갯수
        private int[,] TempBlobFixPatternNumber;

        private double[,] TempDistance_CalibrationValue;
        private double[,] TempDistance_LowValue;
        private double[,] TempDistance_HighValue;

        private PGgloble Glob; //전역변수 - CLASS "PGgloble" 참고.

        public bool LightStats = false; //조명 상태
        public bool[] InspectResult = new bool[6]; //검사결과.
        public bool Modelchange = false; //모델체인지

        public Stopwatch[] InspectTime = new Stopwatch[6]; //검사시간
        public double[] OK_Count = new double[6]; //양품개수
        public double[] NG_Count = new double[6]; //NG품개수
        public double[] TOTAL_Count = new double[6]; //총개수
        public double[] NG_Rate = new double[6]; //총개수

        public bool[] InspectFlag = new bool[6]; //검사 플래그.
        public int camcount = 6;
        Thread snap1; //CAM1 Shot 쓰레드
        Thread snap2; //CAM2 Shot 쓰레드
        Thread snap3; //CAM3 Shot 쓰레드

        Thread snap4_1; //CAM4 Shot 쓰레드
        Thread snap4_2;
        Thread snap4_3;
        Thread snap5_1; //CAM5 Shot 쓰레드
        Thread snap5_2;
        Thread snap6; //CAM6 Shot 쓰레드

        Label[] OK_Label;
        Label[] NG_Label;
        Label[] TOTAL_Label;
        Label[] NGRATE_Label;


        // Trigger
        private Boolean IO_DoWork = false;
        //PLC 신호 관련 변수들
        // is selected OCX

        private string[] IOModel = new string[2];
        private Button[] inputBtn;
        private System.Windows.Forms.CheckBox[] outputBtn;
        //private CheckBox[] checkHigh = new CheckBox[16];
        //private CheckBox[] checkLow = new CheckBox[16];
        private uint hInterrupt = 0;
        private Thread EventThread = null;
        private bool bThread = false;

        public readonly static uint INFINITE = 0xFFFFFFFF;
        public readonly static uint STATUS_WAIT_0 = 0x00000000;
        public readonly static uint WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0);

        private DateTime[] TrigTime = new DateTime[12];
        public bool[] gbool_di = new bool[12];
        public bool[] re_gbool_di = new bool[12];

        [DllImport("kernel32", EntryPoint = "WaitForSingleObject", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint WaitForSingleObject(uint hHandle, uint dwMilliseconds);

        [DllImport("KERNEL32", EntryPoint = "SetEvent", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetEvent(long hEvent);

        public Frm_Main()
        {
            Glob = PGgloble.getInstance; //전역변수 사용
            Process.Start($"{Glob.LOADINGFROM}");
            InitializeComponent();

            // 주석처리함
            //this.timerSensor.Tick += this.timerSensor_Tick;

            ColumnHeader h = new ColumnHeader();
            StandFirst(); //처음 실행되어야 하는부분. - 이거 왜했지.. 이유는 모르겠다 일단 냅두자 필요없을꺼같기도함. - 20200121 김형민
            CamSet();
            Glob.RunnModel = new Cogs.Model(); //코그넥스 모델 확인.

            // 최종 트리거 시간 초기화
            for (Int32 i = 0; i < this.TrigTime.Length; i++)
                this.TrigTime[i] = DateTime.Today;
        }
        public void StandFirst()
        {
            Directory.CreateDirectory(Glob.MODELROOT); // 프로그램 모델 루트 디렉토리 작성
            INIControl Config = new INIControl(Glob.CONFIGFILE);
            INIControl Modellist = new INIControl(Glob.MODELLIST);
            INIControl setting = new INIControl(Glob.SETTING); // ini파일 경로
            Glob.ImageSaveRoot = setting.ReadData("SYSTEM", "Image Save Root"); //이미지 저장 경로
            Glob.DataSaveRoot = setting.ReadData("SYSTEM", "Data Save Root"); //데이터 저장 경로
            log.InitializeLog($"{Glob.DataSaveRoot}\\Log");
            log.OnLogEvent += Log_OnLogEvent;
        }
        public void Log_OnLogEvent(object sender, LogItem e)
        {
            logControl1.ManageLog(e);
        }
        private void Frm_Main_Load(object sender, EventArgs e)
        {
            lb_Ver.Text = $"Ver. {Glob.PROGRAM_VERSION}";
            LoadSetup(); //프로그램 셋팅 로드.
            timer_Setting.Start(); //타이머에서 계속해서 확인하는 것들
            CognexModelLoad();
            DigitalIO_Load();//IO Load
            SelectModule();
            log.AddLogMessage(LogType.Infomation, 0, "Vision Program Start");
            Process[] myProcesses = Process.GetProcessesByName("LoadingForm_KHM");
            if (myProcesses.LongLength > 0)
            {
                myProcesses[0].Kill();
            }


            // Trig Start
            new Thread(ReadInputSignal).Start();
        }
        private void CamSet()
        {
            try
            {
                CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();
                Glob.allCameraCount = frameGrabbers.Count + 3;
                log.AddLogMessage(LogType.Program, 0, $"확인 된 카메라 개수 : {Glob.allCameraCount}");
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
            }

        }

        private bool SelectModule()
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;
                short nIndex = 0;
                uint uDataHigh = 0;
                uint uDataLow = 0;
                uint uFlagHigh = 0;
                uint uFlagLow = 0;
                uint uUse = 0;

                CAXD.AxdInfoGetModule(0, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DI32:
                    case AXT_MODULE.AXT_SIO_RDI32:
                    case AXT_MODULE.AXT_SIO_RSIMPLEIOMLII:
                    case AXT_MODULE.AXT_SIO_RDO16AMLII:
                    case AXT_MODULE.AXT_SIO_RDO16BMLII:
                    case AXT_MODULE.AXT_SIO_DI32_P:
                    case AXT_MODULE.AXT_SIO_RDI32RTEX:
                        //groupHigh.Text = "INPUT  0bit ~ 15Bit";
                        //groupLow.Text = "INPUT 16bit ~ 31Bit";

                        if (((AXT_MODULE)uModuleID) == AXT_MODULE.AXT_SIO_RDI32)
                        {
                            //checkInterrupt.Checked = false;
                            //radioCallback.Enabled = false;
                            //radioMessage.Enabled = false;
                            //radioEvent.Enabled = false;
                            //checkRigingEdge.Checked = false;
                            //checkFallingEdge.Checked = false;
                        }
                        else
                        {
                            //checkInterrupt.Checked = true;
                            //radioCallback.Enabled = true;
                            //radioMessage.Enabled = true;
                            //radioEvent.Enabled = true;
                            //checkRigingEdge.Checked = true;
                            //checkFallingEdge.Checked = true;

                            CAXD.AxdiInterruptGetModuleEnable(0, ref uUse);
                            if (uUse == (uint)AXT_USE.ENABLE)
                            {
                                //checkInterrupt.Checked = true;
                                //SelectMessage();
                            }
                            else
                               // checkInterrupt.Checked = false;

                            CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);
                            CAXD.AxdiInterruptEdgeGetWord(0, 1, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataLow);
                            //if (uDataHigh == 0xFFFF && uDataLow == 0xFFFF)
                            //    checkRigingEdge.Checked = true;
                            //else
                            //    checkRigingEdge.Checked = false;

                            CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
                            CAXD.AxdiInterruptEdgeGetWord(0, 1, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataLow);
                            //if (uDataHigh == 0xFFFF && uDataLow == 0xFFFF)
                            //    checkFallingEdge.Checked = true;
                            //else
                            //    checkFallingEdge.Checked = false;
                        }

                        //for (nIndex = 0; nIndex < 16; nIndex++)
                        //{
                        //    checkHigh[nIndex].Text = String.Format("{0:D2}", nIndex);
                        //    checkLow[nIndex].Text = String.Format("{0:D2}", nIndex + 16);
                        //}
                        break;

                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                    case AXT_MODULE.AXT_SIO_DO32T_P:
                    case AXT_MODULE.AXT_SIO_RDO32RTEX:
                       
                        //++
                        // Read outputting signal in WORD
                        CAXD.AxdoReadOutportWord(1, 0, ref uDataHigh);
                        CAXD.AxdoReadOutportWord(1, 1, ref uDataLow);

                        for (nIndex = 0; nIndex < 12; nIndex++)
                        {
                            // Verify the last bit value of data read
                            uFlagHigh = uDataHigh & 0x0001;
                            uFlagLow = uDataLow & 0x0001;

                            // Shift rightward by bit by bit
                            uDataHigh = uDataHigh >> 1;
                            uDataLow = uDataLow >> 1;

                            // Updat bit value in control
                            if (uFlagHigh == 1)
                            {
                                outputBtn[nIndex].BackColor = Color.Lime;
                                gbool_di[nIndex] = true;
                            }

                            else
                            {
                                outputBtn[nIndex].BackColor = SystemColors.Control;
                                gbool_di[nIndex] = false;
                            }
                               

                            //if (uFlagLow == 1)
                            //    outputBtn[nIndex].BackColor = Color.Lime;
                            //else
                            //    outputBtn[nIndex].BackColor = SystemColors.Control;

                            //outputBtn[nIndex].Text = String.Format("{0:D2}", nIndex);
                            //outputBtn[nIndex].Text = String.Format("{0:D2}", nIndex + 16);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                    case AXT_MODULE.AXT_SIO_RDB128MLII:
                    case AXT_MODULE.AXT_SIO_RDB32T:
                    case AXT_MODULE.AXT_SIO_RDB32RTEX:
                    case AXT_MODULE.AXT_SIO_RDB96MLII:
                        //groupHigh.Text = "INPUT  0bit ~ 15Bit";
                        //groupLow.Text = "OUTPUT  0bit ~ 15Bit";

                        //// Only Digital Input was used
                        //checkInterrupt.Enabled = true;
                        //checkRigingEdge.Enabled = true;
                        //checkFallingEdge.Enabled = true;

                        CAXD.AxdiInterruptGetModuleEnable(0, ref uUse);
                        //if (uUse == (uint)AXT_USE.ENABLE)
                        //{
                        //    checkInterrupt.Checked = true;
                        //    SelectMessage();
                        //}
                        //else
                        //    checkInterrupt.Checked = false;

                        CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);
                        //if (uDataHigh == 0xFFFF)
                        //    checkRigingEdge.Checked = true;
                        //else
                        //    checkRigingEdge.Checked = false;

                        CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
                        //if (uDataHigh == 0xFFFF)
                        //    checkFallingEdge.Checked = true;
                        //else
                        //    checkFallingEdge.Checked = false;

                        //++
                        // Read outputting signal in WORD
                        CAXD.AxdoReadOutportWord(0, 0, ref uDataLow);

                        for (nIndex = 0; nIndex < 12; nIndex++)
                        {
                            // Verify the last bit value of data read
                            uFlagLow = uDataLow & 0x0001;

                            // Shift rightward by bit by bit
                            uDataLow = uDataLow >> 1;

                            // Updat bit value in control
                            if (uFlagLow == 1)
                                inputBtn[nIndex].BackColor = Color.Lime;
                            else
                                inputBtn[nIndex].BackColor = SystemColors.Control;

                            //checkHigh[nIndex].Text = String.Format("{0:D2}", nIndex);
                            //checkLow[nIndex].Text = String.Format("{0:D2}", nIndex);
                        }
                        break;
                }
            }

            return true;
        }

        private void DigitalIO_Load()
        {
            inputBtn = new Button[12] { btn_INPUT0, btn_INPUT1, btn_INPUT2, btn_INPUT3, btn_INPUT4, btn_INPUT5, btn_INPUT6, btn_INPUT7, btn_INPUT8, btn_INPUT9, btn_INPUT10, btn_INPUT11 };
            outputBtn = new System.Windows.Forms.CheckBox[12] { btn_OUTPUT0, btn_OUTPUT1, btn_OUTPUT2, btn_OUTPUT3, btn_OUTPUT4, btn_OUTPUT5, btn_OUTPUT6, btn_OUTPUT7, btn_OUTPUT8, btn_OUTPUT9, btn_OUTPUT10, btn_OUTPUT11 };

            if (OpenDevice())
            {
                //radioMessage.Checked = true;
                //timerSensor.Enabled = true;
                //frmDigitalIO = this;
            }
            CheckForIllegalCrossThreadCalls = false;
        }

        private bool OpenDevice()
        {
            //++
            // Initialize library 
            if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                uint uStatus = 0;

                if (CAXD.AxdInfoIsDIOModule(ref uStatus) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    if ((AXT_EXISTENCE)uStatus == AXT_EXISTENCE.STATUS_EXIST)
                    {
                        int nModuleCount = 0;

                        if (CAXD.AxdInfoGetModuleCount(ref nModuleCount) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            short i = 0;
                            int nBoardNo = 0;
                            int nModulePos = 0;
                            uint uModuleID = 0;
                            string strData = "";

                            for (i = 0; i < nModuleCount; i++)
                            {
                                if (CAXD.AxdInfoGetModule(i, ref nBoardNo, ref nModulePos, ref uModuleID) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                                {
                                    switch ((AXT_MODULE)uModuleID)
                                    {
                                        case AXT_MODULE.AXT_SIO_DI32: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DB32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32T", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDI32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDI32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDO32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB128MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB128MLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RSIMPLEIOMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RSIMPLEIOMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO16AMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16AMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO16BMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16BMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB96MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB96MLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO32RTEX: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO32RTEX", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDI32RTEX: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDI32RTEX", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB32RTEX: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32RTEX", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DI32_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32_P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32T_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T_P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32T", nBoardNo, i); break;
                                    }
                                    IOModel.Append(strData);
                                    //comboModule.Items.Add(strData);
                                }
                            }

                            //comboModule.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Module not exist.");

                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Open Error!");
            }

            return true;
        }

        private void LoadSetup()
        {
            try
            {
                OK_Label = new Label[6] { lb_CAM1_OK, lb_CAM2_OK, lb_CAM3_OK, lb_CAM4_OK, lb_CAM5_OK, lb_CAM6_OK };
                NG_Label = new Label[6] { lb_CAM1_NG, lb_CAM2_NG, lb_CAM3_NG, lb_CAM4_NG, lb_CAM5_NG, lb_CAM6_NG };
                TOTAL_Label = new Label[6] { lb_CAM1_TOTAL, lb_CAM2_TOTAL, lb_CAM3_TOTAL, lb_CAM4_TOTAL, lb_CAM5_TOTAL, lb_CAM6_TOTAL };
                NGRATE_Label = new Label[6] { lb_CAM1_NGRATE, lb_CAM2_NGRATE, lb_CAM3_NGRATE, lb_CAM4_NGRATE, lb_CAM5_NGRATE, lb_CAM6_NGRATE };
                TempCogDisplay = new CogDisplay[6] { cdyDisplay, cdyDisplay2, cdyDisplay3, cdyDisplay4, cdyDisplay5, cdyDisplay6 };

                INIControl Modellist = new INIControl(Glob.MODELLIST); ;
                INIControl CFGFILE = new INIControl(Glob.CONFIGFILE); ;

                INIControl setting = new INIControl(Glob.SETTING);
                string LastModel = CFGFILE.ReadData("LASTMODEL", "NAME"); //마지막 사용모델 확인.
                //확인 필요. - LastModel Name 변수에 들어오는 String값 확인하기.
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{LastModel}\\CamSet.ini");
                for (int i = 0; i < Glob.allCameraCount; i++)
                {
                    Glob.RunnModel.Loadmodel(LastModel, Glob.MODELROOT, i); //VISION TOOL LOAD
                }

                Glob.ImageSaveRoot = setting.ReadData("SYSTEM", "Image Save Root"); //이미지 저장 경로
                Glob.DataSaveRoot = setting.ReadData("SYSTEM", "Data Save Root"); //데이터 저장 경로
                Glob.ImageSaveDay = Convert.ToInt32(setting.ReadData("SYSTEM", "Image Save Day")); //이미지 보관일수

                //****************************COMPORT 연결관련****************************//
                Glob.PortName = setting.ReadData("COMMUNICATION", "Port number");
                Glob.Parity = setting.ReadData("COMMUNICATION", "Parity Check");
                Glob.StopBits = setting.ReadData("COMMUNICATION", "Stop bits");
                Glob.DataBit = setting.ReadData("COMMUNICATION", "Data Bits");
                Glob.BaudRate = setting.ReadData("COMMUNICATION", "Baud Rate");

                //****************************조명 채널****************************//
                Glob.LightCH1 = setting.ReadData("LightControl", "CH1");
                Glob.LightCH2 = setting.ReadData("LightControl", "CH1");

                //****************************검사 사용유무****************************//
                Glob.InspectUsed = setting.ReadData("SYSTEM", "Inspect Used Check", true) == "1" ? true : false;
                Glob.OKImageSave = setting.ReadData("SYSTEM", "OK IMAGE SAVE", true) == "1" ? true : false;
                Glob.NGImageSave = setting.ReadData("SYSTEM", "NG IMAGE SAVE", true) == "1" ? true : false;
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        private void btn_ToolSetUp_Click(object sender, EventArgs e)
        {
            frm_toolsetup = new Frm_ToolSetUp(this);
            frm_toolsetup.Show();
        }
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            INIControl setting = new INIControl(Glob.SETTING);
            DateTime dt = DateTime.Now;
            setting.WriteData("Exit Date", "Date", dt.ToString("yyyyMMdd"));
            Application.Exit();
        }

        private void btn_SystemSetup_Click(object sender, EventArgs e)
        {
            Frm_SystemSetUp FrmSystemSetUp = new Frm_SystemSetUp(this);
            FrmSystemSetUp.Show();
        }

        private void timer_Setting_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lb_Time.Text = dt.ToString("yyyy년 MM월 dd일 HH:mm:ss"); //현재날짜
            lb_CurruntModelName.Text = Glob.RunnModel.Modelname(); //현재사용중인 모델명 체크
            Glob.CurruntModelName = Glob.RunnModel.Modelname();

            for (int i = 0; i < camcount; i++)
            {
                OK_Label[i].Text = OK_Count[i].ToString();
                NG_Label[i].Text = NG_Count[i].ToString();
                TOTAL_Count[i] = OK_Count[i] + NG_Count[i];
                TOTAL_Label[i].Text = (OK_Count[i] + NG_Count[i]).ToString();

                if (NG_Count[i] != 0)
                {
                    NG_Rate[i] = (NG_Count[i] / TOTAL_Count[i]) * 100;
                    NGRATE_Label[i].Text = NG_Rate[i].ToString("F1") + "%";
                }

            }
        }
      

        private void bk_IO_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {

                //IO CHECK - DISPLAY 표시 부분

                //case 0:
                //    snap1 = new Thread(new ThreadStart(ShotAndInspect_Cam1));
                //    snap1.Priority = ThreadPriority.Highest;
                //    snap1.Start();
                //    break;
                //case 13:
                //    Process.Start($"{Glob.MODELCHANGEFROM}");
                //    for (int k = 0; k < camcount; k++)
                //    {
                //        if (Glob.RunnModel.Loadmodel("K12E DIMPLE", Glob.MODELROOT, k) == true)
                //        {
                //            if (k == camcount - 1)
                //            {
                //                lb_CurruntModelName.Text = Glob.RunnModel.Modelname();
                //                Glob.CurruntModelName = Glob.RunnModel.Modelname();
                //                CamSet();
                //                Process[] myProcesses = Process.GetProcessesByName("ModelChange_KHM");
                //                if (myProcesses.LongLength > 0)
                //                {
                //                    myProcesses[0].Kill();
                //                }
                //                log.AddLogMessage(LogType.Infomation, 0, "모델 전환 성공");
                //            }
                //        }
                //    }
                //    break;
            }
        }

        public void ShotAndInspect_Cam1()
        {
            try
            {
                InspectTime[0] = new Stopwatch();
                InspectTime[0].Reset();
                InspectTime[0].Start();
                TempCogDisplay[0].Image = TempCam[0].Run();
                TempCogDisplay[0].InteractiveGraphics.Clear();
                TempCogDisplay[0].StaticGraphics.Clear();

                if (TempCogDisplay[0].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 1");
                    return;
                }
                if (Inspect_Cam0(TempCogDisplay[0]) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam1_Result.BackColor = Color.Lime;
                        lb_Cam1_Result.Text = "O K";
                        OK_Count[0]++;
                        if (Glob.OKImageSave)
                            ImageSave1("OK", 1, cdyDisplay);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam1_Result.BackColor = Color.Red;
                        lb_Cam1_Result.Text = "N G";
                        NG_Count[0]++;
                        if (Glob.NGImageSave)
                            ImageSave1("NG", 1, cdyDisplay);
                    });
                }

                InspectTime[0].Stop();
                InspectFlag[0] = false;

                BeginInvoke((Action)delegate { lb_Cam1_InsTime.Text = InspectTime[0].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 1 Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam2()
        {
            try
            {
                InspectTime[1] = new Stopwatch();
                InspectTime[1].Reset();
                InspectTime[1].Start();

                TempCogDisplay[1].Image = TempCam[1].Run();
                TempCogDisplay[1].InteractiveGraphics.Clear();
                TempCogDisplay[1].StaticGraphics.Clear();
                if (TempCogDisplay[1].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 2");
                    return;
                }
                if (Inspect_Cam1(TempCogDisplay[1]) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam2_Result.BackColor = Color.Lime;
                        lb_Cam2_Result.Text = "O K";
                        OK_Count[1]++;
                        if (Glob.OKImageSave)
                            ImageSave2("OK", 2, TempCogDisplay[1]);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam2_Result.BackColor = Color.Red;
                        lb_Cam2_Result.Text = "N G";
                        NG_Count[1]++;
                        if (Glob.NGImageSave)
                            ImageSave2("NG", 2, TempCogDisplay[1]);
                    });
                    //검사 결과 NG

                }

                InspectTime[1].Stop();
                InspectFlag[1] = false;

                BeginInvoke((Action)delegate { lb_Cam2_InsTime.Text = InspectTime[1].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 2 Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam3()
        {
            int funCamNumber = 2;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                TempCogDisplay[funCamNumber].Image = TempCam[funCamNumber].Run();
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();
                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber+1}");
                    return;
                }
                if (Inspect_Cam2(TempCogDisplay[funCamNumber]) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam3_Result.BackColor = Color.Lime;
                        lb_Cam3_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave3("OK", funCamNumber+1, TempCogDisplay[funCamNumber]);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam3_Result.BackColor = Color.Red;
                        lb_Cam3_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave3("NG", funCamNumber+1, TempCogDisplay[funCamNumber]);
                    });
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { lb_Cam3_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber+1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam4(CogDisplay cdy)
        {
            int funCamNumber = 3;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                cdy.Image = TempCam[funCamNumber].Run();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (Inspect_Cam3(cdy) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam4_Result.BackColor = Color.Lime;
                        lb_Cam4_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave4("OK", funCamNumber + 1, cdy);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam4_Result.BackColor = Color.Red;
                        lb_Cam4_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave4("NG", funCamNumber + 1, cdy);
                    });
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { lb_Cam4_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam5(CogDisplay cdy)
        {
            int funCamNumber = 4;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                cdy.Image = TempCam[funCamNumber].Run();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if ( cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (Inspect_Cam4(cdy) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam5_Result.BackColor = Color.Lime;
                        lb_Cam5_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave5("OK", funCamNumber + 1, cdy);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam5_Result.BackColor = Color.Red;
                        lb_Cam5_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave5("NG", funCamNumber + 1, cdy);
                    });
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { lb_Cam5_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam6(CogDisplay cdy)
        {
            int funCamNumber = 5;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                cdy.Image = TempCam[funCamNumber].Run();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();
                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (Inspect_Cam5(cdy) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam6_Result.BackColor = Color.Lime;
                        lb_Cam6_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave6("OK", funCamNumber + 1, cdy);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam6_Result.BackColor = Color.Red;
                        lb_Cam6_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave6("NG", funCamNumber + 1, cdy);
                    });
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { lb_Cam6_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

       
        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IO_DoWork = false;

            if (bk_IO.IsBusy == true)
            {
                bk_IO.CancelAsync();
            }
            if (LightStats == true)
            {
                LightOFF();
            }
            for (int i = 0; i < Glob.allCameraCount; i++)
            {
                TempCam[i].Close();
            }
            
        }

        private void btn_Status_Click(object sender, EventArgs e)
        {
            log.AddLogMessage(LogType.Infomation, 0, "AUTO MODE START");
            btn_Status.Enabled = false;
            btn_ToolSetUp.Enabled = false;
            btn_Model.Enabled = false;
            btn_SystemSetup.Enabled = false;
            btn_Stop.Enabled = true;
            this.IO_DoWork = true;
            CognexModelLoad();

            // 주석처리함
            //if(timerSensor.Enabled == false)
            //{
            //    timerSensor.Enabled = true;
            //}
        }
        public void CognexModelLoad()
        {
            Glob = PGgloble.getInstance;
            TempModel = Glob.RunnModel;
            TempLines = TempModel.Line();
            TempLineEnable = TempModel.LineEnables();
            TempBlobs = TempModel.Blob();
            TempBlobEnable = TempModel.BlobEnables();
            TempBlobOKCount = TempModel.BlobOKCounts();
            TempBlobFixPatternNumber = TempModel.BlobFixPatternNumbers();
            TempCircles = TempModel.Circle();
            TempCircleEnable = TempModel.CircleEnables();
            TempMulti = TempModel.MultiPatterns();
            TempMultiEnable = TempModel.MultiPatternEnables();
            TempDistance = TempModel.Distancess();
            TempDistanceEnable = TempModel.DistanceEnables();
            TempDistance_CalibrationValue = TempModel.Distance_CalibrationValues();
            TempDistance_LowValue = TempModel.Distance_LowValues();
            TempDistance_HighValue = TempModel.Distance_HighValues();
            TempCaliper = TempModel.Calipes();
            TempCaliperEnable = TempModel.CaliperEnables();
            TempCam = TempModel.Cam();
        }

        public void DisplayLabelShow(CogGraphicCollection Collection, CogDisplay cog, int X, int Y, string Text)
        {
            CogCreateGraphicLabelTool Label = new CogCreateGraphicLabelTool();
            Label.InputGraphicLabel.Color = CogColorConstants.Green;
            Label.InputImage = cog.Image;
            Label.InputGraphicLabel.X = X;
            Label.InputGraphicLabel.Y = Y;
            Label.InputGraphicLabel.Text = Text;
            Label.Run();
            Collection.Add(Label.GetOutputGraphicLabel());
        }
        public void Bolb_Train1(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage1((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }
        public void Bolb_Train2(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage2((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }
        public void Bolb_Train3(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage3((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }
        public void Bolb_Train4(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage4((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }
        public void Bolb_Train5(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage5((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }

        public void Bolb_Train6(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage6((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
                //cdyDisplay.Image = Fiximage;
            }
        }

        #region Inpection CAM0 
        public bool Inspect_Cam0(CogDisplay cog)
        {
            int CameraNumber = 0;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            CogGraphicCollection Collection4 = new CogGraphicCollection(); // 치수
            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }

            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            if (TempModel.Dimension_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {
                CogCreateGraphicLabelTool[] Point_Label = new CogCreateGraphicLabelTool[10];
                CogCreateGraphicLabelTool[] Label = new CogCreateGraphicLabelTool[10];

                for (int lop = 1; lop < TempDistance.Length / Program.CameraList.Count(); lop++)
                {
                    if (TempDistanceEnable[CameraNumber, lop])
                    {
                        double ResultValue = 0;
                        ResultValue = TempDistance[CameraNumber, lop].DistanceValue(lop) * TempDistance_CalibrationValue[CameraNumber, lop];

                        Point_Label[lop] = new CogCreateGraphicLabelTool();
                        Point_Label[lop].InputImage = cog.Image;
                        Point_Label[lop].InputGraphicLabel.X = TempDistance[CameraNumber, lop].GetX(lop);
                        Point_Label[lop].InputGraphicLabel.Y = TempDistance[CameraNumber, lop].GetY(lop);
                        Point_Label[lop].InputGraphicLabel.Text = ResultValue.ToString("F3");

                        Label[lop] = new CogCreateGraphicLabelTool();
                        Label[lop].InputImage = cog.Image;
                        Label[lop].InputGraphicLabel.X = 600;
                        Label[lop].InputGraphicLabel.Y = 170 + (80 * lop);
                        Label[lop].InputGraphicLabel.Text = $"{TempDistance[CameraNumber, lop].ToolName(lop)} : {ResultValue.ToString("F3")}";
                        Label[lop].Run();
                        Collection4.Add(Label[lop].GetOutputGraphicLabel());

                        if (TempDistance_LowValue[CameraNumber, lop] <= ResultValue && TempDistance_HighValue[CameraNumber, lop] >= ResultValue)
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Green;
                            Collection4[lop - 1].Color = CogColorConstants.Green;
                        }
                        else
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Red;
                            Collection4[lop - 1].Color = CogColorConstants.Red;
                            InspectResult[CameraNumber] = false;
                            Glob.MeasureResult[CameraNumber] = false;
                        }

                        Point_Label[lop].Run();
                        cog.StaticGraphics.Add(Point_Label[lop].GetOutputGraphicLabel(), "");
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
            }

            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            cog.StaticGraphics.AddList(Collection4, "");
            return InspectResult[CameraNumber];
        }
        #endregion 

        #region Inpection CAM1 
        public bool Inspect_Cam1(CogDisplay cog)
        {
            int CameraNumber = 1;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            CogGraphicCollection Collection4 = new CogGraphicCollection(); // 블롭
            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }

            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train2(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main2(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            if (TempModel.Dimension_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {
                CogCreateGraphicLabelTool[] Point_Label = new CogCreateGraphicLabelTool[10];
                CogCreateGraphicLabelTool[] Label = new CogCreateGraphicLabelTool[10];

                for (int lop = 1; lop < TempDistance.Length / Program.CameraList.Count(); lop++)
                {
                    if (TempDistanceEnable[CameraNumber, lop])
                    {
                        double ResultValue = 0;
                        ResultValue = TempDistance[CameraNumber, lop].DistanceValue(lop) * TempDistance_CalibrationValue[Glob.CamNumber, lop];

                        Point_Label[lop] = new CogCreateGraphicLabelTool();
                        Point_Label[lop].InputImage = cog.Image;
                        Point_Label[lop].InputGraphicLabel.X = TempDistance[CameraNumber, lop].GetX(lop);
                        Point_Label[lop].InputGraphicLabel.Y = TempDistance[CameraNumber, lop].GetY(lop);
                        Point_Label[lop].InputGraphicLabel.Text = ResultValue.ToString("F3");

                        Label[lop] = new CogCreateGraphicLabelTool();
                        Label[lop].InputImage = cog.Image;
                        Label[lop].InputGraphicLabel.X = 600;
                        Label[lop].InputGraphicLabel.Y = 170 + (80 * lop);
                        Label[lop].InputGraphicLabel.Text = $"{TempDistance[CameraNumber, lop].ToolName(lop)} : {ResultValue.ToString("F3")}";
                        Label[lop].Run();
                        Collection4.Add(Label[lop].GetOutputGraphicLabel());

                        if (TempDistance_LowValue[CameraNumber, lop] <= ResultValue && TempDistance_HighValue[CameraNumber, lop] >= ResultValue)
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Green;
                            Collection4[lop - 1].Color = CogColorConstants.Green;
                        }
                        else
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Red;
                            Collection4[lop - 1].Color = CogColorConstants.Red;
                            InspectResult[CameraNumber] = false;
                            Glob.MeasureResult[CameraNumber] = false;
                        }

                        Point_Label[lop].Run();
                        cog.StaticGraphics.Add(Point_Label[lop].GetOutputGraphicLabel(), "");
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
            }

            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            cog.StaticGraphics.AddList(Collection4, "");
            return InspectResult[CameraNumber];
            //if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            //else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            //if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            //else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            //for (int i = 0; i < Collection2.Count; i++)
            //    Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            //for (int i = 0; i < Collection3.Count; i++)
            //    Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            //for (int i = 0; i < Collection.Count; i++)
            //{
            //    if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
            //        Collection[i].Color = CogColorConstants.Blue;
            //}
            //cog.StaticGraphics.AddList(Collection, "");
            //cog.StaticGraphics.AddList(Collection2, "");
            //cog.StaticGraphics.AddList(Collection3, "");
            //return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM2 
        public bool Inspect_Cam2(CogDisplay cog)
        {
            int CameraNumber = 2;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            CogGraphicCollection Collection4 = new CogGraphicCollection(); // 치수
            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train3(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main3(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            if (TempModel.Dimension_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {
                CogCreateGraphicLabelTool[] Point_Label = new CogCreateGraphicLabelTool[10];
                CogCreateGraphicLabelTool[] Label = new CogCreateGraphicLabelTool[10];

                for (int lop = 1; lop < TempDistance.Length / Program.CameraList.Count(); lop++)
                {
                    if (TempDistanceEnable[CameraNumber, lop])
                    {
                        double ResultValue = 0;
                        ResultValue = TempDistance[CameraNumber, lop].DistanceValue(lop) * TempDistance_CalibrationValue[CameraNumber, lop];

                        Point_Label[lop] = new CogCreateGraphicLabelTool();
                        Point_Label[lop].InputImage = cog.Image;
                        Point_Label[lop].InputGraphicLabel.X = TempDistance[CameraNumber, lop].GetX(lop);
                        Point_Label[lop].InputGraphicLabel.Y = TempDistance[CameraNumber, lop].GetY(lop);
                        Point_Label[lop].InputGraphicLabel.Text = ResultValue.ToString("F3");

                        Label[lop] = new CogCreateGraphicLabelTool();
                        Label[lop].InputImage = cog.Image;
                        Label[lop].InputGraphicLabel.X = 600;
                        Label[lop].InputGraphicLabel.Y = 170 + (80 * lop);
                        Label[lop].InputGraphicLabel.Text = $"{TempDistance[CameraNumber, lop].ToolName(lop)} : {ResultValue.ToString("F3")}";
                        Label[lop].Run();
                        Collection4.Add(Label[lop].GetOutputGraphicLabel());

                        if (TempDistance_LowValue[CameraNumber, lop] <= ResultValue && TempDistance_HighValue[CameraNumber, lop] >= ResultValue)
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Green;
                            Collection4[lop - 1].Color = CogColorConstants.Green;
                        }
                        else
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Red;
                            Collection4[lop - 1].Color = CogColorConstants.Red;
                            InspectResult[CameraNumber] = false;
                            Glob.MeasureResult[CameraNumber] = false;
                        }

                        Point_Label[lop].Run();
                        cog.StaticGraphics.Add(Point_Label[lop].GetOutputGraphicLabel(), "");
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
            }
            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            cog.StaticGraphics.AddList(Collection4, "");
            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM3 
        public bool Inspect_Cam3(CogDisplay cog)
        {
            int CameraNumber = 3;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            CogGraphicCollection Collection4 = new CogGraphicCollection(); // 치수

            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }

            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train4(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main4(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            if (TempModel.Dimension_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {
                CogCreateGraphicLabelTool[] Point_Label = new CogCreateGraphicLabelTool[10];
                CogCreateGraphicLabelTool[] Label = new CogCreateGraphicLabelTool[10];

                for (int lop = 1; lop < TempDistance.Length / Program.CameraList.Count(); lop++)
                {
                    if (TempDistanceEnable[CameraNumber, lop])
                    {
                        double ResultValue = 0;
                        ResultValue = TempDistance[CameraNumber, lop].DistanceValue(lop) * TempDistance_CalibrationValue[CameraNumber, lop];

                        Point_Label[lop] = new CogCreateGraphicLabelTool();
                        Point_Label[lop].InputImage = cog.Image;
                        Point_Label[lop].InputGraphicLabel.X = TempDistance[CameraNumber, lop].GetX(lop);
                        Point_Label[lop].InputGraphicLabel.Y = TempDistance[CameraNumber, lop].GetY(lop);
                        Point_Label[lop].InputGraphicLabel.Text = ResultValue.ToString("F3");

                        Label[lop] = new CogCreateGraphicLabelTool();
                        Label[lop].InputImage = cog.Image;
                        Label[lop].InputGraphicLabel.X = 600;
                        Label[lop].InputGraphicLabel.Y = 170 + (80 * lop);
                        Label[lop].InputGraphicLabel.Text = $"{TempDistance[CameraNumber, lop].ToolName(lop)} : {ResultValue.ToString("F3")}";
                        Label[lop].Run();
                        Collection4.Add(Label[lop].GetOutputGraphicLabel());

                        if (TempDistance_LowValue[CameraNumber, lop] <= ResultValue && TempDistance_HighValue[CameraNumber, lop] >= ResultValue)
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Green;
                            Collection4[lop - 1].Color = CogColorConstants.Green;
                        }
                        else
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Red;
                            Collection4[lop - 1].Color = CogColorConstants.Red;
                            InspectResult[CameraNumber] = false;
                            Glob.MeasureResult[CameraNumber] = false;
                        }

                        Point_Label[lop].Run();
                        cog.StaticGraphics.Add(Point_Label[lop].GetOutputGraphicLabel(), "");
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
            }

            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            cog.StaticGraphics.AddList(Collection4, "");
            return InspectResult[CameraNumber];
        }
        #endregion 

        #region Inpection CAM4 
        public bool Inspect_Cam4(CogDisplay cog)
        {
            int CameraNumber = 4;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }

            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train5(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main5(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }
            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM5 
        public bool Inspect_Cam5(CogDisplay cog)
        {
            int CameraNumber = 5;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭
            CogGraphicCollection Collection4 = new CogGraphicCollection(); // 치수
            //CognexModelLoad();
            string[] temp = new string[30];
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, 0].ResultPoint(TempMulti[CameraNumber, 0].HighestResultToolNumber()), TempMulti[CameraNumber, 0].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, 0].HighestResultToolNumber());
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true)
                    {
                        if (TempMulti[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
                        {
                            InspectResult[CameraNumber] = false;
                            Glob.PatternResult[CameraNumber] = false;
                        }
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
                Glob.PatternResult[CameraNumber] = false;
            }

            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train6(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main6(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            if (TempModel.Dimension_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection))
            {
                CogCreateGraphicLabelTool[] Point_Label = new CogCreateGraphicLabelTool[10];
                CogCreateGraphicLabelTool[] Label = new CogCreateGraphicLabelTool[10];

                for (int lop = 1; lop < TempDistance.Length / Program.CameraList.Count(); lop++)
                {
                    if (TempDistanceEnable[CameraNumber, lop])
                    {
                        double ResultValue = 0;
                        ResultValue = TempDistance[CameraNumber, lop].DistanceValue(lop) * TempDistance_CalibrationValue[CameraNumber, lop];

                        Point_Label[lop] = new CogCreateGraphicLabelTool();
                        Point_Label[lop].InputImage = cog.Image;
                        Point_Label[lop].InputGraphicLabel.X = TempDistance[CameraNumber, lop].GetX(lop);
                        Point_Label[lop].InputGraphicLabel.Y = TempDistance[CameraNumber, lop].GetY(lop);
                        Point_Label[lop].InputGraphicLabel.Text = ResultValue.ToString("F3");

                        Label[lop] = new CogCreateGraphicLabelTool();
                        Label[lop].InputImage = cog.Image;
                        Label[lop].InputGraphicLabel.X = 600;
                        Label[lop].InputGraphicLabel.Y = 170 + (80 * lop);
                        Label[lop].InputGraphicLabel.Text = $"{TempDistance[CameraNumber, lop].ToolName(lop)} : {ResultValue.ToString("F3")}";
                        Label[lop].Run();
                        Collection4.Add(Label[lop].GetOutputGraphicLabel());

                        if (TempDistance_LowValue[CameraNumber, lop] <= ResultValue && TempDistance_HighValue[CameraNumber, lop] >= ResultValue)
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Green;
                            Collection4[lop - 1].Color = CogColorConstants.Green;
                        }
                        else
                        {
                            Point_Label[lop].OutputColor = CogColorConstants.Red;
                            Collection4[lop - 1].Color = CogColorConstants.Red;
                            InspectResult[CameraNumber] = false;
                            Glob.MeasureResult[CameraNumber] = false;
                        }

                        Point_Label[lop].Run();
                        cog.StaticGraphics.Add(Point_Label[lop].GetOutputGraphicLabel(), "");
                    }
                }
            }
            else
            {
                InspectResult[CameraNumber] = false;
            }
            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB NG"); };

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] == null)
                {
                    continue;
                }
                if (Collection[i].ToString() == "Cognex.VisionPro.CogGraphicLabel")
                    Collection[i].Color = CogColorConstants.Blue;
            }

            for (int i = 0; i < Collection2.Count; i++)
                Collection2[i].Color = Glob.PatternResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;
            for (int i = 0; i < Collection3.Count; i++)
                Collection3[i].Color = Glob.BlobResult[CameraNumber] == true ? CogColorConstants.Green : CogColorConstants.Red;

            cog.StaticGraphics.AddList(Collection, "");
            cog.StaticGraphics.AddList(Collection2, "");
            cog.StaticGraphics.AddList(Collection3, "");
            cog.StaticGraphics.AddList(Collection4, "");
            return InspectResult[CameraNumber];
        }
        #endregion


        public void DgvResult(DataGridView dgv, int camnumber, int cellnumber)
        {
            if (frm_toolsetup != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    if (TempBlobEnable[camnumber, i] == true)
                    {
                        if (TempBlobs[camnumber, i].ResultBlobCount() != TempBlobOKCount[camnumber, i]) // - 검사결과 NG
                        {
                            dgv.Rows[i].Cells[3].Value = $"{TempBlobs[camnumber, i].ResultBlobCount()}-({TempBlobOKCount[camnumber, i]})";
                            dgv.Rows[i].Cells[3].Style.BackColor = Color.Red;
                        }
                        else // - 검사결과 OK
                        {
                            dgv.Rows[i].Cells[3].Value = $"{TempBlobs[camnumber, i].ResultBlobCount()}-({TempBlobOKCount[camnumber, i]})";
                            dgv.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                        }
                    }
                    else
                    {
                        dgv.Rows[i].Cells[3].Value = "NOT USED";
                        dgv.Rows[i].Cells[3].Style.BackColor = SystemColors.Control;
                    }
                }
                for (int i = 0; i < 30; i++)
                {
                    if (TempMultiEnable[camnumber, i] == true)
                    {
                        if (TempMulti[camnumber, i].ResultPoint(TempMulti[camnumber, i].HighestResultToolNumber()) != null)
                        {
                            if (Glob.MultiInsPat_Result[camnumber, i] > TempMulti[camnumber, i].Threshold() * 100)
                            {
                                dgv.Rows[i].Cells[cellnumber].Value = Glob.MultiInsPat_Result[camnumber, i].ToString("F2");
                                dgv.Rows[i].Cells[cellnumber].Style.BackColor = Color.Lime;
                            }
                            else
                            {
                                dgv.Rows[i].Cells[cellnumber].Value = Glob.MultiInsPat_Result[camnumber, i].ToString("F2");
                                dgv.Rows[i].Cells[cellnumber].Style.BackColor = Color.Red;
                                InspectResult[camnumber] = false;
                            }
                        }
                        else
                        {
                            dgv.Rows[i].Cells[cellnumber].Value = "NG";
                            dgv.Rows[i].Cells[cellnumber].Style.BackColor = Color.Red;
                            InspectResult[camnumber] = false;
                        }
                    }
                    else
                    {
                        dgv.Rows[i].Cells[cellnumber].Value = "NOT USED";
                        dgv.Rows[i].Cells[cellnumber].Style.BackColor = SystemColors.Control;
                    }
                }
            }
            else
            {

            }

        }

        public void ImageSave1(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
                //string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                //if (!Directory.Exists(Root2))
               // {
                //    Directory.CreateDirectory(Root2);
               // }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave2(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
                //string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                //if (!Directory.Exists(Root2))
                //{
                //    Directory.CreateDirectory(Root2);
                //}
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

               // cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave3(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
               // string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
               // if (!Directory.Exists(Root2))
                //{
                //    Directory.CreateDirectory(Root2);
                //}
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave4(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
              //  string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                //if (!Directory.Exists(Root2))
                //{
                //    Directory.CreateDirectory(Root2);
                //}
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave5(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
                //string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                //if (!Directory.Exists(Root2))
               // {
                //    Directory.CreateDirectory(Root2);
               // }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave6(string Result, int CamNumber, CogDisplay cog)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
                //string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                //if (!Directory.Exists(Root2))
               // {
                //    Directory.CreateDirectory(Root2);
               // }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }

        public void DataSave1(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }
        public void DataSave2(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }
        public void DataSave3(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }
        public void DataSave4(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }
        public void DataSave5(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }
        public void DataSave6(string Time, int CamNumber)
        {
            //DATA 저장부분 TEST 후 적용 시키기.
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}";
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\Data_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{Time}");
            //Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")},CAM1,Point2.4,{Glob.CAM_Point1Value[0]},{Glob.CAM_Point2Value[0]},Point3.3,{Glob.CAM_Point3Value[0]},CAM3,Point2.4,{Glob.CAM_Point1Value[2]},{Glob.CAM_Point2Value[2]},Point3.3,{Glob.CAM_Point3Value[2]}");
            Writer.Close();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            btn_Stop.Enabled = false;
            btn_ToolSetUp.Enabled = true;
            btn_Model.Enabled = true;
            btn_SystemSetup.Enabled = true;
            btn_Status.Enabled = true;
            this.IO_DoWork = false;
        }

        private void btn_Model_Click(object sender, EventArgs e)
        {
            //MODEL FORM 열기.
            Frm_Model frm_model = new Frm_Model(Glob.RunnModel.Modelname(), this);
            frm_model.Show();
        }

        public void LightON()
        {
            if (LightControl.IsOpen == false)
            {
                return;
            }
            LightStats = true;
            string LightValue = string.Format("{0:D3}", 255);
            string LightValue2 = string.Format("{0:D3}", 255);
            LightValue = ":L" + 1 + LightValue + "\r\n";
            LightValue2 = ":L" + 2 + LightValue2 + "\r\n";
            LightControl.Write(LightValue.ToCharArray(), 0, LightValue.ToCharArray().Length);
            LightControl.Write(LightValue2.ToCharArray(), 0, LightValue2.ToCharArray().Length);
        }
        public void LightOFF()
        {
            if (LightControl.IsOpen == false)
            {
                return;
            }
            LightStats = false;
            string LightValue = string.Format("{0:D3}", 0);
            string LightValue2 = string.Format("{0:D3}", 0);
            LightValue = ":L" + 1 + LightValue + "\r\n";
            LightValue2 = ":L" + 2 + LightValue2 + "\r\n";
            LightControl.Write(LightValue.ToCharArray(), 0, LightValue.ToCharArray().Length);
            LightControl.Write(LightValue2.ToCharArray(), 0, LightValue2.ToCharArray().Length);
        }

        private void Frm_Main_KeyDown(object sender, KeyEventArgs e)
        {
            //****************************단축키 모음****************************//
            if (e.Control && e.KeyCode == Keys.T) //ctrl + t : 툴셋팅창 열기
                btn_ToolSetUp.PerformClick();
            if (e.Control && e.KeyCode == Keys.M) //ctrl + m : 모델창 열기
                btn_Model.PerformClick();
            if (e.KeyCode == Keys.Escape) // esc : 프로그램 종료
                btn_Exit.PerformClick();
        }

        private void btn_CamList_Click(object sender, EventArgs e)
        {
            Frm_CamSet frm_camset = new Frm_CamSet(this);
            if (frm_camset.ShowDialog() == DialogResult.OK)
            {
                //Camera Serial Number Setting 이후 프로그램 재시작하여, Camera 연결.
                Application.Restart(); //프로그램 재시작
            }
            else
            {

            }
        }

        private void Frm_Main_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_Log_Click(object sender, EventArgs e)
        {
            int jobNo = Convert.ToInt16((sender as Button).Tag);
            Main_TabControl.SelectedIndex = jobNo;
        }



        private void btn_Analyze_Click(object sender, EventArgs e)
        {
            frm_analyzeresult = new Frm_AnalyzeResult(this);
            frm_analyzeresult.Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            cdyDisplay.Image = TempCam[2].Run();
            //TempCam.Run();
        }

        public void SnapShot(int camNumber, CogDisplay cdy)
        {
            try
            {
                cdy.Image = TempCam[camNumber].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }

        public void SnapShot1()
        {
            try
            {
                cdyDisplay.Image = TempCam[0].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        public void SnapShot2()
        {
            try
            {
                cdyDisplay2.Image = TempCam[1].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        public void SnapShot3()
        {
            try
            {
                cdyDisplay3.Image = TempCam[2].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        public void SnapShot4()
        {
            try
            {
                cdyDisplay4.Image = TempCam[3].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        public void SnapShot5()
        {
            try
            {
                cdyDisplay5.Image = TempCam[4].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        public void SnapShot6()
        {
            try
            {
                cdyDisplay6.Image = TempCam[5].Run();
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }

 
        //private DateTime IO_RunTime = DateTime.Now;
        //private Int32 IO_CheckCount = 0;
        private void ReadInputSignal()
        {
            log.AddLogMessage(LogType.Infomation,0, "IO Read Started!"); ;
            while (IO_DoWork)
            {
                UInt32 iVal = 0;
                //UInt32 oVal = 0;
                CAXD.AxdiReadInportDword(0, 0, ref iVal);  // 입력신호 32점
                //CAXD.AxdoReadOutportDword(1, 0, ref oVal); // 출력신호 32점
                BitArray Inputs = new BitArray(BitConverter.GetBytes(iVal));
                //BitArray Outputs = new BitArray(BitConverter.GetBytes(oVal));

                for (Int32 i = 0; i < gbool_di.Length; i++)
                {
                    Boolean fired = Inputs[i];
                    if (gbool_di[i] == fired) continue;
                    if ((DateTime.Now - this.TrigTime[i]).TotalMilliseconds < 1000) continue; // 1000ms 이내에 Trig되면 패스
                    this.TrigTime[i] = DateTime.Now;
                    gbool_di[i] = fired;
                    //inputBtn[i].BackColor = fired ? Color.Lime : SystemColors.Control;
                    if (!fired) continue;

                    switch (i)
                    {
                        case 0: //1번째 라인스캔 카메라 촬영 신호 Cam 1
                            Task.Run(() => { ShotAndInspect_Cam1(); });
                            break;
                        case 1: //사이드 라인스캔 카메라 촬영신호 Cam 2 & Cam 3
                            Task.Run(() => { ShotAndInspect_Cam2(); });
                            Task.Run(() => { ShotAndInspect_Cam3(); });
                            break;
                        case 2: //4번촬영
                            Task.Run(() => { ShotAndInspect_Cam4(cdyDisplay4); });
                            break;
                        case 3: //4번촬영
                            Task.Run(() => { ShotAndInspect_Cam4(cdyDisplay4_2); });
                            break;
                        case 4: //4번촬영
                            Task.Run(() => { ShotAndInspect_Cam4(cdyDisplay4_3); });
                            break;
                        case 5: //5번촬영
                            Task.Run(() => { ShotAndInspect_Cam5(cdyDisplay5); });
                            break;
                        case 6: //5번촬영
                            Task.Run(() => { ShotAndInspect_Cam5(cdyDisplay5_1); });
                            break;
                        case 7:
                            Task.Run(() => { ShotAndInspect_Cam6(cdyDisplay6); });
                            break;
                        case 8:
                            Task.Run(() => { ShotAndInspect_Cam6(cdyDisplay6_2); });
                            break;
                        case 9:
                            Task.Run(() => { ShotAndInspect_Cam6(cdyDisplay6_3); });
                            break;
                        case 10:
                            Task.Run(() => { ShotAndInspect_Cam6(cdyDisplay6_4); });
                            break;
                        case 11:
                            Task.Run(() => { ShotAndInspect_Cam6(cdyDisplay6_5); });
                            break;
                    }
                }

                Thread.Sleep(1);

                //IO_CheckCount++;
                //if (IO_CheckCount >= 1000)
                //{
                //    Debug.WriteLine($"시간={(DateTime.Now - IO_RunTime).TotalMilliseconds}, 횟수={IO_CheckCount}");
                //    IO_CheckCount = 0;
                //    IO_RunTime = DateTime.Now;
                //}
            }

            Debug.WriteLine("IO Read Ended!");
        }

        /*
        private void timerSensor_Tick(object sender, EventArgs e)
        {
            short nIndex = 0;
            uint uDataHigh = 0;
            uint uDataLow = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            int nBoardNo = 0;
            int nModulePos = 0;
            uint uModuleID = 0;

            CAXD.AxdInfoGetModule(0, ref nBoardNo, ref nModulePos, ref uModuleID);

            switch ((AXT_MODULE)uModuleID)
            {
                case AXT_MODULE.AXT_SIO_DI32:
                case AXT_MODULE.AXT_SIO_RDI32:
                case AXT_MODULE.AXT_SIO_RSIMPLEIOMLII:
                case AXT_MODULE.AXT_SIO_RDO16AMLII:
                case AXT_MODULE.AXT_SIO_RDO16BMLII:
                case AXT_MODULE.AXT_SIO_DI32_P:
                case AXT_MODULE.AXT_SIO_RDI32RTEX:
                    //++
                    // Read inputting signal in WORD
                    for (int i = 0; i < 12; i++)
                    {
                        re_gbool_di[i] = gbool_di[i];
                    }

                    CAXD.AxdiReadInportWord(0, 0, ref uDataHigh);
                    CAXD.AxdiReadInportWord(0, 1, ref uDataLow);

                    for (nIndex = 0; nIndex < 12; nIndex++)
                    {
                        // Verify the last bit value of data read
                        uFlagHigh = uDataHigh & 0x0001;
                        uFlagLow = uDataLow & 0x0001;

                        // Shift rightward by bit by bit
                        uDataHigh = uDataHigh >> 1;
                        uDataLow = uDataLow >> 1;

                        // Updat bit value in control
                        if (uFlagHigh == 1)
                        {
                            gbool_di[nIndex] = true;
                            inputBtn[nIndex].BackColor = Color.Lime;
                        }
                        else
                        {
                            gbool_di[nIndex] = false;
                            inputBtn[nIndex].BackColor = SystemColors.Control;
                        }
                    }

                    for (int i = 0; i < 12; i++)
                    {
                        if (gbool_di[i] != re_gbool_di[i] && gbool_di[i] == true)
                        {
                            log.AddLogMessage(LogType.Infomation, 0, $"Signal Number : {i.ToString()}");
                            switch (i)
                            {
                                case 0: //1번째 라인스캔 카메라 촬영 신호 Cam 1
                                    cdyDisplay.Image = null;
                                    cdyDisplay.InteractiveGraphics.Clear();
                                    cdyDisplay.StaticGraphics.Clear();

                                    snap1 = new Thread(new ThreadStart(ShotAndInspect_Cam1));
                                    snap1.Priority = ThreadPriority.Highest;
                                    snap1.Start();
                                    break;
                                case 1: //사이드 라인스캔 카메라 촬영신호 Cam 2 & Cam 3
                                    cdyDisplay2.Image = null;
                                    cdyDisplay2.InteractiveGraphics.Clear();
                                    cdyDisplay2.StaticGraphics.Clear();

                                    snap2 = new Thread(new ThreadStart(ShotAndInspect_Cam2));
                                    snap2.Priority = ThreadPriority.Highest;
                                    snap2.Start();

                                    cdyDisplay3.Image = null;
                                    cdyDisplay3.InteractiveGraphics.Clear();
                                    cdyDisplay3.StaticGraphics.Clear();

                                    snap3 = new Thread(new ThreadStart(ShotAndInspect_Cam3));
                                    snap3.Priority = ThreadPriority.Highest;
                                    snap3.Start();
                                    break;
                                case 2: //4번촬영
                                    cdyDisplay4.Image = null;
                                    cdyDisplay4.InteractiveGraphics.Clear();
                                    cdyDisplay4.StaticGraphics.Clear();

                                    snap4_1 = new Thread(() => ShotAndInspect_Cam4(cdyDisplay4));
                                    snap4_1.Priority = ThreadPriority.Highest;
                                    snap4_1.Start();
                                    break;
                                case 3: //5번촬영
                                    cdyDisplay4_2.Image = null;
                                    cdyDisplay4_2.InteractiveGraphics.Clear();
                                    cdyDisplay4_2.StaticGraphics.Clear();

                                    snap4_2 = new Thread(() => ShotAndInspect_Cam4(cdyDisplay4_2));
                                    snap4_2.Priority = ThreadPriority.Highest;
                                    snap4_2.Start();
                                    break;
                                case 4: //4번촬영
                                    cdyDisplay4_3.Image = null;
                                    cdyDisplay4_3.InteractiveGraphics.Clear();
                                    cdyDisplay4_3.StaticGraphics.Clear();

                                    snap4_3 = new Thread(() => ShotAndInspect_Cam4(cdyDisplay4_3));
                                    snap4_3.Priority = ThreadPriority.Highest;
                                    snap4_3.Start();
                                    break;
                                case 5: //5번촬영
                                    cdyDisplay5.Image = null;
                                    cdyDisplay5.InteractiveGraphics.Clear();
                                    cdyDisplay5.StaticGraphics.Clear();

                                    snap5_1 = new Thread(() => ShotAndInspect_Cam5(cdyDisplay5));
                                    snap5_1.Priority = ThreadPriority.Highest;
                                    snap5_1.Start();
                                    break;
                                case 6: //4번촬영
                                    cdyDisplay5_1.Image = null;
                                    cdyDisplay5_1.InteractiveGraphics.Clear();
                                    cdyDisplay5_1.StaticGraphics.Clear();

                                    snap5_2 = new Thread(() => ShotAndInspect_Cam5(cdyDisplay5_1));
                                    snap5_2.Priority = ThreadPriority.Highest;
                                    snap5_2.Start();
                                    break;
                                case 7:
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();

                                    snap6 = new Thread(new ThreadStart(ShotAndInspect_Cam6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                    break;
                                case 8:
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();

                                    snap6 = new Thread(new ThreadStart(ShotAndInspect_Cam6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                    break;

                                case 9:
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();

                                    snap6 = new Thread(new ThreadStart(ShotAndInspect_Cam6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                    break;
                                case 10:
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();

                                    snap6 = new Thread(new ThreadStart(ShotAndInspect_Cam6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                    break;
                                case 11:
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();

                                    snap6 = new Thread(new ThreadStart(ShotAndInspect_Cam6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                    break;
                            }
                        }
                    }
                    break;

                case AXT_MODULE.AXT_SIO_DB32P:
                case AXT_MODULE.AXT_SIO_DB32T:
                case AXT_MODULE.AXT_SIO_RDB32T:
                case AXT_MODULE.AXT_SIO_RDB32RTEX:
                case AXT_MODULE.AXT_SIO_RDB96MLII:
                case AXT_MODULE.AXT_SIO_RDB128MLII:
                    //++
                    // Read inputting signal in WORD
                    log.AddLogMessage(LogType.Infomation, 0, "axt sio rdb12ml11");
                    CAXD.AxdiReadInportWord(0, 0, ref uDataHigh);

                    for (nIndex = 0; nIndex < 12; nIndex++)
                    {
                        // Verify the last bit value of data read
                        uFlagHigh = uDataHigh & 0x0001;

                        // Shift rightward by bit by bit
                        uDataHigh = uDataHigh >> 1;

                        // Updat bit value in control
                        if (uFlagHigh == 1)
                            inputBtn[nIndex].BackColor = Color.Lime;
                        else
                            inputBtn[nIndex].BackColor = SystemColors.Control;
                    }
                    break;
            }
        }
        */


        private bool SelectHighIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(1, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        CAXD.AxdoWriteOutportBit(1, nIndex, uValue);
                        break;

                    default:
                        return false;
                }
            }

            return true;
        }

        private void btn_OUTPUT0_CheckedChanged(object sender, EventArgs e)
        {
            int jobNo = Convert.ToInt16((sender as System.Windows.Forms.CheckBox).Tag);
            SelectHighIndex(jobNo, (uint)outputBtn[jobNo].CheckState);
        }
    }
}
public static class ExtensionMethods
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvtype = dgv.GetType();
        PropertyInfo pi = dgvtype.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }
}
