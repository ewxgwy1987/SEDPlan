namespace SEDPlan
{
    partial class Form1
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
            this.ImportTabs = new System.Windows.Forms.TabControl();
            this.tabSACmpn = new System.Windows.Forms.TabPage();
            this.pnlSACmpn = new System.Windows.Forms.Panel();
            this.btn_SC_Open = new System.Windows.Forms.Button();
            this.lb_SC_SAID = new System.Windows.Forms.Label();
            this.btn_SC_Import = new System.Windows.Forms.Button();
            this.tbx_SC_SAID = new System.Windows.Forms.TextBox();
            this.btn_SC_View = new System.Windows.Forms.Button();
            this.lb_SC_FPath = new System.Windows.Forms.Label();
            this.tbx_SC_FPath = new System.Windows.Forms.TextBox();
            this.tabSAVar = new System.Windows.Forms.TabPage();
            this.pnlSAVar = new System.Windows.Forms.Panel();
            this.btn_SV_Open = new System.Windows.Forms.Button();
            this.lb_SV_SAID = new System.Windows.Forms.Label();
            this.btn_SV_Import = new System.Windows.Forms.Button();
            this.tbx_SV_SAID = new System.Windows.Forms.TextBox();
            this.btn_SV_View = new System.Windows.Forms.Button();
            this.lb_SV_FPath = new System.Windows.Forms.Label();
            this.tbx_SV_FPath = new System.Windows.Forms.TextBox();
            this.tabSTDParts = new System.Windows.Forms.TabPage();
            this.pnlSTDParts = new System.Windows.Forms.Panel();
            this.btn_STD_Open = new System.Windows.Forms.Button();
            this.btn_STD_Import = new System.Windows.Forms.Button();
            this.btn_STD_View = new System.Windows.Forms.Button();
            this.lb_STD_Fpath = new System.Windows.Forms.Label();
            this.tbx_STD_FPath = new System.Windows.Forms.TextBox();
            this.tabBOMPlan = new System.Windows.Forms.TabPage();
            this.pnlBOMPlan = new System.Windows.Forms.Panel();
            this.btn_BP_Open = new System.Windows.Forms.Button();
            this.lb_BP_PlanName = new System.Windows.Forms.Label();
            this.btn_BP_Import = new System.Windows.Forms.Button();
            this.tbx_BP_PlanName = new System.Windows.Forms.TextBox();
            this.btn_BP_View = new System.Windows.Forms.Button();
            this.lb_BP_FPath = new System.Windows.Forms.Label();
            this.tbx_BP_FPath = new System.Windows.Forms.TextBox();
            this.menuSEDPlan = new System.Windows.Forms.MenuStrip();
            this.reportViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lbErr = new System.Windows.Forms.Label();
            this.ImportTabs.SuspendLayout();
            this.tabSACmpn.SuspendLayout();
            this.pnlSACmpn.SuspendLayout();
            this.tabSAVar.SuspendLayout();
            this.pnlSAVar.SuspendLayout();
            this.tabSTDParts.SuspendLayout();
            this.pnlSTDParts.SuspendLayout();
            this.tabBOMPlan.SuspendLayout();
            this.pnlBOMPlan.SuspendLayout();
            this.menuSEDPlan.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportTabs
            // 
            this.ImportTabs.Controls.Add(this.tabSACmpn);
            this.ImportTabs.Controls.Add(this.tabSAVar);
            this.ImportTabs.Controls.Add(this.tabSTDParts);
            this.ImportTabs.Controls.Add(this.tabBOMPlan);
            this.ImportTabs.Location = new System.Drawing.Point(0, 35);
            this.ImportTabs.Name = "ImportTabs";
            this.ImportTabs.SelectedIndex = 0;
            this.ImportTabs.Size = new System.Drawing.Size(762, 318);
            this.ImportTabs.TabIndex = 0;
            // 
            // tabSACmpn
            // 
            this.tabSACmpn.Controls.Add(this.pnlSACmpn);
            this.tabSACmpn.Location = new System.Drawing.Point(4, 22);
            this.tabSACmpn.Name = "tabSACmpn";
            this.tabSACmpn.Padding = new System.Windows.Forms.Padding(3);
            this.tabSACmpn.Size = new System.Drawing.Size(754, 292);
            this.tabSACmpn.TabIndex = 0;
            this.tabSACmpn.Text = "SA Component";
            this.tabSACmpn.UseVisualStyleBackColor = true;
            // 
            // pnlSACmpn
            // 
            this.pnlSACmpn.Controls.Add(this.btn_SC_Open);
            this.pnlSACmpn.Controls.Add(this.lb_SC_SAID);
            this.pnlSACmpn.Controls.Add(this.btn_SC_Import);
            this.pnlSACmpn.Controls.Add(this.tbx_SC_SAID);
            this.pnlSACmpn.Controls.Add(this.btn_SC_View);
            this.pnlSACmpn.Controls.Add(this.lb_SC_FPath);
            this.pnlSACmpn.Controls.Add(this.tbx_SC_FPath);
            this.pnlSACmpn.Location = new System.Drawing.Point(-4, -2);
            this.pnlSACmpn.Name = "pnlSACmpn";
            this.pnlSACmpn.Size = new System.Drawing.Size(762, 377);
            this.pnlSACmpn.TabIndex = 7;
            // 
            // btn_SC_Open
            // 
            this.btn_SC_Open.Location = new System.Drawing.Point(598, 90);
            this.btn_SC_Open.Name = "btn_SC_Open";
            this.btn_SC_Open.Size = new System.Drawing.Size(75, 25);
            this.btn_SC_Open.TabIndex = 7;
            this.btn_SC_Open.Text = "Open";
            this.btn_SC_Open.UseVisualStyleBackColor = true;
            this.btn_SC_Open.Click += new System.EventHandler(this.btn_SC_Open_Click);
            // 
            // lb_SC_SAID
            // 
            this.lb_SC_SAID.AutoSize = true;
            this.lb_SC_SAID.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SC_SAID.Location = new System.Drawing.Point(19, 28);
            this.lb_SC_SAID.Name = "lb_SC_SAID";
            this.lb_SC_SAID.Size = new System.Drawing.Size(55, 14);
            this.lb_SC_SAID.TabIndex = 1;
            this.lb_SC_SAID.Text = "SA ID:";
            // 
            // btn_SC_Import
            // 
            this.btn_SC_Import.Location = new System.Drawing.Point(136, 191);
            this.btn_SC_Import.Name = "btn_SC_Import";
            this.btn_SC_Import.Size = new System.Drawing.Size(100, 27);
            this.btn_SC_Import.TabIndex = 6;
            this.btn_SC_Import.Text = "Import";
            this.btn_SC_Import.UseVisualStyleBackColor = true;
            this.btn_SC_Import.Click += new System.EventHandler(this.btn_SC_Import_Click);
            // 
            // tbx_SC_SAID
            // 
            this.tbx_SC_SAID.Location = new System.Drawing.Point(112, 26);
            this.tbx_SC_SAID.Name = "tbx_SC_SAID";
            this.tbx_SC_SAID.Size = new System.Drawing.Size(100, 20);
            this.tbx_SC_SAID.TabIndex = 0;
            // 
            // btn_SC_View
            // 
            this.btn_SC_View.Location = new System.Drawing.Point(22, 191);
            this.btn_SC_View.Name = "btn_SC_View";
            this.btn_SC_View.Size = new System.Drawing.Size(100, 27);
            this.btn_SC_View.TabIndex = 5;
            this.btn_SC_View.Text = "View";
            this.btn_SC_View.UseVisualStyleBackColor = true;
            this.btn_SC_View.Click += new System.EventHandler(this.btn_SC_View_Click);
            // 
            // lb_SC_FPath
            // 
            this.lb_SC_FPath.AutoSize = true;
            this.lb_SC_FPath.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SC_FPath.Location = new System.Drawing.Point(19, 92);
            this.lb_SC_FPath.Name = "lb_SC_FPath";
            this.lb_SC_FPath.Size = new System.Drawing.Size(87, 14);
            this.lb_SC_FPath.TabIndex = 3;
            this.lb_SC_FPath.Text = "File Path:";
            // 
            // tbx_SC_FPath
            // 
            this.tbx_SC_FPath.Location = new System.Drawing.Point(112, 90);
            this.tbx_SC_FPath.Name = "tbx_SC_FPath";
            this.tbx_SC_FPath.Size = new System.Drawing.Size(480, 20);
            this.tbx_SC_FPath.TabIndex = 4;
            // 
            // tabSAVar
            // 
            this.tabSAVar.Controls.Add(this.pnlSAVar);
            this.tabSAVar.Location = new System.Drawing.Point(4, 22);
            this.tabSAVar.Name = "tabSAVar";
            this.tabSAVar.Padding = new System.Windows.Forms.Padding(3);
            this.tabSAVar.Size = new System.Drawing.Size(754, 292);
            this.tabSAVar.TabIndex = 1;
            this.tabSAVar.Text = "SA Variables";
            this.tabSAVar.UseVisualStyleBackColor = true;
            // 
            // pnlSAVar
            // 
            this.pnlSAVar.Controls.Add(this.btn_SV_Open);
            this.pnlSAVar.Controls.Add(this.lb_SV_SAID);
            this.pnlSAVar.Controls.Add(this.btn_SV_Import);
            this.pnlSAVar.Controls.Add(this.tbx_SV_SAID);
            this.pnlSAVar.Controls.Add(this.btn_SV_View);
            this.pnlSAVar.Controls.Add(this.lb_SV_FPath);
            this.pnlSAVar.Controls.Add(this.tbx_SV_FPath);
            this.pnlSAVar.Location = new System.Drawing.Point(-4, -2);
            this.pnlSAVar.Name = "pnlSAVar";
            this.pnlSAVar.Size = new System.Drawing.Size(762, 377);
            this.pnlSAVar.TabIndex = 8;
            // 
            // btn_SV_Open
            // 
            this.btn_SV_Open.Location = new System.Drawing.Point(598, 89);
            this.btn_SV_Open.Name = "btn_SV_Open";
            this.btn_SV_Open.Size = new System.Drawing.Size(75, 25);
            this.btn_SV_Open.TabIndex = 8;
            this.btn_SV_Open.Text = "Open";
            this.btn_SV_Open.UseVisualStyleBackColor = true;
            this.btn_SV_Open.Click += new System.EventHandler(this.btn_SV_Open_Click);
            // 
            // lb_SV_SAID
            // 
            this.lb_SV_SAID.AutoSize = true;
            this.lb_SV_SAID.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SV_SAID.Location = new System.Drawing.Point(19, 28);
            this.lb_SV_SAID.Name = "lb_SV_SAID";
            this.lb_SV_SAID.Size = new System.Drawing.Size(55, 14);
            this.lb_SV_SAID.TabIndex = 1;
            this.lb_SV_SAID.Text = "SA ID:";
            // 
            // btn_SV_Import
            // 
            this.btn_SV_Import.Location = new System.Drawing.Point(136, 191);
            this.btn_SV_Import.Name = "btn_SV_Import";
            this.btn_SV_Import.Size = new System.Drawing.Size(100, 27);
            this.btn_SV_Import.TabIndex = 6;
            this.btn_SV_Import.Text = "Import";
            this.btn_SV_Import.UseVisualStyleBackColor = true;
            this.btn_SV_Import.Click += new System.EventHandler(this.btn_SV_Import_Click);
            // 
            // tbx_SV_SAID
            // 
            this.tbx_SV_SAID.Location = new System.Drawing.Point(112, 26);
            this.tbx_SV_SAID.Name = "tbx_SV_SAID";
            this.tbx_SV_SAID.Size = new System.Drawing.Size(100, 20);
            this.tbx_SV_SAID.TabIndex = 0;
            // 
            // btn_SV_View
            // 
            this.btn_SV_View.Location = new System.Drawing.Point(22, 191);
            this.btn_SV_View.Name = "btn_SV_View";
            this.btn_SV_View.Size = new System.Drawing.Size(100, 27);
            this.btn_SV_View.TabIndex = 5;
            this.btn_SV_View.Text = "View";
            this.btn_SV_View.UseVisualStyleBackColor = true;
            this.btn_SV_View.Click += new System.EventHandler(this.btn_SV_View_Click);
            // 
            // lb_SV_FPath
            // 
            this.lb_SV_FPath.AutoSize = true;
            this.lb_SV_FPath.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SV_FPath.Location = new System.Drawing.Point(19, 93);
            this.lb_SV_FPath.Name = "lb_SV_FPath";
            this.lb_SV_FPath.Size = new System.Drawing.Size(87, 14);
            this.lb_SV_FPath.TabIndex = 3;
            this.lb_SV_FPath.Text = "File Path:";
            // 
            // tbx_SV_FPath
            // 
            this.tbx_SV_FPath.Location = new System.Drawing.Point(112, 91);
            this.tbx_SV_FPath.Name = "tbx_SV_FPath";
            this.tbx_SV_FPath.Size = new System.Drawing.Size(480, 20);
            this.tbx_SV_FPath.TabIndex = 4;
            // 
            // tabSTDParts
            // 
            this.tabSTDParts.Controls.Add(this.pnlSTDParts);
            this.tabSTDParts.Location = new System.Drawing.Point(4, 22);
            this.tabSTDParts.Name = "tabSTDParts";
            this.tabSTDParts.Padding = new System.Windows.Forms.Padding(3);
            this.tabSTDParts.Size = new System.Drawing.Size(754, 292);
            this.tabSTDParts.TabIndex = 3;
            this.tabSTDParts.Text = "Standard Parts";
            this.tabSTDParts.UseVisualStyleBackColor = true;
            // 
            // pnlSTDParts
            // 
            this.pnlSTDParts.Controls.Add(this.btn_STD_Open);
            this.pnlSTDParts.Controls.Add(this.btn_STD_Import);
            this.pnlSTDParts.Controls.Add(this.btn_STD_View);
            this.pnlSTDParts.Controls.Add(this.lb_STD_Fpath);
            this.pnlSTDParts.Controls.Add(this.tbx_STD_FPath);
            this.pnlSTDParts.Location = new System.Drawing.Point(-4, -2);
            this.pnlSTDParts.Name = "pnlSTDParts";
            this.pnlSTDParts.Size = new System.Drawing.Size(762, 377);
            this.pnlSTDParts.TabIndex = 8;
            // 
            // btn_STD_Open
            // 
            this.btn_STD_Open.Location = new System.Drawing.Point(598, 89);
            this.btn_STD_Open.Name = "btn_STD_Open";
            this.btn_STD_Open.Size = new System.Drawing.Size(75, 25);
            this.btn_STD_Open.TabIndex = 8;
            this.btn_STD_Open.Text = "Open";
            this.btn_STD_Open.UseVisualStyleBackColor = true;
            this.btn_STD_Open.Click += new System.EventHandler(this.btn_STD_Open_Click);
            // 
            // btn_STD_Import
            // 
            this.btn_STD_Import.Location = new System.Drawing.Point(136, 191);
            this.btn_STD_Import.Name = "btn_STD_Import";
            this.btn_STD_Import.Size = new System.Drawing.Size(100, 27);
            this.btn_STD_Import.TabIndex = 6;
            this.btn_STD_Import.Text = "Import";
            this.btn_STD_Import.UseVisualStyleBackColor = true;
            this.btn_STD_Import.Click += new System.EventHandler(this.btn_STD_Import_Click);
            // 
            // btn_STD_View
            // 
            this.btn_STD_View.Location = new System.Drawing.Point(22, 191);
            this.btn_STD_View.Name = "btn_STD_View";
            this.btn_STD_View.Size = new System.Drawing.Size(100, 27);
            this.btn_STD_View.TabIndex = 5;
            this.btn_STD_View.Text = "View";
            this.btn_STD_View.UseVisualStyleBackColor = true;
            this.btn_STD_View.Click += new System.EventHandler(this.btn_STD_View_Click);
            // 
            // lb_STD_Fpath
            // 
            this.lb_STD_Fpath.AutoSize = true;
            this.lb_STD_Fpath.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_STD_Fpath.Location = new System.Drawing.Point(19, 93);
            this.lb_STD_Fpath.Name = "lb_STD_Fpath";
            this.lb_STD_Fpath.Size = new System.Drawing.Size(87, 14);
            this.lb_STD_Fpath.TabIndex = 3;
            this.lb_STD_Fpath.Text = "File Path:";
            // 
            // tbx_STD_FPath
            // 
            this.tbx_STD_FPath.Location = new System.Drawing.Point(112, 91);
            this.tbx_STD_FPath.Name = "tbx_STD_FPath";
            this.tbx_STD_FPath.Size = new System.Drawing.Size(480, 20);
            this.tbx_STD_FPath.TabIndex = 4;
            // 
            // tabBOMPlan
            // 
            this.tabBOMPlan.Controls.Add(this.pnlBOMPlan);
            this.tabBOMPlan.Location = new System.Drawing.Point(4, 22);
            this.tabBOMPlan.Name = "tabBOMPlan";
            this.tabBOMPlan.Padding = new System.Windows.Forms.Padding(3);
            this.tabBOMPlan.Size = new System.Drawing.Size(754, 292);
            this.tabBOMPlan.TabIndex = 4;
            this.tabBOMPlan.Text = "BOM Plan";
            this.tabBOMPlan.UseVisualStyleBackColor = true;
            // 
            // pnlBOMPlan
            // 
            this.pnlBOMPlan.Controls.Add(this.btn_BP_Open);
            this.pnlBOMPlan.Controls.Add(this.lb_BP_PlanName);
            this.pnlBOMPlan.Controls.Add(this.btn_BP_Import);
            this.pnlBOMPlan.Controls.Add(this.tbx_BP_PlanName);
            this.pnlBOMPlan.Controls.Add(this.btn_BP_View);
            this.pnlBOMPlan.Controls.Add(this.lb_BP_FPath);
            this.pnlBOMPlan.Controls.Add(this.tbx_BP_FPath);
            this.pnlBOMPlan.Location = new System.Drawing.Point(-4, -2);
            this.pnlBOMPlan.Name = "pnlBOMPlan";
            this.pnlBOMPlan.Size = new System.Drawing.Size(762, 377);
            this.pnlBOMPlan.TabIndex = 8;
            // 
            // btn_BP_Open
            // 
            this.btn_BP_Open.Location = new System.Drawing.Point(598, 89);
            this.btn_BP_Open.Name = "btn_BP_Open";
            this.btn_BP_Open.Size = new System.Drawing.Size(75, 25);
            this.btn_BP_Open.TabIndex = 8;
            this.btn_BP_Open.Text = "Open";
            this.btn_BP_Open.UseVisualStyleBackColor = true;
            this.btn_BP_Open.Click += new System.EventHandler(this.btn_BP_Open_Click);
            // 
            // lb_BP_PlanName
            // 
            this.lb_BP_PlanName.AutoSize = true;
            this.lb_BP_PlanName.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_BP_PlanName.Location = new System.Drawing.Point(19, 28);
            this.lb_BP_PlanName.Name = "lb_BP_PlanName";
            this.lb_BP_PlanName.Size = new System.Drawing.Size(87, 14);
            this.lb_BP_PlanName.TabIndex = 1;
            this.lb_BP_PlanName.Text = "Plan Name:";
            // 
            // btn_BP_Import
            // 
            this.btn_BP_Import.Location = new System.Drawing.Point(136, 191);
            this.btn_BP_Import.Name = "btn_BP_Import";
            this.btn_BP_Import.Size = new System.Drawing.Size(100, 27);
            this.btn_BP_Import.TabIndex = 6;
            this.btn_BP_Import.Text = "Import";
            this.btn_BP_Import.UseVisualStyleBackColor = true;
            this.btn_BP_Import.Click += new System.EventHandler(this.btn_BP_Import_Click);
            // 
            // tbx_BP_PlanName
            // 
            this.tbx_BP_PlanName.Location = new System.Drawing.Point(112, 26);
            this.tbx_BP_PlanName.Name = "tbx_BP_PlanName";
            this.tbx_BP_PlanName.Size = new System.Drawing.Size(100, 20);
            this.tbx_BP_PlanName.TabIndex = 0;
            // 
            // btn_BP_View
            // 
            this.btn_BP_View.Location = new System.Drawing.Point(22, 191);
            this.btn_BP_View.Name = "btn_BP_View";
            this.btn_BP_View.Size = new System.Drawing.Size(100, 27);
            this.btn_BP_View.TabIndex = 5;
            this.btn_BP_View.Text = "View";
            this.btn_BP_View.UseVisualStyleBackColor = true;
            this.btn_BP_View.Click += new System.EventHandler(this.btn_BP_View_Click);
            // 
            // lb_BP_FPath
            // 
            this.lb_BP_FPath.AutoSize = true;
            this.lb_BP_FPath.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_BP_FPath.Location = new System.Drawing.Point(19, 93);
            this.lb_BP_FPath.Name = "lb_BP_FPath";
            this.lb_BP_FPath.Size = new System.Drawing.Size(87, 14);
            this.lb_BP_FPath.TabIndex = 3;
            this.lb_BP_FPath.Text = "File Path:";
            // 
            // tbx_BP_FPath
            // 
            this.tbx_BP_FPath.Location = new System.Drawing.Point(112, 91);
            this.tbx_BP_FPath.Name = "tbx_BP_FPath";
            this.tbx_BP_FPath.Size = new System.Drawing.Size(480, 20);
            this.tbx_BP_FPath.TabIndex = 4;
            // 
            // menuSEDPlan
            // 
            this.menuSEDPlan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportViewerToolStripMenuItem});
            this.menuSEDPlan.Location = new System.Drawing.Point(0, 0);
            this.menuSEDPlan.Name = "menuSEDPlan";
            this.menuSEDPlan.Size = new System.Drawing.Size(762, 24);
            this.menuSEDPlan.TabIndex = 1;
            this.menuSEDPlan.Text = "menuStrip1";
            // 
            // reportViewerToolStripMenuItem
            // 
            this.reportViewerToolStripMenuItem.Name = "reportViewerToolStripMenuItem";
            this.reportViewerToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.reportViewerToolStripMenuItem.Text = "ReportViewer";
            this.reportViewerToolStripMenuItem.Click += new System.EventHandler(this.reportViewerToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // lbErr
            // 
            this.lbErr.AutoSize = true;
            this.lbErr.Location = new System.Drawing.Point(19, 374);
            this.lbErr.Name = "lbErr";
            this.lbErr.Size = new System.Drawing.Size(0, 13);
            this.lbErr.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 442);
            this.Controls.Add(this.lbErr);
            this.Controls.Add(this.ImportTabs);
            this.Controls.Add(this.menuSEDPlan);
            this.MainMenuStrip = this.menuSEDPlan;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ImportTabs.ResumeLayout(false);
            this.tabSACmpn.ResumeLayout(false);
            this.pnlSACmpn.ResumeLayout(false);
            this.pnlSACmpn.PerformLayout();
            this.tabSAVar.ResumeLayout(false);
            this.pnlSAVar.ResumeLayout(false);
            this.pnlSAVar.PerformLayout();
            this.tabSTDParts.ResumeLayout(false);
            this.pnlSTDParts.ResumeLayout(false);
            this.pnlSTDParts.PerformLayout();
            this.tabBOMPlan.ResumeLayout(false);
            this.pnlBOMPlan.ResumeLayout(false);
            this.pnlBOMPlan.PerformLayout();
            this.menuSEDPlan.ResumeLayout(false);
            this.menuSEDPlan.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl ImportTabs;
        private System.Windows.Forms.TabPage tabSACmpn;
        private System.Windows.Forms.TabPage tabSAVar;
        private System.Windows.Forms.TabPage tabSTDParts;
        private System.Windows.Forms.Label lb_SC_SAID;
        private System.Windows.Forms.TextBox tbx_SC_SAID;
        private System.Windows.Forms.TabPage tabBOMPlan;
        private System.Windows.Forms.TextBox tbx_SC_FPath;
        private System.Windows.Forms.Label lb_SC_FPath;
        private System.Windows.Forms.Button btn_SC_Import;
        private System.Windows.Forms.Button btn_SC_View;
        private System.Windows.Forms.Panel pnlSACmpn;
        private System.Windows.Forms.Panel pnlSAVar;
        private System.Windows.Forms.Label lb_SV_SAID;
        private System.Windows.Forms.Button btn_SV_Import;
        private System.Windows.Forms.TextBox tbx_SV_SAID;
        private System.Windows.Forms.Button btn_SV_View;
        private System.Windows.Forms.Label lb_SV_FPath;
        private System.Windows.Forms.TextBox tbx_SV_FPath;
        private System.Windows.Forms.Panel pnlSTDParts;
        private System.Windows.Forms.Button btn_STD_Import;
        private System.Windows.Forms.Button btn_STD_View;
        private System.Windows.Forms.Label lb_STD_Fpath;
        private System.Windows.Forms.TextBox tbx_STD_FPath;
        private System.Windows.Forms.Panel pnlBOMPlan;
        private System.Windows.Forms.Label lb_BP_PlanName;
        private System.Windows.Forms.Button btn_BP_Import;
        private System.Windows.Forms.TextBox tbx_BP_PlanName;
        private System.Windows.Forms.Button btn_BP_View;
        private System.Windows.Forms.Label lb_BP_FPath;
        private System.Windows.Forms.TextBox tbx_BP_FPath;
        private System.Windows.Forms.MenuStrip menuSEDPlan;
        private System.Windows.Forms.ToolStripMenuItem reportViewerToolStripMenuItem;
        private System.Windows.Forms.Button btn_SC_Open;
        private System.Windows.Forms.Button btn_SV_Open;
        private System.Windows.Forms.Button btn_STD_Open;
        private System.Windows.Forms.Button btn_BP_Open;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lbErr;
    }
}

