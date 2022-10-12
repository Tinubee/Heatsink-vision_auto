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
    public partial class Frm_NewModel : Form
    {
        private Class_Common cm { get { return Program.cm; } }
        public Frm_NewModel(int Modelnumber = 0)
        {
            InitializeComponent();
            num_ModelNumber.Value = Modelnumber;
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_MakeModel_Click(object sender, EventArgs e)
        {
            int NewNumber = (int)num_ModelNumber.Value;
            string NewName = tb_ModelName.Text;

            string CheckNumber;
            string CheckName;

            int ModelCount;

            PGgloble gls = PGgloble.getInstance;
            INIControl Modellist = new INIControl(gls.MODELLIST);

            ModelCount = int.Parse(Modellist.ReadData("COUNT", "count", true));

            NewName.Trim();

            CheckName = Modellist.ReadData("NUMBER", NewNumber.ToString());
            CheckNumber = Modellist.ReadData("NAME", CheckName);

            if (NewName == "")
            {
                cm.info("The Model Name is not Entered.");
                return;
            }
            //모델 새로생성시, 기존 모델과 중복되는 부분이 있는지 체크하는 부분 수정 - 191223 김형민.
            if (CheckName == NewName)
            {
                cm.info("The Model Name Already Exists.");
                return;
            }
            if (CheckNumber == NewNumber.ToString())
            {
                cm.info("The Model Number Already Exists.");
                return;
            }

            //모델별 카메라별로 셋팅값 따로 가져가야됨
            //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(gls.MODELROOT + "\\" + gls.RunnModel.Modelname(), gls.MODELROOT + "\\" + NewName, true);
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(gls.MODELROOT + "\\" + gls.RunnModel.Modelname(), gls.MODELROOT + "\\" + NewName, true);
            Modellist.WriteData("NAME", NewName, NewNumber.ToString());
            Modellist.WriteData("NUMBER", NewNumber.ToString(), NewName);

            Modellist.WriteData("COUNT", "count", (++ModelCount).ToString());
            MessageBox.Show("Model [" + NewName + "] 이 [" + NewNumber.ToString() + "]번으로 생성되었습니다..", "새 모델 생성", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
    }
}
