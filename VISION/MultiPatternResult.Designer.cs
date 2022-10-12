namespace VISION
{
    partial class MultiPatternResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_MultiToolResult = new System.Windows.Forms.DataGridView();
            this.colname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coldata = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_MultiToolResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_MultiToolResult
            // 
            this.dgv_MultiToolResult.AllowUserToAddRows = false;
            this.dgv_MultiToolResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_MultiToolResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_MultiToolResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_MultiToolResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colname,
            this.coldata});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_MultiToolResult.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_MultiToolResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_MultiToolResult.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_MultiToolResult.Location = new System.Drawing.Point(0, 0);
            this.dgv_MultiToolResult.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_MultiToolResult.MultiSelect = false;
            this.dgv_MultiToolResult.Name = "dgv_MultiToolResult";
            this.dgv_MultiToolResult.RowHeadersWidth = 10;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.dgv_MultiToolResult.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_MultiToolResult.RowTemplate.Height = 23;
            this.dgv_MultiToolResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_MultiToolResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_MultiToolResult.Size = new System.Drawing.Size(321, 396);
            this.dgv_MultiToolResult.TabIndex = 22;
            // 
            // colname
            // 
            this.colname.HeaderText = "이름";
            this.colname.Name = "colname";
            this.colname.Width = 59;
            // 
            // coldata
            // 
            this.coldata.HeaderText = "결과수치";
            this.coldata.Name = "coldata";
            this.coldata.Width = 85;
            // 
            // MultiPatternResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 396);
            this.Controls.Add(this.dgv_MultiToolResult);
            this.Name = "MultiPatternResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MultiPatternResult";
            this.Load += new System.EventHandler(this.MultiPatternResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_MultiToolResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgv_MultiToolResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colname;
        private System.Windows.Forms.DataGridViewTextBoxColumn coldata;
    }
}