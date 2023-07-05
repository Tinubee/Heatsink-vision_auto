using KimLib;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VISION.UI.ResultCount;

namespace VISION.UI
{
    public partial class ResultCountDisplay : UserControl
    {
        private PGgloble Glob = PGgloble.getInstance;
        private int 검사결과표시타입 = 0;
        private AllResultCount allResultCountDisplay = new AllResultCount();
        private EachCamResult eachCamResultDisplay = new EachCamResult();

        Label[] OK_Label;
        Label[] NG_Label;
        Label[] TOTAL_Label;
        Label[] NGRATE_Label;

        public ResultCountDisplay()
        {
            InitializeComponent();
            btn_종합결과.Click += 검사결과표시설정;
            btn_개별카메라결과.Click += 검사결과표시설정;
            OK_Label = new Label[6] { eachCamResultDisplay.lb_CAM1_OK, eachCamResultDisplay.lb_CAM2_OK, eachCamResultDisplay.lb_CAM3_OK, eachCamResultDisplay.lb_CAM4_OK, eachCamResultDisplay.lb_CAM5_OK, eachCamResultDisplay.lb_CAM6_OK };
            NG_Label = new Label[6] { eachCamResultDisplay.lb_CAM1_NG, eachCamResultDisplay.lb_CAM2_NG, eachCamResultDisplay.lb_CAM3_NG, eachCamResultDisplay.lb_CAM4_NG, eachCamResultDisplay.lb_CAM5_NG, eachCamResultDisplay.lb_CAM6_NG };
            TOTAL_Label = new Label[6] { eachCamResultDisplay.lb_CAM1_TOTAL, eachCamResultDisplay.lb_CAM2_TOTAL, eachCamResultDisplay.lb_CAM3_TOTAL, eachCamResultDisplay.lb_CAM4_TOTAL, eachCamResultDisplay.lb_CAM5_TOTAL, eachCamResultDisplay.lb_CAM6_TOTAL };
            NGRATE_Label = new Label[6] { eachCamResultDisplay.lb_CAM1_NGRATE, eachCamResultDisplay.lb_CAM2_NGRATE, eachCamResultDisplay.lb_CAM3_NGRATE, eachCamResultDisplay.lb_CAM4_NGRATE, eachCamResultDisplay.lb_CAM5_NGRATE, eachCamResultDisplay.lb_CAM6_NGRATE };
            btn_종합결과.PerformClick();
            timer1.Start();
        }

        public void 검사결과표시설정(object sender, EventArgs e)
        {
            검사결과표시타입 = Convert.ToInt16((sender as Button).Tag);

            if (검사결과표시타입 == 0)
            {
                if (resultPanel.Controls.Contains(allResultCountDisplay)) return;

                resultPanel.Controls.Clear();
                resultPanel.Controls.Add(allResultCountDisplay);
                allResultCountDisplay.Dock = DockStyle.Fill;
                btn_종합결과.BackColor = Color.Lime;
                btn_개별카메라결과.BackColor = Color.DimGray;
            }
            else
            {
                if (resultPanel.Controls.Contains(eachCamResultDisplay)) return;

                resultPanel.Controls.Clear();
                resultPanel.Controls.Add(eachCamResultDisplay);
                eachCamResultDisplay.Dock = DockStyle.Fill;
                btn_종합결과.BackColor = Color.DimGray;
                btn_개별카메라결과.BackColor = Color.Lime;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (검사결과표시타입 == 0)
            {
                double okCount = Glob.G_MainForm.AllOK_Count;
                double ngCount = Glob.G_MainForm.AllNG_Count;
                double ng1Count = Glob.G_MainForm.AllNG_1_Count;
                double ng2Count = Glob.G_MainForm.AllNG_2_Count;
                double totalCount = Glob.G_MainForm.AllTotal_Count;

                allResultCountDisplay.lb_OK.Text = okCount.ToString();
                allResultCountDisplay.lb_NG.Text = ngCount.ToString();
                allResultCountDisplay.lb_1_NG.Text = ng1Count.ToString();
                allResultCountDisplay.lb_2_NG.Text = ng2Count.ToString();
                allResultCountDisplay.lb_TOTAL.Text = totalCount.ToString();

                if (ngCount != 0)
                {
                    double ngRate = (ngCount / totalCount) * 100;
                    allResultCountDisplay.lb_NGRATE.Text = $"{ngRate:F1}%";
                }
                else
                {
                    allResultCountDisplay.lb_NGRATE.Text = "0%";
                }
            }
            else
            {
                for (int lop = 0; lop < 6; lop++)
                {
                    OK_Label[lop].Text = Glob.G_MainForm.OK_Count[lop].ToString();
                    NG_Label[lop].Text = Glob.G_MainForm.NG_Count[lop].ToString();
                    Glob.G_MainForm.TOTAL_Count[lop] = Glob.G_MainForm.OK_Count[lop] + Glob.G_MainForm.NG_Count[lop];
                    TOTAL_Label[lop].Text = (Glob.G_MainForm.OK_Count[lop] + Glob.G_MainForm.NG_Count[lop]).ToString();

                    if (Glob.G_MainForm.NG_Count[lop] != 0)
                    {
                        Glob.G_MainForm.NG_Rate[lop] = (Glob.G_MainForm.NG_Count[lop] / Glob.G_MainForm.TOTAL_Count[lop]) * 100;
                        NGRATE_Label[lop].Text = $"{Glob.G_MainForm.NG_Rate[lop]:F1}%";
                    }
                    else
                    {
                        NGRATE_Label[lop].Text = "0%";
                    }
                }
            }
        }

        private void b수량초기화_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("검사결과 수량을 초기화 하시겠습니까?", "EXIT", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"OK : {Glob.G_MainForm.AllOK_Count}");
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"NG : {Glob.G_MainForm.AllNG_Count}");
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"NG - 1 : {Glob.G_MainForm.AllNG_1_Count}");
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"NG - 2 : {Glob.G_MainForm.AllNG_2_Count}");
            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"TOTAL : {Glob.G_MainForm.AllTotal_Count}");

            Glob.G_MainForm.AllOK_Count = 0;
            Glob.G_MainForm.AllNG_Count = 0;
            Glob.G_MainForm.AllNG_1_Count = 0;
            Glob.G_MainForm.AllNG_2_Count = 0;
            Glob.G_MainForm.AllTotal_Count = 0;

            for (int lop = 0; lop < 6; lop++)
            {
                Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"CAM - {lop + 1} OK : {Glob.G_MainForm.OK_Count[lop]}");
                Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"CAM - {lop + 1} NG : {Glob.G_MainForm.NG_Count[lop]}");
                Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"CAM - {lop + 1} TOTAL : {Glob.G_MainForm.TOTAL_Count[lop]}");

                Glob.G_MainForm.OK_Count[lop] = 0;
                Glob.G_MainForm.NG_Count[lop] = 0;
            }

            Glob.G_MainForm.log.AddLogMessage(LogType.Result, 0, $"{MethodBase.GetCurrentMethod().Name} 완료.");
        }
    }
}
