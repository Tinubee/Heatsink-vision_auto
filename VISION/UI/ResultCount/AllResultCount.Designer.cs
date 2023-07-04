namespace VISION.UI.ResultCount
{
    partial class AllResultCount
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
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.lb_OK = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lb_1_NG = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lb_TOTAL = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lb_NGRATE = new System.Windows.Forms.Label();
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_2_NG = new System.Windows.Forms.Label();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel27.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel16.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel16.Controls.Add(this.lb_2_NG, 1, 2);
            this.tableLayoutPanel16.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel16.Controls.Add(this.lb_NGRATE, 1, 4);
            this.tableLayoutPanel16.Controls.Add(this.label24, 0, 4);
            this.tableLayoutPanel16.Controls.Add(this.lb_TOTAL, 1, 3);
            this.tableLayoutPanel16.Controls.Add(this.label22, 0, 3);
            this.tableLayoutPanel16.Controls.Add(this.lb_1_NG, 1, 1);
            this.tableLayoutPanel16.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel16.Controls.Add(this.lb_OK, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 5;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(266, 824);
            this.tableLayoutPanel16.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Black;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.ForeColor = System.Drawing.Color.Lime;
            this.label17.Location = new System.Drawing.Point(4, 1);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 163);
            this.label17.TabIndex = 0;
            this.label17.Text = "OK";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_OK
            // 
            this.lb_OK.AutoSize = true;
            this.lb_OK.BackColor = System.Drawing.Color.Black;
            this.lb_OK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_OK.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_OK.ForeColor = System.Drawing.Color.Lime;
            this.lb_OK.Location = new System.Drawing.Point(83, 1);
            this.lb_OK.Name = "lb_OK";
            this.lb_OK.Size = new System.Drawing.Size(179, 163);
            this.lb_OK.TabIndex = 1;
            this.lb_OK.Text = "0";
            this.lb_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Black;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(4, 165);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 163);
            this.label20.TabIndex = 2;
            this.label20.Text = "1- NG";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_1_NG
            // 
            this.lb_1_NG.AutoSize = true;
            this.lb_1_NG.BackColor = System.Drawing.Color.Black;
            this.lb_1_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_1_NG.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_1_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_1_NG.Location = new System.Drawing.Point(83, 165);
            this.lb_1_NG.Name = "lb_1_NG";
            this.lb_1_NG.Size = new System.Drawing.Size(179, 163);
            this.lb_1_NG.TabIndex = 3;
            this.lb_1_NG.Text = "0";
            this.lb_1_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Black;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(4, 493);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 163);
            this.label22.TabIndex = 4;
            this.label22.Text = "TOTAL";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_TOTAL
            // 
            this.lb_TOTAL.AutoSize = true;
            this.lb_TOTAL.BackColor = System.Drawing.Color.Black;
            this.lb_TOTAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_TOTAL.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_TOTAL.ForeColor = System.Drawing.Color.White;
            this.lb_TOTAL.Location = new System.Drawing.Point(83, 493);
            this.lb_TOTAL.Name = "lb_TOTAL";
            this.lb_TOTAL.Size = new System.Drawing.Size(179, 163);
            this.lb_TOTAL.TabIndex = 5;
            this.lb_TOTAL.Text = "0";
            this.lb_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Black;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.ForeColor = System.Drawing.Color.Yellow;
            this.label24.Location = new System.Drawing.Point(4, 657);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(72, 166);
            this.label24.TabIndex = 6;
            this.label24.Text = "NG RATE";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_NGRATE
            // 
            this.lb_NGRATE.AutoSize = true;
            this.lb_NGRATE.BackColor = System.Drawing.Color.Black;
            this.lb_NGRATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_NGRATE.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_NGRATE.ForeColor = System.Drawing.Color.Yellow;
            this.lb_NGRATE.Location = new System.Drawing.Point(83, 657);
            this.lb_NGRATE.Name = "lb_NGRATE";
            this.lb_NGRATE.Size = new System.Drawing.Size(179, 166);
            this.lb_NGRATE.TabIndex = 7;
            this.lb_NGRATE.Text = "0";
            this.lb_NGRATE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel27.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel27.ColumnCount = 1;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel16, 0, 0);
            this.tableLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel27.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 1;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(274, 832);
            this.tableLayoutPanel27.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(4, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 163);
            this.label1.TabIndex = 8;
            this.label1.Text = "2 - NG";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_2_NG
            // 
            this.lb_2_NG.AutoSize = true;
            this.lb_2_NG.BackColor = System.Drawing.Color.Black;
            this.lb_2_NG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_2_NG.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_2_NG.ForeColor = System.Drawing.Color.Red;
            this.lb_2_NG.Location = new System.Drawing.Point(83, 329);
            this.lb_2_NG.Name = "lb_2_NG";
            this.lb_2_NG.Size = new System.Drawing.Size(179, 163);
            this.lb_2_NG.TabIndex = 9;
            this.lb_2_NG.Text = "0";
            this.lb_2_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AllResultCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel27);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AllResultCount";
            this.Size = new System.Drawing.Size(274, 832);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            this.tableLayoutPanel27.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel27;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lb_NGRATE;
        public System.Windows.Forms.Label lb_TOTAL;
        public System.Windows.Forms.Label lb_1_NG;
        public System.Windows.Forms.Label lb_OK;
        public System.Windows.Forms.Label lb_2_NG;
    }
}
