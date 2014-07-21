namespace SEDPlan
{
    partial class Management
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
            this.dgvSAProcess = new System.Windows.Forms.DataGridView();
            this.lbSAProcess = new System.Windows.Forms.Label();
            this.tbxSAPrssName = new System.Windows.Forms.TextBox();
            this.btnAddSAProcess = new System.Windows.Forms.Button();
            this.btnDeleteSAProcess = new System.Windows.Forms.Button();
            this.lbProjectInfo = new System.Windows.Forms.Label();
            this.dgvProjectInfo = new System.Windows.Forms.DataGridView();
            this.btnDeleteProjectInfo = new System.Windows.Forms.Button();
            this.btnAddProjectInfo = new System.Windows.Forms.Button();
            this.tbxProjectNo = new System.Windows.Forms.TextBox();
            this.lbErr = new System.Windows.Forms.Label();
            this.lbProjectNo = new System.Windows.Forms.Label();
            this.lbProjectName = new System.Windows.Forms.Label();
            this.tbxProjectName = new System.Windows.Forms.TextBox();
            this.lbSAPrssName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSAProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSAProcess
            // 
            this.dgvSAProcess.AllowUserToAddRows = false;
            this.dgvSAProcess.AllowUserToDeleteRows = false;
            this.dgvSAProcess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSAProcess.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvSAProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSAProcess.Location = new System.Drawing.Point(15, 32);
            this.dgvSAProcess.MultiSelect = false;
            this.dgvSAProcess.Name = "dgvSAProcess";
            this.dgvSAProcess.RowHeadersWidth = 15;
            this.dgvSAProcess.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSAProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSAProcess.Size = new System.Drawing.Size(387, 185);
            this.dgvSAProcess.TabIndex = 0;
            this.dgvSAProcess.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSAProcess_CellValueChanged);
            // 
            // lbSAProcess
            // 
            this.lbSAProcess.AutoSize = true;
            this.lbSAProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSAProcess.Location = new System.Drawing.Point(12, 13);
            this.lbSAProcess.Name = "lbSAProcess";
            this.lbSAProcess.Size = new System.Drawing.Size(87, 15);
            this.lbSAProcess.TabIndex = 1;
            this.lbSAProcess.Text = "SA Process :";
            // 
            // tbxSAPrssName
            // 
            this.tbxSAPrssName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSAPrssName.Location = new System.Drawing.Point(101, 225);
            this.tbxSAPrssName.Name = "tbxSAPrssName";
            this.tbxSAPrssName.Size = new System.Drawing.Size(158, 23);
            this.tbxSAPrssName.TabIndex = 2;
            // 
            // btnAddSAProcess
            // 
            this.btnAddSAProcess.Location = new System.Drawing.Point(279, 225);
            this.btnAddSAProcess.Name = "btnAddSAProcess";
            this.btnAddSAProcess.Size = new System.Drawing.Size(75, 23);
            this.btnAddSAProcess.TabIndex = 3;
            this.btnAddSAProcess.Text = "Add";
            this.btnAddSAProcess.UseVisualStyleBackColor = true;
            this.btnAddSAProcess.Click += new System.EventHandler(this.btnAddSAProcess_Click);
            // 
            // btnDeleteSAProcess
            // 
            this.btnDeleteSAProcess.Location = new System.Drawing.Point(360, 225);
            this.btnDeleteSAProcess.Name = "btnDeleteSAProcess";
            this.btnDeleteSAProcess.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteSAProcess.TabIndex = 4;
            this.btnDeleteSAProcess.Text = "Delete";
            this.btnDeleteSAProcess.UseVisualStyleBackColor = true;
            this.btnDeleteSAProcess.Click += new System.EventHandler(this.btnDeleteSAProcess_Click);
            // 
            // lbProjectInfo
            // 
            this.lbProjectInfo.AutoSize = true;
            this.lbProjectInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProjectInfo.Location = new System.Drawing.Point(15, 255);
            this.lbProjectInfo.Name = "lbProjectInfo";
            this.lbProjectInfo.Size = new System.Drawing.Size(92, 15);
            this.lbProjectInfo.TabIndex = 5;
            this.lbProjectInfo.Text = "Project Info : ";
            // 
            // dgvProjectInfo
            // 
            this.dgvProjectInfo.AllowUserToAddRows = false;
            this.dgvProjectInfo.AllowUserToDeleteRows = false;
            this.dgvProjectInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProjectInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvProjectInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjectInfo.Location = new System.Drawing.Point(15, 273);
            this.dgvProjectInfo.MultiSelect = false;
            this.dgvProjectInfo.Name = "dgvProjectInfo";
            this.dgvProjectInfo.RowHeadersWidth = 15;
            this.dgvProjectInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProjectInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjectInfo.Size = new System.Drawing.Size(387, 185);
            this.dgvProjectInfo.TabIndex = 6;
            this.dgvProjectInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProjectInfo_CellValueChanged);
            // 
            // btnDeleteProjectInfo
            // 
            this.btnDeleteProjectInfo.Location = new System.Drawing.Point(360, 465);
            this.btnDeleteProjectInfo.Name = "btnDeleteProjectInfo";
            this.btnDeleteProjectInfo.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteProjectInfo.TabIndex = 9;
            this.btnDeleteProjectInfo.Text = "Delete";
            this.btnDeleteProjectInfo.UseVisualStyleBackColor = true;
            this.btnDeleteProjectInfo.Click += new System.EventHandler(this.btnDeleteProjectInfo_Click);
            // 
            // btnAddProjectInfo
            // 
            this.btnAddProjectInfo.Location = new System.Drawing.Point(279, 465);
            this.btnAddProjectInfo.Name = "btnAddProjectInfo";
            this.btnAddProjectInfo.Size = new System.Drawing.Size(75, 23);
            this.btnAddProjectInfo.TabIndex = 8;
            this.btnAddProjectInfo.Text = "Add";
            this.btnAddProjectInfo.UseVisualStyleBackColor = true;
            this.btnAddProjectInfo.Click += new System.EventHandler(this.btnAddProjectInfo_Click);
            // 
            // tbxProjectNo
            // 
            this.tbxProjectNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxProjectNo.Location = new System.Drawing.Point(101, 465);
            this.tbxProjectNo.Name = "tbxProjectNo";
            this.tbxProjectNo.Size = new System.Drawing.Size(158, 23);
            this.tbxProjectNo.TabIndex = 7;
            // 
            // lbErr
            // 
            this.lbErr.AutoSize = true;
            this.lbErr.Location = new System.Drawing.Point(15, 530);
            this.lbErr.MaximumSize = new System.Drawing.Size(700, 0);
            this.lbErr.Name = "lbErr";
            this.lbErr.Size = new System.Drawing.Size(0, 13);
            this.lbErr.TabIndex = 10;
            // 
            // lbProjectNo
            // 
            this.lbProjectNo.AutoSize = true;
            this.lbProjectNo.Location = new System.Drawing.Point(15, 470);
            this.lbProjectNo.Name = "lbProjectNo";
            this.lbProjectNo.Size = new System.Drawing.Size(63, 13);
            this.lbProjectNo.TabIndex = 11;
            this.lbProjectNo.Text = "Project No :";
            // 
            // lbProjectName
            // 
            this.lbProjectName.AutoSize = true;
            this.lbProjectName.Location = new System.Drawing.Point(15, 496);
            this.lbProjectName.Name = "lbProjectName";
            this.lbProjectName.Size = new System.Drawing.Size(77, 13);
            this.lbProjectName.TabIndex = 13;
            this.lbProjectName.Text = "Project Name :";
            // 
            // tbxProjectName
            // 
            this.tbxProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxProjectName.Location = new System.Drawing.Point(101, 491);
            this.tbxProjectName.Name = "tbxProjectName";
            this.tbxProjectName.Size = new System.Drawing.Size(158, 23);
            this.tbxProjectName.TabIndex = 12;
            // 
            // lbSAPrssName
            // 
            this.lbSAPrssName.AutoSize = true;
            this.lbSAPrssName.Location = new System.Drawing.Point(12, 230);
            this.lbSAPrssName.Name = "lbSAPrssName";
            this.lbSAPrssName.Size = new System.Drawing.Size(82, 13);
            this.lbSAPrssName.TabIndex = 14;
            this.lbSAPrssName.Text = "Process Name :";
            // 
            // Management
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 556);
            this.Controls.Add(this.lbSAPrssName);
            this.Controls.Add(this.lbProjectName);
            this.Controls.Add(this.tbxProjectName);
            this.Controls.Add(this.lbProjectNo);
            this.Controls.Add(this.lbErr);
            this.Controls.Add(this.btnDeleteProjectInfo);
            this.Controls.Add(this.btnAddProjectInfo);
            this.Controls.Add(this.tbxProjectNo);
            this.Controls.Add(this.dgvProjectInfo);
            this.Controls.Add(this.lbProjectInfo);
            this.Controls.Add(this.btnDeleteSAProcess);
            this.Controls.Add(this.btnAddSAProcess);
            this.Controls.Add(this.tbxSAPrssName);
            this.Controls.Add(this.lbSAProcess);
            this.Controls.Add(this.dgvSAProcess);
            this.Name = "Management";
            this.Text = "Management";
            this.Load += new System.EventHandler(this.Management_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSAProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSAProcess;
        private System.Windows.Forms.Label lbSAProcess;
        private System.Windows.Forms.TextBox tbxSAPrssName;
        private System.Windows.Forms.Button btnAddSAProcess;
        private System.Windows.Forms.Button btnDeleteSAProcess;
        private System.Windows.Forms.Label lbProjectInfo;
        private System.Windows.Forms.DataGridView dgvProjectInfo;
        private System.Windows.Forms.Button btnDeleteProjectInfo;
        private System.Windows.Forms.Button btnAddProjectInfo;
        private System.Windows.Forms.TextBox tbxProjectNo;
        private System.Windows.Forms.Label lbErr;
        private System.Windows.Forms.Label lbProjectNo;
        private System.Windows.Forms.Label lbProjectName;
        private System.Windows.Forms.TextBox tbxProjectName;
        private System.Windows.Forms.Label lbSAPrssName;
    }
}