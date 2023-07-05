using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    static class Program
    {
        private const string MutexName = "HeatSinkInspectionProgram";
        public static Class_Common cm = new Class_Common();
        public static List<CamList> CameraList = new List<CamList>();
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, MutexName))
            {
                bool isMutexAcquired = mutex.WaitOne(TimeSpan.Zero, true);
                if (!isMutexAcquired)
                {
                    MessageBox.Show("프로그램이 이미 실행중 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CameraList = CamList.LoadCamInfo();//카메라정보
                if (CameraList.Count == 0)
                {
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Frm_Main fm = new Frm_Main();
                Application.Run(fm);
            }
        }
    }
}
