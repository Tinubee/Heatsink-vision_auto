using KimLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubForm
{
    public partial class SubForm : Form
    {
        public string 제목 { get; set; }

        public string 프로그램버전 { get; set; }

        public SubForm()
        {
            InitializeComponent();
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
            lb_Ver.Text = 프로그램버전;
            lb진행사항이름.Text = 제목;
            lb진행사항이름.ForeColor = lb진행사항이름.ForeColor == Color.Lime ? Color.Black : Color.Lime;
        }
    }
}
