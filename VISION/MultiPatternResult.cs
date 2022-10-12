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
    public partial class MultiPatternResult : Form
    {
        private PGgloble Glob;
        int FindResultNumber = 0;
        public MultiPatternResult(Frm_ToolSetUp frm_toolsetup, int findresultnumber)
        {
            InitializeComponent();
            Glob = PGgloble.getInstance;
            FindResultNumber = findresultnumber;
        }

        private void MultiPatternResult_Load(object sender, EventArgs e)
        {
            MultiPatternResultDataLoad();
        }

        private void MultiPatternResultDataLoad()
        {
            //Glob.MultiPatternResultData[Glob.CamNumber, (int)num_MultiPatternToolNumber.Value]
            dgv_MultiToolResult.Rows.Clear();
            //for (int i = 0; i < 30; i++)
            //{
            //    dgv_MultiToolResult.Rows.Add(Glob.RunnModel.MultiPatterns()[Glob.CamNumber, i].ToolName());
            //}
            for (int i = 0; i < FindResultNumber; i++)
            {
                dgv_MultiToolResult.Rows.Add($"Pattern - {i}");
                dgv_MultiToolResult.Rows[i].Cells[1].Value = Glob.MultiPatternResultData[Glob.CamNumber, i].ToString("F2");
            }
        }
    }
}
