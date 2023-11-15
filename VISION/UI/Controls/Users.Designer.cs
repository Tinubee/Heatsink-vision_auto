namespace VISION.UI.Controls
{
    partial class Users
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Users));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.Bind사용자 = new System.Windows.Forms.BindingSource(this.components);
            this.g유저관리 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.b유저저장 = new DevExpress.XtraEditors.SimpleButton();
            this.GridControl1 = new MvUtils.CustomGrid();
            this.GridView1 = new MvUtils.CustomView();
            this.col성명 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col암호 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col비고 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col권한 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col허용 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind사용자)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g유저관리)).BeginInit();
            this.g유저관리.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(647, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 364);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(647, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 364);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(647, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 364);
            // 
            // Bind사용자
            // 
            this.Bind사용자.DataSource = typeof(VISION.Schemas.유저자료);
            // 
            // g유저관리
            // 
            this.g유저관리.Controls.Add(this.GridControl1);
            this.g유저관리.Controls.Add(this.panelControl1);
            this.g유저관리.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g유저관리.Location = new System.Drawing.Point(0, 0);
            this.g유저관리.Name = "g유저관리";
            this.g유저관리.Size = new System.Drawing.Size(647, 364);
            this.g유저관리.TabIndex = 11;
            this.g유저관리.Text = "사용자 관리";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.b유저저장);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 328);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(3);
            this.panelControl1.Size = new System.Drawing.Size(643, 34);
            this.panelControl1.TabIndex = 5;
            // 
            // b유저저장
            // 
            this.b유저저장.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b유저저장.Appearance.Options.UseFont = true;
            this.b유저저장.Dock = System.Windows.Forms.DockStyle.Right;
            this.b유저저장.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b유저저장.ImageOptions.SvgImage")));
            this.b유저저장.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            this.b유저저장.Location = new System.Drawing.Point(460, 3);
            this.b유저저장.Name = "b유저저장";
            this.b유저저장.Size = new System.Drawing.Size(180, 28);
            this.b유저저장.TabIndex = 1;
            this.b유저저장.Text = "저  장";
            // 
            // GridControl1
            // 
            this.GridControl1.DataSource = this.Bind사용자;
            this.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridControl1.Location = new System.Drawing.Point(2, 27);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.MenuManager = this.barManager1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(643, 301);
            this.GridControl1.TabIndex = 6;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // GridView1
            // 
            this.GridView1.AllowColumnMenu = true;
            this.GridView1.AllowCustomMenu = true;
            this.GridView1.AllowExport = true;
            this.GridView1.AllowPrint = true;
            this.GridView1.AllowSettingsMenu = false;
            this.GridView1.AllowSummaryMenu = true;
            this.GridView1.ApplyFocusedRow = true;
            this.GridView1.Caption = "";
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col성명,
            this.col암호,
            this.col비고,
            this.col권한,
            this.col허용});
            this.GridView1.FooterPanelHeight = 21;
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.GroupRowHeight = 21;
            this.GridView1.IndicatorWidth = 44;
            this.GridView1.MinColumnRowHeight = 24;
            this.GridView1.MinRowHeight = 18;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.Editable = false;
            this.GridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.GridView1.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView1.OptionsPrint.AutoWidth = false;
            this.GridView1.OptionsPrint.UsePrintStyles = false;
            this.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.RowHeight = 20;
            // 
            // col성명
            // 
            this.col성명.AppearanceHeader.Options.UseTextOptions = true;
            this.col성명.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col성명.FieldName = "성명";
            this.col성명.Name = "col성명";
            this.col성명.Visible = true;
            this.col성명.VisibleIndex = 0;
            // 
            // col암호
            // 
            this.col암호.AppearanceHeader.Options.UseTextOptions = true;
            this.col암호.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col암호.FieldName = "암호";
            this.col암호.Name = "col암호";
            this.col암호.Visible = true;
            this.col암호.VisibleIndex = 1;
            // 
            // col비고
            // 
            this.col비고.AppearanceHeader.Options.UseTextOptions = true;
            this.col비고.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col비고.FieldName = "비고";
            this.col비고.Name = "col비고";
            this.col비고.Visible = true;
            this.col비고.VisibleIndex = 2;
            // 
            // col권한
            // 
            this.col권한.AppearanceHeader.Options.UseTextOptions = true;
            this.col권한.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col권한.FieldName = "권한";
            this.col권한.Name = "col권한";
            this.col권한.Visible = true;
            this.col권한.VisibleIndex = 3;
            // 
            // col허용
            // 
            this.col허용.AppearanceHeader.Options.UseTextOptions = true;
            this.col허용.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col허용.FieldName = "허용";
            this.col허용.Name = "col허용";
            this.col허용.Visible = true;
            this.col허용.VisibleIndex = 4;
            // 
            // Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.g유저관리);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "Users";
            this.Size = new System.Drawing.Size(647, 364);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind사용자)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g유저관리)).EndInit();
            this.g유저관리.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.BindingSource Bind사용자;
        private DevExpress.XtraEditors.GroupControl g유저관리;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton b유저저장;
        private MvUtils.CustomGrid GridControl1;
        private MvUtils.CustomView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col성명;
        private DevExpress.XtraGrid.Columns.GridColumn col암호;
        private DevExpress.XtraGrid.Columns.GridColumn col비고;
        private DevExpress.XtraGrid.Columns.GridColumn col권한;
        private DevExpress.XtraGrid.Columns.GridColumn col허용;
    }
}
