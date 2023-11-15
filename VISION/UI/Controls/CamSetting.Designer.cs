namespace VISION.UI.Controls
{
    partial class CamSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamSetting));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.b저장 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.GridControl1 = new MvUtils.CustomGrid();
            this.Bind카메라설정 = new System.Windows.Forms.BindingSource(this.components);
            this.GridView1 = new MvUtils.CustomView();
            this.col구분 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col코드 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col설명1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col시간 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col노출 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col대비 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.e대비 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col밝기 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.e밝기 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col가로 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col세로 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOffsetX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col주소 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col상태 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.e상태 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.GridControl2 = new MvUtils.CustomGrid();
            this.GridView2 = new MvUtils.CustomView();
            this.col카메라 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col포트 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col채널 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col광량 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.e조명밝기 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col설명 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col켜짐 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.e조명켜짐 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.Bind조명설정 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind카메라설정)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e대비)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e밝기)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e상태)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e조명밝기)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e조명켜짐)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind조명설정)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.b저장);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 835);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(854, 36);
            this.panelControl1.TabIndex = 9;
            // 
            // b저장
            // 
            this.b저장.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b저장.Appearance.Options.UseFont = true;
            this.b저장.Dock = System.Windows.Forms.DockStyle.Right;
            this.b저장.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b저장.ImageOptions.SvgImage")));
            this.b저장.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            this.b저장.Location = new System.Drawing.Point(701, 2);
            this.b저장.Name = "b저장";
            this.b저장.Size = new System.Drawing.Size(151, 32);
            this.b저장.TabIndex = 7;
            this.b저장.Text = "설정저장";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.GridControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(854, 440);
            this.groupControl2.TabIndex = 10;
            this.groupControl2.Text = "Cameras";
            // 
            // GridControl1
            // 
            this.GridControl1.DataSource = this.Bind카메라설정;
            this.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridControl1.Location = new System.Drawing.Point(2, 27);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.e상태,
            this.e대비,
            this.e밝기});
            this.GridControl1.Size = new System.Drawing.Size(850, 411);
            this.GridControl1.TabIndex = 8;
            this.GridControl1.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // Bind카메라설정
            // 
            this.Bind카메라설정.DataSource = typeof(VISION.Schemas.카메라장치);
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
            this.col구분,
            this.col코드,
            this.col설명1,
            this.col시간,
            this.col노출,
            this.col대비,
            this.col밝기,
            this.col가로,
            this.col세로,
            this.colOffsetX,
            this.col주소,
            this.col상태});
            this.GridView1.FooterPanelHeight = 21;
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.GroupRowHeight = 21;
            this.GridView1.IndicatorWidth = 44;
            this.GridView1.MinColumnRowHeight = 24;
            this.GridView1.MinRowHeight = 16;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.GridView1.OptionsCustomization.AllowColumnMoving = false;
            this.GridView1.OptionsCustomization.AllowFilter = false;
            this.GridView1.OptionsCustomization.AllowGroup = false;
            this.GridView1.OptionsCustomization.AllowMergedGrouping = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.GridView1.OptionsCustomization.AllowSort = false;
            this.GridView1.OptionsFilter.AllowFilterEditor = false;
            this.GridView1.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView1.OptionsPrint.AutoWidth = false;
            this.GridView1.OptionsPrint.UsePrintStyles = false;
            this.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.OptionsView.ShowIndicator = false;
            this.GridView1.RowHeight = 20;
            // 
            // col구분
            // 
            this.col구분.AppearanceHeader.Options.UseTextOptions = true;
            this.col구분.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col구분.FieldName = "구분";
            this.col구분.Name = "col구분";
            this.col구분.Visible = true;
            this.col구분.VisibleIndex = 0;
            // 
            // col코드
            // 
            this.col코드.AppearanceHeader.Options.UseTextOptions = true;
            this.col코드.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col코드.FieldName = "코드";
            this.col코드.Name = "col코드";
            this.col코드.Visible = true;
            this.col코드.VisibleIndex = 1;
            // 
            // col설명1
            // 
            this.col설명1.AppearanceHeader.Options.UseTextOptions = true;
            this.col설명1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col설명1.FieldName = "설명";
            this.col설명1.Name = "col설명1";
            this.col설명1.Visible = true;
            this.col설명1.VisibleIndex = 2;
            // 
            // col시간
            // 
            this.col시간.AppearanceHeader.Options.UseTextOptions = true;
            this.col시간.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col시간.Caption = "Timeout";
            this.col시간.DisplayFormat.FormatString = "{0:#,0}";
            this.col시간.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col시간.FieldName = "시간";
            this.col시간.Name = "col시간";
            this.col시간.ToolTip = "취득 대기시간(ms)";
            // 
            // col노출
            // 
            this.col노출.AppearanceHeader.Options.UseTextOptions = true;
            this.col노출.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col노출.Caption = "Exposure";
            this.col노출.DisplayFormat.FormatString = "{0:#,0}";
            this.col노출.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col노출.FieldName = "노출";
            this.col노출.Name = "col노출";
            // 
            // col대비
            // 
            this.col대비.AppearanceHeader.Options.UseTextOptions = true;
            this.col대비.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col대비.Caption = "Contrast";
            this.col대비.ColumnEdit = this.e대비;
            this.col대비.DisplayFormat.FormatString = "{0:#,0}";
            this.col대비.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col대비.FieldName = "대비";
            this.col대비.Name = "col대비";
            // 
            // e대비
            // 
            this.e대비.AutoHeight = false;
            this.e대비.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.e대비.IsFloatValue = false;
            this.e대비.MaskSettings.Set("mask", "N00");
            this.e대비.MaxValue = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.e대비.Name = "e대비";
            // 
            // col밝기
            // 
            this.col밝기.AppearanceHeader.Options.UseTextOptions = true;
            this.col밝기.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col밝기.Caption = "Black Level";
            this.col밝기.ColumnEdit = this.e밝기;
            this.col밝기.DisplayFormat.FormatString = "{0:#,0}";
            this.col밝기.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col밝기.FieldName = "밝기";
            this.col밝기.Name = "col밝기";
            this.col밝기.Visible = true;
            this.col밝기.VisibleIndex = 3;
            // 
            // e밝기
            // 
            this.e밝기.AutoHeight = false;
            this.e밝기.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.e밝기.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.e밝기.IsFloatValue = false;
            this.e밝기.MaskSettings.Set("mask", "N00");
            this.e밝기.MaxValue = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.e밝기.Name = "e밝기";
            // 
            // col가로
            // 
            this.col가로.AppearanceHeader.Options.UseTextOptions = true;
            this.col가로.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col가로.DisplayFormat.FormatString = "{0:#,#}";
            this.col가로.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col가로.FieldName = "가로";
            this.col가로.Name = "col가로";
            this.col가로.Visible = true;
            this.col가로.VisibleIndex = 6;
            // 
            // col세로
            // 
            this.col세로.AppearanceHeader.Options.UseTextOptions = true;
            this.col세로.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col세로.DisplayFormat.FormatString = "{0:#,#}";
            this.col세로.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.col세로.FieldName = "세로";
            this.col세로.Name = "col세로";
            this.col세로.Visible = true;
            this.col세로.VisibleIndex = 7;
            // 
            // colOffsetX
            // 
            this.colOffsetX.AppearanceHeader.Options.UseTextOptions = true;
            this.colOffsetX.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colOffsetX.DisplayFormat.FormatString = "{0:#,#}";
            this.colOffsetX.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOffsetX.FieldName = "OffsetX";
            this.colOffsetX.Name = "colOffsetX";
            this.colOffsetX.Visible = true;
            this.colOffsetX.VisibleIndex = 8;
            // 
            // col주소
            // 
            this.col주소.AppearanceCell.Options.UseTextOptions = true;
            this.col주소.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col주소.AppearanceHeader.Options.UseTextOptions = true;
            this.col주소.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col주소.FieldName = "주소";
            this.col주소.Name = "col주소";
            this.col주소.OptionsColumn.AllowEdit = false;
            this.col주소.Visible = true;
            this.col주소.VisibleIndex = 4;
            // 
            // col상태
            // 
            this.col상태.AppearanceHeader.Options.UseTextOptions = true;
            this.col상태.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col상태.ColumnEdit = this.e상태;
            this.col상태.FieldName = "상태";
            this.col상태.Name = "col상태";
            this.col상태.OptionsColumn.AllowEdit = false;
            this.col상태.Visible = true;
            this.col상태.VisibleIndex = 5;
            // 
            // e상태
            // 
            this.e상태.AutoHeight = false;
            this.e상태.Name = "e상태";
            this.e상태.OffText = "Off";
            this.e상태.OnText = "On";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.GridControl2);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 440);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(854, 395);
            this.groupControl3.TabIndex = 11;
            this.groupControl3.Text = "Lights";
            // 
            // GridControl2
            // 
            this.GridControl2.DataSource = this.Bind조명설정;
            this.GridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridControl2.Location = new System.Drawing.Point(2, 27);
            this.GridControl2.MainView = this.GridView2;
            this.GridControl2.Name = "GridControl2";
            this.GridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.e조명켜짐,
            this.e조명밝기});
            this.GridControl2.Size = new System.Drawing.Size(850, 366);
            this.GridControl2.TabIndex = 12;
            this.GridControl2.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            this.GridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView2});
            // 
            // GridView2
            // 
            this.GridView2.AllowColumnMenu = true;
            this.GridView2.AllowCustomMenu = true;
            this.GridView2.AllowExport = true;
            this.GridView2.AllowPrint = true;
            this.GridView2.AllowSettingsMenu = false;
            this.GridView2.AllowSummaryMenu = true;
            this.GridView2.ApplyFocusedRow = true;
            this.GridView2.Caption = "";
            this.GridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col카메라,
            this.col포트,
            this.col채널,
            this.col광량,
            this.col설명,
            this.col켜짐});
            this.GridView2.FooterPanelHeight = 21;
            this.GridView2.GridControl = this.GridControl2;
            this.GridView2.GroupRowHeight = 21;
            this.GridView2.IndicatorWidth = 44;
            this.GridView2.MinColumnRowHeight = 24;
            this.GridView2.MinRowHeight = 16;
            this.GridView2.Name = "GridView2";
            this.GridView2.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.GridView2.OptionsCustomization.AllowColumnMoving = false;
            this.GridView2.OptionsCustomization.AllowFilter = false;
            this.GridView2.OptionsCustomization.AllowGroup = false;
            this.GridView2.OptionsCustomization.AllowQuickHideColumns = false;
            this.GridView2.OptionsCustomization.AllowSort = false;
            this.GridView2.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView2.OptionsPrint.AutoWidth = false;
            this.GridView2.OptionsPrint.UsePrintStyles = false;
            this.GridView2.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.GridView2.OptionsView.ShowGroupPanel = false;
            this.GridView2.OptionsView.ShowIndicator = false;
            this.GridView2.RowHeight = 20;
            // 
            // col카메라
            // 
            this.col카메라.AppearanceHeader.Options.UseTextOptions = true;
            this.col카메라.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col카메라.FieldName = "카메라";
            this.col카메라.Name = "col카메라";
            this.col카메라.OptionsColumn.AllowEdit = false;
            this.col카메라.Visible = true;
            this.col카메라.VisibleIndex = 0;
            // 
            // col포트
            // 
            this.col포트.AppearanceHeader.Options.UseTextOptions = true;
            this.col포트.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col포트.FieldName = "포트";
            this.col포트.Name = "col포트";
            this.col포트.OptionsColumn.AllowEdit = false;
            this.col포트.Visible = true;
            this.col포트.VisibleIndex = 1;
            // 
            // col채널
            // 
            this.col채널.AppearanceHeader.Options.UseTextOptions = true;
            this.col채널.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col채널.FieldName = "채널";
            this.col채널.Name = "col채널";
            this.col채널.OptionsColumn.AllowEdit = false;
            this.col채널.Visible = true;
            this.col채널.VisibleIndex = 2;
            // 
            // col광량
            // 
            this.col광량.AppearanceHeader.Options.UseTextOptions = true;
            this.col광량.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col광량.Caption = "밝기";
            this.col광량.ColumnEdit = this.e조명밝기;
            this.col광량.FieldName = "밝기";
            this.col광량.Name = "col광량";
            this.col광량.Visible = true;
            this.col광량.VisibleIndex = 4;
            // 
            // e조명밝기
            // 
            this.e조명밝기.AutoHeight = false;
            this.e조명밝기.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.e조명밝기.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.e조명밝기.IsFloatValue = false;
            this.e조명밝기.MaskSettings.Set("mask", "N00");
            this.e조명밝기.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.e조명밝기.Name = "e조명밝기";
            // 
            // col설명
            // 
            this.col설명.AppearanceHeader.Options.UseTextOptions = true;
            this.col설명.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col설명.FieldName = "설명";
            this.col설명.Name = "col설명";
            this.col설명.Visible = true;
            this.col설명.VisibleIndex = 3;
            // 
            // col켜짐
            // 
            this.col켜짐.AppearanceHeader.Options.UseTextOptions = true;
            this.col켜짐.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col켜짐.ColumnEdit = this.e조명켜짐;
            this.col켜짐.FieldName = "켜짐";
            this.col켜짐.Name = "col켜짐";
            this.col켜짐.Visible = true;
            this.col켜짐.VisibleIndex = 5;
            // 
            // e조명켜짐
            // 
            this.e조명켜짐.AutoHeight = false;
            this.e조명켜짐.Name = "e조명켜짐";
            this.e조명켜짐.OffText = "Off";
            this.e조명켜짐.OnText = "On";
            // 
            // Bind조명설정
            // 
            this.Bind조명설정.DataSource = typeof(VISION.Schemas.조명제어);
            // 
            // CamSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "CamSetting";
            this.Size = new System.Drawing.Size(854, 871);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind카메라설정)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e대비)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e밝기)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e상태)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e조명밝기)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e조명켜짐)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind조명설정)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton b저장;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private MvUtils.CustomGrid GridControl1;
        private MvUtils.CustomView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col구분;
        private DevExpress.XtraGrid.Columns.GridColumn col코드;
        private DevExpress.XtraGrid.Columns.GridColumn col설명1;
        private DevExpress.XtraGrid.Columns.GridColumn col시간;
        private DevExpress.XtraGrid.Columns.GridColumn col노출;
        private DevExpress.XtraGrid.Columns.GridColumn col대비;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit e대비;
        private DevExpress.XtraGrid.Columns.GridColumn col밝기;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit e밝기;
        private DevExpress.XtraGrid.Columns.GridColumn col가로;
        private DevExpress.XtraGrid.Columns.GridColumn col세로;
        private DevExpress.XtraGrid.Columns.GridColumn colOffsetX;
        private DevExpress.XtraGrid.Columns.GridColumn col주소;
        private DevExpress.XtraGrid.Columns.GridColumn col상태;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch e상태;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private MvUtils.CustomGrid GridControl2;
        private MvUtils.CustomView GridView2;
        private DevExpress.XtraGrid.Columns.GridColumn col카메라;
        private DevExpress.XtraGrid.Columns.GridColumn col포트;
        private DevExpress.XtraGrid.Columns.GridColumn col채널;
        private DevExpress.XtraGrid.Columns.GridColumn col광량;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit e조명밝기;
        private DevExpress.XtraGrid.Columns.GridColumn col설명;
        private DevExpress.XtraGrid.Columns.GridColumn col켜짐;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch e조명켜짐;
        private System.Windows.Forms.BindingSource Bind카메라설정;
        private System.Windows.Forms.BindingSource Bind조명설정;
    }
}
