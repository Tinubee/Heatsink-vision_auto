namespace VISION.UI
{
    partial class CountViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.e타이틀 = new DevExpress.XtraEditors.GroupControl();
            this.e현재값 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.e타이틀)).BeginInit();
            this.e타이틀.SuspendLayout();
            this.SuspendLayout();
            // 
            // e타이틀
            // 
            this.e타이틀.AppearanceCaption.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e타이틀.AppearanceCaption.Options.UseFont = true;
            this.e타이틀.AppearanceCaption.Options.UseTextOptions = true;
            this.e타이틀.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e타이틀.Controls.Add(this.e현재값);
            this.e타이틀.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e타이틀.Location = new System.Drawing.Point(0, 0);
            this.e타이틀.Name = "e타이틀";
            this.e타이틀.Size = new System.Drawing.Size(150, 70);
            this.e타이틀.TabIndex = 1;
            this.e타이틀.Text = "양품률(%)";
            // 
            // e현재값
            // 
            this.e현재값.Appearance.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e현재값.Appearance.Options.UseFont = true;
            this.e현재값.Appearance.Options.UseTextOptions = true;
            this.e현재값.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e현재값.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.e현재값.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e현재값.Location = new System.Drawing.Point(2, 27);
            this.e현재값.Name = "e현재값";
            this.e현재값.Size = new System.Drawing.Size(146, 41);
            this.e현재값.TabIndex = 0;
            this.e현재값.Text = "100.0";
            // 
            // CountViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.e타이틀);
            this.Name = "CountViewer";
            this.Size = new System.Drawing.Size(150, 70);
            ((System.ComponentModel.ISupportInitialize)(this.e타이틀)).EndInit();
            this.e타이틀.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl e타이틀;
        private DevExpress.XtraEditors.LabelControl e현재값;
    }
}
