using System;
using System.Windows.Forms;

namespace FinDownForm {
    public partial class MainForm : Form, IMainForm {
        public MainForm() {
            InitializeComponent();

            butSearch.Click += ButSearch_Click;
            butDownload.Click += ButDownload_Click;
            butIChartsUpdate.Click += ButIChartsUpdate_Click;
            butSaveSettings.Click += ButSaveSettings_Click;

            butChooseICharts.Click += ButChooseICharts_Click;
            butHistDataDirChoose.Click += ButHistDataDirChoose_Click;
            chAllTime.CheckedChanged += ChAllTime_CheckedChanged;
            chFutures.CheckedChanged += ChFutures_CheckedChanged;

            SetInitialValues();

        }

        #region Собственный код формы

        private void SetInitialValues() {
            chFutures.Checked = true;
            chFutures.Checked = false;
        }

        private void ButChooseICharts_Click(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = @"icharts|icharts.js";

            if (dlg.ShowDialog() == DialogResult.OK) {
                fldIChartsPath.Text = dlg.FileName;
            }
        }

        private void ButHistDataDirChoose_Click(object sender, EventArgs e) {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK) {
                fldHistDataDir.Text = dlg.SelectedPath;
            }
        }

        private void ChAllTime_CheckedChanged(object sender, EventArgs e) {
            if (chAllTime.Checked) {
                DtPeriodFrom = DtPeriodMin;
                DtPeriodTo = DtPeriodMax;
            }

            dtpPeriodBeg.Enabled = !chAllTime.Checked;
            dtpPeriodEnd.Enabled = !chAllTime.Checked;
        }

        private void ChFutures_CheckedChanged(object sender, EventArgs e) {
            chSkipUnfinished.Enabled = chFutures.Checked;
            fldIssuerMarket.Enabled = !chFutures.Checked;
            fldIssuerId.Enabled = !chFutures.Checked;

            if (chFutures.Checked) {
                fldIssuerMarket.Text = "";
                fldIssuerId.Text = "";
            }
        }

        #endregion

        #region Проброс событий

        private void ButSearch_Click(object sender, EventArgs e) {
            SearchIssuerClick?.Invoke(sender, EventArgs.Empty);
        }

        private void ButDownload_Click(object sender, EventArgs e) {
            DownloadIssuerClick?.Invoke(sender, EventArgs.Empty);
        }

        private void ButIChartsUpdate_Click(object sender, EventArgs e) {
            UpdateIChartsClick?.Invoke(sender, EventArgs.Empty);
        }

        private void ButSaveSettings_Click(object sender, EventArgs e) {
            SaveSettingsClick?.Invoke(sender, EventArgs.Empty);
        }

        #endregion

        #region IMainForm

        // tab main
        public string IssuerName => fldIssuerName.Text;

        public string IssuerMarket => fldIssuerMarket.Text;

        public string IssuerId => fldIssuerId.Text;

        public bool fEqualName => chEqualName.Checked;

        public bool fAllTime => chAllTime.Checked;

        public bool IsFutures => chFutures.Checked;

        public bool fSkipUnfinished => chSkipUnfinished.Checked;

        public bool fOverwrite => chOverwrite.Checked;

        public DateTime DtPeriodMin { private get; set; }

        public DateTime DtPeriodMax { private get; set; }

        private DateTime GetValueFromMinAndMax(DateTime dt) {
            if (dt < DtPeriodMin) {
                dt = DtPeriodMin;
            }

            if (DtPeriodMax < dt) {
                dt = DtPeriodMax;
            }

            return dt;
        }

        public DateTime DtPeriodFrom {
            get => dtpPeriodBeg.Value;
            private set => dtpPeriodBeg.Value = GetValueFromMinAndMax(value);
        }

        public DateTime DtPeriodTo {
            get => dtpPeriodEnd.Value;
            private set => dtpPeriodEnd.Value = GetValueFromMinAndMax(value);
        }

        public void Logging(string str) {
            fldLog.Text += str;
        }

        public void ClearLog() {
            fldLog.Text = string.Empty;
        }

        public void SetIssuerCount(int count) {
            lblIssuersCount.Text = count.ToString();
        }

        public void SetDownloadedCount(int count) {
            lblDownloadedCount.Text = count.ToString();
        }

        public event EventHandler SearchIssuerClick;
        public event EventHandler DownloadIssuerClick;

        // tab settings
        public string IChartsPath {
            get => fldIChartsPath.Text;
            set => fldIChartsPath.Text = value;
        }

        public string HistDataDir {
            get => fldHistDataDir.Text;
            set => fldHistDataDir.Text = value;
        }

        public bool fAutoUpdate => chIChartsAutoUpdate.Checked;

        public event EventHandler UpdateIChartsClick;
        public event EventHandler SaveSettingsClick;

        #endregion
    }
}
