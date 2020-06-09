using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinDownForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            chAllTime.Checked = true;
            chSkipUnfinished.Enabled = false;
        }

        private void chOverWrite_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chSkipUnfinished_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chAllTime_CheckedChanged(object sender, EventArgs e) {
            if (chAllTime.Checked) {
                dtpPeriodBeg.Value = new DateTime(1970, 1, 1);
                dtpPeriodEnd.Value = DateTime.Now;
            }
            dtpPeriodBeg.Enabled = !chAllTime.Checked;
            dtpPeriodEnd.Enabled = !chAllTime.Checked;
        }

        private void chFutures_CheckedChanged(object sender, EventArgs e) {
            chSkipUnfinished.Enabled = chFutures.Checked;
            fldIssuerMarket.Enabled = !chFutures.Checked;
            fldIssuerId.Enabled = !chFutures.Checked;

            if (chFutures.Checked) {
                fldIssuerMarket.Text= "";
                fldIssuerId.Text= "";
            }

            
        }
    }
}
