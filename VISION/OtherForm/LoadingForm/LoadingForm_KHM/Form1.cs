using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadingForm_KHM
{
    public partial class Form1 : Form
    {
        Button[] button;
        int lop = 0;
        public Form1()
        {
            InitializeComponent();
            button = new Button[10] { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10 };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lb_Time.Text = dt.ToString("yyyy년 MM월 dd일 HH:mm:ss"); //현재날짜
            lb_text.ForeColor = lb_text.ForeColor == Color.Lime ? Color.Black : Color.Lime;
            if (lop == 10)
            {
                for (int i = 0; i < button.Count(); i++)
                {
                    button[i].BackColor = Color.White;
                }
                lop = 0;
            }
            else
            {
                button[lop].BackColor = Color.Lime;
                lop++;
            }
        }
    }
}
