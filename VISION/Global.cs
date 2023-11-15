using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VISION.Schemas;
using VISION.UI;

namespace VISION
{
    public static class Global
    {
        public static MainForm MainForm = null;
        public delegate void BaseEvent();
        public static event EventHandler<Boolean> Initialized;

        public static 그랩제어 그랩제어;
        public static 조명제어 조명제어;
        public static 환경설정 환경설정;
        public static 유저자료 유저자료;
        public static 모델자료 모델자료;
        public static 코그넥스 코그넥스;
        public static 신호제어 신호제어;
        public static 로그자료 로그자료;

        public static class 장치상태
        {
            public static Boolean 정상여부
            {
                get { return true; }
            }
            public static Boolean 카메라1 { get { return Global.그랩제어.상부표면검사카메라.상태; } }
            public static Boolean 카메라2 { get { return Global.그랩제어.좌측표면검사카메라.상태; } }
            public static Boolean 카메라3 { get { return Global.그랩제어.우측표면검사카메라.상태; } }
            public static Boolean 카메라4 { get { return Global.그랩제어.좌측너트검사카메라.상태; } }
            public static Boolean 카메라5 { get { return Global.그랩제어.우측너트검사카메라.상태; } }
            public static Boolean 카메라6 { get { return Global.그랩제어.제품형상검사카메라.상태; } }
            public static Boolean 카메라7 { get { return Global.그랩제어.좌측너트유무검사카메라.상태; } }
            public static Boolean 카메라8 { get { return Global.그랩제어.우측너트유무검사카메라.상태; } }

            //public static Boolean 조명장치 { get { return 조명제어.정상여부; } }
        }

        public static Boolean Init()
        {
            try
            {
                로그자료 = new 로그자료();
                그랩제어 = new 그랩제어();
                조명제어 = new 조명제어();
                환경설정 = new 환경설정();
                유저자료 = new 유저자료();
                모델자료 = new 모델자료();
                코그넥스 = new 코그넥스();
                신호제어 = new 신호제어();

                로그자료.Init();
                //그랩제어.Init();
                환경설정.Init();
                //조명제어.Init();
                유저자료.Init();
                모델자료.Init();
                코그넥스.Init();
                신호제어.Init();

                Initialized?.Invoke(null, true);
                return true;
            }
            catch(Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }
            Initialized.Invoke(null, false);
            return false;
        }

        public static Boolean Close()
        {
            try
            {
                //if (환경설정.동작구분 == 동작구분.Live)
                //{
                //    조명제어.Close();
                //}
                조명제어.Close();
                //장치통신.Close();
                유저자료.Close();
                환경설정.Close();
                로그자료.Close();
                그랩제어.Close();
                모델자료.Close();
                Properties.Settings.Default.Save();
                Debug.WriteLine("시스템 종료");
                return true;
            }
            catch (Exception ex)
            {
                return Utils.ErrorMsg("프로그램 종료 중 오류가 발생하였습니다.\n" + ex.Message);
            }
        }
        public static void Start()
        {
            //장치통신.Start();
            //if (Global.환경설정.동작구분 != 동작구분.Live) return;
            //코드리더.Start();
            //큐알리더.Start();
        }
        public static void DxLocalization()
        {
            if (Localization.CurrentLanguage != Language.KO) return;
            MvUtils.Localization.CurrentLanguage = MvUtils.Localization.Language.KO;
            DxDataGridLocalizer.Enable();
            DxEditorsLocalizer.Enable();
            DxDataFilteringLocalizer.Enable();
            DxLayoutLocalizer.Enable();
            DxBarLocalizer.Enable();
        }
        public static String GetGuid()
        {
            Assembly assembly = typeof(Program).Assembly;
            GuidAttribute attribute = assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0] as GuidAttribute;
            return attribute.Value;
        }
    }

   
}
