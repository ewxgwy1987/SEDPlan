using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SEDPlan
{
    public partial class Management : Form
    {
        #region Class Field and Property

        private const string col_SAPrssID = "Process_ID";
        private const string col_SAPrssName = "Process_Name";
        private const string col_ProjNo = "ProjectNo";
        private const string col_ProjName = "ProjectName";

        private ErrorProvider errProvider;
        Icon ErrIcon;
        Icon InfoIcon;

        private string sqlconn_str;
        private DataTable dt_SAProcess;
        private DataTable dt_ProjInfo;
        private ManagementDataProcess mgtDataProcess;

        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructor, Dispose, Finalize and Destructor

        public Management(string connstr)
        {
            InitializeComponent();
            this.sqlconn_str = connstr;

            errProvider = new ErrorProvider();
            errProvider.SetError(this.lbErr, string.Empty);
            ErrIcon = this.errProvider.Icon;
            InfoIcon = getInfoIcon();
        }

        #endregion

        #region Member Function
        private void Management_Load(object sender, EventArgs e)
        {
            mgtDataProcess = new ManagementDataProcess(sqlconn_str);
            dt_SAProcess = mgtDataProcess.GetSAProcess();
            dt_ProjInfo = mgtDataProcess.GetProjectInfo();

            this.dgvSAProcess.DataSource = dt_SAProcess;
            this.dgvProjectInfo.DataSource = dt_ProjInfo;
        }

        #region Error Info Display
        private Icon getInfoIcon()
        {
            Size iconSize = SystemInformation.SmallIconSize;
            Bitmap bitmap = new Bitmap(iconSize.Width, iconSize.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(SystemIcons.Information.ToBitmap(), new Rectangle(Point.Empty, iconSize));
            }

            Icon smallerInfoIcon = Icon.FromHandle(bitmap.GetHicon());
            return smallerInfoIcon;
        }

        private void ShowError(string errormsg)
        {
            errProvider.SetError(this.lbErr, string.Empty);
            this.lbErr.Text = errormsg;
            errProvider.Icon = this.ErrIcon;
            errProvider.SetIconAlignment(lbErr, ErrorIconAlignment.MiddleLeft);
            errProvider.SetError(lbErr, errormsg);
        }
        

        private void ShowInfo(string infomsg)
        {
            errProvider.SetError(this.lbErr, string.Empty);
            this.lbErr.Text = infomsg;
            errProvider.Icon = this.InfoIcon;
            errProvider.SetIconAlignment(lbErr, ErrorIconAlignment.MiddleLeft);
            errProvider.SetError(lbErr, infomsg);
        }
        #endregion

        #region Event Handler

        private void btnAddSAProcess_Click(object sender, EventArgs e)
        {
            this.btnAddSAProcess.Enabled = false;

            if (mgtDataProcess.AddSAProcess(this.tbxSAPrssName.Text.Trim()))
            {
                ShowInfo("Add the new SA Process(" + this.tbxSAPrssName.Text.Trim() + ") successfully");
                this.dgvSAProcess.DataSource = mgtDataProcess.GetSAProcess();
                
                int lstidx = this.dgvSAProcess.Rows.Count;
                this.dgvSAProcess.Rows[lstidx - 1].Selected = true;
                this.dgvSAProcess.FirstDisplayedScrollingRowIndex = lstidx - 1;
            }
            else
                ShowError("Cannot add the new SA Process(" + this.tbxSAPrssName.Text.Trim() + ")");

            this.btnAddSAProcess.Enabled = true;
        }

        private void btnDeleteSAProcess_Click(object sender, EventArgs e)
        {
            this.btnDeleteSAProcess.Enabled = false;

            int sltidx = this.dgvSAProcess.SelectedRows[0].Index;
            int prssid = Convert.ToInt32(this.dgvSAProcess.SelectedRows[0].Cells[col_SAPrssID].Value);
            string prssname = Convert.ToString(this.dgvSAProcess.SelectedRows[0].Cells[col_SAPrssName].Value);

            if (mgtDataProcess.SAPrssExists(prssid))
            {
                if(mgtDataProcess.DeleteSAProcess(prssid))
                {
                    ShowInfo("Delete the SA Process(" + prssid + ":" + prssname + ") successfully");
                    this.dgvSAProcess.DataSource = mgtDataProcess.GetSAProcess();

                    if (sltidx > this.dgvSAProcess.Rows.Count - 1)
                        sltidx = this.dgvSAProcess.Rows.Count - 1;
                    this.dgvSAProcess.Rows[sltidx].Selected = true;
                    this.dgvSAProcess.FirstDisplayedScrollingRowIndex = sltidx;
                }
                else
                    ShowError("Cannot delete the SA Process(" + prssid + ":" + prssname + ")");
            }
            else
                ShowError("The SA Process(" + prssid + ":" + prssname + ") does not exist");


            this.btnDeleteSAProcess.Enabled = true;
        }

        private void btnAddProjectInfo_Click(object sender, EventArgs e)
        {
            this.btnAddProjectInfo.Enabled = false;

            string projno = this.tbxProjectNo.Text.Trim();
            string projname = this.tbxProjectName.Text.Trim();

            if (mgtDataProcess.AddProject(projno, projname))
            {
                ShowInfo("Add the new Project(" + projno + ":" + projname + ") successfully");
                this.dgvProjectInfo.DataSource = mgtDataProcess.GetProjectInfo();

                int lstidx = this.dgvProjectInfo.Rows.Count;
                this.dgvProjectInfo.Rows[lstidx - 1].Selected = true;
                this.dgvProjectInfo.FirstDisplayedScrollingRowIndex = lstidx - 1;
            }
            else
                ShowError("Cannot add the new Project(" + projno + ":" + projname + ")");

            this.btnAddProjectInfo.Enabled = true;
        }

        private void btnDeleteProjectInfo_Click(object sender, EventArgs e)
        {
            this.btnDeleteProjectInfo.Enabled = false;

            int sltidx = this.dgvProjectInfo.SelectedRows[0].Index;
            string projno = Convert.ToString(this.dgvProjectInfo.SelectedRows[0].Cells[col_ProjNo].Value);
            string projname = Convert.ToString(this.dgvProjectInfo.SelectedRows[0].Cells[col_ProjName].Value);

            if (mgtDataProcess.ProjectExists(projno))
            {
                if (mgtDataProcess.DeleteProject(projno))
                {
                    ShowInfo("Delete the Project(" + projno + ":" + projname + ") successfully");
                    this.dgvProjectInfo.DataSource = mgtDataProcess.GetProjectInfo();

                    if (sltidx > this.dgvProjectInfo.Rows.Count - 1)
                        sltidx = this.dgvProjectInfo.Rows.Count - 1;
                    this.dgvProjectInfo.Rows[sltidx].Selected = true;
                    this.dgvProjectInfo.FirstDisplayedScrollingRowIndex = sltidx;
                }
                else
                    ShowError("Cannot delete the Project(" + projno + ":" + projname + ")");
            }
            else
                ShowError("The Project(" + projno + ":" + projname + ") does not exist");


            this.btnDeleteProjectInfo.Enabled = true;
        }

        private void dgvSAProcess_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int sltidx = e.RowIndex;
            int prssid = Convert.ToInt32(this.dgvSAProcess.Rows[e.RowIndex].Cells[col_SAPrssID].Value);
            string prssname = Convert.ToString(this.dgvSAProcess.Rows[e.RowIndex].Cells[col_SAPrssName].Value);

            if (mgtDataProcess.SAPrssExists(prssid))
            {
                if (mgtDataProcess.UpdateSAProcess(prssid, prssname))
                {
                    ShowInfo("Update the SA Process(" + prssid + ":" + prssname + ") successfully");
                }
                else
                    ShowError("Cannot update the SA Process(" + prssid + ":" + prssname + ")");
            }
            else
                ShowError("The SA Process(" + prssid + ":" + prssname + ") does not exist");

            this.dgvSAProcess.DataSource = mgtDataProcess.GetSAProcess();

            //if (sltidx > this.dgvSAProcess.Rows.Count - 1)
            //    sltidx = this.dgvSAProcess.Rows.Count - 1;
            //this.dgvSAProcess.Rows[0].Selected = true;
            //this.dgvSAProcess.FirstDisplayedScrollingRowIndex = 1;
        }

        private void dgvProjectInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int sltidx = e.RowIndex;
            string projno = Convert.ToString(this.dgvProjectInfo.Rows[e.RowIndex].Cells[col_ProjNo].Value);
            string projname = Convert.ToString(this.dgvProjectInfo.Rows[e.RowIndex].Cells[col_ProjName].Value);

            if (mgtDataProcess.ProjectExists(projno))
            {
                if (mgtDataProcess.UpdateProject(projno, projname))
                {
                    ShowInfo("Update the Project(" + projno + ":" + projname + ") successfully");
                }
                else
                    ShowError("Cannot update the Project(" + projno + ":" + projname + ")");
            }
            else
                ShowError("The Project(" + projno + ":" + projname + ") does not exist");

            this.dgvProjectInfo.DataSource = mgtDataProcess.GetProjectInfo();

            //if (sltidx > this.dgvProjectInfo.Rows.Count - 1)
            //    sltidx = this.dgvProjectInfo.Rows.Count - 1;
            //this.dgvProjectInfo.Rows[sltidx].Selected = true;
            //this.dgvProjectInfo.FirstDisplayedScrollingRowIndex = sltidx;
        }

        #endregion

        #endregion
    }
}
