using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    static class Program
    {
        public static Class_Common cm = new Class_Common();
        public static List<CamList> CameraList = new List<CamList>();
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CameraList = CamList.LoadCamInfo();//카메라정보
            if (CameraList.Count == 0)
            {
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Frm_Loading frm_loading = new Frm_Loading();
            //frm_loading.Show();
            //frm_loading.Refresh();
            Frm_Main fm = new Frm_Main();
            Application.Run(fm);
        }
    }
}
