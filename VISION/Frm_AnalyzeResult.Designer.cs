namespace VISION
{
    partial class Frm_AnalyzeResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_AnalyzeResult));
            this.overlapViewButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_CamNumber = new System.Windows.Forms.ComboBox();
            this.cb_Model = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_DefectType = new System.Windows.Forms.ComboBox();
            this.tb_DefectCount = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btn_Search = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.closeButton = new System.Windows.Forms.Button();
            this.iPFileButton = new System.Windows.Forms.Button();
            this.cdyDisplay_NGImage = new Cognex.VisionPro.Display.CogDisplay();
            this.ImageList = new System.Windows.Forms.ListBox();
            this.cdyDisplay_MasterImage = new Cognex.VisionPro.Display.CogDisplay();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay_NGImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay_MasterImage)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // overlapViewButton
            // 
            this.overlapViewButton.BackColor = System.Drawing.Color.Azure;
            this.overlapViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.overlapViewButton.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold);
            this.overlapViewButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.overlapViewButton.Location = new System.Drawing.Point(829, 717);
            this.overlapViewButton.Name = "overlapViewButton";
            this.overlapViewButton.Size = new System.Drawing.Size(216, 50);
            this.overlapViewButton.TabIndex = 27;
            this.overlapViewButton.Text = "Overlap View";
            this.overlapViewButton.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_CamNumber);
            this.groupBox1.Controls.Add(this.cb_Model);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cb_DefectType);
            this.groupBox1.Controls.Add(this.tb_DefectCount);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.btn_Search);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.startDatePicker);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.endDatePicker);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(11, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(738, 179);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Option";
            // 
            // cb_CamNumber
            // 
            this.cb_CamNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_CamNumber.FormattingEnabled = true;
            this.cb_CamNumber.Location = new System.Drawing.Point(546, 68);
            this.cb_CamNumber.Name = "cb_CamNumber";
            this.cb_CamNumber.Size = new System.Drawing.Size(176, 29);
            this.cb_CamNumber.TabIndex = 37;
            // 
            // cb_Model
            // 
            this.cb_Model.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Model.FormattingEnabled = true;
            this.cb_Model.Location = new System.Drawing.Point(136, 65);
            this.cb_Model.Name = "cb_Model";
            this.cb_Model.Size = new System.Drawing.Size(232, 29);
            this.cb_Model.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(8, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 21);
            this.label4.TabIndex = 35;
            this.label4.Text = "Defect Count :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(17, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 21);
            this.label3.TabIndex = 34;
            this.label3.Text = "Defect Type :";
            // 
            // cb_DefectType
            // 
            this.cb_DefectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DefectType.FormattingEnabled = true;
            this.cb_DefectType.Location = new System.Drawing.Point(136, 100);
            this.cb_DefectType.Name = "cb_DefectType";
            this.cb_DefectType.Size = new System.Drawing.Size(232, 29);
            this.cb_DefectType.TabIndex = 33;
            // 
            // tb_DefectCount
            // 
            this.tb_DefectCount.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.tb_DefectCount.Location = new System.Drawing.Point(136, 138);
            this.tb_DefectCount.Name = "tb_DefectCount";
            this.tb_DefectCount.Size = new System.Drawing.Size(232, 29);
            this.tb_DefectCount.TabIndex = 32;
            this.tb_DefectCount.Text = "0";
            this.tb_DefectCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(62, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 21);
            this.label11.TabIndex = 16;
            this.label11.Text = "Model :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(393, 68);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(147, 21);
            this.label15.TabIndex = 11;
            this.label15.Text = "Camera Number :";
            // 
            // btn_Search
            // 
            this.btn_Search.BackColor = System.Drawing.Color.Azure;
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Search.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Search.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Search.Location = new System.Drawing.Point(397, 103);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(325, 65);
            this.btn_Search.TabIndex = 3;
            this.btn_Search.Text = "Search";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(32, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Date :";
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "yyyy-MM-dd, HH:mm";
            this.startDatePicker.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatePicker.Location = new System.Drawing.Point(136, 30);
            this.startDatePicker.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.startDatePicker.MinDate = new System.DateTime(2008, 1, 1, 0, 0, 0, 0);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(232, 29);
            this.startDatePicker.TabIndex = 1;
            this.startDatePicker.Value = new System.DateTime(2022, 2, 16, 17, 18, 20, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(393, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "End Date :";
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "yyyy-MM-dd, HH:mm";
            this.endDatePicker.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatePicker.Location = new System.Drawing.Point(490, 30);
            this.endDatePicker.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.endDatePicker.MinDate = new System.DateTime(2008, 1, 1, 0, 0, 0, 0);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(232, 29);
            this.endDatePicker.TabIndex = 1;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Red;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.closeButton.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.closeButton.ForeColor = System.Drawing.Color.Yellow;
            this.closeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.closeButton.Location = new System.Drawing.Point(1221, 717);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(164, 50);
            this.closeButton.TabIndex = 24;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = false;
            // 
            // iPFileButton
            // 
            this.iPFileButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.iPFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.iPFileButton.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.iPFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.iPFileButton.Location = new System.Drawing.Point(1051, 717);
            this.iPFileButton.Name = "iPFileButton";
            this.iPFileButton.Size = new System.Drawing.Size(164, 50);
            this.iPFileButton.TabIndex = 25;
            this.iPFileButton.Text = "Image Path";
            this.iPFileButton.UseVisualStyleBackColor = false;
            // 
            // cdyDisplay_NGImage
            // 
            this.cdyDisplay_NGImage.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cdyDisplay_NGImage.ColorMapLowerRoiLimit = 0D;
            this.cdyDisplay_NGImage.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cdyDisplay_NGImage.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cdyDisplay_NGImage.ColorMapUpperRoiLimit = 1D;
            this.cdyDisplay_NGImage.DoubleTapZoomCycleLength = 2;
            this.cdyDisplay_NGImage.DoubleTapZoomSensitivity = 2.5D;
            this.cdyDisplay_NGImage.Location = new System.Drawing.Point(762, 225);
            this.cdyDisplay_NGImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cdyDisplay_NGImage.MouseWheelSensitivity = 1D;
            this.cdyDisplay_NGImage.Name = "cdyDisplay_NGImage";
            this.cdyDisplay_NGImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cdyDisplay_NGImage.OcxState")));
            this.cdyDisplay_NGImage.Size = new System.Drawing.Size(623, 486);
            this.cdyDisplay_NGImage.TabIndex = 28;
            // 
            // ImageList
            // 
            this.ImageList.BackColor = System.Drawing.Color.Black;
            this.ImageList.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ImageList.ForeColor = System.Drawing.Color.White;
            this.ImageList.FormattingEnabled = true;
            this.ImageList.HorizontalScrollbar = true;
            this.ImageList.ItemHeight = 20;
            this.ImageList.Location = new System.Drawing.Point(762, 4);
            this.ImageList.Margin = new System.Windows.Forms.Padding(0);
            this.ImageList.Name = "ImageList";
            this.ImageList.Size = new System.Drawing.Size(623, 184);
            this.ImageList.TabIndex = 29;
            // 
            // cdyDisplay_MasterImage
            // 
            this.cdyDisplay_MasterImage.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cdyDisplay_MasterImage.ColorMapLowerRoiLimit = 0D;
            this.cdyDisplay_MasterImage.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cdyDisplay_MasterImage.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cdyDisplay_MasterImage.ColorMapUpperRoiLimit = 1D;
            this.cdyDisplay_MasterImage.DoubleTapZoomCycleLength = 2;
            this.cdyDisplay_MasterImage.DoubleTapZoomSensitivity = 2.5D;
            this.cdyDisplay_MasterImage.Location = new System.Drawing.Point(11, 225);
            this.cdyDisplay_MasterImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cdyDisplay_MasterImage.MouseWheelSensitivity = 1D;
            this.cdyDisplay_MasterImage.Name = "cdyDisplay_MasterImage";
            this.cdyDisplay_MasterImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cdyDisplay_MasterImage.OcxState")));
            this.cdyDisplay_MasterImage.Size = new System.Drawing.Size(722, 486);
            this.cdyDisplay_MasterImage.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(714, 23);
            this.label5.TabIndex = 31;
            this.label5.Text = "Master Image";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 194);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(722, 25);
            this.tableLayoutPanel1.TabIndex = 32;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(762, 194);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(623, 25);
            this.tableLayoutPanel2.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(615, 23);
            this.label6.TabIndex = 31;
            this.label6.Text = "Load Image";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frm_AnalyzeResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 779);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cdyDisplay_MasterImage);
            this.Controls.Add(this.ImageList);
            this.Controls.Add(this.cdyDisplay_NGImage);
            this.Controls.Add(this.overlapViewButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.iPFileButton);
            this.Name = "Frm_AnalyzeResult";
            this.Text = "Frm_AnalyzeResult";
            this.Load += new System.EventHandler(this.Frm_AnalyzeResult_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay_NGImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay_MasterImage)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button overlapViewButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_DefectType;
        private System.Windows.Forms.TextBox tb_DefectCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button iPFileButton;
        private System.Windows.Forms.ComboBox cb_CamNumber;
        private System.Windows.Forms.ComboBox cb_Model;
        internal Cognex.VisionPro.Display.CogDisplay cdyDisplay_NGImage;
        internal System.Windows.Forms.ListBox ImageList;
        internal Cognex.VisionPro.Display.CogDisplay cdyDisplay_MasterImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label6;
    }
}