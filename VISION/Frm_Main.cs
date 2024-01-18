using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO.Ports;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.FGGigE;
using KimLib;
using VISION.Cogs;
using VISION.UI;
using Euresys.clseremc;
using Microsoft.Win32;
using VISION.Class;
using VISION.UI.Display;
using System.Security.Cryptography.X509Certificates;

namespace VISION
{
    public delegate void EventCallBack(Bitmap bmp);
    public partial class Frm_Main : Form
    {
        public Log log = new Log();
        DriveInfo DriveInfo;
        private Class_Common cm { get { return Program.cm; } } //에러 메세지 보여주기.
        public SubForm.SubForm SubForm;
        public Thread SubTread;
        internal Frm_ToolSetUp frm_toolsetup; //툴셋업창 화면
        internal Frm_SystemSetUp frm_systemsetup; //시스템셋업창 화면

        private Cognex.VisionPro.CogImage8Grey Fiximage; //PMAlign툴의 결과이미지(픽스쳐이미지)
        private string FimageSpace; //PMAlign툴 SpaceName(보정하기위해)

        public HeatSinkMainDisplay HeatSinkMainDisplay = new HeatSinkMainDisplay();
        public ShieldMainDisplay ShieldMainDisplay = new ShieldMainDisplay();
        public NutDisplay NutDisplay = new NutDisplay();
        private ResultCountDisplay ResultCountDisplay = new ResultCountDisplay();

        public CogDisplay[] TempCogDisplay;
        public CogDisplay[] TempCogMasterDisplay;
        public CogDisplay[] TempCogNutDisplay;

        public Label[] lb너트카메라검사결과;
        public Label[] lb너트검사시간;

        public Label[] lb개별카메라검사결과;
        public Label[] lb검사시간;
        public Label[] lb최종결과;

        private PGgloble Glob; //전역변수 - CLASS "PGgloble" 참고.

        public bool[] InspectResult = new bool[8]; //검사결과.
        public bool Modelchange = false; //모델체인지

        public Stopwatch[] InspectTime = new Stopwatch[8]; //검사시간
        public double[] OK_Count = new double[8]; //각 카메라 별 양품개수
        public double[] NG_Count = new double[8]; //각 카메라 별 NG품개수
        public double[] TOTAL_Count = new double[8]; //각 카메라 별 총개수
        public double[] NG_Rate = new double[8]; //각 카메라 별 불량률

        public double AllOK_Count = 0; //종합 판정 OK 개수
        public double AllNG_Count = 0; //종합 판정 NG 개수

        public double AllNG_1_Count = 0; //종합 판정 NG1 개수
        public double AllNG_2_Count = 0; //종합 판정 NG2 개수

        public string 수량체크시작시간;

        public bool[] InspectFlag = new bool[8]; //검사 플래그

        //유레시스 보드 시리얼포트 연결 관련.
        // Handle to the serial port
        IntPtr serialRef = System.IntPtr.Zero;
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
        //List<string> availablePort = new List<string>();
        List<string> baudRates = new List<string>();

        // Trigger
        private Boolean IO_DoWork = false;

        private string[] IOModel = new string[2];
        private Button[] inputBtn;
        private CheckBox[] outputBtn;

        public readonly static uint INFINITE = 0xFFFFFFFF;
        public readonly static uint STATUS_WAIT_0 = 0x00000000;
        public readonly static uint WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0);

        private int IONumber = 32;
        private DateTime[] TrigTime = new DateTime[32];
        public bool[] gbool_di = new bool[32];
        public bool[] re_gbool_di = new bool[32];

        public SerialPort[] LightControl; //조명컨트롤러

        [DllImport("kernel32", EntryPoint = "WaitForSingleObject", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint WaitForSingleObject(uint hHandle, uint dwMilliseconds);

        [DllImport("KERNEL32", EntryPoint = "SetEvent", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetEvent(long hEvent);

        public Inspection 비전검사;

        public CogImage8Grey[] Cam4너트검사이미지 = new CogImage8Grey[3];
        public CogImage8Grey[] Cam5너트검사이미지 = new CogImage8Grey[2];

        public void SubFormRun()
        {
            Application.Run(SubForm);
        }

        public void SubFromStart(string title, string ver)
        {
            SubForm = new SubForm.SubForm();
            SubTread = new Thread(new ThreadStart(SubFormRun));
            SubTread.Start();
            SubForm.제목 = title;
            SubForm.프로그램버전 = $"Ver. {ver}";
        }

        public void SubFromClose()
        {
            SubForm.Close();
            //SubTread.Abort();
        }

        public Frm_Main()
        {
            Glob = PGgloble.getInstance; //전역변수 사용
            Glob.G_MainForm = this;
            SubFromStart("VISION PROGRAM START", Glob.PROGRAM_VERSION);
            InitializeComponent();
            StandFirst();
            CamSet();
            Glob.RunnModel = new Model(); //코그넥스 모델 초기화.
            log.AddLogMessage(LogType.Result, 0, "Cognex 모델 초기화 완료.");
            비전검사 = new Inspection();
            비전검사.Init();
            PGgloble.그랩제어 = new Schemas.그랩제어();
            PGgloble.그랩제어.Init();
            log.AddLogMessage(LogType.Result, 0, "비전검사로직 초기화 완료.");
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
            LightControl = new SerialPort[4] { LightControl1, LightControl2, LightControl3, LightControl4 };
            // 최종 트리거 시간 초기화
            for (Int32 i = 0; i < this.TrigTime.Length; i++)
                this.TrigTime[i] = DateTime.Today;
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
            Initialize_LightControl(); //조명컨틀로 초기화
            Initialize_GeniCam(); //GeniCam 설정
            Set_GeniCam(Glob.CurruntModelName); //Camfile 셋팅.
            CognexModelLoad(); //코그넥스 모델 로드.
            DigitalIO_Load(); //IO Load
            SelectModule(); //IO Board Module Select
            AllCameraOneShot(); //All Camera One Shot.
            log.AddLogMessage(LogType.Infomation, 0, "Vision Program Start");
            SubFromClose();
        }

        public void MainUIDisplaySetting(string modelName)
        {
            if (!p너트검사.Controls.Contains(NutDisplay))
            {
                p너트검사.Controls.Clear();
                p너트검사.Controls.Add(NutDisplay);
                NutDisplay.Dock = DockStyle.Fill;

                TempCogNutDisplay = new CogDisplay[2] { NutDisplay.cogDisplay1, NutDisplay.cogDisplay2 };
                lb너트카메라검사결과 = new Label[2] { NutDisplay.lb_Cam1_Result, NutDisplay.lb_Cam2_Result };
                lb너트검사시간 = new Label[2] { NutDisplay.lb_Cam1_InsTime, NutDisplay.lb_Cam2_InsTime };
            }

            if (modelName == "shield")
            {
                if (MainPanel.Controls.Contains(ShieldMainDisplay)) return;

                MainPanel.Controls.Clear();
                MainPanel.Controls.Add(ShieldMainDisplay);
                ShieldMainDisplay.Dock = DockStyle.Fill;
                TempCogDisplay = new CogDisplay[6] { ShieldMainDisplay.cdyDisplay, null, null, null, null, ShieldMainDisplay.cdyDisplay6 };
                TempCogMasterDisplay = new CogDisplay[6] { ShieldMainDisplay.cdy마스터이미지, null, null, null, null, null };
                lb개별카메라검사결과 = new Label[6] { ShieldMainDisplay.lb_Cam1_Result, null, null, null, null, ShieldMainDisplay.lb_Cam6_Result };
                lb검사시간 = new Label[6] { ShieldMainDisplay.lb_Cam1_InsTime, null, null, null, null, ShieldMainDisplay.lb_Cam6_InsTime };
                lb최종결과 = new Label[2] { ShieldMainDisplay.lb_최종결과, ShieldMainDisplay.lb_최종결과2 };
                for (int lop = 0; lop < TempCogMasterDisplay.Length; lop++)
                {
                    if (TempCogMasterDisplay[lop] != null)
                        메인화면마스터이미지셋팅(lop, TempCogMasterDisplay[lop], modelName);
                }
            }
            else
            {
                if (MainPanel.Controls.Contains(HeatSinkMainDisplay)) return;

                MainPanel.Controls.Clear();
                MainPanel.Controls.Add(HeatSinkMainDisplay);
                HeatSinkMainDisplay.Dock = DockStyle.Fill;
                TempCogDisplay = new CogDisplay[6] { HeatSinkMainDisplay.cdyDisplay, HeatSinkMainDisplay.cdyDisplay2, HeatSinkMainDisplay.cdyDisplay3, HeatSinkMainDisplay.cdyDisplay4, HeatSinkMainDisplay.cdyDisplay5, HeatSinkMainDisplay.cdyDisplay6 };
                TempCogMasterDisplay = new CogDisplay[6] { HeatSinkMainDisplay.cdy마스터이미지, null, null, null, null, null };
                lb개별카메라검사결과 = new Label[6] { HeatSinkMainDisplay.lb_Cam1_Result, HeatSinkMainDisplay.lb_Cam2_Result, HeatSinkMainDisplay.lb_Cam3_Result, HeatSinkMainDisplay.lb_Cam4_Result, HeatSinkMainDisplay.lb_Cam5_Result, HeatSinkMainDisplay.lb_Cam6_Result };
                lb검사시간 = new Label[6] { HeatSinkMainDisplay.lb_Cam1_InsTime, HeatSinkMainDisplay.lb_Cam2_InsTime, HeatSinkMainDisplay.lb_Cam3_InsTime, HeatSinkMainDisplay.lb_Cam4_InsTime, HeatSinkMainDisplay.lb_Cam5_InsTime, HeatSinkMainDisplay.lb_Cam6_InsTime, };
                lb최종결과 = new Label[2] { HeatSinkMainDisplay.lb_최종결과, HeatSinkMainDisplay.lb_최종결과2 };
                for (int lop = 0; lop < TempCogMasterDisplay.Length; lop++)
                {
                    if (TempCogMasterDisplay[lop] != null)
                        메인화면마스터이미지셋팅(lop, TempCogMasterDisplay[lop], modelName);
                }
            }
            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
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
                log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Send Command : {cmd}");
            }
            catch (Euresys.clSerialException error)
            {
                log.AddLogMessage(LogType.Error, 0, error.Message);
                return;
            }
        }

        public void 라인스캔카메라설정파일읽어오기(string modelname)
        {
            INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{modelname}\\CamSet.ini");

            for (int lop = 0; lop < Glob.LineCameraOption.Length; lop++)
            {
                if (CamSet.ReadData($"LineCamera{lop}", "Exposure") == "")
                    CamSet.WriteData($"LineCamera{lop}", "Exposure", modelname == "shield" ? "38" : "76");

                if (CamSet.ReadData($"LineCamera{lop}", "Gain") == "")
                    CamSet.WriteData($"LineCamera{lop}", "Gain", modelname == "shield" ? "200" : "50");

                Glob.LineCameraOption[lop].Exposure = Convert.ToDouble(CamSet.ReadData($"LineCamera{lop}", "Exposure"));
                Glob.LineCameraOption[lop].Gain = Convert.ToDouble(CamSet.ReadData($"LineCamera{lop}", "Gain"));
                Glob.LineCameraOption[lop].CamNumber = lop == 2 ? 6 : lop == 3 ? lop : lop + 1;
            }
        }

        public void Set_GeniCam(string modelName)
        {
            //DualBase #0 Port A & B A=CAM1 B=CAM6
            //DualBase #1 Port A & B A=CAM2 B=CAM3
            try
            {
                라인스캔카메라설정파일읽어오기(modelName);
                // 0= CAM1 , 1=CAM2 , 2=CAM6 , 3=CAM3
                for (int lop = 0; lop < Glob.LineCameraOption.Count(); lop++)
                {
                    if (Glob.LineCameraOption[lop].Port == null) continue;
                    //open serial port
                    CL.SerialInit((UInt32)lop, out serialRef);
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

                    //sendCommand
                    string setExposureCommand = $"I={Glob.LineCameraOption[lop].Exposure}";
                    string setGainCommand = $"G={Glob.LineCameraOption[lop].Gain}";

                    sendCommandToBoard("");
                    string first = readBuffer(serialRef);
                    Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name} - Receive Data : {first}");
                    //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Receive Data : {first}");

                    sendCommandToBoard(setExposureCommand);
                    string second = readBuffer(serialRef);
                    Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name} - Receive Data : {second}");
                    //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Receive Data : {second}");

                    sendCommandToBoard(setGainCommand);
                    string third = readBuffer(serialRef);
                    Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name} - Receive Data : {third}");
                    //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Receive Data : {third}");

                    //close port
                    CL.SerialClose(serialRef);
                }
                log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            }
            catch (Euresys.clSerialException error)
            {
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {error.Message}");
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
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {error.Message}");
            }
            catch (Exception error)
            {
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {error.Message}");
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
                    CL.GetSerialPortIdentifier(i, IntPtr.Zero, out bufferSize);
                }
                catch (Euresys.clSerialException error)
                {
                    if (error.Status != CL.ERR_BUFFER_TOO_SMALL)
                    {
                        log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {error.Message} : GrablinkSerialCommunication error");
                        continue;
                    }
                }
                IntPtr textPort = Marshal.AllocHGlobal((int)bufferSize + 1);
                try
                {
                    // Retrieve the port identifier
                    CL.GetSerialPortIdentifier(i, textPort, out bufferSize);
                    portIdentifier = Marshal.PtrToStringAnsi(textPort);
                    //Glob.availablePort.Add(portIdentifier);
                    Glob.LineCameraOption[i].Port = portIdentifier;
                }
                catch (Euresys.clSerialException error)
                {
                    log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {error.Message} : GrablinkSerialCommunication error");
                }
                Marshal.FreeHGlobal(textPort);
            }

            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
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
                        LCP_100DC(LightControl[i], "1", "o", "0000");
                        LCP_100DC(LightControl[i], "2", "o", "0000");
                        LCP_100DC(LightControl[i], "1", "d", "0000");
                        LCP_100DC(LightControl[i], "2", "d", "0000");
                    }
                }
                log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {ee.Message}");
            }
        }

        //LCP-100DC 모델 포트, 채널번호, 종류(o-출력on,f-출력off,d-data전송),데이터값(4자리 10진수 0~100)
        public void LCP_100DC(SerialPort control, string channel, string dataType, string lightValue)
        {
            try
            {
                //log.AddLogMessage(LogType.Infomation, 0, $"SerialPort : {control.PortName} channel : {channel}, lightValue : {lightValue}");
                string STX = $"{Convert.ToChar(2)}";
                string ETX = $"{Convert.ToChar(3)}";

                string sandCmd = $"{STX}{channel}{dataType}{lightValue}{ETX}";

                control.WriteLine(sandCmd);
                //Debug.WriteLine($"Light Control : {control.PortName} / sendCommand : {sandCmd} finished");
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
            }
        }

        //LCP24_150DC 모델 포트, 채널번호,데이터값(4자리숫자 0~1023)
        public void LCP24_150DC(SerialPort control, string channel, string lightValue)
        {
            try
            {
                //log.AddLogMessage(LogType.Infomation, 0, $"SerialPort : {control.PortName} channel : {channel}, lightValue : {lightValue}");
                string STX = $"{Convert.ToChar(2)}";
                string ETX = $"{Convert.ToChar(3)}";

                string sandCmd = $"{STX}{channel}w{lightValue}{ETX}";

                control.WriteLine(sandCmd);
                //Debug.WriteLine($"Light Control : {control.PortName} / sendCommand : {sandCmd} finished");
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
            }
        }

        private void CamSet()
        {
            try
            {
                CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();
                CogAcqFifoTool fifoTool = new CogAcqFifoTool();

                Glob.allCameraCount = frameGrabbers.Count + 3;
                log.AddLogMessage(LogType.Program, 0, $"확인 된 카메라 개수 : {Glob.allCameraCount}");

                Glob.LineCameraOption = new LineCamSets[4]; //라인카메라 옵션 초기화.
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

                        for (nIndex = 0; nIndex < IONumber; nIndex++)
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
                                if (nIndex > 11)
                                {
                                    gbool_di[nIndex] = true;
                                }
                                else
                                {
                                    outputBtn[nIndex].BackColor = Color.Lime;
                                    gbool_di[nIndex] = true;
                                }

                            }

                            else
                            {
                                if (nIndex > 11)
                                {
                                    gbool_di[nIndex] = true;
                                }
                                else
                                {
                                    outputBtn[nIndex].BackColor = SystemColors.Control;
                                    gbool_di[nIndex] = false;
                                }

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

                        for (nIndex = 0; nIndex < IONumber; nIndex++)
                        {
                            // Verify the last bit value of data read
                            uFlagLow = uDataLow & 0x0001;

                            // Shift rightward by bit by bit
                            uDataLow >>= 1;

                            // Updat bit value in control
                            if (nIndex > 11)
                            {

                            }
                            else
                            {
                                if (uFlagLow == 1)
                                    inputBtn[nIndex].BackColor = Color.Lime;
                                else
                                    inputBtn[nIndex].BackColor = SystemColors.Control;
                            }

                        }
                        break;
                }
            }
            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            return true;
        }

        private void DigitalIO_Load()
        {
            inputBtn = new Button[12] { btn_INPUT0, btn_INPUT1, btn_INPUT2, btn_INPUT3, btn_INPUT4, btn_INPUT5, btn_INPUT6, btn_INPUT7, btn_INPUT8, btn_INPUT9, btn_INPUT10, btn_INPUT11 };
            outputBtn = new CheckBox[12] { btn_OUTPUT0, btn_OUTPUT1, btn_OUTPUT2, btn_OUTPUT3, btn_OUTPUT4, btn_OUTPUT5, btn_OUTPUT6, btn_OUTPUT7, btn_OUTPUT8, btn_OUTPUT9, btn_OUTPUT10, btn_OUTPUT11 };

            if (OpenDevice())
            {
                log.AddLogMessage(LogType.Result, 0, $"PLC Module Open Complete.");
            }
            CheckForIllegalCrossThreadCalls = false;
            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }

        private bool OpenDevice()
        {
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

        private void 메인화면마스터이미지셋팅(int CamNumber, CogDisplay cdyDisplay, string model)
        {
            string ImageType;
            string ImageName = $"{Glob.MODELROOT}\\{model}\\Cam{CamNumber}\\CAM{CamNumber}_Master_1.bmp";
            FileInfo fileInfo = new FileInfo(ImageName);

            cdyDisplay.Image = null;
            cdyDisplay.InteractiveGraphics.Clear();
            cdyDisplay.StaticGraphics.Clear();

            if (!fileInfo.Exists)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ImageName} 마스터 이미지가 없습니다. 마스터이미지를 등록해주시기 바랍니다.");
                return;
            }

            CogImageFileTool curimage = new CogImageFileTool();
            curimage.Operator.Open(ImageName, CogImageFileModeConstants.Read);
            curimage.Run();
            ImageType = curimage.OutputImage.GetType().ToString();
            if (ImageType.Contains("CogImage24PlanarColor"))
            {
                CogImageConvertTool imageconvert = new CogImageConvertTool();
                imageconvert.InputImage = curimage.OutputImage;
                imageconvert.RunParams.RunMode = CogImageConvertRunModeConstants.Plane2;
                imageconvert.Run();
                cdyDisplay.Image = (CogImage8Grey)imageconvert.OutputImage;
            }
            else
            {
                cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage; //JPG 파일
            }
            //cdyDisplay.Image = (CogImage8Grey)curimage.OutputImage;
            cdyDisplay.Fit();
            GC.Collect();
        }

        private void LoadSetup()
        {
            try
            {
                INIControl Modellist = new INIControl(Glob.MODELLIST); ;
                INIControl CFGFILE = new INIControl(Glob.CONFIGFILE); ;

                INIControl setting = new INIControl(Glob.SETTING);
                string LastModel = CFGFILE.ReadData("LASTMODEL", "NAME"); //마지막 사용모델 확인.
                Glob.CurruntModelName = LastModel;
                MainUIDisplaySetting(Glob.CurruntModelName); //MainUIDisplay Setting.
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{LastModel}\\CamSet.ini");
                for (int i = 0; i < Glob.CamCount; i++)
                {
                    if (!File.Exists(Glob.MODELROOT + $"\\{LastModel}\\Cam{i}\\FlipImage.vpp"))
                    {
                        CogIPOneImageTool filterTool = new CogIPOneImageTool();
                        CogSerializer.SaveObjectToFile(filterTool, Glob.MODELROOT + $"\\{LastModel}\\Cam{i}\\FlipImage.vpp");
                    }

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
                //****************************검사 사용유무****************************//
                Glob.InspectUsed = setting.ReadData("SYSTEM", "Inspect Used Check", true) == "1" ? true : false;
                Glob.OKImageSave = setting.ReadData("SYSTEM", "OK IMAGE SAVE", true) == "1" ? true : false;
                Glob.NGImageSave = setting.ReadData("SYSTEM", "NG IMAGE SAVE", true) == "1" ? true : false;
                Glob.NGContainUIImageSave = setting.ReadData("SYSTEM", "NG CONTAIN UI IMAGE SAVE", true) == "1" ? true : false;

                DriveInfo = new DriveInfo(Path.GetPathRoot(Glob.ImageSaveRoot));
                라인스캔카메라설정파일읽어오기(LastModel);
                //수량체크불러오기();
                //frm_Information.infolog = new Log();

                log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            }
            catch (Exception ee)
            {
                Debug.WriteLine($"{ee.Message}");
                log.AddLogMessage(LogType.Error, 0, $"{MethodBase.GetCurrentMethod().Name} - {ee.Message}");
            }
        }

        public void 수량체크불러오기()
        {
            INIControl setting = new INIControl(Glob.SETTING);

            수량체크시작시간 = setting.ReadData("Count", "stDate");
            for (int lop = 0; lop < OK_Count.Length; lop++)
            {
                if (setting.ReadData("Count", $"OK_Count[{lop + 1}]") == "")
                    setting.WriteData("Count", $"OK_Count[{lop + 1}]", "0");
                if (setting.ReadData("Count", $"NG_Count[{lop + 1}]") == "")
                    setting.WriteData("Count", $"NG_Count[{lop + 1}]", "0");

                OK_Count[lop] = Convert.ToDouble(setting.ReadData($"Count", $"OK_Count[{lop + 1}]"));
                NG_Count[lop] = Convert.ToDouble(setting.ReadData($"Count", $"NG_Count[{lop + 1}]"));
            }

            AllOK_Count = Convert.ToDouble(setting.ReadData($"Count", "AllOK_Count"));
            AllNG_2_Count = Convert.ToDouble(setting.ReadData($"Count", "AllNG1_Count"));
            AllNG_1_Count = Convert.ToDouble(setting.ReadData($"Count", "AllNG2_Count"));
            AllNG_Count = Convert.ToDouble(setting.ReadData($"Count", "AllNG_Count"));
        }

        public void CountSave()
        {
            INIControl setting = new INIControl(Glob.SETTING);
            DateTime dt = DateTime.Now;
            setting.WriteData("Count", "stDate", dt.ToString("yyyy-MM-dd HH:mm"));

            for (int lop = 0; lop < OK_Count.Length; lop++)
            {
                setting.WriteData("Count", $"OK_Count[{lop + 1}]", OK_Count[lop].ToString());
                setting.WriteData("Count", $"NG_Count[{lop + 1}]", NG_Count[lop].ToString());
            }
            setting.WriteData("Count", "AllOK_Count", AllOK_Count.ToString());
            setting.WriteData("Count", "AllNG1_Count", AllOK_Count.ToString());
            setting.WriteData("Count", "AllNG2_Count", AllOK_Count.ToString());
            setting.WriteData("Count", "AllNG_Count", AllOK_Count.ToString());
        }

        private void DisplayDiskCapacity()
        {
            //long availableCapacityBytes = DriveInfo.AvailableFreeSpace;
            //double availableCapacityGB = availableCapacityBytes / (1024 * 1024 * 1024); // Convert bytes to GB
            //int usedPercent = 100 - (int)((availableCapacityBytes * 100) / DriveInfo.TotalSize);

            //lb저장공간.Text = $"{FormatSize(DriveInfo.TotalSize)} 중 {availableCapacityGB}GB 사용 가능";
            //lb저장공간.ForeColor = usedPercent > 80 ? Color.Tomato : usedPercent > 50 ? Color.Gold : Color.Lime;

            //// Update the progress bar value
            //pb저장공간.Value = usedPercent;
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double size = bytes;
            int i = 0;

            while (size >= 1024 && i < sizes.Length - 1)
            {
                size /= 1024;
                i++;
            }

            return $"{size:0}{sizes[i]}";
        }

        private void btn_ToolSetUp_Click(object sender, EventArgs e)
        {
            if (FromOpenCheck(frm_toolsetup)) return;

            frm_toolsetup = new Frm_ToolSetUp(this);
            frm_toolsetup.Show(this);
        }
        private void 프로그램종료_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name}");
            INIControl setting = new INIControl(Glob.SETTING);
            DateTime dt = DateTime.Now;
            setting.WriteData("Exit Date", "Date", dt.ToString("yyyyMMdd"));
            Application.Exit();
        }

        private void btn_SystemSetup_Click(object sender, EventArgs e)
        {
            //if (FromOpenCheck(frm_systemsetup)) return;

            frm_systemsetup = new Frm_SystemSetUp(this);
            frm_systemsetup.Show(this);
        }

        private void timer_Setting_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lb_Time.Text = dt.ToString("yyyy년 MM월 dd일 HH:mm:ss"); //현재날짜
            lb_CurruntModelName.Text = Glob.RunnModel.Modelname(); //현재사용중인 모델명 체크
            Glob.CurruntModelName = Glob.RunnModel.Modelname();
            DisplayDiskCapacity();
        }

        public void ScratchErrorInit()
        {
            if (frm_toolsetup == null)
            {
                if (Glob.firstInspection[0])
                {
                    Glob.scratchError[0] = false;
                }
                else
                {
                    Glob.scratchError[1] = false;
                }
            }
        }

        public void ScratchErrorSet()
        {
            if (Glob.firstInspection[0])
            {
                Glob.scratchError[0] = true;
            }
            else
            {
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

        public void 최종결과표시(bool 스크레치검사결과, bool 패턴블롭검사결과, string[] res)
        {
            for (int lop = 0; lop < lb최종결과.Count(); lop++)
            {
                lb최종결과[lop].Text = $"{lop + 1}-{res[lop]}";
                lb최종결과[lop].ForeColor = res[lop] == "OK" ? Color.Lime : Color.Red;
                if (lop == 1)
                {
                    AllOK_Count = (!스크레치검사결과 && !패턴블롭검사결과) ? AllOK_Count + 1 : AllOK_Count;
                    AllNG_1_Count = (스크레치검사결과 && !패턴블롭검사결과) ? AllNG_1_Count + 1 : AllNG_1_Count;
                    AllNG_2_Count = (!스크레치검사결과 && 패턴블롭검사결과) ? AllNG_2_Count + 1 : AllNG_2_Count;
                    AllNG_Count = (스크레치검사결과 || 패턴블롭검사결과) ? AllNG_Count + 1 : AllNG_Count;
                }
            }
        }

        public void DisplayLabelSet(string Model, string Result, int camNumber)
        {
            //log.AddLogMessage(LogType.Infomation, 0, $"Cam - {camNumber + 1} : {Result}");
            try
            {
                if (Result == string.Empty)
                {
                    lb개별카메라검사결과[camNumber].ForeColor = Color.White;
                    lb개별카메라검사결과[camNumber].Text = "Wait";
                    lb검사시간[camNumber].Text = "0msec";
                }
                else
                {
                    //if (Model == "shield")
                    //{
                    //    lb개별카메라검사결과[camNumber].ForeColor = Result == "O K" ? Color.Lime : Color.Red;
                    //    lb개별카메라검사결과[camNumber].Text = Result;
                    //    //lb개별카메라검사결과[camNumber].ForeColor = Color.White;
                    //    lb검사시간[camNumber].Text = InspectTime[camNumber].ElapsedMilliseconds.ToString() + "msec";
                    //}
                    //else
                    //{
                    lb개별카메라검사결과[camNumber].ForeColor = Result == "O K" ? Color.Lime : Color.Red;
                    lb개별카메라검사결과[camNumber].Text = Result;
                    //lb개별카메라검사결과[camNumber].ForeColor = Color.White;
                    lb검사시간[camNumber].Text = InspectTime[camNumber].ElapsedMilliseconds.ToString() + "msec";
                    //}
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
            }
        }

        public void 이미지저장(int funCamNumber, string strResult)
        {
            if (strResult == "O K")
                Debug.WriteLine($"Cam - {funCamNumber} / {strResult} ImageSave");

            if (funCamNumber == 1 || funCamNumber == 2 || funCamNumber == 5)
                ImageSave2(strResult, funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, TempCogDisplay[funCamNumber]);
            else
                ImageSave1(strResult, funCamNumber + 1, TempCogDisplay[funCamNumber]);
        }

        public void 검사결과체크(bool bResult, string strResult, int funCamNumber)
        {
            DisplayLabelSet(Glob.CurruntModelName, strResult, funCamNumber);
            if (bResult)
            {
                OK_Count[funCamNumber]++;
                if (Glob.OKImageSave) 이미지저장(funCamNumber, strResult);
            }
            else
            {
                NG_Count[funCamNumber]++;
                if (Glob.NGImageSave) 이미지저장(funCamNumber, strResult);
                if (!Glob.statsOK)
                {
                    if (funCamNumber == 0 || funCamNumber == 1 || funCamNumber == 2)
                        ScratchErrorSet();
                    else
                        NoScratchErrorSet();
                }
            }
        }

        #region Shot CAM1 
        public void ShotAndInspect_Cam1(int shotNumber)
        {
            try
            {
                int funCamNumber = 0;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                TempCogDisplay[funCamNumber].Image = Glob.코그넥스파일.카메라[funCamNumber].Run();
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                TempCogDisplay[funCamNumber].Fit();
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                ScratchErrorInit();

                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 1");
                    return;
                }

                bool r = false;
                r = 비전검사.Run(TempCogDisplay[funCamNumber], funCamNumber, 1);
                result = r ? "O K" : "N G";
                BeginInvoke((Action)delegate { 검사결과체크(r, result, funCamNumber); });
                //if (Inspect_Cam1(TempCogDisplay[funCamNumber], shotNumber) == true) // 검사 결과
                //{
                //    //검사 결과 OK
                //    BeginInvoke((Action)delegate { 검사결과양품(result, funCamNumber); });
                //    //DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //    //OK_Count[funCamNumber]++;
                //    //if (Glob.OKImageSave)
                //    //    ImageSave1(result, funCamNumber + 1, TempCogDisplay[funCamNumber]);
                //}
                //else
                //{
                //    BeginInvoke((Action)delegate { 검사결과불량(result, funCamNumber); });
                //    //BeginInvoke((Action)delegate
                //    //{

                //    //    DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //    //    NG_Count[funCamNumber]++;
                //    //    if (Glob.NGImageSave)
                //    //        ImageSave1(result, funCamNumber + 1, TempCogDisplay[funCamNumber]);
                //    //});
                //}
                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 1 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM2 
        public void ShotAndInspect_Cam2(int shotNumber)
        {
            try
            {
                int funCamNumber = 1;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                Glob.FlipImageTool[funCamNumber].InputImage = Glob.코그넥스파일.카메라[funCamNumber].Run();
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                Glob.FlipImageTool[funCamNumber].Run();

                TempCogDisplay[funCamNumber].Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                TempCogDisplay[funCamNumber].Fit();
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 2");
                    return;
                }

                bool r = false;
                r = 비전검사.Run(TempCogDisplay[funCamNumber], funCamNumber, 1);
                result = r ? "O K" : "N G";
                BeginInvoke((Action)delegate { 검사결과체크(r, result, funCamNumber); });

                //if (Inspect_Cam2(TempCogDisplay[funCamNumber], shotNumber) == true) // 검사 결과
                //{
                //    //검사 결과 OK
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "O K";
                //        OK_Count[funCamNumber]++;
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        if (Glob.OKImageSave)
                //            ImageSave2("OK", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, TempCogDisplay[funCamNumber]);
                //    });
                //}
                //else
                //{
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "N G";
                //        NG_Count[funCamNumber]++;
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        if (Glob.NGImageSave)
                //            ImageSave2("NG", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, TempCogDisplay[funCamNumber]);
                //    });
                //    if (!Glob.statsOK)
                //    {
                //        ScratchErrorSet();
                //    }
                //    //검사 결과 NG
                //}

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 2 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM3
        public void ShotAndInspect_Cam3(int shotNumber)
        {
            try
            {
                int funCamNumber = 2;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();
                Glob.FlipImageTool[funCamNumber].InputImage = Glob.코그넥스파일.카메라[funCamNumber].Run();
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                Glob.FlipImageTool[funCamNumber].Run();

                TempCogDisplay[funCamNumber].Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                TempCogDisplay[funCamNumber].Fit();
                TempCogDisplay[funCamNumber].InteractiveGraphics.Clear();
                TempCogDisplay[funCamNumber].StaticGraphics.Clear();

                //LCP_100DC(LightControl[0], "2", "d", "0000");

                if (TempCogDisplay[funCamNumber].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }

                bool r = false;
                r = 비전검사.Run(TempCogDisplay[funCamNumber], funCamNumber, shotNumber);
                result = r ? "O K" : "N G";
                BeginInvoke((Action)delegate { 검사결과체크(r, result, funCamNumber); });

                //if (Inspect_Cam3(TempCogDisplay[funCamNumber], shotNumber) == true) // 검사 결과
                //{
                //    //검사 결과 OK
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "O K";
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        OK_Count[funCamNumber]++;
                //        if (Glob.OKImageSave)
                //            ImageSave3("OK", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, TempCogDisplay[funCamNumber]);
                //    });
                //}
                //else
                //{
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "N G";
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        NG_Count[funCamNumber]++;
                //        if (Glob.NGImageSave)
                //            ImageSave3("NG", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, TempCogDisplay[funCamNumber]);

                //        if (!Glob.statsOK)
                //        {
                //            ScratchErrorSet();
                //        }
                //    });
                //}

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 3 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM4 
        public void ShotAndInspect_Cam4(CogDisplay cdy, int shotNumber)
        {
            try
            {
                int funCamNumber = 3;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                NoScratchErrorInit();

                //Cam4너트검사이미지[shotNumber - 1] = Glob.코그넥스파일.카메라[funCamNumber].Run();
                //cdy.Image = Cam4너트검사이미지[shotNumber - 1];
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                cdy.Fit();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }

                if (shotNumber == 3)
                {
                    bool r = false;
                    r = 비전검사.Run(HeatSinkMainDisplay.cdyDisplay4, funCamNumber, 1);
                    Glob.Inspect4[0] = r;
                    r = 비전검사.Run(HeatSinkMainDisplay.cdyDisplay4_2, funCamNumber, 2);
                    Glob.Inspect4[1] = r;
                    r = 비전검사.Run(HeatSinkMainDisplay.cdyDisplay4_3, funCamNumber, 3);
                    Glob.Inspect4[2] = r;

                    InspectTime[funCamNumber].Stop();
                    InspectFlag[funCamNumber] = false;
                    if (Glob.Inspect4[0] == false || Glob.Inspect4[1] == false || Glob.Inspect4[2] == false)
                    {
                        //log.AddLogMessage(LogType.Result, 0, $"Cam - 4 No Tab Error : 1:{Glob.Inspect4[0]} / 2:{Glob.Inspect4[1]} / 3:{Glob.Inspect4[2]}");
                        BeginInvoke((Action)delegate
                        {
                            result = "N G";

                            if (Glob.NGImageSave)
                            {
                                ImageSave4("NG", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4, 1);
                                ImageSave4("NG", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4_2, 2);
                                ImageSave4("NG", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4_3, 3);
                            }

                            DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                            NG_Count[funCamNumber]++;

                            if (!Glob.statsOK)
                            {
                                NoScratchErrorSet();
                            }
                        });
                    }
                    else
                    {
                        BeginInvoke((Action)delegate
                        {
                            result = "O K";

                            if (Glob.OKImageSave)
                            {
                                ImageSave4("OK", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4, 1);
                                ImageSave4("OK", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4_2, 2);
                                ImageSave4("OK", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay4_3, 3);
                            }

                            DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                            OK_Count[funCamNumber]++;
                        });
                    }
                }
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 4 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM5
        public void ShotAndInspect_Cam5(CogDisplay cdy, int shotNumber)
        {
            try
            {
                int funCamNumber = 4;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();


                //Cam5너트검사이미지[shotNumber - 1] = Glob.코그넥스파일.카메라[funCamNumber].Run();
                //cdy.Image = Cam5너트검사이미지[shotNumber - 1];
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                cdy.Fit();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }

                if (shotNumber == 2)
                {
                    bool r = false;

                    r = 비전검사.Run(HeatSinkMainDisplay.cdyDisplay5, funCamNumber, 1);
                    Glob.Inspect5[0] = r;

                    r = 비전검사.Run(HeatSinkMainDisplay.cdyDisplay5_1, funCamNumber, 2);
                    Glob.Inspect5[1] = r;

                    BeginInvoke((Action)delegate
                    {

                    });

                    InspectTime[funCamNumber].Stop();
                    InspectFlag[funCamNumber] = false;
                    if (Glob.Inspect5[0] == false || Glob.Inspect5[1] == false)
                    {
                        //log.AddLogMessage(LogType.Result, 0, $"Cam - 5 No Tab Error : 1:{Glob.Inspect5[0]} / 2:{Glob.Inspect5[1]}");
                        BeginInvoke((Action)delegate
                        {
                            result = "N G";

                            if (Glob.NGImageSave)
                            {
                                ImageSave5("NG", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay5, 1);
                                ImageSave5("NG", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay5_1, 2);
                            }

                            DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                            NG_Count[funCamNumber]++;
                        });
                        if (!Glob.statsOK)
                        {
                            NoScratchErrorSet();
                        }
                    }
                    else
                    {
                        BeginInvoke((Action)delegate
                        {
                            result = "O K";
                            DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                            OK_Count[funCamNumber]++;

                            if (Glob.OKImageSave)
                            {
                                ImageSave5("OK", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay5, 1);
                                ImageSave5("OK", funCamNumber + 1, HeatSinkMainDisplay.cdyDisplay5_1, 2);
                            }
                        });
                    }
                }
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 5 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM6
        public void ShotAndInspect_Cam6(CogDisplay cdy, int shotNumber)
        {
            try
            {
                int funCamNumber = 5;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                if ((Glob.CurruntModelName == "shield"))
                {
                    NoScratchErrorInit();
                }

                Glob.FlipImageTool[funCamNumber].InputImage = Glob.코그넥스파일.카메라[funCamNumber].Run();
                log.AddLogMessage(LogType.Result, 0, $"CAM{funCamNumber + 1} shot end");
                Glob.FlipImageTool[funCamNumber].Run();

                cdy.Image = Glob.FlipImageTool[funCamNumber].OutputImage;
                cdy.Fit();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    return;
                }

                bool r = false;
                r = 비전검사.Run(TempCogDisplay[funCamNumber], funCamNumber, shotNumber);
                result = r ? "O K" : "N G";
                BeginInvoke((Action)delegate { 검사결과체크(r, result, funCamNumber); });

                //if (Inspect_Cam6(cdy, shotNumber) == true) // 검사 결과
                //{
                //    //검사 결과 OK
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "O K";
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        OK_Count[funCamNumber]++;
                //        if (Glob.OKImageSave)
                //            ImageSave6("OK", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, cdy);
                //    });
                //}
                //else
                //{
                //    BeginInvoke((Action)delegate
                //    {
                //        result = "N G";
                //        DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                //        NG_Count[funCamNumber]++;
                //        if (Glob.NGImageSave)
                //            ImageSave6("NG", funCamNumber + 1, (CogImage8Grey)Glob.FlipImageTool[funCamNumber].InputImage, cdy);
                //    });
                //    if (!Glob.statsOK)
                //    {
                //        NoScratchErrorSet();
                //    }
                //}
                if (shotNumber == 1)
                {
                    ErrorCheckAndSendPLC();
                }
                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 6 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM7
        public void ShotAndInspect_Cam7(CogDisplay cdy, int shotNumber)
        {
            try
            {
                int funCamNumber = 6;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                cdy.Image = Glob.코그넥스파일.카메라[funCamNumber].Run();
                cdy.Fit();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                //LCP24_150DC(LightControl[3], "0", "0000");

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    Press1NutErrorCheckAndSendPLC();
                    return;
                }

                if (Inspect_Cam7(cdy, shotNumber) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        result = "O K";
                        //DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave7("OK", funCamNumber + 1, (CogImage8Grey)cdy.Image, cdy);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        result = "N G";
                        //DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave7("NG", funCamNumber + 1, (CogImage8Grey)cdy.Image, cdy);
                    });
                }

                Press1NutErrorCheckAndSendPLC();

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 7 Error : {ee.Message}");
            }
        }
        #endregion

        #region Shot CAM8
        public void ShotAndInspect_Cam8(CogDisplay cdy, int shotNumber)
        {
            try
            {
                int funCamNumber = 7;
                string result = string.Empty;

                InspectTime[funCamNumber] = new Stopwatch();
                InspectTime[funCamNumber].Reset();
                InspectTime[funCamNumber].Start();

                cdy.Image = Glob.코그넥스파일.카메라[funCamNumber].Run();
                cdy.Fit();
                cdy.InteractiveGraphics.Clear();
                cdy.StaticGraphics.Clear();

                //LCP24_150DC(LightControl[3], "0", "0000");

                if (cdy.Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, $"이미지 획들을 하지 못하였습니다. CAM - {funCamNumber + 1}");
                    Press2NutErrorCheckAndSendPLC();
                    return;
                }
                if (Inspect_Cam8(cdy, shotNumber) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        result = "O K";
                        //DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                        OK_Count[funCamNumber]++;
                        if (Glob.OKImageSave)
                            ImageSave8("OK", funCamNumber + 1, (CogImage8Grey)cdy.Image, cdy);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        result = "N G";
                        //DisplayLabelSet(Glob.CurruntModelName, result, funCamNumber);
                        NG_Count[funCamNumber]++;
                        if (Glob.NGImageSave)
                            ImageSave8("NG", funCamNumber + 1, (CogImage8Grey)cdy.Image, cdy);
                    });
                }

                Press2NutErrorCheckAndSendPLC();

                InspectTime[funCamNumber].Stop();
                InspectFlag[funCamNumber] = false;
                //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 8 Error : {ee.Message}");
            }
        }
        #endregion

        public async void Press2NutErrorCheckAndSendPLC()
        {
            //Debug.WriteLine("Press 2 Error Check to PLC Start");
            for (int lop = 0; lop < Glob.press2PinResult.Length; lop++)
            {
                if (Glob.press2PinResult[lop] == "OK")
                {
                    switch (lop)
                    {
                        case 0:
                            SelectHighIndex(9, 1);
                            //await Task.Delay(2000);
                            //SelectHighIndex(9, 0);
                            break;
                        case 1:
                            SelectHighIndex(11, 1);
                            //await Task.Delay(2000);
                            //SelectHighIndex(11, 0);
                            break;
                    }
                }
                else
                {
                    switch (lop)
                    {
                        case 0:
                            SelectHighIndex(10, 1);
                            //await Task.Delay(2000);
                            //SelectHighIndex(10, 0);
                            break;
                        case 1:
                            SelectHighIndex(12, 1);
                            //await Task.Delay(2000);
                            //SelectHighIndex(12, 0);
                            break;
                    }
                }
            }
            await Task.Delay(1500);
            for (int lop = 9; lop < 13; lop++)
            {
                SelectHighIndex(lop, 0);
            }
            //Debug.WriteLine("Press 2 Error Check to PLC End");
        }

        public async void Press1NutErrorCheckAndSendPLC()
        {
            //Debug.WriteLine("Press 1 Error Check to PLC Start");
            for (int lop = 0; lop < Glob.press1PinResult.Length; lop++)
            {
                //Debug.WriteLine(Glob.press1PinResult[lop]);
                if (Glob.press1PinResult[lop] == "OK")
                {
                    switch (lop)
                    {
                        case 0:
                            SelectHighIndex(3, 1);
                            break;
                        case 1:
                            SelectHighIndex(5, 1);
                            break;
                        case 2:
                            SelectHighIndex(7, 1);
                            break;
                    }
                }
                else
                {
                    switch (lop)
                    {
                        case 0:
                            SelectHighIndex(4, 1);
                            break;
                        case 1:
                            SelectHighIndex(6, 1);
                            break;
                        case 2:
                            SelectHighIndex(8, 1);
                            break;
                    }
                }
            }
            await Task.Delay(1500);
            for (int lop = 3; lop < 9; lop++)
            {
                SelectHighIndex(lop, 0);
            }
        }

        public async void ErrorCheckAndSendPLC()
        {
            if (Glob.firstInspection[1])
            {
                string[] res = new string[2];
                res[0] = Glob.scratchError[1] ? "NG" : "OK";
                res[1] = Glob.noScratchError[0] ? "NG" : "OK";
                최종결과표시(Glob.scratchError[1], Glob.noScratchError[0], res);

                Debug.WriteLine($"Glob.firstInspection[1] : 스크레치 - {res[0]} / 스크레치아닌불량 - {res[1]}");
                //log.AddLogMessage(LogType.Infomation, 0, $"Glob.firstInspection[1] : 스크레치 - {res[0]} / 스크레치아닌불량 - {res[1]}");
                if (Glob.scratchError[1])
                {
                    SelectHighIndex(1, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(1, 0);
                    return;
                }
                else if (Glob.noScratchError[0])
                {
                    SelectHighIndex(2, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(2, 0);
                    return;
                }
                else
                {
                    SelectHighIndex(0, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(0, 0);
                }
            }
            else
            {
                string[] res = new string[2];
                res[0] = Glob.scratchError[0] ? "NG" : "OK";
                res[1] = Glob.noScratchError[1] ? "NG" : "OK";

                최종결과표시(Glob.scratchError[0], Glob.noScratchError[1], res);

                Debug.WriteLine($"Glob.firstInspection[0] : 스크레치 - {res[0]} / 스크레치아닌불량 - {res[1]}");
                //log.AddLogMessage(LogType.Infomation, 0, $"Glob.firstInspection[0] : 스크레치 - {res[0]} / 스크레치아닌불량 - {res[1]}");
                if (Glob.scratchError[0])
                {
                    SelectHighIndex(1, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(1, 0);
                    return;
                }
                else if (Glob.noScratchError[1])
                {
                    SelectHighIndex(2, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(2, 0);
                    return;
                }
                else
                {
                    SelectHighIndex(0, 1);
                    await Task.Delay(2000);
                    SelectHighIndex(0, 0);
                }
            }
        }


        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IO_DoWork = false;
            this.ResultCountDisplay.timer1.Dispose();

            PGgloble.그랩제어?.Close();

            for (int i = 0; i < Glob.코그넥스파일.카메라.Count(); i++)
            {
                Glob.코그넥스파일.카메라[i].Close();
                for (int lop = 0; lop < 3; lop++)
                {
                    Glob.코그넥스파일.마스크툴[i, lop].Close();
                }
            }
        }

        private void btn_Status_Click(object sender, EventArgs e)
        {
            //조명온오프제어(true);
            if (frm_toolsetup != null)
            {
                cm.info("Tool SetUp 화면이 열려 있습니다. \n변경사항이 있으면 저장 후 Tool Set Up화면을 닫아 주세요.");
                frm_toolsetup.BringToFront();
                return;
            }
            btn_Status.Enabled = false;
            btn_ToolSetUp.Enabled = false;
            btn_Model.Enabled = false;
            btn_SystemSetup.Enabled = false;
            btn_Stop.Enabled = true;
            tlpUnder.Visible = false;
            this.IO_DoWork = true;
            new Thread(ReadInputSignal) { Priority = ThreadPriority.Highest }.Start();
            CognexModelLoad();
            //AllCameraOneShot();
            Glob.firstInspection[0] = false;
            Glob.firstInspection[1] = false;
            //전체 조명 꺼주기.
            조명온오프제어(false);
            //PGgloble.그랩제어.GetItem(Schemas.CameraType.Cam05).Ready();
            log.AddLogMessage(LogType.Infomation, 0, "AUTO MODE START");
        }

        public void 조명온오프제어(bool 상태)
        {
            if (상태)
            {
                for (int lop = 0; lop < LightControl.Count(); lop++)
                {
                    if (lop == 3)
                    {
                        LCP24_150DC(LightControl[lop], "0", Glob.LightChAndValue[lop, 0].ToString("D4"));
                    }
                    else
                    {
                        //LCP_100DC(LightControl[lop], "1", "o", "0000");
                        //LCP_100DC(LightControl[lop], "2", "o", "0000");
                        LCP_100DC(LightControl[lop], "1", "d", Glob.LightChAndValue[lop, 0].ToString("D4"));
                        LCP_100DC(LightControl[lop], "2", "d", Glob.LightChAndValue[lop, 1].ToString("D4"));
                    }
                }
            }
            else
            {
                for (int lop = 0; lop < LightControl.Count(); lop++)
                {
                    if (lop == 3)
                    {
                        LCP24_150DC(LightControl[lop], "0", "0000");
                    }
                    else
                    {
                        LCP_100DC(LightControl[lop], "1", "d", "0000");
                        LCP_100DC(LightControl[lop], "2", "d", "0000");
                    }
                }
            }
        }

        public void CognexModelLoad()
        {
            Glob.코그넥스파일.모델 = Glob.RunnModel;
            Glob.코그넥스파일.카메라 = Glob.코그넥스파일.모델.Cam();
            Glob.코그넥스파일.마스크툴 = Glob.코그넥스파일.모델.MaskTool();
            Glob.코그넥스파일.패턴툴 = Glob.코그넥스파일.모델.MultiPatterns();
            Glob.코그넥스파일.블롭툴 = Glob.코그넥스파일.모델.Blob();
            Glob.코그넥스파일.거리측정툴 = Glob.코그넥스파일.모델.Distancess();
            Glob.코그넥스파일.라인툴 = Glob.코그넥스파일.모델.Line();
            Glob.코그넥스파일.써클툴 = Glob.코그넥스파일.모델.Circle();
            Glob.코그넥스파일.캘리퍼툴 = Glob.코그넥스파일.모델.Calipes();

            Glob.코그넥스파일.패턴툴사용여부 = Glob.코그넥스파일.모델.MultiPatternEnables();
            Glob.코그넥스파일.패턴툴검사순서번호 = Glob.코그넥스파일.모델.MultiPatternOrderNumbers();

            Glob.코그넥스파일.블롭툴사용여부 = Glob.코그넥스파일.모델.BlobEnables();
            Glob.코그넥스파일.블롭툴양품갯수 = Glob.코그넥스파일.모델.BlobOKCounts();
            Glob.코그넥스파일.블롭툴역검사 = Glob.코그넥스파일.모델.BlobNGOKChanges();
            Glob.코그넥스파일.블롭툴픽스쳐번호 = Glob.코그넥스파일.모델.BlobFixPatternNumbers();

            Glob.코그넥스파일.거리측정툴사용여부 = Glob.코그넥스파일.모델.DistanceEnables();

            Glob.코그넥스파일.라인툴사용여부 = Glob.코그넥스파일.모델.LineEnables();

            Glob.코그넥스파일.써클툴사용여부 = Glob.코그넥스파일.모델.CircleEnables();

            Glob.코그넥스파일.캘리퍼툴사용여부 = Glob.코그넥스파일.모델.CaliperEnables();

            Glob.코그넥스파일.보정값 = Glob.코그넥스파일.모델.Distance_CalibrationValues();
            Glob.코그넥스파일.최소값 = Glob.코그넥스파일.모델.Distance_LowValues();
            Glob.코그넥스파일.최대값 = Glob.코그넥스파일.모델.Distance_HighValues();

            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }

        public void DisplayLabelShow(CogGraphicCollection Collection, CogDisplay cog, int X, int Y, double rotate, string Text)
        {
            CogCreateGraphicLabelTool Label = new CogCreateGraphicLabelTool();
            Label.InputGraphicLabel.Color = CogColorConstants.Green;
            Label.InputImage = cog.Image;
            Label.InputGraphicLabel.X = rotate == 0 ? X : Y;
            Label.InputGraphicLabel.Y = rotate == 0 ? Y : X;
            Label.InputGraphicLabel.Rotation = rotate;
            Label.InputGraphicLabel.Text = Text;
            Label.Run();
            Collection.Add(Label.GetOutputGraphicLabel());
        }

        public void Mask_Train1(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Mask_FixtureImage1((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        public void Bolb_Train1(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage1((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }
        public void Bolb_Train2(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage2((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }
        public void Bolb_Train3(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage3((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }
        public void Bolb_Train4(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage4((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }
        public void Bolb_Train5(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage5((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        public void Bolb_Train6(CogDisplay cdy, int CameraNumber, int toolnumber)
        {
            if (Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].Run((CogImage8Grey)cdy.Image) == true)
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.Blob_FixtureImage6((CogImage8Grey)cdy.Image, Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, toolnumber].ToolName(), CameraNumber, toolnumber, out FimageSpace, usePatternNumber);
            }
        }

        public int FindFirstPatternNumber1(int CameraNumber, int shotNumber)
        {
            for (int lop = 0; lop < 30; lop++)
            {
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
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
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
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
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
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
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
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
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
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
                if (Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                {
                    return lop;
                }
            }
            return 0;
        }

        #region Inpection CAM1 
        public bool Inspect_Cam1(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 0;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber1(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 0)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            //GC.Collect();
            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM2 
        public bool Inspect_Cam2(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 1;
            double 이미지회전각도 = frm_toolsetup == null ? 1.5708 : 0;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber2(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 이미지회전각도)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM3
        public bool Inspect_Cam3(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 2;
            double 이미지회전각도 = frm_toolsetup == null ? 1.5708 : 0;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber3(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 이미지회전각도)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM4 
        public bool Inspect_Cam4(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 3;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber4(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 0)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM5
        public bool Inspect_Cam5(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 4;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber5(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 0)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM6
        public bool Inspect_Cam6(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 5;
            double 이미지회전각도 = frm_toolsetup == null ? 1.5708 : 0;
            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber6(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 이미지회전각도)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 이미지회전각도, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 이미지회전각도, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM7
        public bool Inspect_Cam7(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 6;
            for (int lop = 0; lop < Glob.press1PinResult.Length; lop++)
                Glob.press1PinResult[lop] = string.Empty;

            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber4(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 0)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            for (int lop = 0; lop < Glob.press1PinResult.Length; lop++)
            {
                Glob.press1PinResult[lop] = temp[lop];
            }

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion

        #region Inpection CAM8
        public bool Inspect_Cam8(CogDisplay cog, int shotNumber)
        {
            int CameraNumber = 7;
            for (int lop = 0; lop < Glob.press2PinResult.Length; lop++)
                Glob.press2PinResult[lop] = string.Empty;

            Glob.PatternResult[CameraNumber] = true;
            Glob.BlobResult[CameraNumber] = true;
            Glob.MeasureResult[CameraNumber] = true;

            InspectResult[CameraNumber] = true; //검사 결과는 초기에 무조건 true로 되어있다.
            CogGraphicCollection Collection = new CogGraphicCollection();
            CogGraphicCollection Collection2 = new CogGraphicCollection(); // 패턴
            CogGraphicCollection Collection3 = new CogGraphicCollection(); // 블롭

            string[] temp = new string[30];
            int FixPatternNumber = FindFirstPatternNumber4(CameraNumber, shotNumber);
            if (Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].Run((CogImage8Grey)cog.Image))
            {
                int usePatternNumber = Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].HighestResultToolNumber();
                Fiximage = Glob.코그넥스파일.모델.FixtureImage((CogImage8Grey)cog.Image, Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ResultPoint(usePatternNumber), Glob.코그넥스파일.패턴툴[CameraNumber, FixPatternNumber].ToolName(), CameraNumber, out FimageSpace, usePatternNumber, FixPatternNumber);
            }
            //*******************************MultiPattern Tool Run******************************//
            if (Glob.코그넥스파일.모델.MultiPattern_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber, 0)) //검사결과가 true 일때
            {
                for (int lop = 0; lop < 30; lop++)
                {
                    if (Glob.코그넥스파일.패턴툴사용여부[CameraNumber, lop] == true && Glob.코그넥스파일.패턴툴검사순서번호[CameraNumber, lop] == shotNumber)
                    {
                        if (Glob.코그넥스파일.패턴툴[CameraNumber, lop].Threshold() * 100 > Glob.MultiInsPat_Result[CameraNumber, lop])
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
            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Pattern Tool 완료.");
            //블롭툴 넘버와 패턴툴넘버 맞추는 작업.
            for (int toolnum = 0; toolnum < 29; toolnum++)
            {
                if (Glob.코그넥스파일.블롭툴사용여부[CameraNumber, toolnum])
                {
                    Bolb_Train1(cog, CameraNumber, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum]);
                    Glob.코그넥스파일.블롭툴[CameraNumber, toolnum].Area_Affine_Main1(ref cog, (CogImage8Grey)cog.Image, Glob.코그넥스파일.블롭툴픽스쳐번호[CameraNumber, toolnum].ToString());
                }
            }
            //******************************Blob Tool Run******************************//
            if (Glob.코그넥스파일.모델.Blob_Inspection(ref cog, (CogImage8Grey)cog.Image, ref temp, CameraNumber, Collection, shotNumber))
            {

            }
            else
            {
                //BLOB 검사 FAIL
                InspectResult[CameraNumber] = false;
                Glob.BlobResult[CameraNumber] = false;
            }

            for (int lop = 0; lop < Glob.press2PinResult.Length; lop++)
            {
                Glob.press2PinResult[lop] = temp[lop];
            }

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Blob Tool 완료.");


            if (Glob.PatternResult[CameraNumber]) { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN OK"); }
            else { DisplayLabelShow(Collection2, cog, 600, 100, 0, "PATTERN NG"); };

            if (Glob.BlobResult[CameraNumber]) { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB OK"); }
            else { DisplayLabelShow(Collection3, cog, 600, 170, 0, "BLOB NG"); };

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

            //log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");

            return InspectResult[CameraNumber];
        }
        #endregion



        public void DgvResult(DataGridView dgv, int camnumber, int cellnumber)
        {
            if (frm_toolsetup != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    int patternIndex = Glob.코그넥스파일.블롭툴픽스쳐번호[camnumber, i];
                    if (Glob.코그넥스파일.블롭툴사용여부[camnumber, i] == true && Glob.코그넥스파일.패턴툴검사순서번호[camnumber, patternIndex] == Glob.InspectOrder)
                    {
                        if (Glob.코그넥스파일.블롭툴[camnumber, i].ResultBlobCount() != Glob.코그넥스파일.블롭툴양품갯수[camnumber, i]) // - 검사결과 NG
                        {
                            dgv.Rows[i].Cells[3].Value = $"{Glob.코그넥스파일.블롭툴[camnumber, i].ResultBlobCount()}-({Glob.코그넥스파일.블롭툴양품갯수[camnumber, i]})";
                            dgv.Rows[i].Cells[3].Style.BackColor = Color.Red;
                        }
                        else // - 검사결과 OK
                        {
                            dgv.Rows[i].Cells[3].Value = $"{Glob.코그넥스파일.블롭툴[camnumber, i].ResultBlobCount()}-({Glob.코그넥스파일.블롭툴양품갯수[camnumber, i]})";
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
                    if (Glob.코그넥스파일.패턴툴사용여부[camnumber, i] == true && Glob.코그넥스파일.패턴툴검사순서번호[camnumber, i] == Glob.InspectOrder)
                    {
                        if (Glob.코그넥스파일.패턴툴[camnumber, i].ResultPoint(Glob.코그넥스파일.패턴툴[camnumber, i].HighestResultToolNumber()) != null)
                        {
                            if (Glob.MultiInsPat_Result[camnumber, i] > Glob.코그넥스파일.패턴툴[camnumber, i].Threshold() * 100)
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
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }

                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave2(string Result, int CamNumber, CogImage8Grey image, CogDisplay cog)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(image);
                ImageSave.Close();
                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave3(string Result, int CamNumber, CogImage8Grey image, CogDisplay cog)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(image);
                ImageSave.Close();
                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave4(string Result, int CamNumber, CogDisplay cog, int shotNumber)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();
                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave5(string Result, int CamNumber, CogDisplay cog, int shotNumber)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();
                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{shotNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }

        public void ImageSave6(string Result, int CamNumber, CogImage8Grey image, CogDisplay cog)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }

                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(image);
                ImageSave.Close();

                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave7(string Result, int CamNumber, CogImage8Grey image, CogDisplay cog)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }

                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(image);
                ImageSave.Close();

                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }
        public void ImageSave8(string Result, int CamNumber, CogImage8Grey image, CogDisplay cog)
        {
            try
            {
                CogImageFileJPEG ImageSave = new CogImageFileJPEG();
                DateTime dt = DateTime.Now;
                string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }

                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(image);
                ImageSave.Close();

                if (Glob.NGContainUIImageSave)
                {
                    string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
                    if (!Directory.Exists(Root2))
                    {
                        Directory.CreateDirectory(Root2);
                    }
                    cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}.bmp", ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
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
            CountSave();
            log.AddLogMessage(LogType.Infomation, 0, "AUTO MODE STOP");
        }

        private void btn_Model_Click(object sender, EventArgs e)
        {
            //MODEL FORM 열기.
            Frm_Model frm_model = new Frm_Model(Glob.RunnModel.Modelname(), this);
            frm_model.Show(this);
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

        public void SnapShot(int camNumber, CogDisplay cdy)
        {
            try
            {
                if (cdy == null) return;

                if (camNumber == 1 || camNumber == 2 || camNumber == 5)
                {
                    Glob.FlipImageTool[camNumber].InputImage = Glob.코그넥스파일.카메라[camNumber].Run();
                    Glob.FlipImageTool[camNumber].Run();
                    cdy.Image = Glob.FlipImageTool[camNumber].OutputImage;
                }
                else
                {
                    cdy.Image = Glob.코그넥스파일.카메라[camNumber].Run();
                }
                log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} - Cam{camNumber + 1} OneShot 완료.");
            }
            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, ex.Message);
            }
        }
        private void ReadInputSignal()
        {
            Glob.InspectOrder = 1818;

            //// 시간 체크
            //Int32 count = 10000;
            //DateTime time = DateTime.Now;
            //List<Double> times = new List<Double>();
            while (IO_DoWork)
            {
                //time = DateTime.Now;

                try
                {
                    UInt32 iVal = 0;
                    CAXD.AxdiReadInportDword(0, 0, ref iVal);  // 입력신호 32점
                    BitArray Inputs = new BitArray(BitConverter.GetBytes(iVal));

                    for (Int32 i = 0; i < gbool_di.Length; i++)
                    {
                        Boolean fired = Inputs[i];
                        if (gbool_di[i] == fired) continue;
                        if ((DateTime.Now - this.TrigTime[i]).TotalMilliseconds < 1000) continue; // 1000ms 이내에 Trig되면 패스
                        this.TrigTime[i] = DateTime.Now;
                        gbool_di[i] = fired;
                        if (i < 11) inputBtn[i].BackColor = fired ? Color.Lime : SystemColors.Control;

                        if (!fired)
                        {
                            switch (i)
                            {
                                case 8:
                                    조명온오프제어(false);
                                    break;
                            }
                            continue;
                        }
                        Debug.WriteLine($"{i} 번신호");
                        switch (i)
                        {
                            case 0: //1번째 라인스캔 카메라 촬영 신호 Cam 1
                                log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam1 Trigger");
                                Glob.firstInspection[0] = Glob.firstInspection[0] ? false : true;
                                Task.Run(() => { ShotAndInspect_Cam1(1); });
                                break;
                            case 1: //사이드 라인스캔 카메라 촬영신호 Cam 2 & Cam 3
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam2 & Cam3 Trigger");
                                    Task.Run(() => { ShotAndInspect_Cam2(1); });
                                    Task.Run(() => { ShotAndInspect_Cam3(1); });
                                }
                                break;
                            case 2: //4번촬영
                                Glob.firstInspection[1] = Glob.firstInspection[1] ? false : true;
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    PGgloble.그랩제어.좌측너트검사카메라.MatImage2.Clear();
                                    PGgloble.그랩제어.좌측너트검사카메라.Ready();
                                    log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam4 Trigger");
                                    //Task.Run(() => { ShotAndInspect_Cam4(TempCogDisplay[3], 1); });
                                }
                                break;
                            //case 3: //4번촬영
                            //    if ((Glob.CurruntModelName == "shield") == false)
                            //    {
                            //        log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam4-2 Trigger");
                            //        Task.Run(() => { ShotAndInspect_Cam4(HeatSinkMainDisplay.cdyDisplay4_2, 2); });
                            //    }
                            //    break;
                            //case 4: //4번촬영
                            //    if ((Glob.CurruntModelName == "shield") == false)
                            //    {
                            //        log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam4-3 Trigger");
                            //        Task.Run(() => { ShotAndInspect_Cam4(HeatSinkMainDisplay.cdyDisplay4_3, 3); });
                            //    }
                            //    break;
                            case 5: //5번촬영
                                if ((Glob.CurruntModelName == "shield") == false)
                                {
                                    PGgloble.그랩제어.우측너트검사카메라.MatImage3.Clear();
                                    PGgloble.그랩제어.우측너트검사카메라.Ready();
                                    log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam5 Trigger");
                                    //Task.Run(() => { ShotAndInspect_Cam5(TempCogDisplay[4], 1); });
                                }
                                break;
                            //case 6: //5번촬영
                            //    if ((Glob.CurruntModelName == "shield") == false)
                            //    {
                            //        log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam5-2 Trigger");
                            //        Task.Run(() => { ShotAndInspect_Cam5(HeatSinkMainDisplay.cdyDisplay5_1, 2); });
                            //    }
                            //    break;
                            case 7://6번촬영
                                log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Cam6 Trigger");
                                Task.Run(() => { ShotAndInspect_Cam6(TempCogDisplay[5], 1); });
                                break;
                            case 8:
                                log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : Light On Trigger");
                                조명온오프제어(true);
                                break;
                            case 9: //1 프레스 트리거 3Point
                                log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : 1 Press Trigger");
                                Task.Run(() => { ShotAndInspect_Cam7(TempCogNutDisplay[0], 1); });
                                break;
                            case 10: //2 프레스 트리거 2Point
                                log.AddLogMessage(LogType.Result, 0, $"PLC 신호 : 2 Press Trigger");
                                Task.Run(() => { ShotAndInspect_Cam8(TempCogNutDisplay[1], 1); });
                                break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    log.AddLogMessage(LogType.Error, 0, $"PLC Read Error : {ee.Message}");
                }
                Thread.Sleep(1);
            }
        }

        private bool SelectHighIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            //string txt = uValue == 1 ? "On" : "Off";
            //log.AddLogMessage(LogType.Result, 0, $"PC -> PLC Output {nIndex}번 {txt}");
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

        private void AllCameraOneShot()
        {
            //012345 67
            for (int lop = 0; lop < Glob.CamCount; lop++)
            {
                if (lop > 5)
                    SnapShot(lop, TempCogNutDisplay[lop - 6]);
                else
                    SnapShot(lop, TempCogDisplay[lop]);
            }
            log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }

        private void btn최소화_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //PGgloble.그랩제어.GetItem(Schemas.CameraType.Cam05).Ready();
        }

        private bool FromOpenCheck(Form form)
        {
            if (form != null)
            {
                form.BringToFront();
                return true;
            }
            return false;
        }

        private void 카메라파일다시로드하기(object sender, EventArgs e)
        {
            //캠파일 ReLoading.
            Frm_CamSet frm_CamSet = new Frm_CamSet();
            frm_CamSet.ShowDialog(this);
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
