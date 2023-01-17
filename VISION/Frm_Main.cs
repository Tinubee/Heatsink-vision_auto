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

        public Frm_Main()
        {
            Glob = PGgloble.getInstance; //전역변수 사용
            Process.Start($"{Glob.LOADINGFROM}");
            InitializeComponent();
            ColumnHeader h = new ColumnHeader();
            StandFirst(); //처음 실행되어야 하는부분. - 이거 왜했지.. 이유는 모르겠다 일단 냅두자 필요없을꺼같기도함. - 20200121 김형민
            CamSet();
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
            LoadSetup(); //프로그램 셋팅 로드.
            timer_Setting.Start(); //타이머에서 계속해서 확인하는 것들
            CognexModelLoad();
            log.AddLogMessage(LogType.Infomation, 0, "Vision Program Start");
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
                CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();
                Glob.allCameraCount = frameGrabbers.Count;
                log.AddLogMessage(LogType.Program, 0, $"확인 된 카메라 개수 : {Glob.allCameraCount}");
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, ee.Message);
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
        //Output 신호들
        // 0 = Line #1 Vision Ready      8 = Line #2 Vision Ready
        // 1 = CAM 1 Ok                  9 = CAM 4 Ok 
        // 2 = CAM 1 NG                 10 = CAM 4 NG
        // 3 = CAM 2 OK                 11 = CAM 5 OK
        // 4 = CAM 2 NG                 12 = CAM 5 NG
        // 5 = CAM 3 OK                 13 = CAM 6 OK
        // 6 = CAM 3 NG                 14 = CAM 6 NG
       
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
                            case 0:
                                snap1 = new Thread(new ThreadStart(ShotAndInspect_Cam1));
                                snap1.Priority = ThreadPriority.Highest;
                                snap1.Start();
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
                        }
                    }
                }
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
                log.AddLogMessage(LogType.Error,0, $"Camera - 1 Error : {ee.Message}");
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
            try
            {
                InspectTime[2] = new Stopwatch();
                InspectTime[2].Reset();
                InspectTime[2].Start();

                TempCogDisplay[2].Image = TempCam[2].Run();
                if (TempCogDisplay[2].Image == null)
                {
                    log.AddLogMessage(LogType.Error, 0, "이미지 획들을 하지 못하였습니다. CAM - 3");
                    return;
                }
                if (Inspect_Cam3(TempCogDisplay[2]) == true) // 검사 결과
                {
                    //검사 결과 OK
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam3_Result.BackColor = Color.Lime;
                        lb_Cam3_Result.Text = "O K";
                        OK_Count[2]++;
                        if (Glob.OKImageSave)
                            ImageSave3("OK", 3, TempCogDisplay[2]);
                    });
                }
                else
                {
                    BeginInvoke((Action)delegate
                    {
                        //DgvResult(dgv_Line1, 0, 1); //-추가된함수
                        lb_Cam3_Result.BackColor = Color.Red;
                        lb_Cam3_Result.Text = "N G";
                        NG_Count[2]++;
                        if (Glob.NGImageSave)
                            ImageSave3("NG", 3, TempCogDisplay[2]);
                    });
                }

                InspectTime[2].Stop();
                InspectFlag[2] = false;

                BeginInvoke((Action)delegate { lb_Cam3_InsTime.Text = InspectTime[2].ElapsedMilliseconds.ToString() + "msec"; });
                Thread.Sleep(100);
            }
            catch (Exception ee)
            {
                log.AddLogMessage(LogType.Error, 0, $"Camera - 3 Error : {ee.Message}");
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
        }

        private void btn_Status_Click(object sender, EventArgs e)
        {
            log.AddLogMessage(LogType.Infomation, 0, "AUTO MODE START");
            btn_Status.Enabled = false;
            btn_ToolSetUp.Enabled = false;
            btn_Model.Enabled = false;
            btn_SystemSetup.Enabled = false;
            btn_Stop.Enabled = true;
            CognexModelLoad();
            if (bk_IO.IsBusy == false) //I/O 백그라운드가 돌고있지 않으면.
            {
                //I/O 백드라운드 동작 시작.
                bk_IO.RunWorkerAsync();
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

        private void btn_Stop_Click(object sender, EventArgs e)
        {
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
                log.AddLogMessage(LogType.Error,0,ex.Message);
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
