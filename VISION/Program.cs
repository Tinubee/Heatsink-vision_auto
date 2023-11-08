using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.Class;
using VISION.UI;

namespace VISION
{
    static class Program
    {
        private const string MutexName = "HeatSinkInspectionProgram";
        public static Class_Common cm = new Class_Common();
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Boolean createdNew = false;
            Mutex mtx = new Mutex(true, Global.GetGuid(), out createdNew);
            // 뮤텍스를 얻지 못하면 에러
            if (!createdNew)
            {
                MvUtils.Utils.ErrorMsg("프로그램이 이미 실행중입니다.");
                Application.Exit();
                return;
            }
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.MainForm = new MainForm();
            Application.Run(Global.MainForm);

            //using (Mutex mutex = new Mutex(false, MutexName))
            //{
            //    bool isMutexAcquired = mutex.WaitOne(TimeSpan.Zero, true);
            //    if (!isMutexAcquired)
            //    {
            //        MessageBox.Show("프로그램이 이미 실행중 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Frm_Main fm = new Frm_Main();
            //    Application.Run(fm);
            //}
        }
    }
}
