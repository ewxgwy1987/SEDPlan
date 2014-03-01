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
        private const string XCFG_BPCALSTP = "BomPlanCalSTP";

        private string connstr;
        private string sql_cmbx_SC_SAID;
        private string BomPlanCalSTPName;
        private ExcelView xlsview;
        private ExcelDataImport xlsimport;

        private ErrorProvider errProvider;

        private const string TP_DDV = "DDV";
        private const string TP_STD = "STD";
        private const string TP_FW = "FW";
        private const string TP_DDT = "DDT";
        private const string TP_SC = "SC";
        private const string TP_BP = "BP";

        // DDV
        private DataTable dt_DDVImport;
        private string DDV_xlssheet;
        private string DDV_filepath;

        // STD
        private DataTable dt_STDImport;
        private string STD_xlssheet;
        private string STD_filepath;

        // FW
        private DataTable dt_FWImport;
        private string FW_xlssheet;
        private string FW_filepath;

        // DDT
        private DataTable dt_DDTImport;
        private string DDT_xlssheet;
        private string DDT_filepath;

        // SC
        private DataTable dt_SCImport;
        private string SC_xlssheet;
        private string SC_filepath;

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

        private void btn_DDV_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_DDV_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_STD_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_STD_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_FW_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_FW_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_DDT_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_DDT_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_SC_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_SC_FPath.Text = this.openFileDialog.FileName;
        }

        private void btn_BP_Open_Click(object sender, EventArgs e)
        {
            DialogResult openres = this.openFileDialog.ShowDialog();
            if (openres == DialogResult.OK)
                this.tbx_BP_FPath.Text = this.openFileDialog.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                this.BomPlanCalSTPName = PALS.Utilities.XMLConfig.GetSettingFromInnerText(xmlRoot, XCFG_BPCALSTP, "");

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

                this.tbx_FW_FPath.Text = "../../../excel/Detail Design Fixed Weight.xlsx";

                this.cmbx_DDT_SAID.Text = "SA-Test3";
                this.tbx_DDT_FPath.Text = "../../../excel/SA Detail Design Types.xlsx";

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

        private void btn_DDV_View_Click(object sender, EventArgs e)
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
                    this.dt_DDVImport = xlsview.ImportData;
                    this.DDV_xlssheet = xlsview.SheetName;
                    this.DDV_filepath = xlsview.FilePath;
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
                    this.FW_filepath = xlsview.FilePath;
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
                res &= xlsimport.ImportData(TP_SC);
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
                string infostr = "Data Import Complete.\r\n"
                    + "File Path:" + this.SC_filepath + "\r\n"
                    + "SA ID:" + this.SC_xlssheet + "\r\n";
                this.tbx_SC_FPath.Text = "";
                this.cmbx_SC_SAID.Text = "";
                ShowInfo(infostr);
            }
        }

        private void btn_DDV_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_DDVImport != null)
            {
                res &= xlsimport.Init(this.dt_DDVImport, this.DDV_xlssheet);
                res &= xlsimport.ImportData(TP_DDV);
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_DDVImport != null)
            {
                this.dt_DDVImport.Dispose();
                this.dt_DDVImport = null;
            }

            if (res == true)
            {
                string infostr = "Data Import Complete.\r\n"
                    + "File Path:" + this.DDV_filepath + "\r\n"
                    + "DD ID:" + this.DDV_xlssheet + "\r\n";
                this.tbx_DDV_FPath.Text = "";
                this.tbx_DDV_SAID.Text = "";
                ShowInfo(infostr);
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
                res &= xlsimport.ImportData(TP_STD);
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
                string infostr = "Data Import Complete.\r\n"
                    + "File Path:" + this.STD_filepath + "\r\n"
                    + "Sheet Name:" + this.STD_xlssheet + "\r\n";
                this.tbx_STD_FPath.Text = "";
                ShowInfo(infostr);
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
                res &= xlsimport.ImportData(TP_BP);
                res &= BOMPalnCal(this.BP_xlssheet.Trim()); // calculate statistics result
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
                string infostr = "Data Import Complete. Calculation Complete. \r\n"
                   + "File Path:" + this.BP_filepath + "\r\n"
                   + "Plan Name:" + this.BP_xlssheet + "\r\n";
                this.tbx_BP_FPath.Text = "";
                this.tbx_BP_PlanName.Text = "";
                ShowInfo(infostr);
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

        private void btn_FW_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_FW_FPath.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_FW_FPath.Text.Trim(), "");

                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_FWImport = xlsview.ImportData;
                    this.FW_xlssheet = xlsview.SheetName;
                    this.FW_filepath = xlsview.FilePath;
                }
            }
            else
            {
                if (this.tbx_FW_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_FW_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_FWImport != null)
            {
                res &= xlsimport.Init(this.dt_FWImport, this.FW_xlssheet);
                res &= xlsimport.ImportData(TP_FW);
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_FWImport != null)
            {
                this.dt_FWImport.Dispose();
                this.dt_FWImport = null;
            }

            if (res == true)
            {
                string infostr = "Data Import Complete.\r\n"
                   + "File Path:" + this.FW_filepath + "\r\n"
                   + "Sheet Name:" + this.FW_xlssheet + "\r\n";
                this.tbx_FW_FPath.Text = "";
                ShowInfo(infostr);
            }
        }

        private void btn_DDT_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_DDT_FPath.Text.Trim() != "" && this.cmbx_DDT_SAID.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_DDT_FPath.Text.Trim(), this.cmbx_DDT_SAID.Text.Trim());

                if (xlsview.ImportData != null && xlsview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(xlsview.ImportData);
                    fmShowData.Show();
                    this.dt_DDTImport = xlsview.ImportData;
                    this.DDT_xlssheet = xlsview.SheetName;
                    this.DDT_filepath = xlsview.FilePath;
                }
            }
            else
            {
                if (this.cmbx_DDT_SAID.Text == "")
                    ShowError("Please provide a SA ID");
                else if (this.tbx_DDT_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_DDT_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);
            bool res = true;

            if (this.dt_DDTImport != null)
            {
                res &= xlsimport.Init(this.dt_DDTImport, this.DDT_xlssheet);
                res &= xlsimport.ImportData(TP_DDT);
            }
            else
            {
                ShowError("Please view the imported data at first.");
                res = false;
            }

            if (this.dt_DDTImport != null)
            {
                this.dt_DDTImport.Dispose();
                this.dt_DDTImport = null;
            }

            if (res == true)
            {
                string infostr = "Data Import Complete.\r\n"
                   + "File Path:" + this.DDT_filepath + "\r\n"
                   + "SA ID:" + this.DDT_xlssheet + "\r\n";
                this.cmbx_DDT_SAID.Text = "";
                this.tbx_DDT_FPath.Text = "";
                ShowInfo(infostr);
            }
        }


        private bool BOMPalnCal(string planname)
        {
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";
            string showerrstr = "";
            bool res;

            SqlConnection sqlconn = new SqlConnection(connstr);
            sqlconn.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlconn;

            try
            {
                SqlParameter sPararameter = new SqlParameter("@PlanName", SqlDbType.NVarChar, 100);
                sPararameter.Direction = ParameterDirection.Input;
                sPararameter.Value = planname;
                sqlcmd.Parameters.Add(sPararameter);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = this.BomPlanCalSTPName;
                sqlcmd.ExecuteNonQuery();
                res = true;
            }
            catch (Exception exp)
            {
                errstr += exp.ToString();
                _logger.Error(errstr);
                showerrstr += exp.Message;
                ShowError(showerrstr);
                res = false;
            }
            finally
            {
                if (sqlconn != null && sqlconn.State == ConnectionState.Open)
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                    if (sqlcmd != null)
                    {
                        sqlcmd.Dispose();
                    }
                }
            }
            return res;
        }
    }
}
