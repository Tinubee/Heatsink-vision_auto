namespace VISION.UI
{
    partial class HeatSinkMainDisplay2
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeatSinkMainDisplay2));
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.cdyDisplay6 = new Cognex.VisionPro.Display.CogDisplay();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_Cam6_Result = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lb_Cam6_InsTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_최종결과2 = new System.Windows.Forms.Label();
            this.lb_최종결과 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel19.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay6)).BeginInit();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel19.ColumnCount = 1;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel19.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel19.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 1;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(1772, 543);
            this.tableLayoutPanel19.TabIndex = 32;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.cdyDisplay6, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel12, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1770, 542);
            this.tableLayoutPanel9.TabIndex = 18;
            // 
            // cdyDisplay6
            // 
            this.cdyDisplay6.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cdyDisplay6.ColorMapLowerRoiLimit = 0D;
            this.cdyDisplay6.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cdyDisplay6.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cdyDisplay6.ColorMapUpperRoiLimit = 1D;
            this.cdyDisplay6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cdyDisplay6.DoubleTapZoomCycleLength = 2;
            this.cdyDisplay6.DoubleTapZoomSensitivity = 2.5D;
            this.cdyDisplay6.Location = new System.Drawing.Point(1, 28);
            this.cdyDisplay6.Margin = new System.Windows.Forms.Padding(0);
            this.cdyDisplay6.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cdyDisplay6.MouseWheelSensitivity = 1D;
            this.cdyDisplay6.Name = "cdyDisplay6";
            this.cdyDisplay6.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cdyDisplay6.OcxState")));
            this.cdyDisplay6.Size = new System.Drawing.Size(1768, 513);
            this.cdyDisplay6.TabIndex = 8;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel12.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel12.ColumnCount = 4;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.84686F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.11717F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.51799F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.51799F));
            this.tableLayoutPanel12.Controls.Add(this.lb_Cam6_Result, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.lb_Cam6_InsTime, 2, 0);
            this.tableLayoutPanel12.Controls.Add(this.tableLayoutPanel1, 3, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(1768, 26);
            this.tableLayoutPanel12.TabIndex = 11;
            // 
            // lb_Cam6_Result
            // 
            this.lb_Cam6_Result.AutoSize = true;
            this.lb_Cam6_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Cam6_Result.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Cam6_Result.Location = new System.Drawing.Point(231, 1);
            this.lb_Cam6_Result.Name = "lb_Cam6_Result";
            this.lb_Cam6_Result.Size = new System.Drawing.Size(560, 24);
            this.lb_Cam6_Result.TabIndex = 1;
            this.lb_Cam6_Result.Text = "Result";
            this.lb_Cam6_Result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(1, 1);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(226, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "CAM 6";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_Cam6_InsTime
            // 
            this.lb_Cam6_InsTime.AutoSize = true;
            this.lb_Cam6_InsTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Cam6_InsTime.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_Cam6_InsTime.Location = new System.Drawing.Point(798, 1);
            this.lb_Cam6_InsTime.Name = "lb_Cam6_InsTime";
            this.lb_Cam6_InsTime.Size = new System.Drawing.Size(479, 24);
            this.lb_Cam6_InsTime.TabIndex = 2;
            this.lb_Cam6_InsTime.Text = "label27";
            this.lb_Cam6_InsTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1281, 1);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 24);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lb_최종결과2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lb_최종결과, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(146, 1);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(339, 22);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // lb_최종결과2
            // 
            this.lb_최종결과2.AutoSize = true;
            this.lb_최종결과2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_최종결과2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_최종결과2.Location = new System.Drawing.Point(172, 0);
            this.lb_최종결과2.Name = "lb_최종결과2";
            this.lb_최종결과2.Size = new System.Drawing.Size(164, 22);
            this.lb_최종결과2.TabIndex = 6;
            this.lb_최종결과2.Text = "Result2";
            this.lb_최종결과2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_최종결과
            // 
            this.lb_최종결과.AutoSize = true;
            this.lb_최종결과.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_최종결과.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_최종결과.Location = new System.Drawing.Point(3, 0);
            this.lb_최종결과.Name = "lb_최종결과";
            this.lb_최종결과.Size = new System.Drawing.Size(163, 22);
            this.lb_최종결과.TabIndex = 5;
            this.lb_최종결과.Text = "Result";
            this.lb_최종결과.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(4, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "최종결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HeatSinkMainDisplay2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel19);
            this.Name = "HeatSinkMainDisplay2";
            this.Size = new System.Drawing.Size(1772, 543);
            this.tableLayoutPanel19.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cdyDisplay6)).EndInit();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel19;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        public Cognex.VisionPro.Display.CogDisplay cdyDisplay6;
        public System.Windows.Forms.Label lb_Cam6_Result;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label lb_Cam6_InsTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Label lb_최종결과2;
        public System.Windows.Forms.Label lb_최종결과;
    }
}
