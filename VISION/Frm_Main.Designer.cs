namespace VISION
{
    partial class Frm_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpUnder = new System.Windows.Forms.TableLayoutPanel();
            this.btn_SystemSetup = new System.Windows.Forms.Button();
            this.btn_Model = new System.Windows.Forms.Button();
            this.btn_CamSet = new System.Windows.Forms.Button();
            this.btn_Analyze = new System.Windows.Forms.Button();
            this.tableLayoutPanel32 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_ResultOK = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.num_LightNumber = new System.Windows.Forms.NumericUpDown();
            this.Btn_LightOnOff = new System.Windows.Forms.Button();
            this.btn_ToolSetUp = new System.Windows.Forms.Button();
            this.tlpTopSide = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CurruntModelName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_Time = new System.Windows.Forms.Label();
            this.lb_Ver = new System.Windows.Forms.Label();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Status = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_Setting = new System.Windows.Forms.Timer(this.components);
            this.LightControl1 = new System.IO.Ports.SerialPort(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_INPUT11 = new System.Windows.Forms.Button();
            this.btn_OUTPUT8 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT9 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT6 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT7 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT4 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT5 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT2 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT3 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT0 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT1 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT10 = new System.Windows.Forms.CheckBox();
            this.btn_OUTPUT11 = new System.Windows.Forms.CheckBox();
            this.btn_INPUT7 = new System.Windows.Forms.Button();
            this.btn_INPUT6 = new System.Windows.Forms.Button();
            this.btn_INPUT5 = new System.Windows.Forms.Button();
            this.btn_INPUT4 = new System.Windows.Forms.Button();
            this.btn_INPUT3 = new System.Windows.Forms.Button();
            this.btn_INPUT2 = new System.Windows.Forms.Button();
            this.btn_INPUT1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_INPUT0 = new System.Windows.Forms.Button();
            this.btn_INPUT8 = new System.Windows.Forms.Button();
            this.btn_INPUT9 = new System.Windows.Forms.Button();
            this.btn_INPUT10 = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.logControl1 = new KimLib.LogControl();
            this.Main_TabControl = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel30 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM6_NGRATE = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.lb_CAM6_TOTAL = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.lb_CAM6_NG = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.lb_CAM6_OK = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.tableLayoutPanel29 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM5_NGRATE = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.lb_CAM5_TOTAL = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.lb_CAM5_NG = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.lb_CAM5_OK = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.tableLayoutPanel28 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM4_NGRATE = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.lb_CAM4_TOTAL = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.lb_CAM4_NG = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.lb_CAM4_OK = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM3_NGRATE = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lb_CAM3_TOTAL = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.lb_CAM3_NG = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.lb_CAM3_OK = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM2_NGRATE = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.lb_CAM2_TOTAL = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.lb_CAM2_NG = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.lb_CAM2_OK = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_CAM1_NGRATE = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lb_CAM1_TOTAL = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lb_CAM1_NG = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lb_CAM1_OK = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.LightControl2 = new System.IO.Ports.SerialPort(this.components);
            this.LightControl3 = new System.IO.Ports.SerialPort(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.LightControl4 = new System.IO.Ports.SerialPort(this.components);
            this.tlpUnder.SuspendLayout();
            this.tableLayoutPanel32.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_LightNumber)).BeginInit();
            this.tlpTopSide.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.Main_TabControl.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel27.SuspendLayout();
            this.tableLayoutPanel30.SuspendLayout();
            this.tableLayoutPanel29.SuspendLayout();
            this.tableLayoutPanel28.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpUnder
            // 
            this.tlpUnder.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpUnder.ColumnCount = 12;
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.49103F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.300186F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.49034F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.301922F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.302063F));
            this.tlpUnder.Controls.Add(this.btn_SystemSetup, 11, 0);
            this.tlpUnder.Controls.Add(this.btn_Model, 8, 0);
            this.tlpUnder.Controls.Add(this.btn_CamSet, 7, 0);
            this.tlpUnder.Controls.Add(this.btn_Analyze, 10, 0);
            this.tlpUnder.Controls.Add(this.tableLayoutPanel32, 0, 0);
            this.tlpUnder.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.tlpUnder.Controls.Add(this.btn_ToolSetUp, 5, 0);
            this.tlpUnder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpUnder.Location = new System.Drawing.Point(0, 991);
            this.tlpUnder.Name = "tlpUnder";
            this.tlpUnder.RowCount = 1;
            this.tlpUnder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpUnder.Size = new System.Drawing.Size(1920, 89);
            this.tlpUnder.TabIndex = 1;
            // 
            // btn_SystemSetup
            // 
            this.btn_SystemSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_SystemSetup.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SystemSetup.Image = global::VISION.Properties.Resources.Setting;
            this.btn_SystemSetup.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SystemSetup.Location = new System.Drawing.Point(1777, 1);
            this.btn_SystemSetup.Margin = new System.Windows.Forms.Padding(0);
            this.btn_SystemSetup.Name = "btn_SystemSetup";
            this.btn_SystemSetup.Size = new System.Drawing.Size(142, 87);
            this.btn_SystemSetup.TabIndex = 0;
            this.btn_SystemSetup.Text = "System Setting";
            this.btn_SystemSetup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_SystemSetup.UseVisualStyleBackColor = true;
            this.btn_SystemSetup.Click += new System.EventHandler(this.btn_SystemSetup_Click);
            // 
            // btn_Model
            // 
            this.btn_Model.BackColor = System.Drawing.Color.Transparent;
            this.btn_Model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Model.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Model.Image = global::VISION.Properties.Resources.JobOpen;
            this.btn_Model.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Model.Location = new System.Drawing.Point(1357, 1);
            this.btn_Model.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Model.Name = "btn_Model";
            this.btn_Model.Size = new System.Drawing.Size(139, 87);
            this.btn_Model.TabIndex = 2;
            this.btn_Model.Tag = "";
            this.btn_Model.Text = "Model";
            this.btn_Model.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Model.UseVisualStyleBackColor = false;
            this.btn_Model.Click += new System.EventHandler(this.btn_Model_Click);
            // 
            // btn_CamSet
            // 
            this.btn_CamSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_CamSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CamSet.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_CamSet.Image = global::VISION.Properties.Resources.OneShot;
            this.btn_CamSet.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_CamSet.Location = new System.Drawing.Point(1217, 1);
            this.btn_CamSet.Margin = new System.Windows.Forms.Padding(0);
            this.btn_CamSet.Name = "btn_CamSet";
            this.btn_CamSet.Size = new System.Drawing.Size(139, 87);
            this.btn_CamSet.TabIndex = 51;
            this.btn_CamSet.Tag = "";
            this.btn_CamSet.Text = "Camera Setting";
            this.btn_CamSet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_CamSet.UseVisualStyleBackColor = true;
            this.btn_CamSet.Visible = false;
            this.btn_CamSet.Click += new System.EventHandler(this.btn_CamList_Click);
            // 
            // btn_Analyze
            // 
            this.btn_Analyze.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Analyze.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Analyze.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Analyze.Image = global::VISION.Properties.Resources.Analyze;
            this.btn_Analyze.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Analyze.Location = new System.Drawing.Point(1637, 1);
            this.btn_Analyze.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analyze.Name = "btn_Analyze";
            this.btn_Analyze.Size = new System.Drawing.Size(139, 87);
            this.btn_Analyze.TabIndex = 52;
            this.btn_Analyze.Tag = "";
            this.btn_Analyze.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Analyze.UseVisualStyleBackColor = true;
            this.btn_Analyze.Click += new System.EventHandler(this.btn_Analyze_Click);
            // 
            // tableLayoutPanel32
            // 
            this.tableLayoutPanel32.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel32.ColumnCount = 2;
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel32.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel32.Controls.Add(this.cb_ResultOK, 1, 0);
            this.tableLayoutPanel32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel32.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel32.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel32.Name = "tableLayoutPanel32";
            this.tableLayoutPanel32.RowCount = 1;
            this.tableLayoutPanel32.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel32.Size = new System.Drawing.Size(257, 87);
            this.tableLayoutPanel32.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(4, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 85);
            this.label10.TabIndex = 0;
            this.label10.Text = "상시 OK";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_ResultOK
            // 
            this.cb_ResultOK.AutoSize = true;
            this.cb_ResultOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_ResultOK.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_ResultOK.ForeColor = System.Drawing.Color.Red;
            this.cb_ResultOK.Location = new System.Drawing.Point(132, 4);
            this.cb_ResultOK.Name = "cb_ResultOK";
            this.cb_ResultOK.Size = new System.Drawing.Size(121, 79);
            this.cb_ResultOK.TabIndex = 1;
            this.cb_ResultOK.Text = "UNUSED";
            this.cb_ResultOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_ResultOK.UseVisualStyleBackColor = true;
            this.cb_ResultOK.CheckedChanged += new System.EventHandler(this.cb_ResultOK_CheckedChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.num_LightNumber, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.Btn_LightOnOff, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(399, 1);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(257, 87);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // num_LightNumber
            // 
            this.num_LightNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.num_LightNumber.Font = new System.Drawing.Font("Consolas", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_LightNumber.Location = new System.Drawing.Point(4, 4);
            this.num_LightNumber.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.num_LightNumber.Name = "num_LightNumber";
            this.num_LightNumber.Size = new System.Drawing.Size(121, 70);
            this.num_LightNumber.TabIndex = 54;
            this.num_LightNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_LightNumber.ValueChanged += new System.EventHandler(this.num_LightNumber_ValueChanged);
            // 
            // Btn_LightOnOff
            // 
            this.Btn_LightOnOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_LightOnOff.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_LightOnOff.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Btn_LightOnOff.Location = new System.Drawing.Point(129, 1);
            this.Btn_LightOnOff.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_LightOnOff.Name = "Btn_LightOnOff";
            this.Btn_LightOnOff.Size = new System.Drawing.Size(127, 85);
            this.Btn_LightOnOff.TabIndex = 53;
            this.Btn_LightOnOff.Tag = "Front";
            this.Btn_LightOnOff.Text = "조명 ON / OFF";
            this.Btn_LightOnOff.UseVisualStyleBackColor = true;
            this.Btn_LightOnOff.Click += new System.EventHandler(this.Btn_LightOnOff_Click);
            // 
            // btn_ToolSetUp
            // 
            this.btn_ToolSetUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_ToolSetUp.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ToolSetUp.Image = global::VISION.Properties.Resources.Setup;
            this.btn_ToolSetUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_ToolSetUp.Location = new System.Drawing.Point(940, 4);
            this.btn_ToolSetUp.Name = "btn_ToolSetUp";
            this.btn_ToolSetUp.Size = new System.Drawing.Size(133, 81);
            this.btn_ToolSetUp.TabIndex = 56;
            this.btn_ToolSetUp.Tag = "Front";
            this.btn_ToolSetUp.Text = "Tool Setting";
            this.btn_ToolSetUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_ToolSetUp.UseVisualStyleBackColor = true;
            this.btn_ToolSetUp.Click += new System.EventHandler(this.btn_ToolSetUp_Click);
            // 
            // tlpTopSide
            // 
            this.tlpTopSide.ColumnCount = 7;
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.65755F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.65533F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.14597F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.53649F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.669042F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.669042F));
            this.tlpTopSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.666586F));
            this.tlpTopSide.Controls.Add(this.lb_CurruntModelName, 3, 0);
            this.tlpTopSide.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tlpTopSide.Controls.Add(this.btn_Exit, 6, 0);
            this.tlpTopSide.Controls.Add(this.btn_Stop, 5, 0);
            this.tlpTopSide.Controls.Add(this.btn_Status, 4, 0);
            this.tlpTopSide.Controls.Add(this.pictureBox1, 0, 0);
            this.tlpTopSide.Controls.Add(this.label1, 2, 0);
            this.tlpTopSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpTopSide.Location = new System.Drawing.Point(0, 0);
            this.tlpTopSide.Name = "tlpTopSide";
            this.tlpTopSide.RowCount = 1;
            this.tlpTopSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tlpTopSide.Size = new System.Drawing.Size(1920, 77);
            this.tlpTopSide.TabIndex = 2;
            // 
            // lb_CurruntModelName
            // 
            this.lb_CurruntModelName.AutoSize = true;
            this.lb_CurruntModelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lb_CurruntModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CurruntModelName.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CurruntModelName.Location = new System.Drawing.Point(1336, 0);
            this.lb_CurruntModelName.Name = "lb_CurruntModelName";
            this.lb_CurruntModelName.Size = new System.Drawing.Size(196, 77);
            this.lb_CurruntModelName.TabIndex = 1;
            this.lb_CurruntModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lb_Time, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_Ver, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(265, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 71);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lb_Time
            // 
            this.lb_Time.AutoSize = true;
            this.lb_Time.BackColor = System.Drawing.Color.Black;
            this.lb_Time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Time.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Time.ForeColor = System.Drawing.Color.White;
            this.lb_Time.Location = new System.Drawing.Point(3, 0);
            this.lb_Time.Name = "lb_Time";
            this.lb_Time.Size = new System.Drawing.Size(250, 35);
            this.lb_Time.TabIndex = 0;
            this.lb_Time.Text = "0000-00-00 오전 00:00:00";
            this.lb_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_Ver
            // 
            this.lb_Ver.AutoSize = true;
            this.lb_Ver.BackColor = System.Drawing.Color.Black;
            this.lb_Ver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Ver.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Ver.ForeColor = System.Drawing.Color.White;
            this.lb_Ver.Location = new System.Drawing.Point(3, 35);
            this.lb_Ver.Name = "lb_Ver";
            this.lb_Ver.Size = new System.Drawing.Size(250, 36);
            this.lb_Ver.TabIndex = 4;
            this.lb_Ver.Text = "Ver. 1.0.0";
            this.lb_Ver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Exit
            // 
            this.btn_Exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Exit.Image = global::VISION.Properties.Resources.Close;
            this.btn_Exit.Location = new System.Drawing.Point(1794, 3);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(123, 71);
            this.btn_Exit.TabIndex = 0;
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Stop.Image = global::VISION.Properties.Resources.Stop;
            this.btn_Stop.Location = new System.Drawing.Point(1666, 3);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(122, 71);
            this.btn_Stop.TabIndex = 9;
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Status
            // 
            this.btn_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Status.Image = global::VISION.Properties.Resources.Run;
            this.btn_Status.Location = new System.Drawing.Point(1538, 3);
            this.btn_Status.Name = "btn_Status";
            this.btn_Status.Size = new System.Drawing.Size(122, 71);
            this.btn_Status.TabIndex = 5;
            this.btn_Status.UseVisualStyleBackColor = true;
            this.btn_Status.Click += new System.EventHandler(this.btn_Status_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::VISION.Properties.Resources.logo1;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(527, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(803, 77);
            this.label1.TabIndex = 11;
            this.label1.Text = "HeatSink Vision Program";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_Setting
            // 
            this.timer_Setting.Tick += new System.EventHandler(this.timer_Setting_Tick);
            // 
            // LightControl1
            // 
            this.LightControl1.BaudRate = 19200;
            this.LightControl1.PortName = "COM6";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(274, 882);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "I/O";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.DimGray;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT11, 0, 12);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT8, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT9, 1, 10);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT6, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT7, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT4, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT5, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT2, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT3, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT0, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT1, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT10, 1, 11);
            this.tableLayoutPanel3.Controls.Add(this.btn_OUTPUT11, 1, 12);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT7, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT6, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT5, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT4, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT3, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT2, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT0, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT8, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT9, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.btn_INPUT10, 0, 11);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 13;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692545F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691777F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691777F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691777F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691777F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(268, 876);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btn_INPUT11
            // 
            this.btn_INPUT11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT11.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT11.Location = new System.Drawing.Point(3, 807);
            this.btn_INPUT11.Name = "btn_INPUT11";
            this.btn_INPUT11.Size = new System.Drawing.Size(128, 66);
            this.btn_INPUT11.TabIndex = 47;
            this.btn_INPUT11.Tag = "7";
            this.btn_INPUT11.Text = "INPUT11";
            this.btn_INPUT11.UseVisualStyleBackColor = true;
            // 
            // btn_OUTPUT8
            // 
            this.btn_OUTPUT8.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT8.AutoSize = true;
            this.btn_OUTPUT8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT8.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT8.Location = new System.Drawing.Point(137, 606);
            this.btn_OUTPUT8.Name = "btn_OUTPUT8";
            this.btn_OUTPUT8.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT8.TabIndex = 43;
            this.btn_OUTPUT8.Tag = "8";
            this.btn_OUTPUT8.Text = "Cam 5 OK";
            this.btn_OUTPUT8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT8.UseVisualStyleBackColor = true;
            this.btn_OUTPUT8.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT9
            // 
            this.btn_OUTPUT9.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT9.AutoSize = true;
            this.btn_OUTPUT9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT9.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT9.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT9.Location = new System.Drawing.Point(137, 673);
            this.btn_OUTPUT9.Name = "btn_OUTPUT9";
            this.btn_OUTPUT9.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT9.TabIndex = 42;
            this.btn_OUTPUT9.Tag = "9";
            this.btn_OUTPUT9.Text = "Cam 5 NG";
            this.btn_OUTPUT9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT9.UseVisualStyleBackColor = true;
            this.btn_OUTPUT9.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT6
            // 
            this.btn_OUTPUT6.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT6.AutoSize = true;
            this.btn_OUTPUT6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT6.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT6.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT6.Location = new System.Drawing.Point(137, 472);
            this.btn_OUTPUT6.Name = "btn_OUTPUT6";
            this.btn_OUTPUT6.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT6.TabIndex = 41;
            this.btn_OUTPUT6.Tag = "6";
            this.btn_OUTPUT6.Text = "Cam 4 OK";
            this.btn_OUTPUT6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT6.UseVisualStyleBackColor = true;
            this.btn_OUTPUT6.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT7
            // 
            this.btn_OUTPUT7.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT7.AutoSize = true;
            this.btn_OUTPUT7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT7.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT7.Location = new System.Drawing.Point(137, 539);
            this.btn_OUTPUT7.Name = "btn_OUTPUT7";
            this.btn_OUTPUT7.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT7.TabIndex = 40;
            this.btn_OUTPUT7.Tag = "7";
            this.btn_OUTPUT7.Text = "Cam 4 NG";
            this.btn_OUTPUT7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT7.UseVisualStyleBackColor = true;
            this.btn_OUTPUT7.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT4
            // 
            this.btn_OUTPUT4.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT4.AutoSize = true;
            this.btn_OUTPUT4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT4.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT4.Location = new System.Drawing.Point(137, 338);
            this.btn_OUTPUT4.Name = "btn_OUTPUT4";
            this.btn_OUTPUT4.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT4.TabIndex = 39;
            this.btn_OUTPUT4.Tag = "4";
            this.btn_OUTPUT4.Text = "Cam 3 OK";
            this.btn_OUTPUT4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT4.UseVisualStyleBackColor = true;
            this.btn_OUTPUT4.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT5
            // 
            this.btn_OUTPUT5.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT5.AutoSize = true;
            this.btn_OUTPUT5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT5.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT5.Location = new System.Drawing.Point(137, 405);
            this.btn_OUTPUT5.Name = "btn_OUTPUT5";
            this.btn_OUTPUT5.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT5.TabIndex = 38;
            this.btn_OUTPUT5.Tag = "5";
            this.btn_OUTPUT5.Text = "Cam 3 NG";
            this.btn_OUTPUT5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT5.UseVisualStyleBackColor = true;
            this.btn_OUTPUT5.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT2
            // 
            this.btn_OUTPUT2.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT2.AutoSize = true;
            this.btn_OUTPUT2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT2.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT2.Location = new System.Drawing.Point(137, 204);
            this.btn_OUTPUT2.Name = "btn_OUTPUT2";
            this.btn_OUTPUT2.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT2.TabIndex = 37;
            this.btn_OUTPUT2.Tag = "2";
            this.btn_OUTPUT2.Text = "Cam 2 OK";
            this.btn_OUTPUT2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT2.UseVisualStyleBackColor = true;
            this.btn_OUTPUT2.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT3
            // 
            this.btn_OUTPUT3.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT3.AutoSize = true;
            this.btn_OUTPUT3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT3.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT3.Location = new System.Drawing.Point(137, 271);
            this.btn_OUTPUT3.Name = "btn_OUTPUT3";
            this.btn_OUTPUT3.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT3.TabIndex = 36;
            this.btn_OUTPUT3.Tag = "3";
            this.btn_OUTPUT3.Text = "Cam 2 NG";
            this.btn_OUTPUT3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT3.UseVisualStyleBackColor = true;
            this.btn_OUTPUT3.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT0
            // 
            this.btn_OUTPUT0.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT0.AutoSize = true;
            this.btn_OUTPUT0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT0.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT0.Location = new System.Drawing.Point(137, 70);
            this.btn_OUTPUT0.Name = "btn_OUTPUT0";
            this.btn_OUTPUT0.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT0.TabIndex = 37;
            this.btn_OUTPUT0.Tag = "0";
            this.btn_OUTPUT0.Text = "Cam 1 OK";
            this.btn_OUTPUT0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT0.UseVisualStyleBackColor = true;
            this.btn_OUTPUT0.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT1
            // 
            this.btn_OUTPUT1.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT1.AutoSize = true;
            this.btn_OUTPUT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT1.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT1.Location = new System.Drawing.Point(137, 137);
            this.btn_OUTPUT1.Name = "btn_OUTPUT1";
            this.btn_OUTPUT1.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT1.TabIndex = 36;
            this.btn_OUTPUT1.Tag = "1";
            this.btn_OUTPUT1.Text = "Cam 1 NG";
            this.btn_OUTPUT1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT1.UseVisualStyleBackColor = true;
            this.btn_OUTPUT1.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT10
            // 
            this.btn_OUTPUT10.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT10.AutoSize = true;
            this.btn_OUTPUT10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT10.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT10.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT10.Location = new System.Drawing.Point(137, 740);
            this.btn_OUTPUT10.Name = "btn_OUTPUT10";
            this.btn_OUTPUT10.Size = new System.Drawing.Size(128, 61);
            this.btn_OUTPUT10.TabIndex = 35;
            this.btn_OUTPUT10.Tag = "10";
            this.btn_OUTPUT10.Text = "Cam 6 OK";
            this.btn_OUTPUT10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT10.UseVisualStyleBackColor = true;
            this.btn_OUTPUT10.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_OUTPUT11
            // 
            this.btn_OUTPUT11.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_OUTPUT11.AutoSize = true;
            this.btn_OUTPUT11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_OUTPUT11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OUTPUT11.ForeColor = System.Drawing.Color.Black;
            this.btn_OUTPUT11.Location = new System.Drawing.Point(137, 807);
            this.btn_OUTPUT11.Name = "btn_OUTPUT11";
            this.btn_OUTPUT11.Size = new System.Drawing.Size(128, 66);
            this.btn_OUTPUT11.TabIndex = 34;
            this.btn_OUTPUT11.Tag = "11";
            this.btn_OUTPUT11.Text = "Cam 6 NG";
            this.btn_OUTPUT11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_OUTPUT11.UseVisualStyleBackColor = true;
            this.btn_OUTPUT11.CheckedChanged += new System.EventHandler(this.btn_OUTPUT0_CheckedChanged);
            // 
            // btn_INPUT7
            // 
            this.btn_INPUT7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT7.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT7.Location = new System.Drawing.Point(3, 539);
            this.btn_INPUT7.Name = "btn_INPUT7";
            this.btn_INPUT7.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT7.TabIndex = 18;
            this.btn_INPUT7.Tag = "7";
            this.btn_INPUT7.Text = "Cam 6 Trigger";
            this.btn_INPUT7.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT6
            // 
            this.btn_INPUT6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT6.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT6.Location = new System.Drawing.Point(3, 472);
            this.btn_INPUT6.Name = "btn_INPUT6";
            this.btn_INPUT6.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT6.TabIndex = 14;
            this.btn_INPUT6.Tag = "6";
            this.btn_INPUT6.Text = "Cam 5 Trigger";
            this.btn_INPUT6.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT5
            // 
            this.btn_INPUT5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT5.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT5.Location = new System.Drawing.Point(3, 405);
            this.btn_INPUT5.Name = "btn_INPUT5";
            this.btn_INPUT5.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT5.TabIndex = 12;
            this.btn_INPUT5.Tag = "5";
            this.btn_INPUT5.Text = "Cam 5 Trigger";
            this.btn_INPUT5.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT4
            // 
            this.btn_INPUT4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT4.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT4.Location = new System.Drawing.Point(3, 338);
            this.btn_INPUT4.Name = "btn_INPUT4";
            this.btn_INPUT4.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT4.TabIndex = 10;
            this.btn_INPUT4.Tag = "4";
            this.btn_INPUT4.Text = "Cam 4 Trigger";
            this.btn_INPUT4.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT3
            // 
            this.btn_INPUT3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT3.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT3.Location = new System.Drawing.Point(3, 271);
            this.btn_INPUT3.Name = "btn_INPUT3";
            this.btn_INPUT3.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT3.TabIndex = 8;
            this.btn_INPUT3.Tag = "3";
            this.btn_INPUT3.Text = "Cam 4 Trigger";
            this.btn_INPUT3.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT2
            // 
            this.btn_INPUT2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT2.Location = new System.Drawing.Point(3, 204);
            this.btn_INPUT2.Name = "btn_INPUT2";
            this.btn_INPUT2.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT2.TabIndex = 6;
            this.btn_INPUT2.Tag = "2";
            this.btn_INPUT2.Text = "Cam 4 Trigger";
            this.btn_INPUT2.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT1
            // 
            this.btn_INPUT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT1.Location = new System.Drawing.Point(3, 137);
            this.btn_INPUT1.Name = "btn_INPUT1";
            this.btn_INPUT1.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT1.TabIndex = 4;
            this.btn_INPUT1.Tag = "1";
            this.btn_INPUT1.Text = "Cam 2 / Cam 3 Trigger";
            this.btn_INPUT1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 67);
            this.label2.TabIndex = 0;
            this.label2.Text = "INPUT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(137, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 67);
            this.label3.TabIndex = 1;
            this.label3.Text = "OUTPUT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_INPUT0
            // 
            this.btn_INPUT0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT0.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT0.Location = new System.Drawing.Point(3, 70);
            this.btn_INPUT0.Name = "btn_INPUT0";
            this.btn_INPUT0.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT0.TabIndex = 2;
            this.btn_INPUT0.Tag = "0";
            this.btn_INPUT0.Text = "Cam 1 Trigger";
            this.btn_INPUT0.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT8
            // 
            this.btn_INPUT8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT8.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT8.Location = new System.Drawing.Point(3, 606);
            this.btn_INPUT8.Name = "btn_INPUT8";
            this.btn_INPUT8.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT8.TabIndex = 44;
            this.btn_INPUT8.Tag = "7";
            this.btn_INPUT8.Text = "INPUT8";
            this.btn_INPUT8.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT9
            // 
            this.btn_INPUT9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT9.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT9.Location = new System.Drawing.Point(3, 673);
            this.btn_INPUT9.Name = "btn_INPUT9";
            this.btn_INPUT9.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT9.TabIndex = 45;
            this.btn_INPUT9.Tag = "7";
            this.btn_INPUT9.Text = "INPUT9";
            this.btn_INPUT9.UseVisualStyleBackColor = true;
            // 
            // btn_INPUT10
            // 
            this.btn_INPUT10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_INPUT10.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_INPUT10.Location = new System.Drawing.Point(3, 740);
            this.btn_INPUT10.Name = "btn_INPUT10";
            this.btn_INPUT10.Size = new System.Drawing.Size(128, 61);
            this.btn_INPUT10.TabIndex = 46;
            this.btn_INPUT10.Tag = "7";
            this.btn_INPUT10.Text = "INPUT10";
            this.btn_INPUT10.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.logControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(274, 882);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "로그";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // logControl1
            // 
            this.logControl1.Color_ControlBack = System.Drawing.Color.Black;
            this.logControl1.Color_ErrorText = System.Drawing.Color.Red;
            this.logControl1.Color_NormalText = System.Drawing.Color.White;
            this.logControl1.Color_WarningText = System.Drawing.Color.Yellow;
            this.logControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControl1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.logControl1.Location = new System.Drawing.Point(0, 0);
            this.logControl1.Margin = new System.Windows.Forms.Padding(0);
            this.logControl1.Name = "logControl1";
            this.logControl1.Size = new System.Drawing.Size(274, 882);
            this.logControl1.TabIndex = 42;
            // 
            // Main_TabControl
            // 
            this.Main_TabControl.Controls.Add(this.tabPage1);
            this.Main_TabControl.Controls.Add(this.tabPage2);
            this.Main_TabControl.Controls.Add(this.tabPage5);
            this.Main_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_TabControl.Location = new System.Drawing.Point(1635, 3);
            this.Main_TabControl.Name = "Main_TabControl";
            this.Main_TabControl.Padding = new System.Drawing.Point(0, 0);
            this.Main_TabControl.SelectedIndex = 0;
            this.Main_TabControl.Size = new System.Drawing.Size(282, 908);
            this.Main_TabControl.TabIndex = 25;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel27);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(274, 882);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "수량";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel27.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel27.ColumnCount = 2;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel30, 1, 5);
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel29, 1, 4);
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel28, 1, 3);
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel18, 1, 2);
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel17, 1, 1);
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel16, 1, 0);
            this.tableLayoutPanel27.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel27.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel27.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel27.Controls.Add(this.label12, 0, 3);
            this.tableLayoutPanel27.Controls.Add(this.label14, 0, 4);
            this.tableLayoutPanel27.Controls.Add(this.label16, 0, 5);
            this.tableLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel27.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 6;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(268, 876);
            this.tableLayoutPanel27.TabIndex = 0;
            // 
            // tableLayoutPanel30
            // 
            this.tableLayoutPanel30.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel30.ColumnCount = 2;
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel30.Controls.Add(this.lb_CAM6_NGRATE, 1, 3);
            this.tableLayoutPanel30.Controls.Add(this.label59, 0, 3);
            this.tableLayoutPanel30.Controls.Add(this.lb_CAM6_TOTAL, 1, 2);
            this.tableLayoutPanel30.Controls.Add(this.label61, 0, 2);
            this.tableLayoutPanel30.Controls.Add(this.lb_CAM6_NG, 1, 1);
            this.tableLayoutPanel30.Controls.Add(this.label63, 0, 1);
            this.tableLayoutPanel30.Controls.Add(this.lb_CAM6_OK, 1, 0);
            this.tableLayoutPanel30.Controls.Add(this.label65, 0, 0);
            this.tableLayoutPanel30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel30.Location = new System.Drawing.Point(58, 729);
            this.tableLayoutPanel30.Name = "tableLayoutPanel30";
            this.tableLayoutPanel30.RowCount = 4;
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel30.Size = new System.Drawing.Size(206, 143);
            this.tableLayoutPanel30.TabIndex = 11;
            // 
            // lb_CAM6_NGRATE
            // 
            this.lb_CAM6_NGRATE.AutoSize = true;
            this.lb_CAM6_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM6_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM6_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM6_NGRATE.Location = new System.Drawing.Point(65, 106);
            this.lb_CAM6_NGRATE.Name = "lb_CAM6_NGRATE";
            this.lb_CAM6_NGRATE.Size = new System.Drawing.Size(137, 36);
            this.lb_CAM6_NGRATE.TabIndex = 7;
            this.lb_CAM6_NGRATE.Text = "0";
            this.lb_CAM6_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label59.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label59.ForeColor = System.Drawing.Color.Yellow;
            this.label59.Location = new System.Drawing.Point(4, 106);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(54, 36);
            this.label59.TabIndex = 6;
            this.label59.Text = "NG RATE";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM6_TOTAL
            // 
            this.lb_CAM6_TOTAL.AutoSize = true;
            this.lb_CAM6_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM6_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM6_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM6_TOTAL.Location = new System.Drawing.Point(65, 71);
            this.lb_CAM6_TOTAL.Name = "lb_CAM6_TOTAL";
            this.lb_CAM6_TOTAL.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM6_TOTAL.TabIndex = 5;
            this.lb_CAM6_TOTAL.Text = "0";
            this.lb_CAM6_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label61.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(4, 71);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(54, 34);
            this.label61.TabIndex = 4;
            this.label61.Text = "TOTAL";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM6_NG
            // 
            this.lb_CAM6_NG.AutoSize = true;
            this.lb_CAM6_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM6_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM6_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM6_NG.Location = new System.Drawing.Point(65, 36);
            this.lb_CAM6_NG.Name = "lb_CAM6_NG";
            this.lb_CAM6_NG.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM6_NG.TabIndex = 3;
            this.lb_CAM6_NG.Text = "0";
            this.lb_CAM6_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label63.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label63.ForeColor = System.Drawing.Color.Red;
            this.label63.Location = new System.Drawing.Point(4, 36);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(54, 34);
            this.label63.TabIndex = 2;
            this.label63.Text = "NG";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM6_OK
            // 
            this.lb_CAM6_OK.AutoSize = true;
            this.lb_CAM6_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM6_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM6_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM6_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM6_OK.Name = "lb_CAM6_OK";
            this.lb_CAM6_OK.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM6_OK.TabIndex = 1;
            this.lb_CAM6_OK.Text = "0";
            this.lb_CAM6_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label65.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label65.ForeColor = System.Drawing.Color.Lime;
            this.label65.Location = new System.Drawing.Point(4, 1);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(54, 34);
            this.label65.TabIndex = 0;
            this.label65.Text = "OK";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel29
            // 
            this.tableLayoutPanel29.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel29.ColumnCount = 2;
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel29.Controls.Add(this.lb_CAM5_NGRATE, 1, 3);
            this.tableLayoutPanel29.Controls.Add(this.label51, 0, 3);
            this.tableLayoutPanel29.Controls.Add(this.lb_CAM5_TOTAL, 1, 2);
            this.tableLayoutPanel29.Controls.Add(this.label53, 0, 2);
            this.tableLayoutPanel29.Controls.Add(this.lb_CAM5_NG, 1, 1);
            this.tableLayoutPanel29.Controls.Add(this.label55, 0, 1);
            this.tableLayoutPanel29.Controls.Add(this.lb_CAM5_OK, 1, 0);
            this.tableLayoutPanel29.Controls.Add(this.label57, 0, 0);
            this.tableLayoutPanel29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel29.Location = new System.Drawing.Point(58, 584);
            this.tableLayoutPanel29.Name = "tableLayoutPanel29";
            this.tableLayoutPanel29.RowCount = 4;
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel29.Size = new System.Drawing.Size(206, 138);
            this.tableLayoutPanel29.TabIndex = 10;
            // 
            // lb_CAM5_NGRATE
            // 
            this.lb_CAM5_NGRATE.AutoSize = true;
            this.lb_CAM5_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_CAM5_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM5_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM5_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM5_NGRATE.Location = new System.Drawing.Point(65, 103);
            this.lb_CAM5_NGRATE.Name = "lb_CAM5_NGRATE";
            this.lb_CAM5_NGRATE.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM5_NGRATE.TabIndex = 7;
            this.lb_CAM5_NGRATE.Text = "0";
            this.lb_CAM5_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.BackColor = System.Drawing.Color.Black;
            this.label51.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label51.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label51.ForeColor = System.Drawing.Color.Yellow;
            this.label51.Location = new System.Drawing.Point(4, 103);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(54, 34);
            this.label51.TabIndex = 6;
            this.label51.Text = "NG RATE";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM5_TOTAL
            // 
            this.lb_CAM5_TOTAL.AutoSize = true;
            this.lb_CAM5_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_CAM5_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM5_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM5_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM5_TOTAL.Location = new System.Drawing.Point(65, 69);
            this.lb_CAM5_TOTAL.Name = "lb_CAM5_TOTAL";
            this.lb_CAM5_TOTAL.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM5_TOTAL.TabIndex = 5;
            this.lb_CAM5_TOTAL.Text = "0";
            this.lb_CAM5_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.BackColor = System.Drawing.Color.Black;
            this.label53.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label53.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label53.ForeColor = System.Drawing.Color.White;
            this.label53.Location = new System.Drawing.Point(4, 69);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(54, 33);
            this.label53.TabIndex = 4;
            this.label53.Text = "TOTAL";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM5_NG
            // 
            this.lb_CAM5_NG.AutoSize = true;
            this.lb_CAM5_NG.BackColor = System.Drawing.Color.Black;
            this.lb_CAM5_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM5_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM5_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM5_NG.Location = new System.Drawing.Point(65, 35);
            this.lb_CAM5_NG.Name = "lb_CAM5_NG";
            this.lb_CAM5_NG.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM5_NG.TabIndex = 3;
            this.lb_CAM5_NG.Text = "0";
            this.lb_CAM5_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.BackColor = System.Drawing.Color.Black;
            this.label55.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label55.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label55.ForeColor = System.Drawing.Color.Red;
            this.label55.Location = new System.Drawing.Point(4, 35);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(54, 33);
            this.label55.TabIndex = 2;
            this.label55.Text = "NG";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM5_OK
            // 
            this.lb_CAM5_OK.AutoSize = true;
            this.lb_CAM5_OK.BackColor = System.Drawing.Color.Black;
            this.lb_CAM5_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM5_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM5_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM5_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM5_OK.Name = "lb_CAM5_OK";
            this.lb_CAM5_OK.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM5_OK.TabIndex = 1;
            this.lb_CAM5_OK.Text = "0";
            this.lb_CAM5_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.BackColor = System.Drawing.Color.Black;
            this.label57.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label57.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label57.ForeColor = System.Drawing.Color.Lime;
            this.label57.Location = new System.Drawing.Point(4, 1);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(54, 33);
            this.label57.TabIndex = 0;
            this.label57.Text = "OK";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel28
            // 
            this.tableLayoutPanel28.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel28.ColumnCount = 2;
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel28.Controls.Add(this.lb_CAM4_NGRATE, 1, 3);
            this.tableLayoutPanel28.Controls.Add(this.label43, 0, 3);
            this.tableLayoutPanel28.Controls.Add(this.lb_CAM4_TOTAL, 1, 2);
            this.tableLayoutPanel28.Controls.Add(this.label45, 0, 2);
            this.tableLayoutPanel28.Controls.Add(this.lb_CAM4_NG, 1, 1);
            this.tableLayoutPanel28.Controls.Add(this.label47, 0, 1);
            this.tableLayoutPanel28.Controls.Add(this.lb_CAM4_OK, 1, 0);
            this.tableLayoutPanel28.Controls.Add(this.label49, 0, 0);
            this.tableLayoutPanel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel28.Location = new System.Drawing.Point(58, 439);
            this.tableLayoutPanel28.Name = "tableLayoutPanel28";
            this.tableLayoutPanel28.RowCount = 4;
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel28.Size = new System.Drawing.Size(206, 138);
            this.tableLayoutPanel28.TabIndex = 9;
            // 
            // lb_CAM4_NGRATE
            // 
            this.lb_CAM4_NGRATE.AutoSize = true;
            this.lb_CAM4_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_CAM4_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM4_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM4_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM4_NGRATE.Location = new System.Drawing.Point(65, 103);
            this.lb_CAM4_NGRATE.Name = "lb_CAM4_NGRATE";
            this.lb_CAM4_NGRATE.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM4_NGRATE.TabIndex = 7;
            this.lb_CAM4_NGRATE.Text = "0";
            this.lb_CAM4_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.BackColor = System.Drawing.Color.Black;
            this.label43.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label43.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label43.ForeColor = System.Drawing.Color.Yellow;
            this.label43.Location = new System.Drawing.Point(4, 103);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(54, 34);
            this.label43.TabIndex = 6;
            this.label43.Text = "NG RATE";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM4_TOTAL
            // 
            this.lb_CAM4_TOTAL.AutoSize = true;
            this.lb_CAM4_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_CAM4_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM4_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM4_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM4_TOTAL.Location = new System.Drawing.Point(65, 69);
            this.lb_CAM4_TOTAL.Name = "lb_CAM4_TOTAL";
            this.lb_CAM4_TOTAL.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM4_TOTAL.TabIndex = 5;
            this.lb_CAM4_TOTAL.Text = "0";
            this.lb_CAM4_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.BackColor = System.Drawing.Color.Black;
            this.label45.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label45.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label45.ForeColor = System.Drawing.Color.White;
            this.label45.Location = new System.Drawing.Point(4, 69);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(54, 33);
            this.label45.TabIndex = 4;
            this.label45.Text = "TOTAL";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM4_NG
            // 
            this.lb_CAM4_NG.AutoSize = true;
            this.lb_CAM4_NG.BackColor = System.Drawing.Color.Black;
            this.lb_CAM4_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM4_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM4_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM4_NG.Location = new System.Drawing.Point(65, 35);
            this.lb_CAM4_NG.Name = "lb_CAM4_NG";
            this.lb_CAM4_NG.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM4_NG.TabIndex = 3;
            this.lb_CAM4_NG.Text = "0";
            this.lb_CAM4_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.BackColor = System.Drawing.Color.Black;
            this.label47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label47.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label47.ForeColor = System.Drawing.Color.Red;
            this.label47.Location = new System.Drawing.Point(4, 35);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(54, 33);
            this.label47.TabIndex = 2;
            this.label47.Text = "NG";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM4_OK
            // 
            this.lb_CAM4_OK.AutoSize = true;
            this.lb_CAM4_OK.BackColor = System.Drawing.Color.Black;
            this.lb_CAM4_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM4_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM4_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM4_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM4_OK.Name = "lb_CAM4_OK";
            this.lb_CAM4_OK.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM4_OK.TabIndex = 1;
            this.lb_CAM4_OK.Text = "0";
            this.lb_CAM4_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.BackColor = System.Drawing.Color.Black;
            this.label49.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label49.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label49.ForeColor = System.Drawing.Color.Lime;
            this.label49.Location = new System.Drawing.Point(4, 1);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(54, 33);
            this.label49.TabIndex = 0;
            this.label49.Text = "OK";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel18.Controls.Add(this.lb_CAM3_NGRATE, 1, 3);
            this.tableLayoutPanel18.Controls.Add(this.label35, 0, 3);
            this.tableLayoutPanel18.Controls.Add(this.lb_CAM3_TOTAL, 1, 2);
            this.tableLayoutPanel18.Controls.Add(this.label37, 0, 2);
            this.tableLayoutPanel18.Controls.Add(this.lb_CAM3_NG, 1, 1);
            this.tableLayoutPanel18.Controls.Add(this.label39, 0, 1);
            this.tableLayoutPanel18.Controls.Add(this.lb_CAM3_OK, 1, 0);
            this.tableLayoutPanel18.Controls.Add(this.label41, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(58, 294);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 4;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(206, 138);
            this.tableLayoutPanel18.TabIndex = 8;
            // 
            // lb_CAM3_NGRATE
            // 
            this.lb_CAM3_NGRATE.AutoSize = true;
            this.lb_CAM3_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_CAM3_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM3_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM3_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM3_NGRATE.Location = new System.Drawing.Point(65, 103);
            this.lb_CAM3_NGRATE.Name = "lb_CAM3_NGRATE";
            this.lb_CAM3_NGRATE.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM3_NGRATE.TabIndex = 7;
            this.lb_CAM3_NGRATE.Text = "0";
            this.lb_CAM3_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Black;
            this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label35.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label35.ForeColor = System.Drawing.Color.Yellow;
            this.label35.Location = new System.Drawing.Point(4, 103);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(54, 34);
            this.label35.TabIndex = 6;
            this.label35.Text = "NG RATE";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM3_TOTAL
            // 
            this.lb_CAM3_TOTAL.AutoSize = true;
            this.lb_CAM3_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_CAM3_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM3_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM3_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM3_TOTAL.Location = new System.Drawing.Point(65, 69);
            this.lb_CAM3_TOTAL.Name = "lb_CAM3_TOTAL";
            this.lb_CAM3_TOTAL.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM3_TOTAL.TabIndex = 5;
            this.lb_CAM3_TOTAL.Text = "0";
            this.lb_CAM3_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Black;
            this.label37.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label37.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label37.ForeColor = System.Drawing.Color.White;
            this.label37.Location = new System.Drawing.Point(4, 69);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(54, 33);
            this.label37.TabIndex = 4;
            this.label37.Text = "TOTAL";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM3_NG
            // 
            this.lb_CAM3_NG.AutoSize = true;
            this.lb_CAM3_NG.BackColor = System.Drawing.Color.Black;
            this.lb_CAM3_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM3_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM3_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM3_NG.Location = new System.Drawing.Point(65, 35);
            this.lb_CAM3_NG.Name = "lb_CAM3_NG";
            this.lb_CAM3_NG.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM3_NG.TabIndex = 3;
            this.lb_CAM3_NG.Text = "0";
            this.lb_CAM3_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Black;
            this.label39.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label39.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label39.ForeColor = System.Drawing.Color.Red;
            this.label39.Location = new System.Drawing.Point(4, 35);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(54, 33);
            this.label39.TabIndex = 2;
            this.label39.Text = "NG";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM3_OK
            // 
            this.lb_CAM3_OK.AutoSize = true;
            this.lb_CAM3_OK.BackColor = System.Drawing.Color.Black;
            this.lb_CAM3_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM3_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM3_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM3_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM3_OK.Name = "lb_CAM3_OK";
            this.lb_CAM3_OK.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM3_OK.TabIndex = 1;
            this.lb_CAM3_OK.Text = "0";
            this.lb_CAM3_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.Black;
            this.label41.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label41.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label41.ForeColor = System.Drawing.Color.Lime;
            this.label41.Location = new System.Drawing.Point(4, 1);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(54, 33);
            this.label41.TabIndex = 0;
            this.label41.Text = "OK";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel17.ColumnCount = 2;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel17.Controls.Add(this.lb_CAM2_NGRATE, 1, 3);
            this.tableLayoutPanel17.Controls.Add(this.label27, 0, 3);
            this.tableLayoutPanel17.Controls.Add(this.lb_CAM2_TOTAL, 1, 2);
            this.tableLayoutPanel17.Controls.Add(this.label29, 0, 2);
            this.tableLayoutPanel17.Controls.Add(this.lb_CAM2_NG, 1, 1);
            this.tableLayoutPanel17.Controls.Add(this.label31, 0, 1);
            this.tableLayoutPanel17.Controls.Add(this.lb_CAM2_OK, 1, 0);
            this.tableLayoutPanel17.Controls.Add(this.label33, 0, 0);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(58, 149);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 4;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(206, 138);
            this.tableLayoutPanel17.TabIndex = 7;
            // 
            // lb_CAM2_NGRATE
            // 
            this.lb_CAM2_NGRATE.AutoSize = true;
            this.lb_CAM2_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_CAM2_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM2_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM2_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM2_NGRATE.Location = new System.Drawing.Point(65, 103);
            this.lb_CAM2_NGRATE.Name = "lb_CAM2_NGRATE";
            this.lb_CAM2_NGRATE.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM2_NGRATE.TabIndex = 7;
            this.lb_CAM2_NGRATE.Text = "0";
            this.lb_CAM2_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Black;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.ForeColor = System.Drawing.Color.Yellow;
            this.label27.Location = new System.Drawing.Point(4, 103);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(54, 34);
            this.label27.TabIndex = 6;
            this.label27.Text = "NG RATE";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM2_TOTAL
            // 
            this.lb_CAM2_TOTAL.AutoSize = true;
            this.lb_CAM2_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_CAM2_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM2_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM2_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM2_TOTAL.Location = new System.Drawing.Point(65, 69);
            this.lb_CAM2_TOTAL.Name = "lb_CAM2_TOTAL";
            this.lb_CAM2_TOTAL.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM2_TOTAL.TabIndex = 5;
            this.lb_CAM2_TOTAL.Text = "0";
            this.lb_CAM2_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.Black;
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.ForeColor = System.Drawing.Color.White;
            this.label29.Location = new System.Drawing.Point(4, 69);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(54, 33);
            this.label29.TabIndex = 4;
            this.label29.Text = "TOTAL";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM2_NG
            // 
            this.lb_CAM2_NG.AutoSize = true;
            this.lb_CAM2_NG.BackColor = System.Drawing.Color.Black;
            this.lb_CAM2_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM2_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM2_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM2_NG.Location = new System.Drawing.Point(65, 35);
            this.lb_CAM2_NG.Name = "lb_CAM2_NG";
            this.lb_CAM2_NG.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM2_NG.TabIndex = 3;
            this.lb_CAM2_NG.Text = "0";
            this.lb_CAM2_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Black;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label31.ForeColor = System.Drawing.Color.Red;
            this.label31.Location = new System.Drawing.Point(4, 35);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(54, 33);
            this.label31.TabIndex = 2;
            this.label31.Text = "NG";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM2_OK
            // 
            this.lb_CAM2_OK.AutoSize = true;
            this.lb_CAM2_OK.BackColor = System.Drawing.Color.Black;
            this.lb_CAM2_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM2_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM2_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM2_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM2_OK.Name = "lb_CAM2_OK";
            this.lb_CAM2_OK.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM2_OK.TabIndex = 1;
            this.lb_CAM2_OK.Text = "0";
            this.lb_CAM2_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Black;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label33.ForeColor = System.Drawing.Color.Lime;
            this.label33.Location = new System.Drawing.Point(4, 1);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(54, 33);
            this.label33.TabIndex = 0;
            this.label33.Text = "OK";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel16.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel16.Controls.Add(this.lb_CAM1_NGRATE, 1, 3);
            this.tableLayoutPanel16.Controls.Add(this.label24, 0, 3);
            this.tableLayoutPanel16.Controls.Add(this.lb_CAM1_TOTAL, 1, 2);
            this.tableLayoutPanel16.Controls.Add(this.label22, 0, 2);
            this.tableLayoutPanel16.Controls.Add(this.lb_CAM1_NG, 1, 1);
            this.tableLayoutPanel16.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel16.Controls.Add(this.lb_CAM1_OK, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(58, 4);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 4;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(206, 138);
            this.tableLayoutPanel16.TabIndex = 6;
            // 
            // lb_CAM1_NGRATE
            // 
            this.lb_CAM1_NGRATE.AutoSize = true;
            this.lb_CAM1_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_CAM1_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM1_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM1_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_CAM1_NGRATE.Location = new System.Drawing.Point(65, 103);
            this.lb_CAM1_NGRATE.Name = "lb_CAM1_NGRATE";
            this.lb_CAM1_NGRATE.Size = new System.Drawing.Size(137, 34);
            this.lb_CAM1_NGRATE.TabIndex = 7;
            this.lb_CAM1_NGRATE.Text = "0";
            this.lb_CAM1_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Black;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.ForeColor = System.Drawing.Color.Yellow;
            this.label24.Location = new System.Drawing.Point(4, 103);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(54, 34);
            this.label24.TabIndex = 6;
            this.label24.Text = "NG RATE";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM1_TOTAL
            // 
            this.lb_CAM1_TOTAL.AutoSize = true;
            this.lb_CAM1_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_CAM1_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM1_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM1_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_CAM1_TOTAL.Location = new System.Drawing.Point(65, 69);
            this.lb_CAM1_TOTAL.Name = "lb_CAM1_TOTAL";
            this.lb_CAM1_TOTAL.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM1_TOTAL.TabIndex = 5;
            this.lb_CAM1_TOTAL.Text = "0";
            this.lb_CAM1_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Black;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(4, 69);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(54, 33);
            this.label22.TabIndex = 4;
            this.label22.Text = "TOTAL";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM1_NG
            // 
            this.lb_CAM1_NG.AutoSize = true;
            this.lb_CAM1_NG.BackColor = System.Drawing.Color.Black;
            this.lb_CAM1_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM1_NG.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM1_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_CAM1_NG.Location = new System.Drawing.Point(65, 35);
            this.lb_CAM1_NG.Name = "lb_CAM1_NG";
            this.lb_CAM1_NG.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM1_NG.TabIndex = 3;
            this.lb_CAM1_NG.Text = "0";
            this.lb_CAM1_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Black;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(4, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(54, 33);
            this.label20.TabIndex = 2;
            this.label20.Text = "NG";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_CAM1_OK
            // 
            this.lb_CAM1_OK.AutoSize = true;
            this.lb_CAM1_OK.BackColor = System.Drawing.Color.Black;
            this.lb_CAM1_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CAM1_OK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_CAM1_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_CAM1_OK.Location = new System.Drawing.Point(65, 1);
            this.lb_CAM1_OK.Name = "lb_CAM1_OK";
            this.lb_CAM1_OK.Size = new System.Drawing.Size(137, 33);
            this.lb_CAM1_OK.TabIndex = 1;
            this.lb_CAM1_OK.Text = "0";
            this.lb_CAM1_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Black;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.ForeColor = System.Drawing.Color.Lime;
            this.label17.Location = new System.Drawing.Point(4, 1);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(54, 33);
            this.label17.TabIndex = 0;
            this.label17.Text = "OK";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 144);
            this.label5.TabIndex = 0;
            this.label5.Text = "CAM1";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(4, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 144);
            this.label6.TabIndex = 1;
            this.label6.Text = "CAM2";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Black;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(4, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 144);
            this.label8.TabIndex = 2;
            this.label8.Text = "CAM3";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Black;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(4, 436);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 144);
            this.label12.TabIndex = 3;
            this.label12.Text = "CAM4";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Black;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(4, 581);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 144);
            this.label14.TabIndex = 4;
            this.label14.Text = "CAM5";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Black;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(4, 726);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 149);
            this.label16.TabIndex = 5;
            this.label16.Text = "CAM6";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LightControl2
            // 
            this.LightControl2.BaudRate = 19200;
            this.LightControl2.PortName = "COM7";
            // 
            // LightControl3
            // 
            this.LightControl3.BaudRate = 19200;
            this.LightControl3.PortName = "COM8";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Controls.Add(this.MainPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Main_TabControl, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 77);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1920, 914);
            this.tableLayoutPanel2.TabIndex = 33;
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(3, 3);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1626, 908);
            this.MainPanel.TabIndex = 34;
            // 
            // LightControl4
            // 
            this.LightControl4.PortName = "COM9";
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tlpTopSide);
            this.Controls.Add(this.tlpUnder);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VISION PROGRAM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_Main_KeyDown);
            this.tlpUnder.ResumeLayout(false);
            this.tableLayoutPanel32.ResumeLayout(false);
            this.tableLayoutPanel32.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_LightNumber)).EndInit();
            this.tlpTopSide.ResumeLayout(false);
            this.tlpTopSide.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.Main_TabControl.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel27.ResumeLayout(false);
            this.tableLayoutPanel27.PerformLayout();
            this.tableLayoutPanel30.ResumeLayout(false);
            this.tableLayoutPanel30.PerformLayout();
            this.tableLayoutPanel29.ResumeLayout(false);
            this.tableLayoutPanel29.PerformLayout();
            this.tableLayoutPanel28.ResumeLayout(false);
            this.tableLayoutPanel28.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel18.PerformLayout();
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel17.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpUnder;
        private System.Windows.Forms.Button btn_SystemSetup;
        private System.Windows.Forms.TableLayoutPanel tlpTopSide;
        private System.Windows.Forms.Label lb_Ver;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label lb_Time;
        private System.Windows.Forms.Timer timer_Setting;
        private System.Windows.Forms.Button btn_Status;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Model;
        private System.Windows.Forms.Label lb_CurruntModelName;
        public System.IO.Ports.SerialPort LightControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_CamSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Button btn_INPUT6;
        public System.Windows.Forms.Button btn_INPUT5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl Main_TabControl;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel27;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel30;
        private System.Windows.Forms.Label lb_CAM6_NGRATE;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label lb_CAM6_TOTAL;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label lb_CAM6_NG;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label lb_CAM6_OK;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel29;
        private System.Windows.Forms.Label lb_CAM5_NGRATE;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label lb_CAM5_TOTAL;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label lb_CAM5_NG;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label lb_CAM5_OK;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel28;
        private System.Windows.Forms.Label lb_CAM4_NGRATE;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label lb_CAM4_TOTAL;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label lb_CAM4_NG;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label lb_CAM4_OK;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.Label lb_CAM3_NGRATE;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label lb_CAM3_TOTAL;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label lb_CAM3_NG;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label lb_CAM3_OK;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
        private System.Windows.Forms.Label lb_CAM2_NGRATE;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lb_CAM2_TOTAL;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label lb_CAM2_NG;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label lb_CAM2_OK;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Label lb_CAM1_NGRATE;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lb_CAM1_TOTAL;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lb_CAM1_NG;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lb_CAM1_OK;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private KimLib.LogControl logControl1;
        private System.Windows.Forms.Button btn_Analyze;
        public System.Windows.Forms.Button btn_INPUT4;
        public System.Windows.Forms.Button btn_INPUT3;
        public System.Windows.Forms.Button btn_INPUT2;
        public System.Windows.Forms.Button btn_INPUT1;
        public System.Windows.Forms.Button btn_INPUT0;
        public System.Windows.Forms.Button btn_INPUT7;
        private System.Windows.Forms.CheckBox btn_OUTPUT8;
        private System.Windows.Forms.CheckBox btn_OUTPUT9;
        private System.Windows.Forms.CheckBox btn_OUTPUT6;
        private System.Windows.Forms.CheckBox btn_OUTPUT7;
        private System.Windows.Forms.CheckBox btn_OUTPUT4;
        private System.Windows.Forms.CheckBox btn_OUTPUT5;
        private System.Windows.Forms.CheckBox btn_OUTPUT2;
        private System.Windows.Forms.CheckBox btn_OUTPUT3;
        private System.Windows.Forms.CheckBox btn_OUTPUT0;
        private System.Windows.Forms.CheckBox btn_OUTPUT1;
        private System.Windows.Forms.CheckBox btn_OUTPUT10;
        private System.Windows.Forms.CheckBox btn_OUTPUT11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel32;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cb_ResultOK;
        public System.IO.Ports.SerialPort LightControl2;
        public System.IO.Ports.SerialPort LightControl3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.NumericUpDown num_LightNumber;
        private System.Windows.Forms.Button Btn_LightOnOff;
        private System.Windows.Forms.Button btn_ToolSetUp;
        public System.IO.Ports.SerialPort LightControl4;
        public System.Windows.Forms.Button btn_INPUT11;
        public System.Windows.Forms.Button btn_INPUT8;
        public System.Windows.Forms.Button btn_INPUT9;
        public System.Windows.Forms.Button btn_INPUT10;
    }
}

