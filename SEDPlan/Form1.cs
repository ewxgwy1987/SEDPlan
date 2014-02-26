using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml;
using PALS;

namespace SEDPlan
{
    public partial class Form1 : Form
    {
        #region Class Field and Property

        private const string Path_XMLFileSetting = @"../../../cfg/CFG_SEDPlan.xml";
        private const string Path_XMLFileSetting2 = @"./cfg/CFG_MDSReader.xml";
        private const string XCFG_CONNSTRING = "connectionString";
        private const string XCFG_SQL_cmbx_SC_SAID = "sql-cmbx_SC_SAID";

        private string connstr;
        private string sql_cmbx_SC_SAID;
        private ExcelView xlsview;
        private ExcelDataImport xlsimport;

        private ErrorProvider errProvider;

        // SC
        private DataTable dt_SCImport;
        private string SC_xlssheet;
        private string SC_filepath;

        // SV
        private DataTable dt_SVImport;
        private string SV_xlssheet;
        private string SV_filepath;

        // STD
        private DataTable dt_STDImport;
        private string STD_xlssheet;
        private string STD_filepath;

        // BOM Plan
        private DataTable dt_BPImport;
        private string BP_xlssheet;
        private string BP_filepath;

        Icon ErrIcon;
        Icon InfoIcon;

        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public Form1()
        {
            InitializeComponent();
            errProvider = new ErrorProvider();
            errProvider.SetError(this.lbErr, string.Empty);
            ErrIcon = this.errProvider.Icon;
            InfoIcon = getInfoIcon();
        }

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

       
        private void reportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("PALS.ReportViewer.exe");
        }

        private void btn_SC_Open_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
            this.tbx_SC_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_SV_Open_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
            this.tbx_DDV_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_STD_Open_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
            this.tbx_STD_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_BP_Open_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
            this.tbx_BP_FPath.Text = this.openFileDialog.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sEDPLANDataSet.SA_Component' table. You can move, or remove it, as needed.
            this.sA_ComponentTableAdapter.Fill(this.sEDPLANDataSet.SA_Component);
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                // Load the log4net configuration
                
                FileInfo FI_Setting = new FileInfo(Path_XMLFileSetting);
                if (!FI_Setting.Exists)
                {
                    FI_Setting = new FileInfo(Path_XMLFileSetting2);
                    if (!FI_Setting.Exists)
                    {
                        errstr += "Cannot find configuration file.";
                        _logger.Error(errstr);
                        return;
                    }
                }
                XmlElement xmlRoot = PALS.Utilities.XMLConfig.GetConfigFileRootElement(ref FI_Setting);
                XmlElement log4netConfig = (XmlElement)PALS.Utilities.XMLConfig.GetConfigSetElement(ref xmlRoot, "log4net");
                log4net.Config.XmlConfigurator.Configure(log4netConfig);

                this.connstr = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xmlRoot, XCFG_CONNSTRING, "");
                this.sql_cmbx_SC_SAID = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xmlRoot, XCFG_SQL_cmbx_SC_SAID, "");

                // Init view function
                xlsview = new ExcelView();
                xlsview.OnErrorHappened += new EventHandler<ErrorEventArgs>(ExcelView_OnError);

                // Init import function
                XmlNode xcfg_ExcelDataImport = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xmlRoot, "configSet","name","SEDPlan.ExcelDataImport");
                xlsimport = new ExcelDataImport(xcfg_ExcelDataImport, this.connstr);
                xlsimport.OnErrorHappened += new EventHandler<ErrorEventArgs>(ExcelView_OnError);

                // test
                this.tbx_DDV_SAID.Text = "MS01-17-301";
                this.tbx_DDV_FPath.Text = "../../../excel/Values Chart for Variables A degrees_ L and LS dated 08112013.xls";

                this.tbx_STD_FPath.Text = "../../../excel/Standard Parts.xlsx";

                this.tbx_FW_FPath.Text = "../../../excel/Detail Design Fixed Weight.xlsx"

                this.cmbx_SC_SAID.Text = "SA-Test1"; // SA-Test2 SA-Test3
                this.tbx_SC_FPath.Text = "../../../excel/Bill Of Material dated 14102013.xls";

                this.tbx_BP_PlanName.Text = "BOMPlanTest";
                this.tbx_BP_FPath.Text = "../../../excel/BomPlan.xlsx";

            }
            catch(Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                errProvider.SetError(this.lbErr, exp.Message);
            }


        }

        private void btn_SC_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_SC_FPath.Text.Trim() != "" && this.cmbx_SC_SAID.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_SC_FPath.Text.Trim(), this.cmbx_SC_SAID.Text.Trim());
                
                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_SCImport = xlsview.ImportData;
                    this.SC_xlssheet = xlsview.SheetName;
                    this.SC_filepath = xlsview.FilePath;
                }
            }
            else
            {
                if (this.cmbx_SC_SAID.Text == "")
                    ShowError("Please provide a SA ID");
                else if (this.tbx_SC_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_SV_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_DDV_FPath.Text.Trim() != "" && this.tbx_DDV_SAID.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_DDV_FPath.Text.Trim(), this.tbx_DDV_SAID.Text.Trim());

                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_SVImport = xlsview.ImportData;
                    this.SV_xlssheet = xlsview.SheetName;
                    this.SV_filepath = xlsview.FilePath;
                }
            }
            else
            {
                if (this.tbx_DDV_SAID.Text == "")
                    ShowError("Please provide a SA ID");
                else if (this.tbx_DDV_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_STD_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_STD_FPath.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_STD_FPath.Text.Trim(), "");

                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_STDImport = xlsview.ImportData;
                    this.STD_xlssheet = xlsview.SheetName;
                    this.STD_filepath = xlsview.FilePath;
                }
            }
            else
            {
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_BP_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_BP_FPath.Text.Trim() != "" && this.tbx_BP_PlanName.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_BP_FPath.Text.Trim(), this.tbx_BP_PlanName.Text.Trim());

                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_BPImport = xlsview.ImportData;
                    this.BP_xlssheet = xlsview.SheetName;
                    this.BP_filepath = xlsview.FilePath;
                }
            }
            else
            {
                if (this.tbx_BP_PlanName.Text == "")
                    ShowError("Please provide a Plan Name");
                else if (this.tbx_BP_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void ExcelView_OnError(object sender, ErrorEventArgs e)
        {
            ShowError(e.ErrorMsg);
        }

        private void ShowError(string errormsg)
        {
            this.lbErr.Text = errormsg;
            errProvider.Icon = this.ErrIcon;
            errProvider.SetIconAlignment(lbErr, ErrorIconAlignment.MiddleLeft);
            errProvider.SetError(lbErr, errormsg);
        }

        private void ShowInfo(string infomsg)
        {
            this.lbErr.Text = infomsg;
            errProvider.Icon = this.InfoIcon;
            errProvider.SetIconAlignment(lbErr, ErrorIconAlignment.MiddleLeft);
            errProvider.SetError(lbErr, infomsg);
        }

        private void btn_SC_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_SCImport != null)
            {
                res &= xlsimport.Init(this.dt_SCImport, this.SC_xlssheet);
                res &= xlsimport.ImportData("SC");
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_SCImport != null)
            {
                this.dt_SCImport.Dispose();
                this.dt_SCImport = null;
            }

            if (res == true)
            {
                ShowInfo("Data Import Complete.");
            }
        }

        private void btn_DDV_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_SVImport != null)
            {
                res &= xlsimport.Init(this.dt_SVImport, this.SV_xlssheet);
                res &= xlsimport.ImportData("SV");
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_SVImport != null)
            {
                this.dt_SVImport.Dispose();
                this.dt_SVImport = null;
            }

            if (res == true)
            {
                ShowInfo("Data Import Complete.");
            }
        }

        private void btn_STD_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_STDImport != null)
            {
                res &= xlsimport.Init(this.dt_STDImport, this.STD_xlssheet);
                res &= xlsimport.ImportData("STD");
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_STDImport != null)
            {
                this.dt_STDImport.Dispose();
                this.dt_STDImport = null;
            }

            if (res == true)
            {
                ShowInfo("Data Import Complete.");
            }
        }

        private void btn_BP_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_BPImport != null)
            {
                res &= xlsimport.Init(this.dt_BPImport, this.BP_xlssheet);
                res &= xlsimport.ImportData("BP");
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_BPImport != null)
            {
                this.dt_BPImport.Dispose();
                this.dt_BPImport = null;
            }

            if (res == true)
            {
                ShowInfo("Data Import Complete.");
            }
        }


        private void cmbx_SC_SAID_KeyUP(object sender, KeyEventArgs e)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
                return;

            string showerrstr = "";
            try
            {
                string filterstr = this.cmbx_SC_SAID.Text;
                if (this.connstr != null && this.connstr != "" && this.sql_cmbx_SC_SAID != null && this.sql_cmbx_SC_SAID != "")
                {
                    SqlConnection sqlconn = new SqlConnection(connstr);
                    sqlconn.Open();
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlconn;
                    DataTable dt_loc = new DataTable();

                    if (filterstr != "")
                        sqlcmd.CommandText = this.sql_cmbx_SC_SAID + " where SA_ID like '" + filterstr + "%'";
                    else
                        sqlcmd.CommandText = this.sql_cmbx_SC_SAID;

                    sqlcmd.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlcmd.ExecuteReader();
                    if (reader.HasRows)
                        dt_loc.Load(reader);

                    if (dt_loc.Rows.Count > 0)
                    {
                        this.cmbx_SC_SAID.DataSource = dt_loc;
                        this.cmbx_SC_SAID.DisplayMember = "SA_ID";
                        this.cmbx_SC_SAID.ValueMember = "SA_ID";
                        this.cmbx_SC_SAID.Focus(); 
                        this.cmbx_SC_SAID.DroppedDown = true;
                        
                        //this.cmbx_SC_SAID.SelectedIndex = 0;
                        this.cmbx_SC_SAID.Text = filterstr;
                        this.cmbx_SC_SAID.SelectionStart = this.cmbx_SC_SAID.Text.Length + 1;
                        Cursor.Current = Cursors.Default;
                        //e.Handled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                return;
            }
        }

        
    }
}
