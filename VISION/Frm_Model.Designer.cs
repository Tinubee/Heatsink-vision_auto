namespace VISION
{
    partial class Frm_Model
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
            this.tb_SelectModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_CurruntModel = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvModelList = new System.Windows.Forms.DataGridView();
            this.cNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_LoadModel = new System.Windows.Forms.Button();
            this.btn_NewModel = new System.Windows.Forms.Button();
            this.btn_DeleteModel = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModelList)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 553);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.Controls.Add(this.tb_SelectModel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tb_CurruntModel, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(488, 76);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tb_SelectModel
            // 
            this.tb_SelectModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_SelectModel.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SelectModel.Location = new System.Drawing.Point(173, 41);
            this.tb_SelectModel.Name = "tb_SelectModel";
            this.tb_SelectModel.ReadOnly = true;
            this.tb_SelectModel.Size = new System.Drawing.Size(312, 41);
            this.tb_SelectModel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Model";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Lime;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select Model";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_CurruntModel
            // 
            this.tb_CurruntModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_CurruntModel.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_CurruntModel.Location = new System.Drawing.Point(173, 3);
            this.tb_CurruntModel.Name = "tb_CurruntModel";
            this.tb_CurruntModel.ReadOnly = true;
            this.tb_CurruntModel.Size = new System.Drawing.Size(312, 41);
            this.tb_CurruntModel.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Controls.Add(this.dgvModelList, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 85);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(488, 465);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // dgvModelList
            // 
            this.dgvModelList.AllowUserToAddRows = false;
            this.dgvModelList.AllowUserToDeleteRows = false;
            this.dgvModelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cNumber,
            this.cName});
            this.dgvModelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvModelList.Location = new System.Drawing.Point(3, 3);
            this.dgvModelList.MultiSelect = false;
            this.dgvModelList.Name = "dgvModelList";
            this.dgvModelList.ReadOnly = true;
            this.dgvModelList.RowTemplate.Height = 23;
            this.dgvModelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvModelList.Size = new System.Drawing.Size(335, 459);
            this.dgvModelList.TabIndex = 0;
            this.dgvModelList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvModelList_CellClick);
            // 
            // cNumber
            // 
            this.cNumber.Frozen = true;
            this.cNumber.HeaderText = "Model Number";
            this.cNumber.Name = "cNumber";
            this.cNumber.ReadOnly = true;
            this.cNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cName
            // 
            this.cName.Frozen = true;
            this.cName.HeaderText = "Model Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cName.Width = 200;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btn_LoadModel, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_NewModel, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btn_DeleteModel, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.btn_Exit, 0, 7);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(344, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(141, 459);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btn_LoadModel
            // 
            this.btn_LoadModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_LoadModel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_LoadModel.Location = new System.Drawing.Point(3, 3);
            this.btn_LoadModel.Name = "btn_LoadModel";
            this.btn_LoadModel.Size = new System.Drawing.Size(135, 51);
            this.btn_LoadModel.TabIndex = 0;
            this.btn_LoadModel.Text = "Change Model";
            this.btn_LoadModel.UseVisualStyleBackColor = true;
            this.btn_LoadModel.Click += new System.EventHandler(this.btn_LoadModel_Click);
            // 
            // btn_NewModel
            // 
            this.btn_NewModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_NewModel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_NewModel.Location = new System.Drawing.Point(3, 117);
            this.btn_NewModel.Name = "btn_NewModel";
            this.btn_NewModel.Size = new System.Drawing.Size(135, 51);
            this.btn_NewModel.TabIndex = 0;
            this.btn_NewModel.Text = "Create Model";
            this.btn_NewModel.UseVisualStyleBackColor = true;
            this.btn_NewModel.Click += new System.EventHandler(this.btn_NewModel_Click);
            // 
            // btn_DeleteModel
            // 
            this.btn_DeleteModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_DeleteModel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DeleteModel.Location = new System.Drawing.Point(3, 231);
            this.btn_DeleteModel.Name = "btn_DeleteModel";
            this.btn_DeleteModel.Size = new System.Drawing.Size(135, 51);
            this.btn_DeleteModel.TabIndex = 0;
            this.btn_DeleteModel.Text = "Delete Model";
            this.btn_DeleteModel.UseVisualStyleBackColor = true;
            this.btn_DeleteModel.Click += new System.EventHandler(this.btn_DeleteModel_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Exit.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Exit.Location = new System.Drawing.Point(3, 402);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(135, 54);
            this.btn_Exit.TabIndex = 0;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // Frm_Model
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 553);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Model";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_Model";
            this.Load += new System.EventHandler(this.Frm_Model_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvModelList)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tb_SelectModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_CurruntModel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dgvModelList;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btn_LoadModel;
        private System.Windows.Forms.Button btn_NewModel;
        private System.Windows.Forms.Button btn_DeleteModel;
        private System.Windows.Forms.Button btn_Exit;
    }
}