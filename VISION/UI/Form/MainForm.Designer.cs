namespace VISION.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.p검사뷰어 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer1 = new DevExpress.XtraBars.TabFormContentContainer();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.e상태뷰어 = new VISION.UI.Controls.State();
            this.tabFormControl1 = new DevExpress.XtraBars.TabFormControl();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.skinPaletteDropDownButtonItem1 = new DevExpress.XtraBars.SkinPaletteDropDownButtonItem();
            this.p검사설정 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer2 = new DevExpress.XtraBars.TabFormContentContainer();
            this.p검사내역 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer3 = new DevExpress.XtraBars.TabFormContentContainer();
            this.p환경설정 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer4 = new DevExpress.XtraBars.TabFormContentContainer();
            this.t환경설정 = new DevExpress.XtraTab.XtraTabControl();
            this.t변수설정 = new DevExpress.XtraTab.XtraTabPage();
            this.t장치설정 = new DevExpress.XtraTab.XtraTabPage();
            this.deviceSettings1 = new VISION.UI.Controls.DeviceSettings();
            this.t로그내역 = new DevExpress.XtraTab.XtraTabPage();
            this.e로그내역 = new VISION.UI.LogViewer();
            this.tabFormContentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).BeginInit();
            this.tabFormContentContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).BeginInit();
            this.t환경설정.SuspendLayout();
            this.t장치설정.SuspendLayout();
            this.t로그내역.SuspendLayout();
            this.SuspendLayout();
            // 
            // p검사뷰어
            // 
            this.p검사뷰어.ContentContainer = this.tabFormContentContainer1;
            this.p검사뷰어.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p검사뷰어.ImageOptions.SvgImage")));
            this.p검사뷰어.Name = "p검사뷰어";
            this.p검사뷰어.Text = "검사뷰어";
            // 
            // tabFormContentContainer1
            // 
            this.tabFormContentContainer1.Controls.Add(this.panelControl1);
            this.tabFormContentContainer1.Controls.Add(this.e상태뷰어);
            this.tabFormContentContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer1.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer1.Name = "tabFormContentContainer1";
            this.tabFormContentContainer1.Size = new System.Drawing.Size(1920, 980);
            this.tabFormContentContainer1.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 104);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1920, 876);
            this.panelControl1.TabIndex = 2;
            // 
            // e상태뷰어
            // 
            this.e상태뷰어.Dock = System.Windows.Forms.DockStyle.Top;
            this.e상태뷰어.Location = new System.Drawing.Point(0, 0);
            this.e상태뷰어.Name = "e상태뷰어";
            this.e상태뷰어.Size = new System.Drawing.Size(1920, 104);
            this.e상태뷰어.TabIndex = 0;
            // 
            // tabFormControl1
            // 
            this.tabFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1,
            this.skinPaletteDropDownButtonItem1});
            this.tabFormControl1.Location = new System.Drawing.Point(0, 0);
            this.tabFormControl1.Name = "tabFormControl1";
            this.tabFormControl1.Pages.Add(this.p검사뷰어);
            this.tabFormControl1.Pages.Add(this.p검사설정);
            this.tabFormControl1.Pages.Add(this.p검사내역);
            this.tabFormControl1.Pages.Add(this.p환경설정);
            this.tabFormControl1.SelectedPage = this.p환경설정;
            this.tabFormControl1.ShowAddPageButton = false;
            this.tabFormControl1.ShowTabCloseButtons = false;
            this.tabFormControl1.ShowTabsInTitleBar = DevExpress.XtraBars.ShowTabsInTitleBar.True;
            this.tabFormControl1.Size = new System.Drawing.Size(1920, 30);
            this.tabFormControl1.TabForm = this;
            this.tabFormControl1.TabIndex = 0;
            this.tabFormControl1.TabLeftItemLinks.Add(this.barStaticItem1);
            this.tabFormControl1.TabRightItemLinks.Add(this.skinPaletteDropDownButtonItem1);
            this.tabFormControl1.TabStop = false;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "HeatSink Total Inspection";
            this.barStaticItem1.Id = 2;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // skinPaletteDropDownButtonItem1
            // 
            this.skinPaletteDropDownButtonItem1.Id = 0;
            this.skinPaletteDropDownButtonItem1.Name = "skinPaletteDropDownButtonItem1";
            // 
            // p검사설정
            // 
            this.p검사설정.ContentContainer = this.tabFormContentContainer2;
            this.p검사설정.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p검사설정.ImageOptions.SvgImage")));
            this.p검사설정.Name = "p검사설정";
            this.p검사설정.Text = "검사설정";
            // 
            // tabFormContentContainer2
            // 
            this.tabFormContentContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer2.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer2.Name = "tabFormContentContainer2";
            this.tabFormContentContainer2.Size = new System.Drawing.Size(1920, 980);
            this.tabFormContentContainer2.TabIndex = 2;
            // 
            // p검사내역
            // 
            this.p검사내역.ContentContainer = this.tabFormContentContainer3;
            this.p검사내역.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p검사내역.ImageOptions.SvgImage")));
            this.p검사내역.Name = "p검사내역";
            this.p검사내역.Text = "검사내역";
            // 
            // tabFormContentContainer3
            // 
            this.tabFormContentContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer3.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer3.Name = "tabFormContentContainer3";
            this.tabFormContentContainer3.Size = new System.Drawing.Size(1920, 980);
            this.tabFormContentContainer3.TabIndex = 2;
            // 
            // p환경설정
            // 
            this.p환경설정.ContentContainer = this.tabFormContentContainer4;
            this.p환경설정.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("p환경설정.ImageOptions.SvgImage")));
            this.p환경설정.Name = "p환경설정";
            this.p환경설정.Text = "환경설정";
            // 
            // tabFormContentContainer4
            // 
            this.tabFormContentContainer4.Controls.Add(this.t환경설정);
            this.tabFormContentContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer4.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer4.Name = "tabFormContentContainer4";
            this.tabFormContentContainer4.Size = new System.Drawing.Size(1920, 980);
            this.tabFormContentContainer4.TabIndex = 3;
            // 
            // t환경설정
            // 
            this.t환경설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t환경설정.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.t환경설정.Location = new System.Drawing.Point(0, 0);
            this.t환경설정.Name = "t환경설정";
            this.t환경설정.SelectedTabPage = this.t변수설정;
            this.t환경설정.Size = new System.Drawing.Size(1920, 980);
            this.t환경설정.TabIndex = 2;
            this.t환경설정.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.t변수설정,
            this.t장치설정,
            this.t로그내역});
            // 
            // t변수설정
            // 
            this.t변수설정.Name = "t변수설정";
            this.t변수설정.Size = new System.Drawing.Size(1918, 949);
            this.t변수설정.Text = "변수설정";
            // 
            // t장치설정
            // 
            this.t장치설정.Controls.Add(this.deviceSettings1);
            this.t장치설정.Name = "t장치설정";
            this.t장치설정.Size = new System.Drawing.Size(1918, 949);
            this.t장치설정.Text = "장치설정";
            // 
            // deviceSettings1
            // 
            this.deviceSettings1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceSettings1.Location = new System.Drawing.Point(0, 0);
            this.deviceSettings1.Name = "deviceSettings1";
            this.deviceSettings1.Size = new System.Drawing.Size(1918, 949);
            this.deviceSettings1.TabIndex = 0;
            // 
            // t로그내역
            // 
            this.t로그내역.Controls.Add(this.e로그내역);
            this.t로그내역.Name = "t로그내역";
            this.t로그내역.Size = new System.Drawing.Size(1918, 949);
            this.t로그내역.Text = "로그내역";
            // 
            // e로그내역
            // 
            this.e로그내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e로그내역.Location = new System.Drawing.Point(0, 0);
            this.e로그내역.Name = "e로그내역";
            this.e로그내역.Size = new System.Drawing.Size(1918, 949);
            this.e로그내역.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1010);
            this.Controls.Add(this.tabFormContentContainer4);
            this.Controls.Add(this.tabFormControl1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TabFormControl = this.tabFormControl1;
            this.Text = "HeatSink Vision 검사기";
            this.tabFormContentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).EndInit();
            this.tabFormContentContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).EndInit();
            this.t환경설정.ResumeLayout(false);
            this.t장치설정.ResumeLayout(false);
            this.t로그내역.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.TabFormPage p검사뷰어;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer1;
        private DevExpress.XtraBars.TabFormControl tabFormControl1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.TabFormPage p검사설정;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer2;
        private DevExpress.XtraBars.TabFormPage p검사내역;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer3;
        private DevExpress.XtraBars.TabFormPage p환경설정;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer4;
        private Controls.State e상태뷰어;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl t환경설정;
        private DevExpress.XtraTab.XtraTabPage t변수설정;
        private DevExpress.XtraTab.XtraTabPage t장치설정;
        private DevExpress.XtraTab.XtraTabPage t로그내역;
        private Controls.DeviceSettings deviceSettings1;
        private DevExpress.XtraBars.SkinPaletteDropDownButtonItem skinPaletteDropDownButtonItem1;
        private LogViewer e로그내역;
    }
}