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
    interface IMainForm {
        string IChartsPath { get; }
        string HistDataDir { get; }

        event EventHandler IChartsUpdateClick;
        event Action<string, string, bool> FormClosing;

    }
    public partial class MainForm : Form, IMainForm
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

        private void butSettings_Click(object sender, EventArgs e)
        {
            var FormSettings = new FormSettings();
        }


        // FormSettings

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save settings to ini file
        }

        private void butIChartsUpdate_Click(object sender, EventArgs e)
        {

        }

        #region IFormSettings

        public string IChartsPath {
            get { return fldIChartsPath.Text; }
        }

        public string HistDataDir {
            get { return fldHistDataDir.Text; }
        }

        public event EventHandler IChartsUpdateClick;
        public event Action<string, string, bool> FormClosing;

        #endregion


        private void butChooseICharts_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = @"icharts|icharts.js";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fldIChartsPath.Text = dlg.FileName;
            }
        }

        private void butHistDataDirChoose_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fldHistDataDir.Text = dlg.SelectedPath;
            }
        }
    }
}
