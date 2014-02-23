using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using System.Xml;
using PALS;

namespace SEDPlan
{
    public partial class Form1 : Form
    {
        #region Class Field and Property

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
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void reportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
        }

        private void btn_SC_View_Click(object sender, EventArgs e)
        {
            //IS1-30-001
            //F:\1Pteris Global Ltd\EWXGWY1987_DEV\SEDPlan\excel\Bill Of Material dated 14102013.xls
            if (this.tbx_SC_FPath.Text != "" && this.tbx_SC_SAID.Text != "")
            {
                ExcelView elxview = new ExcelView(this.tbx_SC_FPath.Text, this.tbx_SC_SAID.Text);
                if (elxview.ImportData != null && elxview.ImportData.Rows.Count > 0)
                {
                    Form fmShowData = new ShowData(elxview.ImportData);
                    fmShowData.Show();
                }
                else
                {
                }
            }
            else
            {
            }
        }
    }
}
