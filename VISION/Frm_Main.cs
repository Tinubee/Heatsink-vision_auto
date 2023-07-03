﻿using System;
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
using Microsoft.VisualBasic.Logging;
using Microsoft.VisualBasic.Devices;
using VISION.UI;
using Euresys.clseremc;
using Microsoft.Win32;
using System.ComponentModel.Design;

namespace VISION
{
    public delegate void EventCallBack(Bitmap bmp);
    public partial class Frm_Main : Form
    {
        public KimLib.Log log = new KimLib.Log();
        private Class_Common cm { get { return Program.cm; } } //에러 메세지 보여주기.
        internal Frm_ToolSetUp frm_toolsetup; //툴셋업창 화면
        internal Frm_AnalyzeResult frm_analyzeresult;

        private CogImage8Grey Fiximage; //PMAlign툴의 결과이미지(픽스쳐이미지)
        private string FimageSpace; //PMAlign툴 SpaceName(보정하기위해)

        private Camera[] TempCam;
        private Mask[] TempMask;


        public HeatSinkMainDisplay HeatSinkMainDisplay = new HeatSinkMainDisplay();
        public CogDisplay[] TempCogDisplay;

        private Model TempModel; //모델
        private Blob[,] TempBlobs; //블롭툴
        private Line[,] TempLines; //라인툴
        private Circle[,] TempCircles; //써클툴
        private MultiPMAlign[,] TempMulti;
        private Distance[,] TempDistance;
        private Caliper[,] TempCaliper;


        private bool[,] TempLineEnable; //라인툴 사용여부
        private bool[,] TempBlobEnable;//블롭툴 사용여부
        private bool[,] TempBlobNGOKChange;
        private bool[,] TempCircleEnable; //써클툴 사용여부
        private bool[,] TempMultiEnable;
        private bool[,] TempDistanceEnable;
        private bool[,] TempCaliperEnable;

        private int[,] TempBlobOKCount;//블롭툴 설정갯수
        private int[,] TempBlobFixPatternNumber;
        private int[,] TempMultiOrderNumber;

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

        public bool[] InspectFlag = new bool[6]; //검사 플래그
        public int camcount = 6;

        Label[] OK_Label;
        Label[] NG_Label;
        Label[] TOTAL_Label;
        Label[] NGRATE_Label;

        //유레시스 보드 시리얼포트 연결 관련.
        // Handle to the serial port
        IntPtr serialRef = System.IntPtr.Zero;
        // Index of the serial port
        UInt32 serialIndex = 0;
        // Number of serial ports
        UInt32 numPorts = 0;

        Tuple<UInt32, String>[] CL_BAUDRATES =
       {
            Tuple.Create(CL.BAUDRATE_9600, "9600"),
            Tuple.Create(CL.BAUDRATE_19200, "19200"),
            Tuple.Create(CL.BAUDRATE_38400, "38400"),
            Tuple.Create(CL.BAUDRATE_57600, "57600"),
            Tuple.Create(CL.BAUDRATE_115200, "115200"),
            Tuple.Create(CL.BAUDRATE_230400, "230400"),
            Tuple.Create(CL.BAUDRATE_460800, "460800"),
            Tuple.Create(CL.BAUDRATE_921600, "921600"),
        };
        //연결 가능한 포트.
        List<string> availablePort = new List<string>();
        List<string> baudRates = new List<string>();


        // Trigger
        private Boolean IO_DoWork = false;

        private string[] IOModel = new string[2];
        private Button[] inputBtn;
        private System.Windows.Forms.CheckBox[] outputBtn;

        public readonly static uint INFINITE = 0xFFFFFFFF;
        public readonly static uint STATUS_WAIT_0 = 0x00000000;
        public readonly static uint WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0);

        private DateTime[] TrigTime = new DateTime[12];
        public bool[] gbool_di = new bool[12];
        public bool[] re_gbool_di = new bool[12];

        public SerialPort[] LightControl; //조명컨트롤러

        [DllImport("kernel32", EntryPoint = "WaitForSingleObject", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint WaitForSingleObject(uint hHandle, uint dwMilliseconds);

        [DllImport("KERNEL32", EntryPoint = "SetEvent", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetEvent(long hEvent);

        public Frm_Main()
        {
            Glob = PGgloble.getInstance; //전역변수 사용
            Process.Start($"{Glob.LOADINGFROM}");
            Debug.WriteLine("프로그램 시작");
            InitializeComponent();
            ColumnHeader h = new ColumnHeader();
            LightControl = new SerialPort[4] { LightControl1, LightControl2, LightControl3, LightControl4 };
            StandFirst();
            Debug.WriteLine("StartFirst 완료.");
            CamSet();
            Debug.WriteLine("CamSet완료.");
            Glob.RunnModel = new Model(); //코그넥스 모델 확인.
            Debug.WriteLine("Cognex 모델 확인 완료.");

            Glob.G_MainForm = this;

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

            Debug.WriteLine("프로그램 셋팅 로드");
            LoadSetup(); //프로그램 셋팅 로드.

            Debug.WriteLine("타이머 시작");
            timer_Setting.Start(); //타이머에서 계속해서 확인하는 것들

            Debug.WriteLine("조명 컨트롤 연결 시작.");
            Initialize_LightControl(); //조명컨틀로 초기화

            //GeniCam 설정
            Debug.WriteLine("GeniCam 초기화.");
            Initialize_GeniCam();

            //Camfile 셋팅.
            Set_GeniCam(Glob.CurruntModelName);

            Debug.WriteLine("코그넥스 모델 로드");
            CognexModelLoad(); //코그넥스 모델 로드.

            Debug.WriteLine("PLC IO READ LOAD");
            DigitalIO_Load();//IO Load

            Debug.WriteLine("PLC IO SET MODULE");
            SelectModule();

            AllCameraOneShot();

            Debug.WriteLine($"현재모델 이름 : {Glob.CurruntModelName}");

            log.AddLogMessage(LogType.Infomation, 0, "Vision Program Start");
            Process[] myProcesses = Process.GetProcessesByName("LoadingForm_KHM"); //로딩폼 죽이기.
            if (myProcesses.LongLength > 0)
            {
                myProcesses[0].Kill();
            }
        }

        public string readBuffer(IntPtr serial)
        {
            try
            {
                // Retrieve the number of bytes in the read buffer
                UInt32 numBytes;
                CL.GetNumBytesAvail(serial, out numBytes);

                if (numBytes == 0)
                {
                    return "<NO DATA>";
                }
                else
                {
                    // Retrieve the data in the read buffer
                    IntPtr receivedData = Marshal.AllocHGlobal((int)numBytes + 1);
                    CL.SerialRead(serialRef, receivedData, out numBytes, 5000);
                    String data = Marshal.PtrToStringAnsi(receivedData, (int)numBytes - 1);
                    Marshal.FreeHGlobal(receivedData);
                    return data;
                }
            }
            catch (Euresys.clSerialException error)
            {
                return error.Message;
            }
        }

        public void sendCommandToBoard(string cmd)
        {
            try
            {
                // Write the command to the port
                cmd += Convert.ToChar(13);
                UInt32 numBytes = (UInt32)cmd.Length;
                CL.SerialWrite(serialRef, cmd, out numBytes, 5000);
            }
            catch (Euresys.clSerialException error)
            {
                Debug.WriteLine(error.Message);
                return;
            }
        }

        public void Set_GeniCam(string modelName)
        {
            //DualBase #0 Port A & B
            try
            {
                for (int lop = 0; lop < availablePort.Count(); lop++)
                {
                    //open serial port
                    CL.SerialInit((UInt32)lop, out serialRef);
                    Debug.WriteLine($"out serial : {serialRef}");

                    UInt32 supportedBaudRates;
                    CL.GetSupportedBaudRates(serialRef, out supportedBaudRates);

                    foreach (var clBaudRate in CL_BAUDRATES)
                    {
                        if ((supportedBaudRates & clBaudRate.Item1) != 0)
                        {
                            baudRates.Add(clBaudRate.Item2);
                        }
                    }


                    String selectedBaudRate = "9600";
                    UInt32 baudRate = 0;
                    foreach (var clBaudRate in CL_BAUDRATES)
                    {
                        if (selectedBaudRate == clBaudRate.Item2)
                        {
                            baudRate = clBaudRate.Item1;
                        }
                    }


                    CL.SetBaudRate(serialRef, baudRate);
                    Debug.WriteLine($"{serialRef} set baudrate");


                    //sendCommand
                    Debug.WriteLine($"GeniCamSet Model : {modelName}");
                    string setExposureCommand = modelName == "shield" ? "I=38" : "I=76";
                    Debug.WriteLine($"command : {setExposureCommand}");


                    sendCommandToBoard("");
                    string first = readBuffer(serialRef);
                    Debug.WriteLine($"read first buffer : {first}");

                    sendCommandToBoard(setExposureCommand);
                    string second = readBuffer(serialRef);
                    Debug.WriteLine($"read second buffer : {second}");

                    //close port
                    CL.SerialClose(serialRef);
                }

            }
            catch (Euresys.clSerialException error)
            {
                Debug.WriteLine(error.Message);
                log.AddLogMessage(LogType.Error, 0, error.Message);
            }
        }

        public void Initialize_GeniCam()
        {
            // Add path to clseremc in DLL search path
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\cameralink"))
            {
                CL.SetDllDirectory((string)registryKey.GetValue("CLSERIALPATH"));
            }

            // Retrieve the number of serial ports
            try
            {
                CL.GetNumSerialPorts(out numPorts);
            }
            catch (Euresys.clSerialException error)
            {
                Debug.WriteLine($"Error : {error.Message}");
            }
            catch (System.Exception error)
            {
                Debug.WriteLine($"Error : {error.Message}");
                return;
            }

            // Retrieve the identifier of each port           
            String portIdentifier = "";
            for (UInt32 i = 0; i < numPorts; i++)
            {
                UInt32 bufferSize = 0;
                try
                {
                    // Retrieve the buffer size
                    CL.GetSerialPortIdentifier(i, System.IntPtr.Zero, out bufferSize);
                }
                catch (Euresys.clSerialException error)
                {
                    if (error.Status != CL.ERR_BUFFER_TOO_SMALL)
                    {
                        MessageBox.Show(error.Message, "GrablinkSerialCommunication error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }
                IntPtr textPort = Marshal.AllocHGlobal((int)bufferSize + 1);
                try
                {
                    // Retrieve the port identifier
                    CL.GetSerialPortIdentifier(i, textPort, out bufferSize);
                    portIdentifier = Marshal.PtrToStringAnsi(textPort);
                    //if(portIdentifier.Contains("DualBase#0"))
                    //{
                    availablePort.Add(portIdentifier);
                    //}
                    //availablePorts.Items.Add(portIdentifier);
                }
                catch (Euresys.clSerialException error)
                {
                    MessageBox.Show(error.Message, "GrablinkSerialCommunication error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Marshal.FreeHGlobal(textPort);
            }
        }

        public void Initialize_LightControl()
        {
            try
            {
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");
                INIControl setting = new INIControl(Glob.SETTING);
                for (int i = 0; i < LightControl.Count(); i++)
                {
                    if (LightControl[i].IsOpen == true)
                    {
                        LightControl[i].Close();
                    }
                    LightControl[i].BaudRate = Convert.ToInt32(Glob.BaudRate[i]);
                    LightControl[i].Parity = Parity.None;
                    LightControl[i].DataBits = Convert.ToInt32(Glob.DataBit[i]);
                    LightControl[i].StopBits = StopBits.One;
                    LightControl[i].PortName = Glob.PortName[i];
                    LightControl[i].Open();
                    if (i == 3)
                    {
                        LCP24_150DC(LightControl[i], "0", "0000");
                    }
                    else
                    {
                        LCP_100DC(LightControl[i], "1", "f", "0000");
                        LCP_100DC(LightControl[i], "2", "f", "0000");
                    }
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {ee.Message}");
            }
        }

        //LCP-100DC 모델 포트, 채널번호, 종류(o-출력on,f-출력off,d-data전송),데이터값(4자리 10진수 0~100)
        public void LCP_100DC(SerialPort control, string channel, string dataType, string lightValue)
        {
            string STX = $"{Convert.ToChar(2)}";
            string ETX = $"{Convert.ToChar(3)}";

            string sandCmd = $"{STX}{channel}{dataType}{lightValue}{ETX}";

            control.WriteLine(sandCmd);
        }

        //LCP-100DC 모델 포트, 채널번호,데이터값(4자리숫자 0~1023)
        public void LCP24_150DC(SerialPort control, string channel, string lightValue)
        {
            string STX = $"{Convert.ToChar(2)}";
            string ETX = $"{Convert.ToChar(3)}";

            string sandCmd = $"{STX}{channel}w{lightValue}{ETX}";

            control.WriteLine(sandCmd);
        }

        private void Btn_LightOnOff_Click(object sender, EventArgs e)
        {
            //Port 1 , ch1 - 라인카메라 왼쪽. -2
            //Port 1 , ch2 - 라인카메라 오른쪽. -2
            //Port 2 , ch1 - top1 조명.  - 1
            //Port 3, ch1 - AreaCam 왼쪽. -3
            //Port 3, ch2 - AreaCam 오른쪽. -3
            //LightControlSendCommand(LightControl[Glob.LightControlNumber], "1f0000");

            // 0000 ~ 1023
            //Port 4 , ch0 - 백라이트 값: 0792 - 4
            //LightControlSendCommand(LightControl[Glob.LightControlNumber], "2w0000");

            //백라이트 77% - 2개모델 고정.
            //탑조명 heat 탑조명 10% ,옆면 10%, 에어리어 99% //shield  탑조명 99%, 옆면 0%, 에어리어 0%
        }

        public void LightControlSendCommand(SerialPort control, string command)
        {
            //LightOn = {(Int32)정보.채널}f0000
            //LightOff = $"{(Int32)정보.채널}o0000"
            string STX = $"{Convert.ToChar(2)}";
            string ETX = $"{Convert.ToChar(3)}";

            string sandCmd = $"{STX}{command}{ETX}";
            Debug.WriteLine(sandCmd);

            control.WriteLine($"{STX}{command}{ETX}");
        }

        private void CamSet()
        {
            try
            {
                CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();
                CogAcqFifoTool fifoTool = new CogAcqFifoTool();

                Debug.WriteLine(frameGrabbers.Count);
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
                        }
                        else
                        {
                            CAXD.AxdiInterruptGetModuleEnable(0, ref uUse);
                            if (uUse == (uint)AXT_USE.ENABLE)
                            {
                                //SelectMessage();
                            }
                            else
                                CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);

                            CAXD.AxdiInterruptEdgeGetWord(0, 1, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataLow);

                            CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
                            CAXD.AxdiInterruptEdgeGetWord(0, 1, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataLow);
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                    case AXT_MODULE.AXT_SIO_DO32T_P:
                    case AXT_MODULE.AXT_SIO_RDO32RTEX:
                        //++
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
                        }
                        break;

                    case AXT_MODULE.AXT_SIO_DB32P:
                    case AXT_MODULE.AXT_SIO_DB32T:
                    case AXT_MODULE.AXT_SIO_RDB128MLII:
                    case AXT_MODULE.AXT_SIO_RDB32T:
                    case AXT_MODULE.AXT_SIO_RDB32RTEX:
                    case AXT_MODULE.AXT_SIO_RDB96MLII:
                        CAXD.AxdiInterruptGetModuleEnable(0, ref uUse);
                        CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.UP_EDGE, ref uDataHigh);
                        CAXD.AxdiInterruptEdgeGetWord(0, 0, (uint)AXT_DIO_EDGE.DOWN_EDGE, ref uDataHigh);
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
                                }
                            }
                        }
                    }
                    else
                    {
                        log.AddLogMessage(LogType.Error, 0, "Module not exist.");
                        return false;
                    }
                }
            }
            else
            {
                log.AddLogMessage(LogType.Error, 0, "Open Error!");
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
                MainPanel.Controls.Add(HeatSinkMainDisplay);
                HeatSinkMainDisplay.Dock = DockStyle.Fill;
                TempCogDisplay = new CogDisplay[6] { HeatSinkMainDisplay.cdyDisplay, HeatSinkMainDisplay.cdyDisplay2, HeatSinkMainDisplay.cdyDisplay3, HeatSinkMainDisplay.cdyDisplay4, HeatSinkMainDisplay.cdyDisplay5, HeatSinkMainDisplay.cdyDisplay6 };

                INIControl Modellist = new INIControl(Glob.MODELLIST); ;
                INIControl CFGFILE = new INIControl(Glob.CONFIGFILE); ;

                INIControl setting = new INIControl(Glob.SETTING);
                string LastModel = CFGFILE.ReadData("LASTMODEL", "NAME"); //마지막 사용모델 확인.
                Glob.CurruntModelName = LastModel;
                //확인 필요. - LastModel Name 변수에 들어오는 String값 확인하기.
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{LastModel}\\CamSet.ini");
                for (int i = 0; i < camcount; i++)
                {
                    Glob.FlipImageTool[i] = (CogIPOneImageTool)CogSerializer.LoadObjectFromFile(Glob.MODELROOT + $"\\{LastModel}\\Cam{i}\\FlipImage.vpp");
                    Glob.RunnModel.Loadmodel(LastModel, Glob.MODELROOT, i); //VISION TOOL LOAD
                }

                Glob.ImageSaveRoot = setting.ReadData("SYSTEM", "Image Save Root"); //이미지 저장 경로
                Glob.DataSaveRoot = setting.ReadData("SYSTEM", "Data Save Root"); //데이터 저장 경로
                Glob.ImageSaveDay = Convert.ToInt32(setting.ReadData("SYSTEM", "Image Save Day")); //이미지 보관일수

                //****************************COMPORT 연결관련****************************//
                for (int i = 0; i < Glob.PortName.Count(); i++)
                {
                    Glob.PortName[i] = setting.ReadData("COMMUNICATION", $"Port number{i}");
                    Glob.Parity[i] = setting.ReadData("COMMUNICATION", $"Parity Check{i}");
                    Glob.StopBits[i] = setting.ReadData("COMMUNICATION", $"Stop bits{i}");
                    Glob.DataBit[i] = setting.ReadData("COMMUNICATION", $"Data Bits{i}");
                    Glob.BaudRate[i] = setting.ReadData("COMMUNICATION", $"Baud Rate{i}");
                    Glob.LightChAndValue[i, 0] = Convert.ToInt32(CamSet.ReadData($"LightControl{i}", "CH1"));
                    Glob.LightChAndValue[i, 1] = Convert.ToInt32(CamSet.ReadData($"LightControl{i}", "CH2"));
                }

                //****************************조명 채널****************************//
                //Glob.LightCH1 = setting.ReadData("LightControl", "CH1");
                //Glob.LightCH2 = setting.ReadData("LightControl", "CH1");

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

        public void ScratchErrorInit()
        {
            if (frm_toolsetup == null)
            {
                if (Glob.firstInspection[0])
                {
                    Debug.WriteLine("스크레치 에러 초기화 1");
                    Glob.scratchError[0] = false;
                }
                else
                {
                    Debug.WriteLine("스크레치 에러 초기화 2");
                    Glob.scratchError[1] = false;
                }
            }
        }

        public void ScratchErrorSet()
        {
            if (Glob.firstInspection[0])
            {
                Debug.WriteLine("Glob.scratchError[0] : true");
                Glob.scratchError[0] = true;
            }
            else
            {
                Debug.WriteLine("Glob.scratchError[1] : true");
                Glob.scratchError[1] = true;
            }
        }

        public void NoScratchErrorInit()
        {
            if (frm_toolsetup == null)
            {
                if (Glob.firstInspection[1])
                {
                    Glob.noScratchError[0] = false;
                }
                else
                {
                    Glob.noScratchError[1] = false;
                }
            }
        }

        public void NoScratchErrorSet()
        {
            if (Glob.firstInspection[1])
            {
                Glob.noScratchError[0] = true;
            }
            else
            {
                Glob.noScratchError[1] = true;
            }
        }

        public void 최종결과표시(bool result)
        {
            HeatSinkMainDisplay.lb_최종결과.Text = result ? "OK" : "NG";
            HeatSinkMainDisplay.lb_최종결과.ForeColor = result ? Color.Lime : Color.Red;
        }

        public void ShotAndInspect_Cam1(int shotNumber)
        {
            try
            {
                int funCamNumber = 0;
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                TempCogDisplay[funCamNumber].Image = TempCam[0].Run();
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                //조명꺼주기
                LCP_100DC(LightControl[1], "1", "f", "0000");

                ScratchErrorInit();

                if (TempCogDisplay[0].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 1");
                    return;
                }
                if (Inspect_Cam0(TempCogDisplay[0], shotNumber) == true) // 검사 결과
                {
                    Debug.WriteLine("CAM1 검사결과 OK");
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam1_Result.BackColor = Color.Lime;
                        HeatSinkMainDisplay.lb_Cam1_Result.Text = "O K";
                        OK_Count[0]++;
                        if (Glob.OKImageSave)
                            ImageSave1("OK", 1, TempCogDisplay[funCamNumber]);
                    });
                }
                else
                {
                    Debug.WriteLine("CAM1 검사결과 NG");
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam1_Result.BackColor = Color.Red;
                        HeatSinkMainDisplay.lb_Cam1_Result.Text = "N G";
                        NG_Count[0]++;
                        if (Glob.NGImageSave)
                            ImageSave1("NG", 1, TempCogDisplay[funCamNumber]);
                    });
                    if (!Glob.statsOK)
                    {
                        Debug.WriteLine("cam1 스크레치 에러 set");
                        ScratchErrorSet();
                    }

                }

                InspectTime[0].Stop();
                InspectFlag[0] = false;

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam1_InsTime.Text = InspectTime[0].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 1 Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam2(int shotNumber)
        {
            int funCamNumber = 1;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                Glob.FlipImageTool[funCamNumber].InputImage = TempCam[funCamNumber].Run();
                Glob.FlipImageTool[funCamNumber].Run();

                TempCogDisplay[funCamNumber].Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                Debug.WriteLine("2번카메라 조명 끄기 시작");
                LCP_100DC(LightControl[0], "1", "f", "0000");
                Debug.WriteLine("2번카메라 조명 끄기 종료");

                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 2");
                    return;
                }
                if (Inspect_Cam1(TempCogDisplay[funCamNumber], shotNumber) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam2_Result.BackColor = Color.Lime;
                        HeatSinkMainDisplay.lb_Cam2_Result.Text = "O K";
                        OK_Count[1]++;
                        if (Glob.OKImageSave)
                            ImageSave2("OK", 2, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam2_Result.BackColor = Color.Red;
                        HeatSinkMainDisplay.lb_Cam2_Result.Text = "N G";
                        NG_Count[1]++;
                        if (Glob.NGImageSave)
                            ImageSave2("NG", 2, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage);
                    });
                    if (!Glob.statsOK)
                    {
                        Debug.WriteLine("cam2 스크레치 에러 set");
                        ScratchErrorSet();
                    }
                    //검사 결과 NG
                }

                InspectTime[1].Stop();
                InspectFlag[1] = false;

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam2_InsTime.Text = InspectTime[1].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 2 Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam3(int shotNumber)
        {
            int funCamNumber = 2;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();
                Glob.FlipImageTool[funCamNumber].InputImage = TempCam[funCamNumber].Run();
                Glob.FlipImageTool[funCamNumber].Run();

                TempCogDisplay[funCamNumber].Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                Debug.WriteLine("3번카메라 조명 끄기 시작");
                LCP_100DC(LightControl[0], "2", "f", "0000");
                Debug.WriteLine("3번카메라 조명 끄기 종료");

                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (Inspect_Cam2(TempCogDisplay[funCamNumber], shotNumber) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam3_Result.BackColor = Color.Lime;
                        HeatSinkMainDisplay.lb_Cam3_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave3("OK", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam3_Result.BackColor = Color.Red;
                        HeatSinkMainDisplay.lb_Cam3_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave3("NG", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage);

                        if (!Glob.statsOK)
                        {
                            Debug.WriteLine("cam3 스크레치 에러 set");
                            ScratchErrorSet();
                        }
                    });
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam3_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam4(CogDisplay cdy, int shotNumber)
        {
            int funCamNumber = 3;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                NoScratchErrorInit();

                cdy.Image = TempCam[funCamNumber].Run();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (Inspect_Cam3(cdy, shotNumber) == true) // 검사 결과
                {
                    Glob.Inspect4[shotNumber - 1] = true;
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        if (Glob.OKImageSave)
                            ImageSave4("OK", funCamNumber + 1, cdy, shotNumber);
                    });
                }
                else
                {
                    Glob.Inspect4[shotNumber - 1] = false;

                    BeginInvoke((Action)delegate
                    {
                        if (Glob.NGImageSave)
                            ImageSave4("NG", funCamNumber + 1, cdy, shotNumber);
                    });
                    if (!Glob.statsOK)
                    {
                        NoScratchErrorSet();
                    }

                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                Debug.WriteLine($"inspect 4 shotnumber : {shotNumber}");
                if (shotNumber == 3)
                {
                    if (Glob.Inspect4[0] == false || Glob.Inspect4[1] == false || Glob.Inspect4[2] == false)
                    {
                        BeginInvoke((Action)delegate
                        {
                            //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                            HeatSinkMainDisplay.lb_Cam4_Result.BackColor = Color.Red;
                            HeatSinkMainDisplay.lb_Cam4_Result.Text = "N G";
                            NG_Count[funCamNumber]++;
                            if (Glob.NGImageSave)
                                ImageSave4("NG", funCamNumber + 1, cdy, shotNumber);
                        });
                    }
                    else
                    {
                        BeginInvoke((Action)delegate
                        {
                            //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                            HeatSinkMainDisplay.lb_Cam4_Result.BackColor = Color.Lime;
                            HeatSinkMainDisplay.lb_Cam4_Result.Text = "O K";
                            OK_Count[funCamNumber]++;
                            if (Glob.OKImageSave)
                                ImageSave4("OK", funCamNumber + 1, cdy, shotNumber);
                        });
                    }
                    LCP_100DC(LightControl[2], "1", "f", "0000");
                    LCP_100DC(LightControl[2], "2", "f", "0000");
                }

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam4_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam5(CogDisplay cdy, int shotNumber)
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

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                if (shotNumber == 2)
                {
                    Debug.WriteLine("inspection 5 shotnumber 2");
                }
                if (Inspect_Cam4(cdy, shotNumber) == true) // 검사 결과
                {
                    Glob.Inspect5[shotNumber - 1] = true;
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        if (Glob.OKImageSave)
                            ImageSave5("OK", funCamNumber + 1, cdy, shotNumber);
                    });
                }
                else
                {
                    Glob.Inspect5[shotNumber - 1] = false;
                    BeginInvoke((Action)delegate
                    {
                        if (Glob.NGImageSave)
                            ImageSave5("NG", funCamNumber + 1, cdy, shotNumber);
                    });
                    if (!Glob.statsOK)
                    {
                        NoScratchErrorSet();
                    }
                }
                Debug.WriteLine($"inspect 5 shotnumber : {shotNumber}");

                if (shotNumber == 2)
                {
                    if (Glob.Inspect5[0] == false || Glob.Inspect5[1] == false)
                    {
                        BeginInvoke((Action)delegate
                        {
                            //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                            HeatSinkMainDisplay.lb_Cam5_Result.BackColor = Color.Red;
                            HeatSinkMainDisplay.lb_Cam5_Result.Text = "N G";
                            NG_Count[funCamNumber]++;
                            if (Glob.NGImageSave)
                                ImageSave5("NG", funCamNumber + 1, cdy, shotNumber);
                        });
                    }
                    else
                    {
                        BeginInvoke((Action)delegate
                        {
                            //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                            HeatSinkMainDisplay.lb_Cam5_Result.BackColor = Color.Lime;
                            HeatSinkMainDisplay.lb_Cam5_Result.Text = "O K";
                            OK_Count[funCamNumber]++;
                            if (Glob.OKImageSave)
                                ImageSave5("OK", funCamNumber + 1, cdy, shotNumber);
                        });

                    }
                }

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam5_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public void ShotAndInspect_Cam6(CogDisplay cdy, int shotNumber)
        {
            int funCamNumber = 5;
            try
            {
                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                if ((Glob.CurruntModelName == "shield"))
                {
                    Debug.WriteLine("shield 일때 에러 초기화");
                    NoScratchErrorInit();
                }

                Glob.FlipImageTool[funCamNumber].InputImage = TempCam[funCamNumber].Run();
                Glob.FlipImageTool[funCamNumber].Run();

                cdy.Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                LCP24_150DC(LightControl[3], "0", "0000");

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }
                Debug.WriteLine("검사 시작.");
                if (Inspect_Cam5(cdy, shotNumber) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam6_Result.BackColor = Color.Lime;
                        HeatSinkMainDisplay.lb_Cam6_Result.Text = "O K";
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave6("OK", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, shotNumber);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        HeatSinkMainDisplay.lb_Cam6_Result.BackColor = Color.Red;
                        HeatSinkMainDisplay.lb_Cam6_Result.Text = "N G";
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave6("NG", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, shotNumber);
                    });
                    if (!Glob.statsOK)
                    {
                        Debug.WriteLine("형상 및 블롭에러");
                        NoScratchErrorSet();
                    }
                }

                if (shotNumber == 1)
                {
                    Debug.WriteLine("에러체크 후 PLC전송");
                    ErrorCheckAndSendPLC();
                }


                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;

                BeginInvoke((Action)delegate { HeatSinkMainDisplay.lb_Cam6_InsTime.Text = InspectTime[funCamNumber].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - {funCamNumber + 1} Error : {ee.Message}");
            }
        }

        public async void ErrorCheckAndSendPLC()
        {
            if (Glob.firstInspection[1])
            {
                Debug.WriteLine($"Glob.scratchError[0] : {Glob.scratchError[0]}");
                Debug.WriteLine($"Glob.noScratchError[0] : {Glob.noScratchError[0]}");
                if (Glob.scratchError[1])
                {
                    //Debug.WriteLine($"Glob.scratchError[0] : {Glob.scratchError[0]}");
                    최종결과표시(false);
                    SelectHighIndex(1, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(1, 0);
                    return;
                }
                else if (Glob.noScratchError[0])
                {
                    //Debug.WriteLine($"Glob.noScratchError[0] : {Glob.noScratchError[0]}");
                    최종결과표시(false);
                    SelectHighIndex(2, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(2, 0);
                    return;
                }
                else
                {
                    Debug.WriteLine("검사결과 OK");
                    최종결과표시(true);
                    SelectHighIndex(0, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(0, 0);
                }
            }
            else
            {
                Debug.WriteLine($"Glob.scratchError[1] : {Glob.scratchError[1]}");
                Debug.WriteLine($"Glob.noScratchError[1] : {Glob.noScratchError[1]}");
                if (Glob.scratchError[0])
                {
                    최종결과표시(false);
                    SelectHighIndex(1, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(1, 0);
                    return;
                }
                else if (Glob.noScratchError[1])
                {
                    최종결과표시(false);
                    SelectHighIndex(2, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(2, 0);
                    return;
                }
                else
                {
                    최종결과표시(true);
                    Debug.WriteLine("검사결과 OK");
                    SelectHighIndex(0, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(0, 0);
                }
            }
        }


        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IO_DoWork = false;

            if (LightStats == true)
            {
                LightOFF();
            }
            for (int i = 0; i < TempCam.Count(); i++)
            {
                TempCam[i].Close();
                TempMask[i].Close();
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
            tlpUnder.Visible = false;
            this.IO_DoWork = true;
            new Thread(ReadInputSignal).Start();
            CognexModelLoad();
            //AllCameraOneShot();
            Glob.firstInspection[0] = false;
            Glob.firstInspection[1] = false;
            //전체 조명 꺼주기.
            for (int lop = 0; lop < LightControl.Count(); lop++)
            {
                if (lop == 3)
                {
                    LCP24_150DC(LightControl[lop], "0", "0000");
                }
                else
                {
                    LCP_100DC(LightControl[lop], "1", "f", "0000");
                    LCP_100DC(LightControl[lop], "2", "f", "0000");
                }
            }
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
            TempBlobNGOKChange = TempModel.BlobNGOKChanges();
            TempBlobFixPatternNumber = TempModel.BlobFixPatternNumbers();
            TempCircles = TempModel.Circle();
            TempCircleEnable = TempModel.CircleEnables();
            TempMulti = TempModel.MultiPatterns();
            TempMultiEnable = TempModel.MultiPatternEnables();
            TempMultiOrderNumber = TempModel.MultiPatternOrderNumbers();
            TempDistance = TempModel.Distancess();
            TempDistanceEnable = TempModel.DistanceEnables();
            TempDistance_CalibrationValue = TempModel.Distance_CalibrationValues();
            TempDistance_LowValue = TempModel.Distance_LowValues();
            TempDistance_HighValue = TempModel.Distance_HighValues();
            TempCaliper = TempModel.Calipes();
            TempCaliperEnable = TempModel.CaliperEnables();
            TempCam = TempModel.Cam();
            TempMask = TempModel.MaskTool();
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

        public void Mask_Train1(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Mask_FixtureImage1((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }

        public void Bolb_Train1(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage1((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }
        public void Bolb_Train2(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage2((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }
        public void Bolb_Train3(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage3((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }
        public void Bolb_Train4(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage4((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }
        public void Bolb_Train5(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage5((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }

        public void Bolb_Train6(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (TempMulti[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                Fiximage = TempModel.Blob_FixtureImage6((CogImage8Grey)cdy.Image, TempMulti[CameraNumber, toolnumber].ResultPoint(TempMulti[CameraNumber, toolnumber].HighestResultToolNumber()), TempMulti[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, TempMulti[CameraNumber, toolnumber].HighestResultToolNumber());
            }
        }

        public int FindFirstPatternNumber1(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }
        public int FindFirstPatternNumber2(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }
        public int FindFirstPatternNumber3(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }
        public int FindFirstPatternNumber4(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }
        public int FindFirstPatternNumber5(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }
        public int FindFirstPatternNumber6(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }

        public void BlobMaskAreaSetting(CogDisplay cog, int CameraNumber, int toolnum)
        {
            //TempMask[CameraNumber].Run((CogImage8Grey)cog.Image); //MaskTool Run
            //TempBlobs[Glob.CamNumber, toolnum].MaskAreaSet(TempMask[CameraNumber].MaskArea()); //검사 제외영역 입력.
        }

        #region Inpection CAM0 
        public bool Inspect_Cam0(CogDisplay cog, int shotNumber)
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
            int FixPatternNumber = FindFirstPatternNumber1(CameraNumber, shotNumber);
            if (TempMulti[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber()), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber(), FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
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
                Debug.WriteLine("CAM 1 Pattern OK");
            }
            else
            {
                Debug.WriteLine("CAM 1 Pattern NG");
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
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {
                Debug.WriteLine("CAM 1 Blob OK");
            }
            else
            {
                //BLOB 검사 FAIL
                Debug.WriteLine("CAM 1 Blob NG");
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
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
        public bool Inspect_Cam1(CogDisplay cog, int shotNumber)
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
            int FixPatternNumber = FindFirstPatternNumber2(CameraNumber, shotNumber);
            if (TempMulti[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber()), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber(), FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
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
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
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
        public bool Inspect_Cam2(CogDisplay cog, int shotNumber)
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
            int FixPatternNumber = FindFirstPatternNumber3(CameraNumber, shotNumber);
            if (TempMulti[CameraNumber, 0].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber()), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber(), FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
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
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
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
        public bool Inspect_Cam3(CogDisplay cog, int shotNumber)
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
            int FixPatternNumber = FindFirstPatternNumber4(CameraNumber, shotNumber);
            if (TempMulti[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber()), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber(), FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true && TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
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
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
                Glob.BlobResult[shotNumber - 1] = false;
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

            if (Glob.BlobResult[CameraNumber] && Glob.BlobResult[shotNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, "BLOB OK"); }
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
        public bool Inspect_Cam4(CogDisplay cog, int shotNumber)
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
            int FixPatternNumber = FindFirstPatternNumber5(CameraNumber, shotNumber);
            if (TempMulti[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber()), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber(), FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true && TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
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
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
                Glob.BlobResult5[shotNumber - 1] = false;
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
        public bool Inspect_Cam5(CogDisplay cog, int shotNumber)
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

            int FixPatternNumber = FindFirstPatternNumber6(CameraNumber, shotNumber);
            int highestResultNumber = TempMulti[CameraNumber, FixPatternNumber].HighestResultToolNumber();
            Debug.WriteLine("Fixture Tool Start");
            if (TempMulti[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                Fiximage = TempModel.FixtureImage((CogImage8Grey)cog.Image, TempMulti[CameraNumber, FixPatternNumber].ResultPoint(highestResultNumber), TempMulti[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, highestResultNumber, FixPatternNumber);

            }
            //*******************************MultiPattern Tool Run******************************//
            Debug.WriteLine("Pattern Tool Start");
            if (TempModel.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (TempMultiEnable[CameraNumber, lop] == true && TempMultiOrderNumber[CameraNumber, lop] == shotNumber)
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
            Debug.WriteLine("Blob Tool Start");
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (TempBlobEnable[CameraNumber, toolnum])
                {
                    Bolb_Train6(cog, CameraNumber, TempBlobFixPatternNumber[CameraNumber, toolnum]);
                    TempBlobs[CameraNumber, toolnum].Area_Affine_Main6(ref cog, (CogImage8Grey)cog.Image, TempBlobFixPatternNumber[CameraNumber, toolnum].ToString());
                    BlobMaskAreaSetting(cog, CameraNumber, toolnum);
                }
            }
            //******************************Blob Tool Run******************************//
            if (TempModel.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            Debug.WriteLine("Dimension Tool Start");
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

            Debug.WriteLine("검사끝");

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
                    int patternIndex = TempBlobFixPatternNumber[camnumber, i];
                    if (TempBlobEnable[camnumber, i] == true && TempMultiOrderNumber[camnumber, patternIndex] == Glob.InspectOrder)
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
                    if (TempMultiEnable[camnumber, i] == true && TempMultiOrderNumber[camnumber, i] == Glob.InspectOrder)
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
        public void ImageSave2(string Result, int CamNumber, CogImage8Grey image)
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
                ImageSave.Append(image);
                ImageSave.Close();

                // cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave3(string Result, int CamNumber, CogImage8Grey image)
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
                ImageSave.Append(image);
                ImageSave.Close();

                //cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
                //cm.info(ee.Message);
            }
        }
        public void ImageSave4(string Result, int CamNumber, CogDisplay cog, int shotNumber)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}";
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
        public void ImageSave5(string Result, int CamNumber, CogDisplay cog, int shotNumber)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}";
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
        public void ImageSave6(string Result, int CamNumber, CogImage8Grey image, int shotNumber)
        {
            //NG 이미지와 OK 이미지 구별이 필요할 것 같음 - 따로 요청이 없어서 구별해놓진 않음
            try
            {
                CogImageFileJPEG ImageSave = new Cognex.VisionPro.ImageFile.CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}";
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
                ImageSave.Append(image);
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
            Glob.firstInspection[0] = true;
            Glob.firstInspection[1] = true;
            tlpUnder.Visible = true;
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
            if (LightControl1.IsOpen == false)
            {
                return;
            }
            LightStats = true;
            string LightValue = string.Format("{0:D3}", 255);
            string LightValue2 = string.Format("{0:D3}", 255);
            LightValue = ":L" + 1 + LightValue + "\r\n";
            LightValue2 = ":L" + 2 + LightValue2 + "\r\n";
            LightControl1.Write(LightValue.ToCharArray(), 0, LightValue.ToCharArray().Length);
            LightControl1.Write(LightValue2.ToCharArray(), 0, LightValue2.ToCharArray().Length);
        }
        public void LightOFF()
        {
            if (LightControl1.IsOpen == false)
            {
                return;
            }
            LightStats = false;
            string LightValue = string.Format("{0:D3}", 0);
            string LightValue2 = string.Format("{0:D3}", 0);
            LightValue = ":L" + 1 + LightValue + "\r\n";
            LightValue2 = ":L" + 2 + LightValue2 + "\r\n";
            LightControl1.Write(LightValue.ToCharArray(), 0, LightValue.ToCharArray().Length);
            LightControl1.Write(LightValue2.ToCharArray(), 0, LightValue2.ToCharArray().Length);
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

        public void SnapShot(int camNumber, CogDisplay cdy)
        {
            try
            {
                if (camNumber == 1 || camNumber == 2 || camNumber == 5)
                {
                    Glob.FlipImageTool[camNumber].InputImage = TempCam[camNumber].Run();
                    Glob.FlipImageTool[camNumber].Run();

                    cdy.Image = Glob.FlipImageTool[camNumber].OutputImage;
                }
                else
                {
                    cdy.Image = TempCam[camNumber].Run();
                }

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
                HeatSinkMainDisplay.cdyDisplay.Image = TempCam[0].Run();
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
                HeatSinkMainDisplay.cdyDisplay2.Image = TempCam[1].Run();
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
                HeatSinkMainDisplay.cdyDisplay3.Image = TempCam[2].Run();
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
                HeatSinkMainDisplay.cdyDisplay4.Image = TempCam[3].Run();
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
                HeatSinkMainDisplay.cdyDisplay5.Image = TempCam[4].Run();
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
                HeatSinkMainDisplay.cdyDisplay6.Image = TempCam[5].Run();
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
            try
            {
                Glob.InspectOrder = 1818;
                int check = 0;
                Debug.WriteLine("PLC Signal Read Start");
                while (IO_DoWork)
                {
                    if (check == 0)
                    {
                        Debug.WriteLine("반복문 시작.");
                        check++;
                    }
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
                        inputBtn[i].BackColor = fired ? Color.Lime : SystemColors.Control;
                        if (!fired) continue;

                        Debug.WriteLine($"PLC 신호 : {i}");

                        switch (i)
                        {
                            case 0: //1번째 라인스캔 카메라 촬영 신호 Cam 1
                                log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                Glob.firstInspection[0] = Glob.firstInspection[0] ? false : true;
                                Task.Run(() => { ShotAndInspect_Cam1(1); });
                                break;
                            case 1: //사이드 라인스캔 카메라 촬영신호 Cam 2 & Cam 3
                                if ((Glob.CurruntModelName == "shield") == false)
                                {

                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam2(1); });
                                    Task.Run(() => { ShotAndInspect_Cam3(1); });
                                }
                                break;
                            case 2: //4번촬영
                                Glob.firstInspection[1] = Glob.firstInspection[1] ? false : true;
                                if ((Glob.CurruntModelName == "shield") == false)
                                {

                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam4(HeatSinkMainDisplay.cdyDisplay4, 1); });
                                }
                                break;
                            case 3: //4번촬영
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam4(HeatSinkMainDisplay.cdyDisplay4_2, 2); });
                                }
                                break;
                            case 4: //4번촬영
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam4(HeatSinkMainDisplay.cdyDisplay4_3, 3); });
                                }
                                break;
                            case 5: //5번촬영
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam5(HeatSinkMainDisplay.cdyDisplay5, 1); });
                                }
                                break;
                            case 6: //5번촬영
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                    Task.Run(() => { ShotAndInspect_Cam5(HeatSinkMainDisplay.cdyDisplay5_1, 2); });
                                }
                                break;
                            case 7://6번촬영
                                log.AddLogMessage(LogType.Infomation, 0, $"PLC 신호 : {i}");
                                Task.Run(() => { ShotAndInspect_Cam6(HeatSinkMainDisplay.cdyDisplay6, 1); });
                                break;
                            case 8:
                                //조명 켜주기.
                                for (int lop = 0; lop < LightControl.Count(); lop++)
                                {
                                    if (lop == 3)
                                    {
                                        LCP24_150DC(LightControl[lop], "0", Glob.LightChAndValue[lop, 0].ToString("D4"));
                                    }
                                    else
                                    {
                                        LCP_100DC(LightControl[lop], "1", "o", "0000");
                                        LCP_100DC(LightControl[lop], "2", "o", "0000");
                                        Debug.WriteLine($"LightControl{lop + 1} Value = {Glob.LightChAndValue[lop, 0]}");
                                        LCP_100DC(LightControl[lop], "1", "d", Glob.LightChAndValue[lop, 0].ToString("D4"));
                                        LCP_100DC(LightControl[lop], "2", "d", Glob.LightChAndValue[lop, 1].ToString("D4"));
                                    }
                                }
                                break;
                        }
                    }

                    Thread.Sleep(1);
                }

                Debug.WriteLine("IO Read Ended!");
            }
            catch (Exception ee)
            {
                Debug.WriteLine($"PLC Read Error : {ee.Message}");
            }
        }

        private bool SelectHighIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            log.AddLogMessage(LogType.Infomation, 0, $"Index : {nIndex}, Value : {uValue}");

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

        private void cb_ResultOK_CheckedChanged(object sender, EventArgs e)
        {
            Glob.statsOK = cb_ResultOK.Checked;
            cb_ResultOK.Text = cb_ResultOK.Checked == true ? "USE" : "UNUSED";
            cb_ResultOK.ForeColor = cb_ResultOK.Checked == true ? Color.Lime : Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ImageType;
            OpenFileDialog ofd = new OpenFileDialog();
            CogImage8Grey Monoimage = new CogImage8Grey();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Glob.ImageFilePath = ofd.FileName.Substring(0, ofd.FileName.Length - ofd.SafeFileName.Length);
                Glob.ImageFilePath = ofd.FileName;
                string type = Path.GetExtension(ofd.FileName);
                string[] ImageFileName = ofd.FileNames;
                if (type == ".bmp")
                {
                    CogImageFileBMP Imageopen = new CogImageFileBMP();
                    Imageopen.Open(ofd.FileName, CogImageFileModeConstants.Read);
                    Imageopen.Close();
                }
                else
                {
                    CogImageFileTool curimage = new CogImageFileTool();
                    curimage.Operator.Open(ofd.FileName, CogImageFileModeConstants.Read);
                    curimage.Run();
                    ImageType = curimage.OutputImage.GetType().ToString();
                    if (ImageType.Contains("CogImage24PlanarColor"))
                    {
                        CogImageConvertTool imageconvert = new CogImageConvertTool();
                        imageconvert.InputImage = curimage.OutputImage;
                        imageconvert.RunParams.RunMode = CogImageConvertRunModeConstants.Plane2;
                        imageconvert.Run();

                        Glob.FlipImageTool[5].InputImage = curimage.OutputImage;
                        Glob.FlipImageTool[5].Run();

                        HeatSinkMainDisplay.cdyDisplay6.Image = Glob.FlipImageTool[5].OutputImage;
                    }
                    else
                    {
                        Glob.FlipImageTool[5].InputImage = curimage.OutputImage;
                        Glob.FlipImageTool[5].Run();
                        HeatSinkMainDisplay.cdyDisplay6.Image = (CogImage8Grey)Glob.FlipImageTool[5].OutputImage;//JPG 파일
                    }
                    //cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage;
                    HeatSinkMainDisplay.cdyDisplay6.Fit();
                    GC.Collect();
                }
            }
        }

        private void num_LightNumber_ValueChanged(object sender, EventArgs e)
        {
            Glob.LightControlNumber = (int)num_LightNumber.Value;
        }

        private void AllCameraOneShot()
        {
            SnapShot1();
            SnapShot2();
            SnapShot3();
            SnapShot4();
            SnapShot5();
            SnapShot6();
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
