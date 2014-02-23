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
    public partial class ShowData : Form
    {
        
        public ShowData(DataTable dt)
        {
            InitializeComponent();
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Refresh();
        }

        private void btnOKClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
