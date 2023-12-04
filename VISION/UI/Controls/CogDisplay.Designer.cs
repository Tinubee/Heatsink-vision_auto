namespace VISION.UI.Controls
{
    partial class CogDisplay
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogDisplay));
            this.cdyDisplay = new Cognex.VisionPro.Display.CogDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // cdyDisplay
            // 
            this.cdyDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cdyDisplay.ColorMapLowerRoiLimit = 0D;
            this.cdyDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cdyDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cdyDisplay.ColorMapUpperRoiLimit = 1D;
            this.cdyDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cdyDisplay.DoubleTapZoomCycleLength = 2;
            this.cdyDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.cdyDisplay.Location = new System.Drawing.Point(0, 0);
            this.cdyDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.cdyDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cdyDisplay.MouseWheelSensitivity = 1D;
            this.cdyDisplay.Name = "cdyDisplay";
            this.cdyDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cdyDisplay.OcxState")));
            this.cdyDisplay.Size = new System.Drawing.Size(923, 715);
            this.cdyDisplay.TabIndex = 12;
            // 
            // CogDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cdyDisplay);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CogDisplay";
            this.Size = new System.Drawing.Size(923, 715);
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Cognex.VisionPro.Display.CogDisplay cdyDisplay;
    }
}
