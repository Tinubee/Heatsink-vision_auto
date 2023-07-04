namespace VISION.UI
{
    partial class ResultCountDisplay
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_종합결과 = new System.Windows.Forms.Button();
            this.btn_개별카메라결과 = new System.Windows.Forms.Button();
            this.resultPanel = new DevExpress.XtraEditors.PanelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.resultPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 875);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btn_종합결과, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_개별카메라결과, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(274, 43);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_종합결과
            // 
            this.btn_종합결과.BackColor = System.Drawing.Color.DimGray;
            this.btn_종합결과.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_종합결과.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_종합결과.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_종합결과.Location = new System.Drawing.Point(0, 0);
            this.btn_종합결과.Margin = new System.Windows.Forms.Padding(0);
            this.btn_종합결과.Name = "btn_종합결과";
            this.btn_종합결과.Size = new System.Drawing.Size(137, 43);
            this.btn_종합결과.TabIndex = 0;
            this.btn_종합결과.Tag = "0";
            this.btn_종합결과.Text = "종합 결과";
            this.btn_종합결과.UseVisualStyleBackColor = false;
            // 
            // btn_개별카메라결과
            // 
            this.btn_개별카메라결과.BackColor = System.Drawing.Color.DimGray;
            this.btn_개별카메라결과.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_개별카메라결과.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_개별카메라결과.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_개별카메라결과.Location = new System.Drawing.Point(137, 0);
            this.btn_개별카메라결과.Margin = new System.Windows.Forms.Padding(0);
            this.btn_개별카메라결과.Name = "btn_개별카메라결과";
            this.btn_개별카메라결과.Size = new System.Drawing.Size(137, 43);
            this.btn_개별카메라결과.TabIndex = 1;
            this.btn_개별카메라결과.Tag = "1";
            this.btn_개별카메라결과.Text = "개별 결과";
            this.btn_개별카메라결과.UseVisualStyleBackColor = false;
            // 
            // resultPanel
            // 
            this.resultPanel.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.resultPanel.Appearance.Options.UseBackColor = true;
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(0, 43);
            this.resultPanel.Margin = new System.Windows.Forms.Padding(0);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(274, 832);
            this.resultPanel.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ResultCountDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ResultCountDisplay";
            this.Size = new System.Drawing.Size(274, 875);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_종합결과;
        private System.Windows.Forms.Button btn_개별카메라결과;
        private DevExpress.XtraEditors.PanelControl resultPanel;
        public System.Windows.Forms.Timer timer1;
    }
}
