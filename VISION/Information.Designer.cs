namespace VISION
{
    partial class Infomation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lb_server = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.lb_Message = new System.Windows.Forms.Label();
            this.timer_effect = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.lb_server);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 31);
            this.panel1.TabIndex = 59;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // lb_server
            // 
            this.lb_server.AutoSize = true;
            this.lb_server.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_server.ForeColor = System.Drawing.Color.White;
            this.lb_server.Location = new System.Drawing.Point(158, 0);
            this.lb_server.Name = "lb_server";
            this.lb_server.Size = new System.Drawing.Size(95, 32);
            this.lb_server.TabIndex = 60;
            this.lb_server.Text = "ERROR";
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.Gray;
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OK.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btn_OK.Location = new System.Drawing.Point(306, 230);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(93, 44);
            this.btn_OK.TabIndex = 61;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // lb_Message
            // 
            this.lb_Message.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Message.ForeColor = System.Drawing.Color.Black;
            this.lb_Message.Location = new System.Drawing.Point(8, 52);
            this.lb_Message.Name = "lb_Message";
            this.lb_Message.Size = new System.Drawing.Size(391, 152);
            this.lb_Message.TabIndex = 62;
            this.lb_Message.Text = "Message";
            this.lb_Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_effect
            // 
            this.timer_effect.Interval = 500;
            this.timer_effect.Tick += new System.EventHandler(this.timer_effect_Tick);
            // 
            // Infomation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(411, 286);
            this.ControlBox = false;
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lb_Message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Infomation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Information";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Information_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lb_server;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label lb_Message;
        private System.Windows.Forms.Timer timer_effect;
    }
}