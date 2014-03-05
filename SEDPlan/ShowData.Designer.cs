namespace SEDPlan
{
    partial class ShowData
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnOKClose = new System.Windows.Forms.Button();
            this.lbProjectNo = new System.Windows.Forms.Label();
            this.tbxProjectNo = new System.Windows.Forms.TextBox();
            this.lbProjectName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(876, 415);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnOKClose
            // 
            this.btnOKClose.Location = new System.Drawing.Point(358, 488);
            this.btnOKClose.Name = "btnOKClose";
            this.btnOKClose.Size = new System.Drawing.Size(147, 38);
            this.btnOKClose.TabIndex = 1;
            this.btnOKClose.Text = "OK";
            this.btnOKClose.UseVisualStyleBackColor = true;
            this.btnOKClose.Click += new System.EventHandler(this.btnOKClose_Click);
            // 
            // lbProjectNo
            // 
            this.lbProjectNo.AutoSize = true;
            this.lbProjectNo.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProjectNo.Location = new System.Drawing.Point(12, 9);
            this.lbProjectNo.Name = "lbProjectNo";
            this.lbProjectNo.Size = new System.Drawing.Size(111, 14);
            this.lbProjectNo.TabIndex = 2;
            this.lbProjectNo.Text = "Project No : ";
            // 
            // tbxProjectNo
            // 
            this.tbxProjectNo.BackColor = System.Drawing.SystemColors.Info;
            this.tbxProjectNo.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxProjectNo.ForeColor = System.Drawing.Color.OrangeRed;
            this.tbxProjectNo.Location = new System.Drawing.Point(129, 6);
            this.tbxProjectNo.Name = "tbxProjectNo";
            this.tbxProjectNo.Size = new System.Drawing.Size(141, 23);
            this.tbxProjectNo.TabIndex = 3;
            // 
            // lbProjectName
            // 
            this.lbProjectName.AutoSize = true;
            this.lbProjectName.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProjectName.Location = new System.Drawing.Point(302, 9);
            this.lbProjectName.Name = "lbProjectName";
            this.lbProjectName.Size = new System.Drawing.Size(127, 14);
            this.lbProjectName.TabIndex = 4;
            this.lbProjectName.Text = "Project Name : ";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ForeColor = System.Drawing.Color.OrangeRed;
            this.textBox1.Location = new System.Drawing.Point(435, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(281, 23);
            this.textBox1.TabIndex = 5;
            // 
            // ShowData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 542);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbProjectName);
            this.Controls.Add(this.tbxProjectNo);
            this.Controls.Add(this.lbProjectNo);
            this.Controls.Add(this.btnOKClose);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ShowData";
            this.Text = "ShowData";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnOKClose;
        private System.Windows.Forms.Label lbProjectNo;
        private System.Windows.Forms.TextBox tbxProjectNo;
        private System.Windows.Forms.Label lbProjectName;
        private System.Windows.Forms.TextBox textBox1;
    }
}