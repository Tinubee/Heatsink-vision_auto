namespace VISION.UI.Controls
{
    partial class SetInspection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetInspection));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.b설정저장 = new DevExpress.XtraEditors.SimpleButton();
            this.e카메라선택 = new DevExpress.XtraEditors.LookUpEdit();
            this.Bind카메라구분 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e카메라선택.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind카메라구분)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.e카메라선택);
            this.panelControl1.Controls.Add(this.b설정저장);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(3);
            this.panelControl1.Size = new System.Drawing.Size(1244, 52);
            this.panelControl1.TabIndex = 6;
            // 
            // b설정저장
            // 
            this.b설정저장.Appearance.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.b설정저장.Appearance.Options.UseFont = true;
            this.b설정저장.Dock = System.Windows.Forms.DockStyle.Right;
            this.b설정저장.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b설정저장.ImageOptions.SvgImage")));
            this.b설정저장.Location = new System.Drawing.Point(1059, 5);
            this.b설정저장.Name = "b설정저장";
            this.b설정저장.Size = new System.Drawing.Size(180, 42);
            this.b설정저장.TabIndex = 0;
            this.b설정저장.Text = "설정저장";
            // 
            // e카메라선택
            // 
            this.e카메라선택.Dock = System.Windows.Forms.DockStyle.Left;
            this.e카메라선택.Location = new System.Drawing.Point(5, 5);
            this.e카메라선택.Name = "e카메라선택";
            this.e카메라선택.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e카메라선택.Properties.Appearance.Options.UseFont = true;
            this.e카메라선택.Properties.Appearance.Options.UseTextOptions = true;
            this.e카메라선택.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e카메라선택.Properties.AppearanceDropDown.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e카메라선택.Properties.AppearanceDropDown.Options.UseFont = true;
            this.e카메라선택.Properties.AutoHeight = false;
            this.e카메라선택.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.e카메라선택.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("모델구분", "구분", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("모델설명", "설명", 240, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.e카메라선택.Properties.NullText = "[카메라선택]";
            this.e카메라선택.Size = new System.Drawing.Size(374, 42);
            this.e카메라선택.TabIndex = 9;
            // 
            // SetInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Name = "SetInspection";
            this.Size = new System.Drawing.Size(1244, 781);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.e카메라선택.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind카메라구분)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LookUpEdit e카메라선택;
        private DevExpress.XtraEditors.SimpleButton b설정저장;
        private System.Windows.Forms.BindingSource Bind카메라구분;
    }
}
