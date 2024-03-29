﻿namespace VISION.UI
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_종합결과 = new System.Windows.Forms.Button();
            this.btn_개별카메라결과 = new System.Windows.Forms.Button();
            this.b수량초기화 = new System.Windows.Forms.PictureBox();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lb수량체크시작날짜 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb수량체크현재시간 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b수량초기화)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.resultPanel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.47619F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 875);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(274, 41);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.80602F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.80602F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.38796F));
            this.tableLayoutPanel2.Controls.Add(this.btn_종합결과, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_개별카메라결과, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.b수량초기화, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(274, 41);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_종합결과
            // 
            this.btn_종합결과.BackColor = System.Drawing.Color.DimGray;
            this.btn_종합결과.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_종합결과.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_종합결과.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_종합결과.Location = new System.Drawing.Point(1, 1);
            this.btn_종합결과.Margin = new System.Windows.Forms.Padding(0);
            this.btn_종합결과.Name = "btn_종합결과";
            this.btn_종합결과.Size = new System.Drawing.Size(112, 39);
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
            this.btn_개별카메라결과.Location = new System.Drawing.Point(114, 1);
            this.btn_개별카메라결과.Margin = new System.Windows.Forms.Padding(0);
            this.btn_개별카메라결과.Name = "btn_개별카메라결과";
            this.btn_개별카메라결과.Size = new System.Drawing.Size(112, 39);
            this.btn_개별카메라결과.TabIndex = 1;
            this.btn_개별카메라결과.Tag = "1";
            this.btn_개별카메라결과.Text = "개별 결과";
            this.btn_개별카메라결과.UseVisualStyleBackColor = false;
            // 
            // b수량초기화
            // 
            this.b수량초기화.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.b수량초기화.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.b수량초기화.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b수량초기화.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b수량초기화.Image = global::VISION.Properties.Resources.Reset;
            this.b수량초기화.ImageLocation = "";
            this.b수량초기화.Location = new System.Drawing.Point(227, 1);
            this.b수량초기화.Margin = new System.Windows.Forms.Padding(0);
            this.b수량초기화.Name = "b수량초기화";
            this.b수량초기화.Size = new System.Drawing.Size(46, 39);
            this.b수량초기화.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.b수량초기화.TabIndex = 2;
            this.b수량초기화.TabStop = false;
            this.b수량초기화.Click += new System.EventHandler(this.b수량초기화_Click);
            // 
            // resultPanel
            // 
            this.resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(0, 82);
            this.resultPanel.Margin = new System.Windows.Forms.Padding(0);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(274, 793);
            this.resultPanel.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.12546F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.749077F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.12546F));
            this.tableLayoutPanel4.Controls.Add(this.lb수량체크현재시간, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lb수량체크시작날짜, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(274, 41);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // lb수량체크시작날짜
            // 
            this.lb수량체크시작날짜.AutoSize = true;
            this.lb수량체크시작날짜.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb수량체크시작날짜.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb수량체크시작날짜.ForeColor = System.Drawing.Color.Black;
            this.lb수량체크시작날짜.Location = new System.Drawing.Point(0, 0);
            this.lb수량체크시작날짜.Margin = new System.Windows.Forms.Padding(0);
            this.lb수량체크시작날짜.Name = "lb수량체크시작날짜";
            this.lb수량체크시작날짜.Size = new System.Drawing.Size(126, 41);
            this.lb수량체크시작날짜.TabIndex = 1;
            this.lb수량체크시작날짜.Text = "2023-01-01 00:00";
            this.lb수량체크시작날짜.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(126, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 41);
            this.label2.TabIndex = 2;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb수량체크현재시간
            // 
            this.lb수량체크현재시간.AutoSize = true;
            this.lb수량체크현재시간.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb수량체크현재시간.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb수량체크현재시간.ForeColor = System.Drawing.Color.Black;
            this.lb수량체크현재시간.Location = new System.Drawing.Point(147, 0);
            this.lb수량체크현재시간.Margin = new System.Windows.Forms.Padding(0);
            this.lb수량체크현재시간.Name = "lb수량체크현재시간";
            this.lb수량체크현재시간.Size = new System.Drawing.Size(127, 41);
            this.lb수량체크현재시간.TabIndex = 3;
            this.lb수량체크현재시간.Text = "2023-01-01 00:00";
            this.lb수량체크현재시간.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.b수량초기화)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_종합결과;
        private System.Windows.Forms.Button btn_개별카메라결과;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel resultPanel;
        public System.Windows.Forms.PictureBox b수량초기화;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lb수량체크현재시간;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb수량체크시작날짜;
    }
}
