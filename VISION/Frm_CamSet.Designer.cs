namespace VISION
{
    partial class Frm_CamSet
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn저장 = new System.Windows.Forms.Button();
            this.btn종료 = new System.Windows.Forms.Button();
            this.cb카메라목록 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb노출값 = new System.Windows.Forms.TextBox();
            this.tb감도값 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 148);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btn저장, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn종료, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb카메라목록, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(546, 58);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn저장
            // 
            this.btn저장.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn저장.Image = global::VISION.Properties.Resources.SaveSetting;
            this.btn저장.Location = new System.Drawing.Point(273, 1);
            this.btn저장.Margin = new System.Windows.Forms.Padding(0);
            this.btn저장.Name = "btn저장";
            this.btn저장.Size = new System.Drawing.Size(135, 56);
            this.btn저장.TabIndex = 0;
            this.btn저장.UseVisualStyleBackColor = true;
            // 
            // btn종료
            // 
            this.btn종료.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn종료.Image = global::VISION.Properties.Resources.Close;
            this.btn종료.Location = new System.Drawing.Point(409, 1);
            this.btn종료.Margin = new System.Windows.Forms.Padding(0);
            this.btn종료.Name = "btn종료";
            this.btn종료.Size = new System.Drawing.Size(136, 56);
            this.btn종료.TabIndex = 1;
            this.btn종료.UseVisualStyleBackColor = true;
            this.btn종료.Click += new System.EventHandler(this.btn종료_Click);
            // 
            // cb카메라목록
            // 
            this.cb카메라목록.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb카메라목록.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb카메라목록.FormattingEnabled = true;
            this.cb카메라목록.Location = new System.Drawing.Point(1, 1);
            this.cb카메라목록.Margin = new System.Windows.Forms.Padding(0);
            this.cb카메라목록.Name = "cb카메라목록";
            this.cb카메라목록.Size = new System.Drawing.Size(271, 53);
            this.cb카메라목록.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tb노출값, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tb감도값, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 60);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(546, 87);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exposure";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(1, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 42);
            this.label2.TabIndex = 1;
            this.label2.Text = "Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb노출값
            // 
            this.tb노출값.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb노출값.Font = new System.Drawing.Font("맑은 고딕", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb노출값.Location = new System.Drawing.Point(273, 1);
            this.tb노출값.Margin = new System.Windows.Forms.Padding(0);
            this.tb노출값.Name = "tb노출값";
            this.tb노출값.Size = new System.Drawing.Size(272, 41);
            this.tb노출값.TabIndex = 2;
            this.tb노출값.Text = "0";
            this.tb노출값.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb감도값
            // 
            this.tb감도값.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb감도값.Font = new System.Drawing.Font("맑은 고딕", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb감도값.Location = new System.Drawing.Point(273, 44);
            this.tb감도값.Margin = new System.Windows.Forms.Padding(0);
            this.tb감도값.Name = "tb감도값";
            this.tb감도값.Size = new System.Drawing.Size(272, 41);
            this.tb감도값.TabIndex = 3;
            this.tb감도값.Text = "0";
            this.tb감도값.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Frm_CamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 148);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_CamSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_NewModel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn저장;
        private System.Windows.Forms.Button btn종료;
        private System.Windows.Forms.ComboBox cb카메라목록;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb노출값;
        private System.Windows.Forms.TextBox tb감도값;
    }
}