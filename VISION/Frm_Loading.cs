using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISION
{
    public partial class Frm_Loading : Form
    {
        //delegate void ProgressDelegate(int i);
        //delegate void CloseDelegate();

        public Frm_Loading()
        {
            InitializeComponent();
        }

        private void Frm_Loading_Load(object sender, EventArgs e)
        {
            //backgroundWorker1.RunWorkerAsync();
            //Thread loading = new Thread(Thread);
            //loading.Start();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (backgroundWorker1.CancellationPending == true)
                    return;
            }
        }

        private void Frm_Loading_FormClosing(object sender, FormClosingEventArgs e)
        {
            //backgroundWorker1.CancelAsync();
        }
        //private void Step(int i)
        //{
        //    progressBar1.Value = i;
        //}

        //private void FormClose()
        //{
        //    this.Close();
        //}
        //private void Thread()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        Invoke(new ProgressDelegate(Step), i);
        //        System.Threading.Thread.Sleep(50);
        //    }
        //    Invoke(new CloseDelegate(FormClose));
        //}
    }
}
