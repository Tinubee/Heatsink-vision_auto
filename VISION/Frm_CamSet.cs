using KimLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    public partial class Frm_CamSet : Form
    {
        private PGgloble Glob;
        private int selectPort = 0;

        public Frm_CamSet(int Modelnumber = 0)
        {
            InitializeComponent();
            Glob = PGgloble.getInstance;
            보드에연결된카메라목록초기화();
            카메라설정값불러오기();

            cb카메라목록.SelectedIndexChanged += 선택카메라변경;
            btn저장.Click += 카메라설정값저장;
            tb감도값.KeyPress += 숫자만입력;
            tb노출값.KeyPress += 숫자만입력;
        }

        private void 보드에연결된카메라목록초기화()
        {
            cb카메라목록.Items.Clear();
            for (int lop = 0; lop < Glob.LineCameraOption.Length; lop++)
            {
                cb카메라목록.Items.Add($"Cam {Glob.LineCameraOption[lop].CamNumber}");
            }
            cb카메라목록.SelectedIndex = selectPort;
        }

        private void 선택카메라변경(object sender, EventArgs e)
        {
            selectPort = cb카메라목록.SelectedIndex;
            카메라설정값불러오기();
        }

        private void 카메라설정값불러오기()
        {
            tb노출값.Text = Glob.LineCameraOption[selectPort].Exposure.ToString();
            tb감도값.Text = Glob.LineCameraOption[selectPort].Gain.ToString();
        }

        private void 카메라설정값저장(object sender, EventArgs e)
        {
            if (MessageBox.Show("셋팅값을 저장 하시겠습니까?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            Glob.G_MainForm.SubFromStart("라인카메라 설정값을 저장중 입니다.", Glob.PROGRAM_VERSION);
            INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{Glob.RunnModel.Modelname()}\\CamSet.ini");

            CamSet.WriteData($"LineCamera{selectPort}", "Exposure", tb노출값.Text);
            CamSet.WriteData($"LineCamera{selectPort}", "Gain", tb감도값.Text);

            Glob.LineCameraOption[selectPort].Exposure = Convert.ToDouble(tb노출값.Text);
            Glob.LineCameraOption[selectPort].Gain = Convert.ToDouble(tb감도값.Text);

            Glob.G_MainForm.Set_GeniCam(Glob.CurruntModelName);
            Glob.G_MainForm.SubFromClose();
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }
        private void 숫자만입력(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void btn종료_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("라인스캔카메라 설정창을 종료 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            this.Close();
        }
    }
}
