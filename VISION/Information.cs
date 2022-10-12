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
    public partial class Infomation : Form
    {
        public string msg { get; set; }
        public bool flag { get; set; }
        public Infomation()
        {
            InitializeComponent();
        }

        private void Information_Load(object sender, EventArgs e)
        {
            if (flag)
            {

            }
            lb_Message.Text = msg;
            timer_effect.Enabled = true;
        }

        private void timer_effect_Tick(object sender, EventArgs e)
        {
            lb_Message.ForeColor = lb_Message.ForeColor == Color.Black ? Color.Red : Color.Black;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            timer_effect.Enabled = false;
        }

        //화면상단 Panel이용하여 창 움직일 수 있도록////////////////////////////////////
        bool TagMove;
        int X, Y;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            TagMove = true;
            X = e.X;
            Y = e.Y;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TagMove == true)
            {
                this.SetDesktopLocation(MousePosition.X - X, MousePosition.Y - Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            TagMove = false;
        }
        ////////////////////////////////////////////////////////////////////////////////
    }
}
