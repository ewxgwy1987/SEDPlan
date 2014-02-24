using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            this.tbx_SV_FPath.Text = this.openFileDialog.FileName;
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
            string thisMethod = _className + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + "()";
            string errstr = "Class:[" + _className + "]" + "Method:<" + thisMethod + ">\n";

            try
            {
                // Load the log4net configuration
                string Path_XMLFileSetting = @"../../../cfg/CFG_SEDPlan.xml";
                FileInfo FI_Setting = new FileInfo(Path_XMLFileSetting);
                if (!FI_Setting.Exists)
                {
                    Path_XMLFileSetting = @"./cfg/CFG_MDSReader.xml";
                    FI_Setting = new FileInfo(Path_XMLFileSetting);
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

                // Init view function
                xlsview = new ExcelView();
                xlsview.OnErrorHappened += new EventHandler<ErrorEventArgs>(ExcelView_OnError);

                // Init import function
                XmlNode xcfg_ExcelDataImport = PALS.Utilities.XMLConfig.GetConfigSetElement(ref xmlRoot, "configSet","name","SEDPlan.ExcelDataImport");
                xlsimport = new ExcelDataImport(xcfg_ExcelDataImport);
                xlsimport.OnErrorHappened += new EventHandler<ErrorEventArgs>(ExcelView_OnError);

                // test
                this.tbx_SC_SAID.Text = "SA-Test1"; // SA-Test2 SA-Test3
                this.tbx_SC_FPath.Text = "../../../excel/Bill Of Material dated 14102013.xls";

                this.tbx_SV_SAID.Text = "SA-Test1";
                this.tbx_SV_FPath.Text = "../../../excel/Values Chart for Variables A degrees_ L and LS dated 08112013.xls";

                this.tbx_STD_FPath.Text = "../../../excel/Standard Parts.xlsx";

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

            if (this.tbx_SC_FPath.Text.Trim() != "" && this.tbx_SC_SAID.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_SC_FPath.Text.Trim(), this.tbx_SC_SAID.Text.Trim());
                
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
                if (this.tbx_SC_SAID.Text == "")
                    ShowError("Please provide a SA ID");
                else if (this.tbx_SC_FPath.Text == "")
                    ShowError("Please specify the imported file");
            }
        }

        private void btn_SV_View_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.tbx_SV_FPath.Text.Trim() != "" && this.tbx_SV_SAID.Text.Trim() != "")
            {
                xlsview.ClearData();
                xlsview.Init(this.tbx_SV_FPath.Text.Trim(), this.tbx_SV_SAID.Text.Trim());

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
                if (this.tbx_SV_SAID.Text == "")
                    ShowError("Please provide a SA ID");
                else if (this.tbx_SV_FPath.Text == "")
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
            errProvider.SetError(lbErr, errormsg);
        }

        private void btn_SC_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.dt_SCImport != null)
            {
                xlsimport.Init(this.dt_SCImport, this.SC_xlssheet);
                xlsimport.ImportData("SC");
            }
            else
            {
                ShowError("Please view the imported data at first."); 
            }
        }

        private void btn_SV_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.dt_SVImport != null)
            {
                xlsimport.Init(this.dt_SVImport, this.SV_xlssheet);
                xlsimport.ImportData("SV");
            }
            else
            {
                ShowError("Please view the imported data at first.");
            }
        }

        private void btn_STD_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.dt_STDImport != null)
            {
                xlsimport.Init(this.dt_STDImport, this.STD_xlssheet);
                xlsimport.ImportData("STD");
            }
            else
            {
                ShowError("Please view the imported data at first.");
            }
        }

        private void btn_BP_Import_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            errProvider.SetError(lbErr, string.Empty);

            if (this.dt_BPImport != null)
            {
                xlsimport.Init(this.dt_BPImport, this.BP_xlssheet);
                xlsimport.ImportData("BP");
            }
            else
            {
                ShowError("Please view the imported data at first.");
            }
        }

        
    }
}
