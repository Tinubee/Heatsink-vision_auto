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
        public static 모델자료 모델자료;
        public static 코그넥스 코그넥스;

        public static Boolean Init()
        {
            try
            {
                그랩제어 = new 그랩제어();
                조명제어 = new 조명제어();
                환경설정 = new 환경설정();
                모델자료 = new 모델자료();
                코그넥스 = new 코그넥스();

                //그랩제어.Init();
                환경설정.Init();
                //조명제어.Init();
                모델자료.Init();
                코그넥스.Init();

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
                //유저자료.Close();
                환경설정.Close();
                //로그자료.Close();
                그랩제어.Close();
                모델자료.Close();
                Properties.Settings.Default.Save();
                Debug.WriteLine("시스템 종료");
                return true;
            }
            catch (Exception ex)
            {
                return MvUtils.Utils.ErrorMsg("프로그램 종료 중 오류가 발생하였습니다.\n" + ex.Message);
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
            MvUtils.DxDataGridLocalizer.Enable();
            MvUtils.DxEditorsLocalizer.Enable();
            MvUtils.DxDataFilteringLocalizer.Enable();
            MvUtils.DxLayoutLocalizer.Enable();
            MvUtils.DxBarLocalizer.Enable();
        }
        public static String GetGuid()
        {
            Assembly assembly = typeof(Program).Assembly;
            GuidAttribute attribute = assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0] as GuidAttribute;
            return attribute.Value;
        }
    }

   
}
