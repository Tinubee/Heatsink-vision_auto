namespace VISION
{
    partial class Frm_NewModel
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
            this.btn_MakeModel = new System.Windows.Forms.Button();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.num_ModelNumber = new System.Windows.Forms.NumericUpDown();
            this.tb_ModelName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_ModelNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.num_ModelNumber, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_ModelName, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(603, 156);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.Controls.Add(this.btn_MakeModel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_Cancle, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(183, 107);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(417, 46);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_MakeModel
            // 
            this.btn_MakeModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_MakeModel.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_MakeModel.Location = new System.Drawing.Point(3, 3);
            this.btn_MakeModel.Name = "btn_MakeModel";
            this.btn_MakeModel.Size = new System.Drawing.Size(181, 40);
            this.btn_MakeModel.TabIndex = 0;
            this.btn_MakeModel.Text = "Create";
            this.btn_MakeModel.UseVisualStyleBackColor = true;
            this.btn_MakeModel.Click += new System.EventHandler(this.btn_MakeModel_Click);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Cancle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cancle.Location = new System.Drawing.Point(231, 3);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(183, 40);
            this.btn_Cancle.TabIndex = 0;
            this.btn_Cancle.Text = "Exit";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 52);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(183, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(417, 52);
            this.label2.TabIndex = 1;
            this.label2.Text = "Model Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // num_ModelNumber
            // 
            this.num_ModelNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.num_ModelNumber.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_ModelNumber.Location = new System.Drawing.Point(3, 55);
            this.num_ModelNumber.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.num_ModelNumber.Name = "num_ModelNumber";
            this.num_ModelNumber.Size = new System.Drawing.Size(174, 43);
            this.num_ModelNumber.TabIndex = 2;
            this.num_ModelNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_ModelName
            // 
            this.tb_ModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_ModelName.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ModelName.Location = new System.Drawing.Point(183, 55);
            this.tb_ModelName.Name = "tb_ModelName";
            this.tb_ModelName.Size = new System.Drawing.Size(417, 43);
            this.tb_ModelName.TabIndex = 3;
            // 
            // Frm_NewModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 156);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_NewModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_NewModel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_ModelNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_MakeModel;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_ModelNumber;
        private System.Windows.Forms.TextBox tb_ModelName;
    }
}