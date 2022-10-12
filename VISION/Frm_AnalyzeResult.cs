using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    public partial class Frm_AnalyzeResult : Form
    {
        private PGgloble Glob;
        public Frm_AnalyzeResult(Frm_Main main)
        {
            InitializeComponent();
            Glob = PGgloble.getInstance;
        }

        private void Frm_AnalyzeResult_Load(object sender, EventArgs e)
        {

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string StartDate = startDatePicker.Text;
            string[] s_StartDate = StartDate.Split(',');
            string Date = s_StartDate[0].Replace("-", string.Empty);
            string[] Time = s_StartDate[1].Split(':');
            //string Root = Glob.ImageSaveRoot + $@"\{Glob.CurruntModelName}\{Date}\CAM{CamNumber}\{Result}Display";
        }
    }
}
