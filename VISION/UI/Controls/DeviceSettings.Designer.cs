namespace VISION.UI.Controls
{
    partial class DeviceSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceSettings));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.camSetting1 = new VISION.UI.Controls.CamSetting();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.b캠트리거리셋 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.e배출구분 = new DevExpress.XtraEditors.ToggleSwitch();
            this.e강제배출 = new DevExpress.XtraEditors.ToggleSwitch();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.config1 = new VISION.UI.Controls.Config();
            this.users1 = new VISION.UI.Controls.Users();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e배출구분.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e강제배출.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.camSetting1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.xtraTabControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1920, 1040);
            this.splitContainerControl1.SplitterPosition = 1288;
            this.splitContainerControl1.TabIndex = 1;
            // 
            // camSetting1
            // 
            this.camSetting1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camSetting1.Location = new System.Drawing.Point(0, 0);
            this.camSetting1.Name = "camSetting1";
            this.camSetting1.Size = new System.Drawing.Size(1288, 1040);
            this.camSetting1.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(622, 1040);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.b캠트리거리셋);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(620, 1009);
            this.xtraTabPage1.Text = "Others";
            // 
            // b캠트리거리셋
            // 
            this.b캠트리거리셋.Appearance.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b캠트리거리셋.Appearance.Options.UseFont = true;
            this.b캠트리거리셋.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b캠트리거리셋.ImageOptions.SvgImage")));
            this.b캠트리거리셋.Location = new System.Drawing.Point(3, 85);
            this.b캠트리거리셋.Name = "b캠트리거리셋";
            this.b캠트리거리셋.Size = new System.Drawing.Size(302, 55);
            this.b캠트리거리셋.TabIndex = 4;
            this.b캠트리거리셋.Text = "카메라 트리거 리셋";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(620, 79);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Forced Ejection";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.e배출구분);
            this.layoutControl1.Controls.Add(this.e강제배출);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 27);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(616, 50);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // e배출구분
            // 
            this.e배출구분.EnterMoveNextControl = true;
            this.e배출구분.Location = new System.Drawing.Point(361, 14);
            this.e배출구분.Name = "e배출구분";
            this.e배출구분.Properties.OffText = "NG";
            this.e배출구분.Properties.OnText = "OK";
            this.e배출구분.Size = new System.Drawing.Size(241, 22);
            this.e배출구분.StyleController = this.layoutControl1;
            this.e배출구분.TabIndex = 5;
            // 
            // e강제배출
            // 
            this.e강제배출.EnterMoveNextControl = true;
            this.e강제배출.Location = new System.Drawing.Point(64, 14);
            this.e강제배출.Name = "e강제배출";
            this.e강제배출.Properties.OffText = "Off";
            this.e강제배출.Properties.OnText = "On";
            this.e강제배출.Size = new System.Drawing.Size(239, 22);
            this.e강제배출.StyleController = this.layoutControl1;
            this.e강제배출.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(616, 50);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.e강제배출;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem1.Size = new System.Drawing.Size(297, 30);
            this.layoutControlItem1.Text = "On/Off";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(38, 15);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.e배출구분;
            this.layoutControlItem2.Location = new System.Drawing.Point(297, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem2.Size = new System.Drawing.Size(299, 30);
            this.layoutControlItem2.Text = "NG/OK";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(38, 15);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.users1);
            this.xtraTabPage2.Controls.Add(this.config1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(620, 1009);
            this.xtraTabPage2.Text = "Config";
            // 
            // config1
            // 
            this.config1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.config1.Location = new System.Drawing.Point(0, 0);
            this.config1.Name = "config1";
            this.config1.Size = new System.Drawing.Size(620, 1009);
            this.config1.TabIndex = 0;
            // 
            // users1
            // 
            this.users1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.users1.Location = new System.Drawing.Point(0, 645);
            this.users1.Name = "users1";
            this.users1.Size = new System.Drawing.Size(620, 364);
            this.users1.TabIndex = 1;
            // 
            // DeviceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "DeviceSettings";
            this.Size = new System.Drawing.Size(1920, 1040);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.e배출구분.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e강제배출.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.SimpleButton b캠트리거리셋;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ToggleSwitch e배출구분;
        private DevExpress.XtraEditors.ToggleSwitch e강제배출;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private CamSetting camSetting1;
        private Config config1;
        private Users users1;
    }
}
