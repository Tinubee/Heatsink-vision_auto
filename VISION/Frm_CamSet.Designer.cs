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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.num_CamNumber = new System.Windows.Forms.NumericUpDown();
            this.tb_CamSerialNumber = new System.Windows.Forms.TextBox();
            this.btn_CamListAdd = new System.Windows.Forms.Button();
            this.DG_CamLiast = new System.Windows.Forms.DataGridView();
            this.camListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CamNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DG_CamLiast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.num_CamNumber, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_CamSerialNumber, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_CamListAdd, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(648, 100);
            this.tableLayoutPanel1.TabIndex = 5;
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
            this.label1.Size = new System.Drawing.Size(123, 50);
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
            this.label2.Location = new System.Drawing.Point(132, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(382, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "Camera Serial Number";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // num_CamNumber
            // 
            this.num_CamNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.num_CamNumber.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_CamNumber.Location = new System.Drawing.Point(3, 53);
            this.num_CamNumber.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.num_CamNumber.Name = "num_CamNumber";
            this.num_CamNumber.Size = new System.Drawing.Size(123, 43);
            this.num_CamNumber.TabIndex = 2;
            this.num_CamNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_CamSerialNumber
            // 
            this.tb_CamSerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_CamSerialNumber.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_CamSerialNumber.Location = new System.Drawing.Point(132, 53);
            this.tb_CamSerialNumber.Name = "tb_CamSerialNumber";
            this.tb_CamSerialNumber.Size = new System.Drawing.Size(382, 43);
            this.tb_CamSerialNumber.TabIndex = 0;
            // 
            // btn_CamListAdd
            // 
            this.btn_CamListAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_CamListAdd.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_CamListAdd.Location = new System.Drawing.Point(520, 53);
            this.btn_CamListAdd.Name = "btn_CamListAdd";
            this.btn_CamListAdd.Size = new System.Drawing.Size(125, 44);
            this.btn_CamListAdd.TabIndex = 1;
            this.btn_CamListAdd.Text = "ADD";
            this.btn_CamListAdd.UseVisualStyleBackColor = true;
            this.btn_CamListAdd.Click += new System.EventHandler(this.btn_CamListAdd_Click);
            // 
            // DG_CamLiast
            // 
            this.DG_CamLiast.AllowUserToAddRows = false;
            this.DG_CamLiast.AllowUserToResizeRows = false;
            this.DG_CamLiast.AutoGenerateColumns = false;
            this.DG_CamLiast.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DG_CamLiast.DataSource = this.camListBindingSource;
            this.DG_CamLiast.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DG_CamLiast.Location = new System.Drawing.Point(0, 100);
            this.DG_CamLiast.Name = "DG_CamLiast";
            this.DG_CamLiast.ReadOnly = true;
            this.DG_CamLiast.RowHeadersWidth = 50;
            this.DG_CamLiast.RowTemplate.Height = 23;
            this.DG_CamLiast.Size = new System.Drawing.Size(648, 286);
            this.DG_CamLiast.TabIndex = 4;
            this.DG_CamLiast.TabStop = false;
            // 
            // Frm_CamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 386);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.DG_CamLiast);
            this.Name = "Frm_CamSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_CamList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_CamSet_FormClosing);
            this.Load += new System.EventHandler(this.Frm_CamList_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CamNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DG_CamLiast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camListBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_CamNumber;
        private System.Windows.Forms.TextBox tb_CamSerialNumber;
        private System.Windows.Forms.Button btn_CamListAdd;
        private System.Windows.Forms.DataGridView DG_CamLiast;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource camListBindingSource;
    }
}