using KimLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    public partial class Frm_SystemSetUp : Form
    {
        Frm_Main Main;
        private PGgloble Unit;
        public Frm_SystemSetUp(Frm_Main main)
        {
            Main = main;
            InitializeComponent();
            Unit = PGgloble.getInstance;
            string[] Parity = { "NONE", "Odd", "Even", "Mark", "Space" };
            string[] Stopbit = { "NONE", "1", "2", "1.5" };
            string[] BaudRate = { "300", "600", "1200", "2400", "9600", "14400", "19200", "28800", "38400", "57600", "115200" };
            string[] Databit = { "5", "6", "7", "8" };
            cb_PortNumber.Items.Add("NONE");
            cb_PortNumber.Items.AddRange(SerialPort.GetPortNames());

            cb_ParityCheck.Items.AddRange(Parity);
            cb_Stopbit.Items.AddRange(Stopbit);
            cb_BaudRate.Items.AddRange(BaudRate);
            cb_Databit.Items.AddRange(Databit);
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("시스템 설정창을 종료 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            this.Close();
        }

        private void btn_ImageRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb_ImageSaveRoot.Text = fbd.SelectedPath;
            }
        }

        private void btn_DataRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb_DataSaveRoot.Text = fbd.SelectedPath;
            }
        }

        private void Frm_SystemSetUp_Load(object sender, EventArgs e)
        {
            INIControl setting = new INIControl(Unit.SETTING);

            tb_ImageSaveRoot.Text = Unit.ImageSaveRoot;
            tb_DataSaveRoot.Text = Unit.DataSaveRoot;

            cb_PortNumber.SelectedItem = Unit.PortName[Unit.LightControlNumber];
            cb_ParityCheck.SelectedItem = Unit.Parity[Unit.LightControlNumber];
            cb_Stopbit.SelectedItem = Unit.StopBits[Unit.LightControlNumber];
            cb_BaudRate.SelectedItem = Unit.BaudRate[Unit.LightControlNumber];
            cb_Databit.SelectedItem = Unit.DataBit[Unit.LightControlNumber];

            Unit.InspectUsed = setting.ReadData("SYSTEM", "Inspect Used Check", true) == "1" ? true : false;
            Unit.OKImageSave = setting.ReadData("SYSTEM", "OK IMAGE SAVE", true) == "1" ? true : false;
            Unit.NGImageSave = setting.ReadData("SYSTEM", "NG IMAGE SAVE", true) == "1" ? true : false;
            Unit.NGContainUIImageSave = setting.ReadData("SYSTEM", "NG CONTAIN UI IMAGE SAVE", true) == "1" ? true : false;

            cb_Used.Checked = Unit.InspectUsed;
            cb_OkSave.Checked = Unit.OKImageSave;
            cb_NGSave.Checked = Unit.NGImageSave;

            num_ImageSaveDay.Value = Unit.ImageSaveDay;
            Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }

        private void 시스템설정저장_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("시스템 설정값을 저장 하시겠습니까?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            Process.Start($"{Unit.SAVEFROM}");

            INIControl Writer = new INIControl(this.Unit.SETTING);
            Unit.ImageSaveRoot = tb_ImageSaveRoot.Text;
            Unit.DataSaveRoot = tb_DataSaveRoot.Text;

            if (cb_PortNumber.SelectedItem != null)
            {
                Unit.PortName[Unit.LightControlNumber] = cb_PortNumber.SelectedItem.ToString();
                Unit.Parity[Unit.LightControlNumber] = cb_ParityCheck.SelectedItem.ToString();
                Unit.StopBits[Unit.LightControlNumber] = cb_Stopbit.SelectedItem.ToString();
                Unit.BaudRate[Unit.LightControlNumber] = cb_BaudRate.SelectedItem.ToString();
                Unit.DataBit[Unit.LightControlNumber] = cb_Databit.SelectedItem.ToString();
            }

            Unit.InspectUsed = cb_Used.Checked;

            Writer.WriteData("SYSTEM", "Image Save Root", Unit.ImageSaveRoot);
            Writer.WriteData("SYSTEM", "Data Save Root", Unit.DataSaveRoot);
            Writer.WriteData("SYSTEM", "Image Save Day", Unit.ImageSaveDay.ToString());

            Writer.WriteData("COMMUNICATION", $"Port number{Unit.LightControlNumber}", Unit.PortName[Unit.LightControlNumber]);
            Writer.WriteData("COMMUNICATION", $"Parity Check{Unit.LightControlNumber}", Unit.Parity[Unit.LightControlNumber]);
            Writer.WriteData("COMMUNICATION", $"Stop bits{Unit.LightControlNumber}", Unit.StopBits[Unit.LightControlNumber]);
            Writer.WriteData("COMMUNICATION", $"Data Bits{Unit.LightControlNumber}", Unit.DataBit[Unit.LightControlNumber]);
            Writer.WriteData("COMMUNICATION", $"Baud Rate{Unit.LightControlNumber}", Unit.BaudRate[Unit.LightControlNumber]);

            if (Unit.InspectUsed)
            {
                Writer.WriteData("SYSTEM", "Inspect Used Check", "1");
            }
            else
            {
                Writer.WriteData("SYSTEM", "Inspect Used Check", "0");
            }
            if (cb_OkSave.Checked)
            {
                Writer.WriteData("SYSTEM", "OK IMAGE SAVE", "1");
            }
            else
            {
                Writer.WriteData("SYSTEM", "OK IMAGE SAVE", "0");
            }
            if (cb_NGSave.Checked)
            {
                Writer.WriteData("SYSTEM", "NG IMAGE SAVE", "1");
            }
            else
            {
                Writer.WriteData("SYSTEM", "NG IMAGE SAVE", "0");
            }
            if (cb_NGContainUISave.Checked)
            {
                Writer.WriteData("SYSTEM", "NG CONTAIN UI IMAGE SAVE", "1");
            }
            else
            {
                Writer.WriteData("SYSTEM", "NG CONTAIN UI IMAGE SAVE", "0");
            }

            Process[] myProcesses = Process.GetProcessesByName("SaveForm_KHM");
            if (myProcesses.LongLength > 0)
            {
                myProcesses[0].Kill();
            }
            Main.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
            MessageBox.Show("저장 완료", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cb_Used_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Used.Checked == true)
            {
                cb_Used.Text = "Used";
                cb_Used.BackColor = Color.Lime;
            }
            else
            {
                cb_Used.Text = "Not Used";
                cb_Used.BackColor = Color.Red;
            }
        }

        private void cb_OkSave_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_OkSave.Checked == true)
            {
                cb_OkSave.Text = "Save";
                cb_OkSave.BackColor = Color.Lime;
            }
            else
            {
                cb_OkSave.Text = "UnSave";
                cb_OkSave.BackColor = Color.Red;
            }
        }

        private void cb_NGSave_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_NGSave.Checked == true)
            {
                cb_NGSave.Text = "Save";
                cb_NGSave.BackColor = Color.Lime;
            }
            else
            {
                cb_NGSave.Text = "UnSave";
                cb_NGSave.BackColor = Color.Red;
            }
        }

        private void num_ImageSaveDay_ValueChanged(object sender, EventArgs e)
        {
            Unit.ImageSaveDay = (int)num_ImageSaveDay.Value;
        }

        private void num_LightNumber_ValueChanged(object sender, EventArgs e)
        {
            Unit.LightControlNumber = (int)num_LightNumber.Value;
            LightControlChange(Unit.LightControlNumber);
        }

        public void LightControlChange(int LightNumber)
        {
            cb_PortNumber.SelectedItem = Unit.PortName[Unit.LightControlNumber];
            cb_ParityCheck.SelectedItem = Unit.Parity[Unit.LightControlNumber];
            cb_Stopbit.SelectedItem = Unit.StopBits[Unit.LightControlNumber];
            cb_BaudRate.SelectedItem = Unit.BaudRate[Unit.LightControlNumber];
            cb_Databit.SelectedItem = Unit.DataBit[Unit.LightControlNumber];
        }

        private void cb_PortNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit.PortName[Unit.LightControlNumber] = cb_PortNumber.SelectedItem.ToString();
        }

        private void cb_ParityCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit.Parity[Unit.LightControlNumber] = cb_ParityCheck.SelectedItem.ToString();
        }

        private void cb_Stopbit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit.StopBits[Unit.LightControlNumber] = cb_Stopbit.SelectedItem.ToString();
        }

        private void cb_BaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit.BaudRate[Unit.LightControlNumber] = cb_BaudRate.SelectedItem.ToString();
        }

        private void cb_Databit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit.DataBit[Unit.LightControlNumber] = cb_Databit.SelectedItem.ToString();
        }

        private void cb_NGContainUISave_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_NGContainUISave.Checked == true)
            {
                cb_NGContainUISave.Text = "Save";
                cb_NGContainUISave.BackColor = Color.Lime;
            }
            else
            {
                cb_NGContainUISave.Text = "UnSave";
                cb_NGContainUISave.BackColor = Color.Red;
            }
        }
    }
}
