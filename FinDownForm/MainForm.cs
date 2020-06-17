using System;
using System.Windows.Forms;

namespace FinDownForm {
    public partial class MainForm : Form, IMainForm {
        public MainForm() {
            InitializeComponent();

            dtpPeriodFrom.ValueChanged += DtpPeriodFrom_ValueChanged;
            dtpPeriodTo.ValueChanged += DtpPeriodTo_ValueChanged;

            butSearch.Click += ButSearch_Click;
            butDownload.Click += ButDownload_Click;
            butCancel.Click += ButCancel_Click;
            butIChartsUpdate.Click += ButIChartsUpdate_Click;
            butSaveSettings.Click += ButSaveSettings_Click;

            butChooseICharts.Click += ButChooseICharts_Click;
            butHistDataDirChoose.Click += ButHistDataDirChoose_Click;
            chAllTime.CheckedChanged += ChAllTime_CheckedChanged;
            chFutures.CheckedChanged += ChFutures_CheckedChanged;

            SetInitialValues();
            this.FormClosing += MainForm_FormClosing;
        }


        private void DtpPeriodTo_ValueChanged(object sender, EventArgs e) {
            DtPeriodTo = dtpPeriodTo.Value;
        }

        private void DtpPeriodFrom_ValueChanged(object sender, EventArgs e) {
            DtPeriodFrom = dtpPeriodFrom.Value;
        }

        #region Собственный код формы

        private void ClearLog() {
            fldLog.Text = string.Empty;
        }

        private void SetInitialValues() {
            chFutures.Checked = true;
            chFutures.Checked = false;
        }

        private void ButChooseICharts_Click(object sender, EventArgs e) {
            var dlg = new OpenFileDialog {Filter = @"icharts|icharts.js"};

            if (dlg.ShowDialog() == DialogResult.OK) {
                fldIChartsPath.Text = dlg.FileName;
            }
        }

        private void ButHistDataDirChoose_Click(object sender, EventArgs e) {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK) {
                fldHistDataDir.Text = dlg.SelectedPath + @"\";
            }
        }

        private void ChAllTime_CheckedChanged(object sender, EventArgs e) {
            if (chAllTime.Checked) {
                DtPeriodFrom = DtPeriodMin;
                DtPeriodTo = DtPeriodMax;
            }

            dtpPeriodFrom.Enabled = !chAllTime.Checked;
            dtpPeriodTo.Enabled = !chAllTime.Checked;
        }

        private void ChFutures_CheckedChanged(object sender, EventArgs e) {
            chSkipUnfinished.Enabled = chFutures.Checked;
            chExactMatchName.Enabled = !chFutures.Checked;

            if (chFutures.Checked) {
                chExactMatchName.Checked = false;
            }
        }

        #endregion

        #region Проброс событий

        private void ButSearch_Click(object sender, EventArgs e) {
            ClearLog();
            SearchIssuerClick?.Invoke(sender, EventArgs.Empty);
        }

        private void ButDownload_Click(object sender, EventArgs e) {
            ClearLog();
            FlipButtons(false);
            DownloadIssuerClick?.Invoke(sender, EventArgs.Empty);
        }
        private void ButCancel_Click(object sender, EventArgs e)
        {
            DownloadCancelClick?.Invoke(sender, EventArgs.Empty);
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
        public event EventHandler FormClosing;

        public string IssuerName => fldIssuerName.Text;

        public string IssuerMarket => fldIssuerMarket.Text;

        public string IssuerId => fldIssuerId.Text;

        public bool FExactMatchName => chExactMatchName.Checked;

        public bool FMatchCase => chMatchCase.Checked;

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
            get => dtpPeriodFrom.Value;
            set => dtpPeriodFrom.Value = GetValueFromMinAndMax(value);
        }

        public DateTime DtPeriodTo {
            get => dtpPeriodTo.Value;
            set => dtpPeriodTo.Value = GetValueFromMinAndMax(value);
        }

        public void Logging(string str) {
            Action action = () => {
                fldLog.AppendText(str + Environment.NewLine);
            };
            this.InvokeEx(action);
        }

        public void SetIssuerCount(int count) {
            lblIssuersCount.Text = count.ToString();
        }

        public void SetDownloadedCount(int count) {
            Action action = () => {
                lblDownloadedCount.Text = count.ToString();
            };
            this.InvokeEx(action);
        }

        public void FlipButtons(bool fEnable)
        {
            Action action = () =>
            {
                butDownload.Enabled = fEnable;
                butSearch.Enabled = fEnable;
            };
            this.InvokeEx(action);
        }


        public event EventHandler SearchIssuerClick;
        public event EventHandler DownloadIssuerClick;
        public event EventHandler DownloadCancelClick;

        // tab settings
        public string IchartsPath {
            get => fldIChartsPath.Text;
            set => fldIChartsPath.Text = value;
        }

        public string HistDataDir {
            get => fldHistDataDir.Text;
            set => fldHistDataDir.Text = value;
        }

        public bool FAutoUpdate {
            get => chIChartsAutoUpdate.Checked;
            set => chIChartsAutoUpdate.Checked = value;
        }

        public event EventHandler UpdateIChartsClick;
        public event EventHandler SaveSettingsClick;

        #endregion

        private void MainForm_FormClosing(object sender, EventArgs e)
        {
            FormClosing?.Invoke(sender, EventArgs.Empty);
        }
    }
}
