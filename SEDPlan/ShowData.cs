using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;

namespace SEDPlan
{
    public partial class ShowData : Form
    {
        // The name of current class 
        private static readonly string _className =
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        // Create a logger for use in this class
        private static readonly log4net.ILog _logger =
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ShowData(DataTable dt, string projno, string projname)
        {
            InitializeComponent();
            if (dt != null)
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Refresh();
            }

            if (projno != "" && projname != "")
            {
                this.lbProjectNo.Visible = true;
                this.lbProjectName.Visible = true;
                this.tbxProjectNo.Visible = true;
                this.tbxProjectName.Visible = true;
                this.tbxProjectNo.Text = projno;
                this.tbxProjectNo.ReadOnly = true;
                this.tbxProjectName.Text = projname;
                this.tbxProjectName.ReadOnly = true;
            }
        }

        private void btnOKClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
