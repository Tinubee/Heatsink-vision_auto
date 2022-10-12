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
using BGAPI2;
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

        Thread snap1; //CAM1 Shot 쓰레드
        Thread snap2; //CAM2 Shot 쓰레드
        Thread snap3; //CAM3 Shot 쓰레드

        Thread snap4; //CAM4 Shot 쓰레드
        Thread snap5; //CAM5 Shot 쓰레드
        Thread snap6; //CAM6 Shot 쓰레드

        Label[] OK_Label;
        Label[] NG_Label;
        Label[] TOTAL_Label;
        Label[] NGRATE_Label;

        #region ADLINK DIO
        //PLC <-> PC 통신 시 I/O 확인하는 변수들
        public short m_dev;
        bool[] gbool_di = new bool[16];
        bool[] re_gbool_di = new bool[16];
        bool[] gbool_do = new bool[16];
        ushort[] didata = new ushort[16];
        #endregion 

        //********************BAUMER CAMERA 변수********************//
        #region BAUMER CAMEREA
        public int camcount;
        internal string[] m_sSerialNumber;
        internal int[] m_nBufferNum;
        internal SystemList[] systemList = null;
        internal BGAPI2.System[] mSystem = null;
        internal string[] sSystemID;

        internal InterfaceList[] interfaceList = null;
        internal Interface[] mInterface = null;
        internal string[] sInterfaceID;

        internal DeviceList[] deviceList = null;
        internal Device[] mDevice = null;
        internal string[] sDeviceID;

        internal DataStreamList[] datastreamList = null;
        internal DataStream[] mDataStream = null;
        internal string[] sDataStreamID;

        internal BufferList[] bufferList = null;
        internal BGAPI2.Buffer[] mBuffer = null;

        internal Node[] mNode = null;
        internal int[] returnCode;

        internal int[] m_nCamWidth;
        internal int[] m_nCamHeight;
        internal int[] frames;
        internal double[] ExposureValue;
        internal double[] GainValue;
        internal bool[] m_FinalGrabState;

        internal bool[] bGrabberValid;

        internal string[] mStrExc;

        internal IntPtr[] m_SnapBuffer;

        internal Bitmap[] m_SnapImage;

        public EventCallBack m_Callback;

        internal bool[] m_bSingleSnap;
        Frm_Loading frm_loading;
        #endregion

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public Frm_Main()
        {
            Glob = PGgloble.getInstance; //전역변수 사용
            //별도 로딩프로그램 만들기
            Process.Start($"{Glob.LOADINGFROM}");
            InitializeComponent();
            ColumnHeader h = new ColumnHeader();
            StandFirst(); //처음 실행되어야 하는부분. - 이거 왜했지.. 이유는 모르겠다 일단 냅두자 필요없을꺼같기도함. - 20200121 김형민
            Glob.RunnModel = new Cogs.Model(); //코그넥스 모델 확인.
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
            Initialize_CamvalueInit(); //카메라 초기화
            LoadSetup(); //프로그램 셋팅 로드.
            //Line1DataLoad(); //LINE #1 DATA LOAD - TOOL NAME
            //Line2DataLoad(); //LINE #2 DATA LOAD - TOOL NAME
            timer_Setting.Start(); //타이머에서 계속해서 확인하는 것들
            InitializeDIO(); //IO보드 통신연결.
            Initialize_CameraInit(); //카메라 초기화 및 연결 - 카메라연결을 제일 마지막에 해줘야한다.
            CamSet();
            CognexModelLoad();
            log.AddLogMessage(LogType.Infomation, 0, "Vision Program Start");
            //Process.Start($"{Glob.LOADINGFROM}");
            Process[] myProcesses = Process.GetProcessesByName("LoadingForm_KHM");
            if (myProcesses.LongLength > 0)
            {
                myProcesses[0].Kill();
            }
        }
        private void CamSet()
        {
            try
            {
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");
                for (int i = 0; i < camcount; i++)
                {
                    ExposureValue[i] = Convert.ToInt32(CamSet.ReadData($"Camera{i}", "Exposure"));
                    GainValue[i] = Convert.ToInt32(CamSet.ReadData($"Camera{i}", "Gain"));
                }
                for (int i = 0; i < deviceList[0].Count; i++) //decivelist로 변경 - 20210803 김형민
                {
                    // 20201216 김형민 추가 - 카메라 셋팅 불러오기.
                    Thread.Sleep(200);
                    mDevice[i].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[i].StopAcquisition();
                    Thread.Sleep(200);
                    mDevice[i].RemoteNodeList["ExposureTime"].Value = ExposureValue[i];
                    mDevice[i].RemoteNodeList["Gain"].Value = GainValue[i];
                    Thread.Sleep(200);

                    bGrabberValid[i] = true;
                }
            }
            catch(Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
            }
         
        }

        private void Initialize_LightControl()
        {
            try
            {
                INIControl setting = new INIControl(Glob.SETTING);
                if (LightControl.IsOpen == true)
                {
                    LightControl.Close();
                }
                LightControl.BaudRate = Convert.ToInt32(setting.ReadData("COMMUNICATION", "Baud Rate"));
                LightControl.Parity = Parity.None;
                LightControl.DataBits = Convert.ToInt32(setting.ReadData("COMMUNICATION", "Data Bits"));
                LightControl.StopBits = StopBits.One;
                LightControl.PortName = setting.ReadData("COMMUNICATION", "Port number");
                LightControl.Open();
                LightOFF(); // 처음 실행했을때는 조명을 꺼주자. (AUTO모드로 변경됐을때, 조명 켜주자)
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }

        private void Initialize_CamvalueInit()
        {
            camcount = Program.CameraList.Count();
            Colorimage = new CogImage24PlanarColor[camcount];
            Monoimage = new CogImage8Grey[camcount];
            trigflag = new bool[camcount];
            m_sSerialNumber = new string[camcount];
            m_nBufferNum = new int[camcount];
            systemList = new SystemList[camcount];
            mSystem = new BGAPI2.System[camcount];
            sSystemID = new string[camcount];

            interfaceList = new InterfaceList[camcount];
            mInterface = new Interface[camcount];
            sInterfaceID = new string[camcount];

            deviceList = new DeviceList[camcount];
            mDevice = new Device[camcount];
            sDeviceID = new string[camcount];

            datastreamList = new DataStreamList[camcount];
            mDataStream = new DataStream[camcount];
            sDataStreamID = new string[camcount];

            bufferList = new BufferList[camcount];
            mBuffer = new BGAPI2.Buffer[camcount];

            mNode = new Node[camcount];
            returnCode = new int[camcount];

            m_nCamWidth = new int[camcount];
            m_nCamHeight = new int[camcount];
            frames = new int[camcount];

            m_FinalGrabState = new bool[camcount];

            bGrabberValid = new bool[camcount];

            mStrExc = new string[camcount];

            m_SnapBuffer = new IntPtr[camcount];

            m_SnapImage = new Bitmap[camcount];

            m_bSingleSnap = new bool[camcount];
            ExposureValue = new double[camcount];
            GainValue = new double[camcount];

            for (int i = 0; i < camcount; i++)
            {
                //시리얼입력
                m_sSerialNumber[i] = "";
                m_nBufferNum[i] = 10;
                systemList[i] = null;
                mSystem[i] = null;
                sSystemID[i] = "";
                interfaceList[i] = null;
                mInterface[i] = null;
                sInterfaceID[i] = "";
                deviceList[i] = null;
                mDevice[i] = null;
                sDeviceID[i] = "";
                datastreamList[i] = null;
                mDataStream[i] = null;
                sDataStreamID[i] = "";
                bufferList[i] = null;
                mBuffer[i] = null;
                mNode[i] = null;
                returnCode[i] = 0;
                m_nCamWidth[i] = 0;
                m_nCamHeight[i] = 0;
                frames[i] = 10;
                m_FinalGrabState[i] = false;
                bGrabberValid[i] = false;
                mStrExc[i] = "";
                m_SnapImage[i] = null;
                m_bSingleSnap[i] = false;
                Colorimage[i] = null;
                trigflag[i] = false;
                ExposureValue[i] = 0;
                GainValue[i] = 0;
            }
            for (int i = 0; i < camcount; i++)
            {
                m_sSerialNumber[i] = Program.CameraList[i].SerialNum; //카메라별 시리얼넘버.
            }
            //m_sSerialNumber[0] = "1202442317";
        }
        internal void CreateGrayColorMap(ref Bitmap image)
        {
            ColorPalette pal = image.Palette;
            Color[] entries = pal.Entries;
            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }
            image.Palette = pal;
        }
        #region 카메라 초기화부분
        private void Initialize_CameraInit()
        {
            //연결하면서 CAMERA에 EXPOSURE값 및 GAIN값 적용하는 부분 추가해야됨 // 20201215 - 김형민
            try
            {
                bool bDeviceHit = false;
                //Find specified camera
                try
                {
                    for (int i = 0; i < camcount; i++)
                    {
                        if (i == 0)
                        {
                            systemList[0] = SystemList.Instance;
                            systemList[0].Refresh();
                        }
                        bDeviceHit = false;
                        foreach (KeyValuePair<string, BGAPI2.System> sys_pair in BGAPI2.SystemList.Instance)
                        {
                            //Camera  Open  확인.
                            if (sys_pair.Value.IsOpen == false)
                            {
                                sys_pair.Value.Open();
                            }
                            interfaceList[0] = sys_pair.Value.Interfaces;
                            interfaceList[0].Refresh(100);

                            if (interfaceList[0].Count <= 0)
                            {
                                sys_pair.Value.Close();
                                continue;
                            }

                            foreach (KeyValuePair<string, Interface> ifc_pair in interfaceList[0])
                            {
                                try
                                {
                                    if (ifc_pair.Value.IsOpen == false)
                                    {
                                        ifc_pair.Value.Open();
                                    }
                                    deviceList[0] = ifc_pair.Value.Devices;
                                    deviceList[0].Refresh(100);

                                    if (deviceList[0].Count <= 0)
                                    {
                                        ifc_pair.Value.Close();
                                        continue;
                                    }
                                    foreach (KeyValuePair<string, Device> dev_pair in deviceList[0])
                                    {
                                        if (dev_pair.Value.IsOpen == false)
                                        {
                                            dev_pair.Value.Open();
                                        }
                                        if (dev_pair.Value.SerialNumber == m_sSerialNumber[i])
                                        {
                                            bDeviceHit = true;
                                            sSystemID[i] = sys_pair.Key;
                                            sInterfaceID[i] = ifc_pair.Key;
                                            sDeviceID[i] = dev_pair.Key;
                                            mSystem[i] = systemList[0][sSystemID[i]];
                                            mInterface[i] = interfaceList[0][sInterfaceID[i]];
                                            mDevice[i] = deviceList[0][sDeviceID[i]];
                                        }
                                        if (bDeviceHit) break;
                                    }
                                }
                                catch (BGAPI2.Exceptions.ResourceInUseException ex)
                                {
                                    log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
                                    //cm.info(ex.Message);
                                    return;
                                }
                                if (bDeviceHit) break;
                            }
                            if (bDeviceHit) break;
                        }
                    }
                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
                    return;
                }

                if (bDeviceHit == false)
                {
                    return;  //there are no camera has specified displayname in system.
                }
                //SetTriggerMode(false);
                //Set data stream
                try
                {
                    //COUNTING AVAILABLE DATASTREAMS
                    for (int i = 0; i < camcount; i++)
                    {
                        datastreamList[i] = mDevice[i].DataStreams;
                        datastreamList[i].Refresh();
                        //OPEN THE FIRST DATASTREAM IN THE LIST
                        //datastreamList[0] => datastreamList[i] 변경. - 20200217
                        foreach (KeyValuePair<string, DataStream> dst_pair in datastreamList[i])
                        {
                            dst_pair.Value.Open();
                            sDataStreamID[i] = dst_pair.Key;
                            break;
                        }

                        if (sDataStreamID[i] == "")
                        {
                            mDevice[i].Close();
                            mInterface[i].Close();
                            mSystem[i].Close();
                            log.AddLogMessage(LogType.Error, 0, $"CAM{i + 1} NOT CONNECTED");
                            //StatsCheck($"CAM{i + 1} NOT CONNECTED", true);
                            //return;
                        }
                        else
                        {
                            switch (i) //CAMERA 이벤트 생성.
                            {
                                case 0:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent1);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent1);
                                    break;
                                case 1:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent2);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent2);
                                    break;
                                case 2:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent3);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent3);
                                    break;
                                case 3:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent4);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent4);
                                    break;
                                case 4:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent5);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent5);
                                    break;
                                case 5:
                                    mInterface[i].RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mInterface[i].PnPEvent += new BGAPI2.Events.InterfaceEventControl.PnPEventHandler(mInterface_PnPEvent6);
                                    mDataStream[i] = datastreamList[i][sDataStreamID[i]];
                                    mDataStream[i].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                    mDataStream[i].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent6);
                                    break;
                            }
                        }
                    }

                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
                    return;
                }

                //Set Buffer
                try
                {
                    for (int j = 0; j < camcount; j++)
                    {
                        bufferList[j] = mDataStream[j].BufferList;

                        // 4 buffers using internal buffer mode
                        for (int i = 0; i < m_nBufferNum[j]; i++)
                        {
                            mBuffer[j] = new BGAPI2.Buffer();
                            bufferList[j].Add(mBuffer[j]);
                            mBuffer[j].QueueBuffer();
                        }
                    }
                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
                    return;
                }
                for (int i = 0; i < camcount; i++)
                {
                    int.TryParse(mDevice[i].RemoteNodeList["Height"].Value.ToString(), out m_nCamHeight[i]);
                    int.TryParse(mDevice[i].RemoteNodeList["Width"].Value.ToString(), out m_nCamWidth[i]);
                    // 20201216 김형민 추가 - 카메라 셋팅 불러오기.
                    //StartLive1(i);
                    //Thread.Sleep(200);
                    //mDevice[i].RemoteNodeList["AcquisitionStop"].Execute();
                    //mDataStream[i].StopAcquisition();
                    //Thread.Sleep(200);
                    //////exposurevalue[Glob.CamNumber] = (double)num_Exposure.Value;
                    //mDevice[i].RemoteNodeList["ExposureTimeAbs"].Value = ExposureValue[i];
                    //mDevice[i].RemoteNodeList["GainAbs"].Value = GainValue[i];
                    //Thread.Sleep(200);
                    //StopLive1(i);
                    //Thread.Sleep(200);
                    //if (FormLoad == false)
                    //{
                    //    Main.mDataStream[Glob.CamNumber].StartAcquisition();
                    //    Main.mDevice[Glob.CamNumber].RemoteNodeList["AcquisitionStart"].Execute();
                    //}
                    //mDevice[i].RemoteNodeList["ExposureTimeAbs"].Value = ExposureValue[i];
                    //mDevice[i].RemoteNodeList["GainAbs"].Value = GainValue[i];
                    //mDevice[i].RemoteNodeList["ReverseX"].Value = true;
                    bGrabberValid[i] = true;
                }
            }

            catch (Exception ex)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
            }
        }
        protected void mInterface_PnPEvent1(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(0);
                    bool bSuccess = ReconnectGrabber(0);
                    if (bSuccess == true)
                    {
                        log.AddLogMessage(LogType.Infomation, 0, "Camera ReConnect Success");
                    }
                    else
                    {
                        //log.AddLogMessage(LogType.Error, 0, $"{ex.Message}");
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void mInterface_PnPEvent2(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(1);
                    bool bSuccess = ReconnectGrabber(1);
                    if (bSuccess == true)
                    {
                        //Log("Camera ReConnect Success");
                    }
                    else
                    {
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void mInterface_PnPEvent3(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(2);
                    bool bSuccess = ReconnectGrabber(2);
                    if (bSuccess == true)
                    {
                        //Log("Camera ReConnect Success");
                    }
                    else
                    {
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void mInterface_PnPEvent4(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(3);
                    bool bSuccess = ReconnectGrabber(3);
                    if (bSuccess == true)
                    {
                        //Log("Camera ReConnect Success");
                    }
                    else
                    {
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void mInterface_PnPEvent5(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(4);
                    bool bSuccess = ReconnectGrabber(4);
                    if (bSuccess == true)
                    {
                        //Log("Camera ReConnect Success");
                    }
                    else
                    {
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void mInterface_PnPEvent6(object sender, BGAPI2.Events.PnPEventArgs mPnPEvent)
        {
            try
            {
                string strDevID = mPnPEvent.ID;
                string serialNumber = mPnPEvent.SerialNumber;
                BGAPI2.Events.PnPType type = mPnPEvent.PnPType;

                if (type == BGAPI2.Events.PnPType.DEVICEREMOVED)
                {
                    ResetGrabber(5);
                    bool bSuccess = ReconnectGrabber(5);
                    if (bSuccess == true)
                    {
                        //Log("Camera ReConnect Success");
                    }
                    else
                    {
                        cm.info("Camera ReConnect Fail");
                    }
                    //timer_camreconnect.Start();
                }
            }
            catch (Exception ex)
            {
                cm.info(ex.Message);
            }
        }
        protected void ResetGrabber(int camnumber)
        {
            try
            {
                mDataStream[camnumber].StopAcquisition();
                bufferList[camnumber].DiscardAllBuffers();
                while (bufferList[camnumber].Count > 0)
                {
                    mBuffer[camnumber] = bufferList[camnumber].Values.First();
                    bufferList[camnumber].RevokeBuffer(mBuffer[camnumber]);
                }
                mDataStream[camnumber].Close();
                mDevice[camnumber].Close();
            }
            catch (BGAPI2.Exceptions.IException bgEx)
            {
                log.AddLogMessage(LogType.Error, 0, $"{bgEx.Message}");
                // cm.info(bgEx.Message);
            }
            catch (Exception e)
            {
                log.AddLogMessage(LogType.Error, 0, $"{e.Message}");
                //cm.info(e.Message);
            }

            return;
        }
        #endregion

        //********************실질적으로 카메라가 촬영하는 부분********************//
        #region 카메라 이미지 그랩이벤트 CAM0
        void mDataStream_NewBufferEvent1(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null) //메인화면일때.
                {
                    //log.AddLogMessage(LogType.Infomation, 0, "cam1 shot start");
                    InspectFlag[0] = true;
                    OutPutSignal_Off(1);
                    OutPutSignal_Off(2);
                }
                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    //Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {
                    m_SnapBuffer[0] = bufferFilled.MemPtr + nBufferOffset;
                    m_SnapImage[0] = new Bitmap(m_nCamWidth[0], m_nCamHeight[0], m_nCamWidth[0], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[0]);
                    Monoimage[0] = new CogImage8Grey(m_SnapImage[0]);

                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[0];
                        if (m_bSingleSnap[0] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[0] = new Stopwatch();
                        InspectTime[0].Reset();
                        InspectTime[0].Start();

                        cdyDisplay.Image = Monoimage[0];
                        if (m_bSingleSnap[0] == true)
                            cdyDisplay.Fit();
                        if (Inspect_Cam0(cdyDisplay) == true) // 검사 결과
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
                            OutPutSignal_On(1);
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
                            //검사 결과 NG
                            OutPutSignal_On(2);
                        }

                        //그래프 넣는거 테스트 진행중 - 2022/02/10 김형민
                        //double dd = TempDistance[0, 1].DistanceValue(1) * TempDistance_CalibrationValue[0, 1];
                        //Chart_Point1.Series["2.4부"].Points.AddY(dd);
                        //Chart_Point1.Series["3.3부"].Points.AddY(Glob.Point1_InspectResult2);

                        InspectTime[0].Stop();
                        InspectFlag[0] = false;
                        //StatsCheck($"InspcetFlag[0]={InspectFlag[0].ToString()}", false);
                        //DataSave1(InspectTime[0].ElapsedMilliseconds.ToString(), 0);


                        BeginInvoke((Action)delegate { lb_Cam1_InsTime.Text = InspectTime[0].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }

                    // queue buffer again
                    //System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[0])
                    {
                        m_bSingleSnap[0] = false;
                        mDevice[0].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[0].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    //System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[0]++;
                Monoimage[0] = null;
                m_SnapImage[0] = null;
                if (frames[0] >= 2)
                {
                    frames[0] = 0;
                    GC.Collect();
                }
                //Thread.Sleep(100);
                //OutPutSignal_Off(1);
                //OutPutSignal_Off(2);
                return;

            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                //Log(ex.Message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

            }
        }
        #endregion

        #region 카메라 이미지 그랩이벤트 CAM1
        void mDataStream_NewBufferEvent2(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null)
                {
                    InspectFlag[1] = true;
                    //StatsCheck($"InspcetFlag[1]={InspectFlag[1].ToString()}", false);
                    OutPutSignal_Off(3);
                    OutPutSignal_Off(4);
                }
                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    //System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {

                    m_SnapBuffer[1] = bufferFilled.MemPtr + nBufferOffset;
                    //BGAPI2.Image mImage = imgProcessor.CreateImage((uint)m_nCamWidth[1], (uint)m_nCamHeight[1], (string)bufferFilled.PixelFormat, m_SnapBuffer[1], (ulong)bufferFilled.MemSize);
                    //m_SnapImage[1] = mImage.CreateBitmap(true);//mTransformImage.CreateBitmap(true);
                    m_SnapImage[1] = new Bitmap(m_nCamWidth[1], m_nCamHeight[1], m_nCamWidth[1], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[1]);
                    Monoimage[1] = new CogImage8Grey(m_SnapImage[1]);

                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[1];
                        if (m_bSingleSnap[1] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[1] = new Stopwatch();
                        InspectTime[1].Reset();
                        InspectTime[1].Start();

                        cdyDisplay2.Image = Monoimage[1];
                        if (m_bSingleSnap[1] == true)
                            cdyDisplay2.Fit();
                        if (Inspect_Cam1(cdyDisplay2) == true) // 검사 결과
                        {
                            //검사 결과 OK

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line1, 1, 3);
                                lb_Cam2_Result.BackColor = Color.Lime;
                                lb_Cam2_Result.Text = "O K";
                                OK_Count[1]++;
                                if (Glob.OKImageSave)
                                    ImageSave2("OK", 2, cdyDisplay2);
                            });
                            OutPutSignal_On(3);
                        }
                        else
                        {
                            //검사 결과 NG

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line1, 1, 3);
                                lb_Cam2_Result.BackColor = Color.Red;
                                lb_Cam2_Result.Text = "N G";
                                NG_Count[1]++;
                                if (Glob.NGImageSave)
                                    ImageSave2("NG", 2, cdyDisplay2);
                            });
                            OutPutSignal_On(4);
                        }
                        InspectTime[1].Stop();
                        InspectFlag[1] = false;
                        //OutPutSignal_Off(3);
                        //OutPutSignal_Off(4);
                        //DataSave2(InspectTime[1].ElapsedMilliseconds.ToString(), 1);
                        //StatsCheck($"InspcetFlag[1]={InspectFlag[1].ToString()}", false);
                        BeginInvoke((Action)delegate { lb_Cam2_InsTime.Text = InspectTime[1].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }

                    // queue buffer again
                    //System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[1])
                    {
                        m_bSingleSnap[1] = false;
                        mDevice[1].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[1].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[1]++;
                Monoimage[1] = null;
                m_SnapImage[1] = null;
                if (frames[1] >= 2)
                {
                    frames[1] = 0;
                    GC.Collect();
                }

                //Thread.Sleep(500);
                //OutPutSignal_Off(3);
                //OutPutSignal_Off(4);

                return;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                //Log(ex.Message);
            }

        }
        #endregion

        #region 카메라 이미지 그랩이벤트 CAM2
        void mDataStream_NewBufferEvent3(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null)
                {
                    InspectFlag[2] = true;
                    //StatsCheck($"InspcetFlag[2]={InspectFlag[2].ToString()}", false);
                    OutPutSignal_Off(5);
                    OutPutSignal_Off(6);
                }

                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {

                    m_SnapBuffer[2] = bufferFilled.MemPtr + nBufferOffset;

                    //BGAPI2.Image mImage = imgProcessor.CreateImage((uint)m_nCamWidth[2], (uint)m_nCamHeight[2], (string)bufferFilled.PixelFormat, m_SnapBuffer[2], (ulong)bufferFilled.MemSize);
                    //m_SnapImage[2] = mImage.CreateBitmap(true);//mTransformImage.CreateBitmap(true);
                    m_SnapImage[2] = new Bitmap(m_nCamWidth[2], m_nCamHeight[2], m_nCamWidth[2], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[2]);

                    Monoimage[2] = new CogImage8Grey(m_SnapImage[2]);

                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[2];
                        if (m_bSingleSnap[2] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[2] = new Stopwatch();
                        InspectTime[2].Reset();
                        InspectTime[2].Start();

                        cdyDisplay3.Image = Monoimage[2];
                        if (m_bSingleSnap[2] == true)
                            cdyDisplay3.Fit();
                        if (Inspect_Cam2(cdyDisplay3) == true) // 검사 결과
                        {
                            //검사 결과 OK

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line1, 2, 5); //-추가된함수
                                lb_Cam3_Result.BackColor = Color.Lime;
                                lb_Cam3_Result.Text = "O K";
                                OK_Count[2]++;
                                if (Glob.OKImageSave)
                                    ImageSave3("OK", 3, cdyDisplay3);
                            });
                            OutPutSignal_On(5);
                        }
                        else
                        {
                            //검사 결과 NG

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line1, 2, 5); //-추가된함수
                                lb_Cam3_Result.BackColor = Color.Red;
                                lb_Cam3_Result.Text = "N G";
                                NG_Count[2]++;
                                if (Glob.NGImageSave)
                                    ImageSave3("NG", 3, cdyDisplay3);
                            });
                            OutPutSignal_On(6);
                        }
                        InspectTime[2].Stop();
                        InspectFlag[2] = false;
                        //OutPutSignal_Off(5);
                        //OutPutSignal_Off(6);
                        //DataSave3(InspectTime[2].ElapsedMilliseconds.ToString(), 2);
                        //StatsCheck($"InspcetFlag[2]={InspectFlag[2].ToString()}", false);
                        BeginInvoke((Action)delegate { lb_Cam3_InsTime.Text = InspectTime[2].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }

                    // queue buffer again
                    System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[2])
                    {
                        m_bSingleSnap[2] = false;
                        mDevice[2].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[2].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[2]++;
                Monoimage[2] = null;
                m_SnapImage[2] = null;
                if (frames[2] >= 2)
                {
                    frames[2] = 0;
                    GC.Collect();
                }

                //Thread.Sleep(500);
                //OutPutSignal_Off(5);
                //OutPutSignal_Off(6);

                return;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                //Log(ex.Message);
            }
        }
        #endregion

        #region 카메라 이미지 그랩이벤트 CAM3
        void mDataStream_NewBufferEvent4(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null)
                {
                    InspectFlag[3] = true;
                    //StatsCheck($"InspcetFlag[3]={InspectFlag[3].ToString()}", false);
                    OutPutSignal_Off(9);
                    OutPutSignal_Off(10);
                }
                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {

                    m_SnapBuffer[3] = bufferFilled.MemPtr + nBufferOffset;

                    //BGAPI2.Image mImage = imgProcessor.CreateImage((uint)m_nCamWidth[3], (uint)m_nCamHeight[3], (string)bufferFilled.PixelFormat, m_SnapBuffer[3], (ulong)bufferFilled.MemSize);
                    //m_SnapImage[3] = mImage.CreateBitmap(true);//mTransformImage.CreateBitmap(true);
                    m_SnapImage[3] = new Bitmap(m_nCamWidth[3], m_nCamHeight[3], m_nCamWidth[3], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[3]);

                    Monoimage[3] = new CogImage8Grey(m_SnapImage[3]);
                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[3];
                        if (m_bSingleSnap[3] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[3] = new Stopwatch();
                        InspectTime[3].Reset();
                        InspectTime[3].Start();

                        cdyDisplay4.Image = Monoimage[3];
                        cdyDisplay4.Fit();
                        if (Inspect_Cam3(cdyDisplay4) == true) // 검사 결과
                        {
                            //검사 결과 OK

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 3, 1); //-추가된함수
                                lb_Cam4_Result.BackColor = Color.Lime;
                                lb_Cam4_Result.Text = "O K";
                                OK_Count[3]++;
                                if (Glob.OKImageSave)
                                    ImageSave4("OK", 4, cdyDisplay4);
                            });
                            OutPutSignal_On(9);
                        }
                        else
                        {
                            //검사 결과 NG

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 3, 1); //-추가된함수
                                lb_Cam4_Result.BackColor = Color.Red;
                                lb_Cam4_Result.Text = "N G";
                                NG_Count[3]++;
                                if (Glob.NGImageSave)
                                    ImageSave4("NG", 4, cdyDisplay4);
                            });
                            OutPutSignal_On(10);
                        }
                        InspectTime[3].Stop();
                        InspectFlag[3] = false;
                        DataSave4(InspectTime[3].ElapsedMilliseconds.ToString(), 3);
                        //StatsCheck($"InspcetFlag[3]={InspectFlag[3].ToString()}", false);
                        BeginInvoke((Action)delegate { lb_Cam4_InsTime.Text = InspectTime[3].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }
                    // queue buffer again
                    System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[3])
                    {
                        m_bSingleSnap[3] = false;
                        mDevice[3].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[3].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[3]++;
                Colorimage[3] = null;
                m_SnapImage[3] = null;
                if (frames[3] >= 2)
                {
                    frames[3] = 0;
                    GC.Collect();
                }

                //Thread.Sleep(500);
                //OutPutSignal_Off(9);
                //OutPutSignal_Off(10);

                return;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                //Log(ex.Message);
            }

            // return;
        }
        #endregion

        #region  카메라 이미지 그랩이벤트 CAM4
        void mDataStream_NewBufferEvent5(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null)
                {
                    InspectFlag[4] = true;
                    //StatsCheck($"InspcetFlag[4]={InspectFlag[4].ToString()}", false);
                    OutPutSignal_Off(11);
                    OutPutSignal_Off(12);
                }
                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {

                    m_SnapBuffer[4] = bufferFilled.MemPtr + nBufferOffset;

                    //BGAPI2.Image mImage = imgProcessor.CreateImage((uint)m_nCamWidth[4], (uint)m_nCamHeight[4], (string)bufferFilled.PixelFormat, m_SnapBuffer[4], (ulong)bufferFilled.MemSize);
                    //m_SnapImage[4] = mImage.CreateBitmap(true);//mTransformImage.CreateBitmap(true);
                    m_SnapImage[4] = new Bitmap(m_nCamWidth[4], m_nCamHeight[4], m_nCamWidth[4], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[4]);

                    Monoimage[4] = new CogImage8Grey(m_SnapImage[4]);

                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[4];
                        if (m_bSingleSnap[4] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[4] = new Stopwatch();
                        InspectTime[4].Reset();
                        InspectTime[4].Start();

                        cdyDisplay5.Image = Monoimage[4];
                        cdyDisplay5.Fit();
                        if (Inspect_Cam4(cdyDisplay5) == true) // 검사 결과
                        {
                            //검사 결과 OK

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 4, 3);
                                lb_Cam5_Result.BackColor = Color.Lime;
                                lb_Cam5_Result.Text = "O K";
                                OK_Count[4]++;
                                if (Glob.OKImageSave)
                                    ImageSave5("OK", 5, cdyDisplay5);
                            });
                            OutPutSignal_On(11);
                        }
                        else
                        {
                            //검사 결과 NG

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 4, 3);
                                lb_Cam5_Result.BackColor = Color.Red;
                                lb_Cam5_Result.Text = "N G";
                                NG_Count[4]++;
                                if (Glob.NGImageSave)
                                    ImageSave5("NG", 5, cdyDisplay5);
                            });
                            OutPutSignal_On(12);
                        }
                        InspectTime[4].Stop();
                        InspectFlag[4] = false;
                        DataSave5(InspectTime[4].ElapsedMilliseconds.ToString(), 4);
                        //StatsCheck($"InspcetFlag[4]={InspectFlag[4].ToString()}", false);
                        BeginInvoke((Action)delegate { lb_Cam5_InsTime.Text = InspectTime[4].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }
                    // queue buffer again
                    System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[4])
                    {
                        m_bSingleSnap[4] = false;
                        mDevice[4].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[4].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[4]++;
                Colorimage[4] = null;
                m_SnapImage[4] = null;
                if (frames[4] >= 2)
                {
                    frames[4] = 0;
                    GC.Collect();
                }

                //Thread.Sleep(500);
                //OutPutSignal_Off(11);
                //OutPutSignal_Off(12);

                return;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                // Log(ex.Message);
            }

        }
        #endregion

        #region 카메라 이미지 그랩이벤트 CAM5
        void mDataStream_NewBufferEvent6(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                if (frm_toolsetup == null)
                {
                    InspectFlag[5] = true;
                    //StatsCheck($"InspcetFlag[5]={InspectFlag[5].ToString()}", false);
                    OutPutSignal_Off(13);
                    OutPutSignal_Off(14);
                }
                ImageProcessor imgProcessor = imgProcessor = ImageProcessor.Instance;
                BGAPI2.Buffer bufferFilled = null;
                bufferFilled = mDSEvent.BufferObj;
                int nBufferOffset = (int)bufferFilled.ImageOffset;
                if (bufferFilled == null)
                {
                    System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                }
                else if (bufferFilled != null)
                {

                    m_SnapBuffer[5] = bufferFilled.MemPtr + nBufferOffset;

                    //BGAPI2.Image mImage = imgProcessor.CreateImage((uint)m_nCamWidth[5], (uint)m_nCamHeight[5], (string)bufferFilled.PixelFormat, m_SnapBuffer[5], (ulong)bufferFilled.MemSize);
                    //m_SnapImage[5] = mImage.CreateBitmap(true);//mTransformImage.CreateBitmap(true);
                    m_SnapImage[5] = new Bitmap(m_nCamWidth[5], m_nCamHeight[5], m_nCamWidth[5], PixelFormat.Format8bppIndexed, bufferFilled.MemPtr + nBufferOffset);
                    CreateGrayColorMap(ref m_SnapImage[5]);

                    Monoimage[5] = new CogImage8Grey(m_SnapImage[5]);

                    if (frm_toolsetup != null)
                    {
                        frm_toolsetup.cdyDisplay.Image = Monoimage[5];
                        if (m_bSingleSnap[5] == true)
                        {
                            frm_toolsetup.cdyDisplay.Fit();
                        }
                    }
                    else
                    {
                        InspectTime[5] = new Stopwatch();
                        InspectTime[5].Reset();
                        InspectTime[5].Start();

                        cdyDisplay6.Image = Monoimage[5];
                        cdyDisplay6.Fit();
                        if (Inspect_Cam5(cdyDisplay6) == true) // 검사 결과
                        {
                            //검사 결과 OK

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 5, 5);
                                lb_Cam6_Result.BackColor = Color.Lime;
                                lb_Cam6_Result.Text = "O K";
                                OK_Count[5]++;
                                if (Glob.OKImageSave)
                                    ImageSave6("OK", 6, cdyDisplay6);
                            });
                            OutPutSignal_On(13);
                        }
                        else
                        {
                            //검사 결과 NG

                            BeginInvoke((Action)delegate
                            {
                                //DgvResult(dgv_Line2, 5, 5);
                                lb_Cam6_Result.BackColor = Color.Red;
                                lb_Cam6_Result.Text = "N G";
                                NG_Count[5]++;
                                if (Glob.NGImageSave)
                                    ImageSave6("NG", 6, cdyDisplay6);
                            });
                            OutPutSignal_On(14);
                        }
                        InspectTime[5].Stop();
                        InspectFlag[5] = false;
                        DataSave6(InspectTime[5].ElapsedMilliseconds.ToString(), 5);
                        //StatsCheck($"InspcetFlag[5]={InspectFlag[5].ToString()}", false);
                        BeginInvoke((Action)delegate { lb_Cam6_InsTime.Text = InspectTime[5].ElapsedMilliseconds.ToString() + "msec"; });
                        Thread.Sleep(100);
                    }
                    // queue buffer again
                    System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", bufferFilled.FrameID, (ulong)bufferFilled.MemPtr);
                    bufferFilled.QueueBuffer();
                    if (m_bSingleSnap[5])
                    {
                        m_bSingleSnap[5] = false;
                        mDevice[5].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[5].StopAcquisition();
                        //gbool_grabcomplete1 = true;
                        //gbool_grab1 = true;
                    }
                }
                else if (bufferFilled.IsIncomplete == true)
                {
                    System.Console.Write("Error: Image is incomplete\r\n");
                    // queue buffer again
                    bufferFilled.QueueBuffer();
                }

                frames[5]++;
                Colorimage[5] = null;
                m_SnapImage[5] = null;
                if (frames[5] >= 2)
                {
                    frames[5] = 0;
                    GC.Collect();
                }

                //Thread.Sleep(500);
                //OutPutSignal_Off(13);
                //OutPutSignal_Off(14);

                return;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                //Log(ex.Message);
            }
        }
        #endregion

        protected bool ReconnectGrabber(int camnumber)
        {
            //Camera Reconnect Grabber 부분 디버깅 해봐야댐
            bool bDeviceHit = false;
            Device curDevice = null;

            //Find specified camera
            try
            {
                systemList[camnumber] = SystemList.Instance;
                systemList[camnumber].Refresh();

                try
                {
                    deviceList[camnumber] = mInterface[camnumber].Devices;
                    deviceList[camnumber].Refresh(100);

                    foreach (KeyValuePair<string, Device> dev_pair in deviceList[camnumber])
                    {
                        curDevice = dev_pair.Value;
                        if (!curDevice.IsOpen) curDevice.Open();
                        if (dev_pair.Value.SerialNumber == m_sSerialNumber[camnumber]) bDeviceHit = true;
                        if (bDeviceHit) break;
                    }
                }
                catch (BGAPI2.Exceptions.ResourceInUseException ex)
                {
                    log.AddLogMessage(LogType.Error, 0, $"Reconnect 1 : {ex.Message}");
                    if (curDevice != null && curDevice.IsOpen)
                    {
                        curDevice.Close();
                    }
                    return false;
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                log.AddLogMessage(LogType.Error, 0, $"Reconnect 2 : {ex.Message}");
                if (curDevice != null && curDevice.IsOpen) curDevice.Close();

                return false;
            }

            if (bDeviceHit == false || curDevice == null)
            {
                if (curDevice != null && curDevice.IsOpen) curDevice.Close();
                return false;  //there are no camera has specified displayname in system.
            }
            mDevice[camnumber] = curDevice;

            //Set data stream
            DataStream curDataStream = null;
            try
            {
                //COUNTING AVAILABLE DATASTREAMS
                datastreamList[camnumber] = mDevice[camnumber].DataStreams;
                datastreamList[camnumber].Refresh();

                //OPEN THE FIRST DATASTREAM IN THE LIST
                foreach (KeyValuePair<string, BGAPI2.DataStream> dst_pair in datastreamList[camnumber])
                {
                    curDataStream = dst_pair.Value;
                    if (!curDataStream.IsOpen) curDataStream.Open();

                    sDataStreamID[camnumber] = dst_pair.Key;
                    break;
                }

                if (sDataStreamID[camnumber] == "")
                {
                    mDevice[camnumber].Close();
                    if (curDataStream != null && curDataStream.IsOpen) curDataStream.Close();

                    return false;
                }
                else
                {
                    mDataStream[camnumber] = curDataStream;// datastreamList[sDataStreamID];
                    mDataStream[camnumber].RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                    mDataStream[camnumber].NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent1);
                }

            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                mDevice[camnumber].Close();
                if (curDataStream != null && curDataStream.IsOpen) curDataStream.Close();
                log.AddLogMessage(LogType.Error, 0, $"Set DataStream : {ex.Message}");
                return false;
            }
            //Set Buffer
            try
            {
                bufferList[camnumber] = mDataStream[camnumber].BufferList;

                for (int i = 0; i < m_nBufferNum[camnumber]; i++)
                {
                    mBuffer[camnumber] = new BGAPI2.Buffer();
                    bufferList[camnumber].Add(mBuffer[camnumber]);
                    mBuffer[camnumber].QueueBuffer();
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                log.AddLogMessage(LogType.Error, 0, $"Set Buffer : {ex.Message}");
                mDevice[camnumber].Close();
                mDataStream[camnumber].Close();
                return false;
            }

            int.TryParse(mDevice[camnumber].RemoteNodeList["Height"].Value.ToString(), out m_nCamHeight[0]);
            int.TryParse(mDevice[camnumber].RemoteNodeList["Width"].Value.ToString(), out m_nCamWidth[0]);
            string strPF = mDevice[camnumber].RemoteNodeList["PixelFormat"].Value.ToString();

            bGrabberValid[camnumber] = true;
            return true;
        }

        //여러개의 카메라 동작시 겹치지 않는지...? 이벤트 중복으로 프로그램 뻗을수있을꺼같으니, 테스트 후 문제가 될 시에 이벤트 나누어주기.
        #region 카메라 동작 
        public void SnapShot(int camnumber)
        {
            try
            {
                if (mDataStream[camnumber].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    mDevice[camnumber].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[camnumber].StopAcquisition();
                    return;
                }
                m_bSingleSnap[camnumber] = true;
                mDataStream[camnumber].StartAcquisition(1);
                mDevice[camnumber].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                cm.info($"Camera is not connected : {ee.Message}");
            }
        }
        public void SnapShot1()
        {
            try
            {
                if (mDataStream[0].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot1 error");
                    mDevice[0].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[0].StopAcquisition();
                    return;
                }
                m_bSingleSnap[0] = true;
                mDataStream[0].StartAcquisition(1);
                mDevice[0].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                cm.info($"Camera 1 is not connected : {ee.Message}");
            }
        }
        public void SnapShot2()
        {
            try
            {
                if (mDataStream[1].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot2 error");
                    mDevice[1].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[1].StopAcquisition();
                    return;
                }
                m_bSingleSnap[1] = true;
                mDataStream[1].StartAcquisition(1);
                mDevice[1].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                cm.info($"Camera 2 is not connected : {ee.Message}");
            }
        }
        public void SnapShot3()
        {
            try
            {
                if (mDataStream[2].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot3 error");
                    mDevice[2].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[2].StopAcquisition();
                    return;
                }
                m_bSingleSnap[2] = true;
                mDataStream[2].StartAcquisition(1);
                mDevice[2].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("Camera is not connected");
                cm.info($"Camera 3 is not connected : {ee.Message}");
            }
        }
        public void SnapShot4()
        {
            try
            {
                if (mDataStream[3].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot4 error");
                    mDevice[3].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[3].StopAcquisition();
                    return;
                }
                m_bSingleSnap[3] = true;
                mDataStream[3].StartAcquisition(1);
                mDevice[3].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("Camera is not connected");
                cm.info($"Camera 4 is not connected : {ee.Message}");
            }
        }
        public void SnapShot5()
        {
            try
            {
                if (mDataStream[4].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot5 error");
                    mDevice[4].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[4].StopAcquisition();
                    return;
                }
                m_bSingleSnap[4] = true;
                mDataStream[4].StartAcquisition(1);
                mDevice[4].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("Camera is not connected");
                cm.info($"Camera 5 is not connected : {ee.Message}");
            }
        }
        public void SnapShot6()
        {
            try
            {
                if (mDataStream[5].IsGrabbing)
                {
                    //검사 NG났을때 이부분 타는지 확인해보기 - 191027
                    log.AddLogMessage(LogType.Infomation, 0, "SnapShot6 error");
                    mDevice[5].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[5].StopAcquisition();
                    return;
                }
                m_bSingleSnap[5] = true;
                mDataStream[5].StartAcquisition(1);
                mDevice[5].RemoteNodeList["AcquisitionStart"].Execute();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("Camera is not connected");
                cm.info($"Camera 6 is not connected : {ee.Message}");
            }
        }
        public bool StartLive1(int camnumber)
        {
            try
            {
                m_bSingleSnap[camnumber] = false;
                if (mDataStream[camnumber].IsGrabbing)
                {
                    mDevice[camnumber].RemoteNodeList["AcquisitionStop"].Execute();
                    mDataStream[camnumber].StopAcquisition();
                }
                mDataStream[camnumber].StartAcquisition();
                mDevice[camnumber].RemoteNodeList["AcquisitionStart"].Execute();
                return true;
            }
            catch (Exception)
            {
                cm.info("Camera is not connected.");
                return false;
            }
        }
        public void StopLive1(int camnumber)
        {
            try
            {
                mDevice[camnumber].RemoteNodeList["AcquisitionStop"].Execute();
                mDataStream[camnumber].StopAcquisition();
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }
        }
        #endregion
        private void InitializeDIO()
        {
            try
            {
                //카드번호 입력.
                m_dev = DASK.Register_Card(DASK.PCI_7230, 0);
                if (m_dev < 0)
                {
                    log.AddLogMessage(LogType.Error, 0, "Register_Card error!");
                }
                else
                {
                    ushort i;
                    short result;
                    for (i = 0; i < 16; i++)
                    {
                        result = DASK.DI_ReadLine((ushort)m_dev, 0, i, out didata[i]); //InPut 읽음 (카드넘버,포트0번,In단자번호,버퍼메모리(In단자1일때 1,In단자0일때 0) 
                        if (didata[i] == 1)
                        {
                            gbool_di[i] = true;
                        }
                        else
                        {
                            gbool_di[i] = false;
                        }
                    }
                    bk_IO.RunWorkerAsync(); // IO 백그라운드 스타트
                }
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"{ee.Message}");
            }
        }

        private void LoadSetup()
        {
            try
            {
                OK_Label = new Label[6] { lb_CAM1_OK, lb_CAM2_OK, lb_CAM3_OK, lb_CAM4_OK, lb_CAM5_OK, lb_CAM6_OK };
                NG_Label = new Label[6] { lb_CAM1_NG, lb_CAM2_NG, lb_CAM3_NG, lb_CAM4_NG, lb_CAM5_NG, lb_CAM6_NG };
                TOTAL_Label = new Label[6] { lb_CAM1_TOTAL, lb_CAM2_TOTAL, lb_CAM3_TOTAL, lb_CAM4_TOTAL, lb_CAM5_TOTAL, lb_CAM6_TOTAL };
                NGRATE_Label = new Label[6] { lb_CAM1_NGRATE, lb_CAM2_NGRATE, lb_CAM3_NGRATE, lb_CAM4_NGRATE, lb_CAM5_NGRATE, lb_CAM6_NGRATE };

                INIControl Modellist = new INIControl(Glob.MODELLIST); ;
                INIControl CFGFILE = new INIControl(Glob.CONFIGFILE); ;

                INIControl setting = new INIControl(Glob.SETTING);
                string LastModel = CFGFILE.ReadData("LASTMODEL", "NAME"); //마지막 사용모델 확인.
                //확인 필요. - LastModel Name 변수에 들어오는 String값 확인하기.
                INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{LastModel}\\CamSet.ini");
                for (int i = 0; i < camcount; i++)
                {
                    Glob.RunnModel.Loadmodel(LastModel, Glob.MODELROOT, i); //VISION TOOL LOAD
                }
                for (int i = 0; i < camcount; i++)
                {
                    ExposureValue[i] = Convert.ToInt32(CamSet.ReadData($"Camera{i}", "Exposure"));
                    GainValue[i] = Convert.ToInt32(CamSet.ReadData($"Camera{i}", "Gain"));
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

                //연결되어있는 디바이스(카메라)확인.
                //for (int i = 0; i < mDevice.Count(); i++)
                //{
                //    if (mDevice[i] != null)
                //    {
                //        mDevice[i].RemoteNodeList["AcquisitionStopAbs"].Execute();
                //        mDataStream[i].StopAcquisition();
                //        Thread.Sleep(200);
                //        mDevice[i].RemoteNodeList["ExposureTime"].Value = Convert.ToDouble(setting.ReadData("Camera", "Exposure"));
                //        Thread.Sleep(50);
                //    }
                //}
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
        //Output 신호들
        // 0 = Line #1 Vision Ready      8 = Line #2 Vision Ready
        // 1 = CAM 1 Ok                  9 = CAM 4 Ok 
        // 2 = CAM 1 NG                 10 = CAM 4 NG
        // 3 = CAM 2 OK                 11 = CAM 5 OK
        // 4 = CAM 2 NG                 12 = CAM 5 NG
        // 5 = CAM 3 OK                 13 = CAM 6 OK
        // 6 = CAM 3 NG                 14 = CAM 6 NG

        public void OutPutSignal_On(int jobNo)
        {
            short ret;
            ret = DASK.DO_WriteLine((ushort)m_dev, 0, (ushort)jobNo, 1);
        }

        public void OutPutSignal_Off(int jobNo)
        {
            short ret;
            ret = DASK.DO_WriteLine((ushort)m_dev, 0, (ushort)jobNo, 0);
        }

        private void OUTPUT_Click(object sender, EventArgs e)
        {
            try
            {
                short ret;
                int jobNo = Convert.ToInt16((sender as Button).Tag);
                ret = DASK.DO_WriteLine((ushort)m_dev, 0, (ushort)jobNo, 1);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }
        private void OUTPUTOFF_Click(object sender, EventArgs e)
        {
            try
            {
                short ret;
                int jobNo = Convert.ToInt16((sender as Button).Tag);
                ret = DASK.DO_WriteLine((ushort)m_dev, 0, (ushort)jobNo, 0);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }
        }
        private void bk_IO_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(200);
                if (bk_IO.CancellationPending == true) //취소요청이 들어오면 return
                {
                    return;
                }
                ushort i;
                short result;
                short o_result;

                for (i = 0; i < 16; i++)
                {
                    re_gbool_di[i] = gbool_di[i];
                }

                for (i = 0; i < 16; i++)
                {
                    result = DASK.DI_ReadLine((ushort)m_dev, 0, i, out didata[i]); //InPut 읽음 (카드넘버,포트0번,In단자번호,버퍼메모리(In단자1일때 1,In단자0일때 0) 
                    //o_result = DASK.di_re
                    if (didata[i] == 1)
                    {
                        gbool_di[i] = true;
                    }
                    else
                    {
                        gbool_di[i] = false;
                    }
                }
                //IO CHECK - DISPLAY 표시 부분
                BeginInvoke((Action)delegate { IOCHECK(); });
                for (i = 0; i < 16; i++)
                {
                    if (gbool_di[i] != re_gbool_di[i] && gbool_di[i] == true)
                    {
                        switch (i)
                        {
                            case 8: // LINE #1 Vision Trigger 신호
                                OutPutSignal_Off(1);
                                OutPutSignal_Off(2);
                                OutPutSignal_Off(3);
                                OutPutSignal_Off(4);
                                OutPutSignal_Off(5);
                                OutPutSignal_Off(6);
                                log.AddLogMessage(LogType.Infomation, 0, "Line #1 Vision Trigger");
                                if (InspectFlag[0] == false && InspectFlag[1] == false && InspectFlag[2] == false)
                                {

                                    cdyDisplay.Image = null;
                                    cdyDisplay.InteractiveGraphics.Clear();
                                    cdyDisplay.StaticGraphics.Clear();
                                    cdyDisplay2.Image = null;
                                    cdyDisplay2.InteractiveGraphics.Clear();
                                    cdyDisplay2.StaticGraphics.Clear();
                                    cdyDisplay3.Image = null;
                                    cdyDisplay3.InteractiveGraphics.Clear();
                                    cdyDisplay3.StaticGraphics.Clear();
                                    BeginInvoke((Action)delegate
                                    {
                                        lb_Cam1_Result.Text = "Result";
                                        lb_Cam1_Result.BackColor = SystemColors.Control;
                                        lb_Cam2_Result.Text = "Result";
                                        lb_Cam2_Result.BackColor = SystemColors.Control;
                                        lb_Cam3_Result.Text = "Result";
                                        lb_Cam3_Result.BackColor = SystemColors.Control;
                                    });

                                    snap1 = new Thread(new ThreadStart(SnapShot1));
                                    snap1.Priority = ThreadPriority.Highest;
                                    snap1.Start();

                                    snap2 = new Thread(new ThreadStart(SnapShot2));
                                    snap2.Priority = ThreadPriority.Highest;
                                    snap2.Start();

                                    snap3 = new Thread(new ThreadStart(SnapShot3));
                                    snap3.Priority = ThreadPriority.Highest;
                                    snap3.Start();
                                }
                                else
                                {
                                    OutPutSignal_Off(1);
                                    OutPutSignal_Off(2);
                                    OutPutSignal_Off(3);
                                    OutPutSignal_Off(4);
                                    OutPutSignal_Off(5);
                                    OutPutSignal_Off(6);
                                    InspectFlag[0] = false;
                                    InspectFlag[1] = false;
                                    InspectFlag[2] = false;
                                    log.AddLogMessage(LogType.Infomation, 0, "LINE #1 검사 결과 초기화");
                                }
                                break;
                            case 9: //LINE #1 결과 체크 신호
                                log.AddLogMessage(LogType.Infomation, 0, "Line #1 Result Check");
                                //if (InspectFlag[0] == false && InspectFlag[1] == false && InspectFlag[2] == false)
                                //{
                                OutPutSignal_Off(1);
                                OutPutSignal_Off(2);
                                OutPutSignal_Off(3);
                                OutPutSignal_Off(4);
                                OutPutSignal_Off(5);
                                OutPutSignal_Off(6);
                                //}
                                break;
                            case 10:// LINE #2 Vision Trigger 신호
                                log.AddLogMessage(LogType.Infomation, 0, "Line #2 Vision Trigger");
                                OutPutSignal_Off(9);
                                OutPutSignal_Off(10);
                                OutPutSignal_Off(11);
                                OutPutSignal_Off(12);
                                OutPutSignal_Off(13);
                                OutPutSignal_Off(14);
                                if (InspectFlag[3] == false && InspectFlag[4] == false && InspectFlag[5] == false)
                                {
                                    cdyDisplay4.Image = null;
                                    cdyDisplay4.InteractiveGraphics.Clear();
                                    cdyDisplay4.StaticGraphics.Clear();
                                    cdyDisplay5.Image = null;
                                    cdyDisplay5.InteractiveGraphics.Clear();
                                    cdyDisplay5.StaticGraphics.Clear();
                                    cdyDisplay6.Image = null;
                                    cdyDisplay6.InteractiveGraphics.Clear();
                                    cdyDisplay6.StaticGraphics.Clear();
                                    BeginInvoke((Action)delegate
                                    {
                                        lb_Cam4_Result.Text = "Result";
                                        lb_Cam4_Result.BackColor = SystemColors.Control;
                                        lb_Cam5_Result.Text = "Result";
                                        lb_Cam5_Result.BackColor = SystemColors.Control;
                                        lb_Cam6_Result.Text = "Result";
                                        lb_Cam6_Result.BackColor = SystemColors.Control;
                                    });
                                    snap4 = new Thread(new ThreadStart(SnapShot4));
                                    snap4.Priority = ThreadPriority.Highest;
                                    snap4.Start();

                                    snap5 = new Thread(new ThreadStart(SnapShot5));
                                    snap5.Priority = ThreadPriority.Highest;
                                    snap5.Start();

                                    snap6 = new Thread(new ThreadStart(SnapShot6));
                                    snap6.Priority = ThreadPriority.Highest;
                                    snap6.Start();
                                }
                                else
                                {
                                    OutPutSignal_Off(9);
                                    OutPutSignal_Off(10);
                                    OutPutSignal_Off(11);
                                    OutPutSignal_Off(12);
                                    OutPutSignal_Off(13);
                                    OutPutSignal_Off(14);
                                    InspectFlag[3] = false;
                                    InspectFlag[4] = false;
                                    InspectFlag[5] = false;
                                    log.AddLogMessage(LogType.Infomation, 0, "LINE #2 검사 결과 초기화");
                                }
                                break;
                            case 11: //LINE #2 결과 체크 신호
                                log.AddLogMessage(LogType.Infomation, 0, "Line #2 Result Check");
                                //if (InspectFlag[3] == false && InspectFlag[4] == false && InspectFlag[5] == false)
                                //{
                                OutPutSignal_Off(9);
                                OutPutSignal_Off(10);
                                OutPutSignal_Off(11);
                                OutPutSignal_Off(12);
                                OutPutSignal_Off(13);
                                OutPutSignal_Off(14);
                                //}
                                break;
                            case 12:
                                break;
                            case 13:
                                Process.Start($"{Glob.MODELCHANGEFROM}");
                                for (int k = 0; k < camcount; k++)
                                {
                                    if (Glob.RunnModel.Loadmodel("K12E DIMPLE", Glob.MODELROOT, k) == true)
                                    {
                                        if (k == camcount - 1)
                                        {
                                            lb_CurruntModelName.Text = Glob.RunnModel.Modelname();
                                            Glob.CurruntModelName = Glob.RunnModel.Modelname();
                                            CamSet();
                                            Process[] myProcesses = Process.GetProcessesByName("ModelChange_KHM");
                                            if (myProcesses.LongLength > 0)
                                            {
                                                myProcesses[0].Kill();
                                            }
                                            log.AddLogMessage(LogType.Infomation, 0, "모델 전환 성공");
                                        }
                                    }
                                }
                                break;
                            case 14: 
                                Process.Start($"{Glob.MODELCHANGEFROM}");
                                for (int k = 0; k < camcount; k++)
                                {
                                    if (Glob.RunnModel.Loadmodel("K12E WEBBING", Glob.MODELROOT, k) == true)
                                    {
                                        if (k == camcount - 1)
                                        {
                                            lb_CurruntModelName.Text = Glob.RunnModel.Modelname();
                                            Glob.CurruntModelName = Glob.RunnModel.Modelname();
                                            CamSet();
                                            Process[] myProcesses = Process.GetProcessesByName("ModelChange_KHM");
                                            if (myProcesses.LongLength > 0)
                                            {
                                                myProcesses[0].Kill();
                                            }
                                            log.AddLogMessage(LogType.Infomation, 0, "모델 전환 성공");
                                        }
                                    }
                                }
                                break;
                            case 15: 
                                Process.Start($"{Glob.MODELCHANGEFROM}");
                                for (int k = 0; k < camcount; k++)
                                {
                                    if (Glob.RunnModel.Loadmodel("K12E CABLE STOPPER", Glob.MODELROOT, k) == true)
                                    {
                                        if (k == camcount - 1)
                                        {
                                            lb_CurruntModelName.Text = Glob.RunnModel.Modelname();
                                            Glob.CurruntModelName = Glob.RunnModel.Modelname();
                                            CamSet();
                                            Process[] myProcesses = Process.GetProcessesByName("ModelChange_KHM");
                                            if (myProcesses.LongLength > 0)
                                            {
                                                myProcesses[0].Kill();
                                            }
                                            log.AddLogMessage(LogType.Infomation, 0, "모델 전환 성공");
                                        }
                                    }
                                }
                                //lb_CurruntModelName.Text = Glob.RunnModel.Modelname();
                                //Glob.CurruntModelName = Glob.RunnModel.Modelname();
                                //CamSet();
                                break;
                        }
                    }
                }
            }
        }
        private void IOCHECK()
        {
            btn_INPUT8.BackColor = gbool_di[8] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT9.BackColor = gbool_di[9] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT10.BackColor = gbool_di[10] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT11.BackColor = gbool_di[11] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT12.BackColor = gbool_di[12] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT13.BackColor = gbool_di[13] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT14.BackColor = gbool_di[14] == true ? Color.Lime : SystemColors.Control;
            btn_INPUT15.BackColor = gbool_di[15] == true ? Color.Lime : SystemColors.Control;
        }
        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bk_IO.IsBusy == true)
            {
                bk_IO.CancelAsync();
            }
            if (LightStats == true)
            {
                LightOFF();
            }
            try
            {
                for (int i = 0; i < camcount; i++)
                {
                    if (mDevice[i] != null)
                    {
                        mDevice[i].RemoteNodeList["AcquisitionStop"].Execute();
                        mDataStream[i].StopAcquisition();
                    }
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
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
            OutPutSignal_On(0); //LINE #1 VISION READY ON 
            OutPutSignal_On(8); //LINE #2 VISION READY ON 
            CognexModelLoad();
            for (int num = 1; num < 7; num++)
            {
                //LINE #1 검사 결과 초기화
                OutPutSignal_Off(num);
            }
            for (int num = 9; num < 15; num++)
            {
                //LINE #2 검사 결과 초기화
                OutPutSignal_Off(num);
            }
            if (bk_IO.IsBusy == false) //I/O 백그라운드가 돌고있지 않으면.
            {
                //I/O 백드라운드 동작 시작.
                bk_IO.RunWorkerAsync();
            }
            //cdyDisplay.Image = null;
            //cdyDisplay.InteractiveGraphics.Clear();
            //cdyDisplay.StaticGraphics.Clear();
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
                //for (int i = 0; i < 30; i++)
                //{
                //    if (TempBlobEnable[camnumber, i] == true)
                //    {
                //        if (TempBlobs[camnumber, i].ResultBlobCount() != TempBlobOKCount[camnumber, i]) // - 검사결과 NG
                //        {
                //            dgv.Rows[i].Cells[cellnumber + 1].Value = $"{TempBlobs[camnumber, i].ResultBlobCount()}-({TempBlobOKCount[camnumber, i]})";
                //            dgv.Rows[i].Cells[cellnumber + 1].Style.BackColor = Color.Red;
                //        }
                //        else // - 검사결과 OK
                //        {
                //            dgv.Rows[i].Cells[cellnumber + 1].Value = $"{TempBlobs[camnumber, i].ResultBlobCount()}-({TempBlobOKCount[camnumber, i]})";
                //            dgv.Rows[i].Cells[cellnumber + 1].Style.BackColor = Color.Lime;
                //        }
                //    }
                //    else
                //    {
                //        dgv.Rows[i].Cells[cellnumber + 1].Value = "NOT USED";
                //        dgv.Rows[i].Cells[cellnumber + 1].Style.BackColor = SystemColors.Control;
                //    }
                //}
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
                string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";

                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                if (!Directory.Exists(Root2))
                {
                    Directory.CreateDirectory(Root2);
                }
                ImageSave.Open(Root + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}" + ".jpg", CogImageFileModeConstants.Write);
                ImageSave.Append(cog.Image);
                ImageSave.Close();

                cog.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(Root2 + $@"\{dt.ToString("yyyyMMdd-HH mm ss")}" + $"_{Result}", ImageFormat.Jpeg);
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
        public void ErrorLogSave()
        {
            DateTime dt = DateTime.Now;
            string Root = Glob.DataSaveRoot;
            StreamWriter Writer;
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
            Root += $@"\ErrorLog_{dt.ToString("yyyyMMdd-HH")}.csv";
            Writer = new StreamWriter(Root, true);
            Writer.WriteLine($"Time,{dt.ToString("yyyyMMdd_HH mm ss")}");
            Writer.Close();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            OutPutSignal_Off(0);
            OutPutSignal_Off(8);
            btn_Stop.Enabled = false;
            btn_ToolSetUp.Enabled = true;
            btn_Model.Enabled = true;
            btn_SystemSetup.Enabled = true;
            btn_Status.Enabled = true;
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
            if (e.Control && e.KeyCode == Keys.C) //ctrl + c : 카메라 셋팅창 열기.
                btn_CamList_Click(sender, e);
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
            if (frm_loading != null)
            {
                frm_loading.Close();
                frm_loading.Dispose();
                frm_loading = null;
            }
        }

        public void Line1DataLoad()
        {
            //dgv_Line1.DoubleBuffered(true);
            //for (int i = 0; i < 30; i++)
            //{
            //    dgv_Line1.Rows.Add(Glob.RunnModel.MultiPatterns()[0, i].ToolName());
            //    dgv_Line1.Rows[i].Cells[2].Value = Glob.RunnModel.MultiPatterns()[1, i].ToolName();
            //    dgv_Line1.Rows[i].Cells[4].Value = Glob.RunnModel.MultiPatterns()[2, i].ToolName();
            //}
        }
        public void Line2DataLoad()
        {
            //dgv_Line2.DoubleBuffered(true);
            //for (int i = 0; i < 30; i++)
            //{
            //    dgv_Line2.Rows.Add(Glob.RunnModel.MultiPatterns()[3, i].ToolName());
            //    dgv_Line2.Rows[i].Cells[2].Value = Glob.RunnModel.MultiPatterns()[4, i].ToolName();
            //    dgv_Line2.Rows[i].Cells[4].Value = Glob.RunnModel.MultiPatterns()[5, i].ToolName();
            //}
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

        private void bk_AutoDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            //*************************************************************Image저장경로*************************************************************//
            //string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}";
            //string Root2 = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{dt.ToString("yyyyMMdd")}\CAM{CamNumber}\{Result}Display";
            //******************************************************************************************************************************************//

            while (true)
            {
                try
                {
                    if (bk_AutoDelete.CancellationPending)
                        return;

                    DirectoryInfo di = new DirectoryInfo(Glob.ImageSaveRoot);
                    if (di.Exists)
                    {
                        DirectoryInfo[] dirInfo = di.GetDirectories();
                        string Date = DateTime.Now.AddDays(-Glob.ImageSaveDay).ToString("yyyyMMdd");
                        foreach (DirectoryInfo dir in dirInfo)
                        {
                            if (Date.CompareTo(dir.CreationTime.ToString("yyyyMMdd")) > 0)
                            {
                                string DeleteName = dir.Name;
                                dir.Attributes = FileAttributes.Normal;
                                dir.Delete(true);
                                log.AddLogMessage(LogType.Infomation, 0, $"{DeleteName} IMAGE 폴더 삭제 완료");
                                //df_Image.EnumerateFiles().ToList().ForEach(f => f.Delete());
                                //df_Image.EnumerateDirectories().ToList().ForEach(d => d.Delete(true));
                                //df_Image.Delete();
                            }
                        }
                    }
                    //DateTime del_img = DateTime.Now.AddDays(-(Glob.ImageSaveDay));
                    //string FolderName_Image = Path.Combine(Glob.ImageSaveRoot, del_img.ToString("yyyyMMdd")); // Day를 뺀날의 이름을 가진 Image 폴더

                    ////Day를 뺀 날짜의 폴더.
                    //DirectoryInfo df_Image = new DirectoryInfo(FolderName_Image);
                    //if (Directory.Exists(FolderName_Image) == false) //Day를 뺀 날짜의 Image폴더가 존재하지 않을 때
                    //{

                    //}
                    //else// Day를 뺀날의 이름을 가진 Image 폴더가 존재 할 때 삭제.
                    //{
                    //    if (df_Image.CreationTime < del_img)
                    //    {
                    //        df_Image.EnumerateFiles().ToList().ForEach(f => f.Delete());
                    //        df_Image.EnumerateDirectories().ToList().ForEach(d => d.Delete(true));
                    //        df_Image.Delete();
                    //    }
                    //}
                }
                catch (Exception ee)
                {
                    log.AddLogMessage(LogType.Error, 0, ee.Message);
                }
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
}
