using KimLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.Class;
using VISION.Properties;

namespace VISION
{
    public partial class Frm_Model : Form
    {
        //Log log = new Log();
        private Class_Common cm { get { return Program.cm; } }
        private PGgloble Glob; //전역변수 - CLASS "PGgloble" 참고.
        private string SelectedModel = "";
        private string NowModel;
        Frm_Main Main;

        public Frm_Model(string NowModelName, Frm_Main main)
        {
            InitializeComponent();
            NowModel = NowModelName;
            Main = main;
            Glob = PGgloble.getInstance; //전역변수 사용
        }

        private void btn_NewModel_Click(object sender, EventArgs e)
        {
            Frm_NewModel Create = new Frm_NewModel(dgvModelList.RowCount);
            if (Create.ShowDialog() == DialogResult.OK)
            {
                RefreashList();
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_LoadModel_Click(object sender, EventArgs e)
        {
            //PGgloble gls = PGgloble.getInstance;
            string r_model = tb_CurruntModel.Text;
            if (SelectedModel == "")
            {
                MessageBox.Show("변경할 모델을 선택해 주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectedModel == r_model)
            {
                MessageBox.Show("선택한 모델은 현재 모델과 같은 모델 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"{SelectedModel} 모델로 변경 하시겠습니까? (현재모델 : {r_model})", "Change Model", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            Main.SubFromStart("모델 변경중 입니다.", Glob.PROGRAM_VERSION);

            Main.Set_GeniCam(SelectedModel);
            Main.MainUIDisplaySetting(SelectedModel);

            for (int i = 0; i < Glob.CamCount; i++)
            {
                if (Glob.RunnModel.Loadmodel(SelectedModel, Glob.MODELROOT, i) == true)
                {
                    if (i == Glob.CamCount - 1)
                    {
                        Main.SubFromClose();
                        Main.log.AddLogMessage(LogType.Infomation, 0, $"Model Change Complete ({r_model} -> {Glob.RunnModel.Modelname()})");
                        MessageBox.Show("모델 전환 성공", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }

            tb_CurruntModel.Text = Glob.RunnModel.Modelname();
            NowModel = Glob.RunnModel.Modelname();

            INIControl CamSet = new INIControl($"{Glob.MODELROOT}\\{NowModel}\\CamSet.ini");
            for (int i = 0; i < Glob.PortName.Count(); i++)
            {
                Glob.LightChAndValue[i, 0] = Convert.ToInt32(CamSet.ReadData($"LightControl{i}", "CH1"));
                Glob.LightChAndValue[i, 1] = Convert.ToInt32(CamSet.ReadData($"LightControl{i}", "CH2"));
            }
        }

        private void Frm_Model_Load(object sender, EventArgs e)
        {
            tb_CurruntModel.Text = NowModel;
            tb_SelectModel.Text = SelectedModel;
            RefreashList();
        }
        private void RefreashList()
        {
            PGgloble gls = PGgloble.getInstance;
            INIControl List = new INIControl(gls.MODELLIST);
            if (System.IO.File.Exists(gls.MODELLIST) == false)
            {
                cm.info("모델 파일을 찾을 수 없습니다.");
                this.Dispose();
                this.Close();
                return;
            }

            System.IO.DirectoryInfo Directorys = new System.IO.DirectoryInfo(gls.MODELROOT);
            System.IO.DirectoryInfo[] dir = Directorys.GetDirectories("*", System.IO.SearchOption.TopDirectoryOnly);

            dgvModelList.RowCount = dir.Length;

            for (int lop = 0; lop <= dir.Length - 1; lop++)
            {
                dgvModelList[cName.Index, lop].Value = dir[lop].Name;
                dgvModelList[cNumber.Index, lop].Value = List.ReadData("NAME", dir[lop].Name, true);
            }
        }

        private void btn_DeleteModel_Click(object sender, EventArgs e)
        {
            PGgloble gls = PGgloble.getInstance;
            INIControl List = new INIControl(gls.MODELLIST);
            if (MessageBox.Show("선택한 모델을 삭제 하시겠습니까?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            if (SelectedModel == "")
            {
                return;
            }

            if (SelectedModel == NowModel)
            {
                cm.info("현재 사용중인 모델은 삭제 할 수 없습니다.");
                return;
            }

            try
            {
                System.IO.Directory.Delete(gls.MODELROOT + "\\" + SelectedModel, true);
            }
            catch (Exception ee)
            {
                cm.info(ee.Message);
            }

            if (System.IO.Directory.Exists(gls.MODELROOT + "\\" + SelectedModel) == false)
            {
                string Modelnumber = List.ReadData("NAME", SelectedModel, true);
                List.DeleteKey("NAME", SelectedModel);
                List.DeleteKey("NUMBER", Modelnumber);
                List.WriteData("COUNT", "count", (int.Parse(List.ReadData("COUNT", "count")) - 1).ToString());
                MessageBox.Show("Deleted Complete", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                cm.info("모델 삭제에 실패 하였습니다.");
            }
            SelectedModel = "";
            tb_SelectModel.Text = SelectedModel;
            RefreashList();
        }

        private void dgvModelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedModel = dgvModelList[cName.Index, dgvModelList.SelectedRows[0].Index].Value.ToString();
            tb_SelectModel.Text = SelectedModel;
        }
    }
}
