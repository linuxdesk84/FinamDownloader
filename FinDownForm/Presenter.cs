using System;
using System.IO;
using System.Threading;
using FinamDownloader;

namespace FinDownForm
{
    class Presenter {
        private readonly IMainForm _mainForm;
        private readonly MessageService _messageService;
        private readonly IssuersManager _issuersManager;
        private readonly ManagerIni _managerIni;
        private readonly IchartsDownloader _ichartsDownloader;

        private int _downloadedCount;
        private bool _fFormNotClosed;

        public Presenter(IMainForm mainForm, MessageService messageService) {
            _downloadedCount = 0;
            _fFormNotClosed = true;

            _mainForm = mainForm;
            _mainForm.FormClosing += _mainForm_FormClosing;
            _mainForm.SaveSettings += _mainForm_SaveSettings;
            _mainForm.UpdateIChartsClick += _mainForm_UpdateIChartsClick;
            _mainForm.SearchIssuerClick += _mainForm_SearchIssuerClick;
            _mainForm.DownloadIssuerClick += _mainForm_DownloadIssuerClick;
            _mainForm.DownloadCancelClick += _mainForm_DownloadCancelClick;
            

            _messageService = messageService;

            var fileIniPath = Directory.GetCurrentDirectory() + "\\" + "FinamDownloader.ini";
            _managerIni = new ManagerIni(fileIniPath);
            _managerIni.Inform += _managerIni_Inform;

            _ichartsDownloader = new IchartsDownloader(_managerIni.FdSettings.IchartsPath);
            if (_managerIni.FdSettings.FAutoUpdate) {
                _ichartsDownloader.TryAutoUpdate(out var message);
                Logging(message);
            }

            _issuersManager = new IssuersManager(_managerIni.FdSettings.IchartsPath, _managerIni.FdSettings.HistDataDir);
            _issuersManager.Inform += _issuersManager_Inform;
            _issuersManager.FileDownloaded += _issuersManager_FileDownloaded;
            _issuersManager.DownloadComplete += _issuersManager_DownloadComplete;

            _mainForm.IchartsPath = _managerIni.FdSettings.IchartsPath;
            _mainForm.HistDataDir = _managerIni.FdSettings.HistDataDir;
            _mainForm.FAutoUpdate = _managerIni.FdSettings.FAutoUpdate;

            _mainForm.SetIssuerCount(_issuersManager.GetIssuersCount());

            _mainForm.DtPeriodMin = _issuersManager.DtPeriodMin;
            _mainForm.DtPeriodMax = _issuersManager.DtPeriodMax;

            _mainForm.DtPeriodFrom = _issuersManager.DtPeriodMax.AddMonths(-3);
            _mainForm.DtPeriodTo = _issuersManager.DtPeriodMax;
        }

        

        private bool CheckFuturesNameLength(string futuresName) {
            if (futuresName.Length != 2) {
                _messageService.ShowMessage(
                    "Для фьючерсов необходимо вводить базовое имя, например: MX, BR, Si");
                return false;
            }
            return true;
        }

        private void _mainForm_DownloadIssuerClick(object sender, EventArgs e) {
            if (_mainForm.IsFutures && !CheckFuturesNameLength(_mainForm.IssuerName)) {
                return;
            }

            _downloadedCount = 0;
            _mainForm.SetDownloadedCount(_downloadedCount);

            var downloadParams = new DownloadParams(
                _mainForm.IssuerName, _mainForm.IssuerMarket, _mainForm.IssuerId,
                _mainForm.FExactMatchName, _mainForm.FMatchCase, _mainForm.IsFutures,
                _mainForm.fAllTime, _mainForm.DtPeriodFrom, _mainForm.DtPeriodTo,
                _mainForm.fOverwrite, _mainForm.fSkipUnfinished);

            var thread = new Thread(_issuersManager.DownloadIssuers);
            thread.Start(downloadParams);

            //_issuersManager.DownloadIssuers(
            //    _mainForm.IssuerName, _mainForm.IssuerMarket, _mainForm.IssuerId,
            //    _mainForm.FExactMatchName, _mainForm.FMatchCase, _mainForm.IsFutures,
            //    _mainForm.fAllTime, _mainForm.DtPeriodFrom, _mainForm.DtPeriodTo,
            //    _mainForm.fOverwrite, _mainForm.fSkipUnfinished);
        }

        private void _mainForm_DownloadCancelClick(object sender, EventArgs e) {
            _issuersManager.DowloadCancel();
        }


        private void _mainForm_SearchIssuerClick(object sender, EventArgs e) {
            if (_mainForm.IsFutures && !CheckFuturesNameLength(_mainForm.IssuerName)) {
                return;
            }

            var str = _issuersManager.FindIssuers(
                _mainForm.IssuerName, _mainForm.IssuerMarket, _mainForm.IssuerId,
                _mainForm.FExactMatchName, _mainForm.FMatchCase, _mainForm.IsFutures);
            Logging(str);
        }

        private void _mainForm_UpdateIChartsClick(object sender, EventArgs e) {
            if (!_ichartsDownloader.TryDownloadAndMark(out var message)) {
                _messageService.ShowError(message);
            } else {
                Logging(message);
            }
        }

        private void _managerIni_Inform(string message) {
            //_messageService.ShowError(message);
            Logging(message);
        }

        private void _issuersManager_Inform(string message) {
            Logging(message);
        }
        private void _issuersManager_FileDownloaded()
        {
            _downloadedCount++;
            if (_fFormNotClosed) {
                _mainForm.SetDownloadedCount(_downloadedCount);
            }
        }
        private void _issuersManager_DownloadComplete(bool fCancelled) {
            var msg = fCancelled ? "Загрузка прервана" : "Загрузка завершена";
            _messageService.ShowMessage(msg);
            _mainForm.FlipButtons(true);
        }


        private void _mainForm_FormClosing(object sender, EventArgs e)
        {
            _issuersManager.DowloadCancel();
            _fFormNotClosed = false;
        }
        private void _mainForm_SaveSettings() {
            var ichartsPath = _mainForm.IchartsPath;
            var histDataDir = _mainForm.HistDataDir;
            var fAutoUpdate = _mainForm.FAutoUpdate;

            if (ichartsPath == _managerIni.FdSettings.IchartsPath &&
                histDataDir == _managerIni.FdSettings.HistDataDir &&
                fAutoUpdate == _managerIni.FdSettings.FAutoUpdate) {
                return;
            }


            if (ichartsPath != string.Empty && !File.Exists(ichartsPath)) {
                _messageService.ShowError($@"File '{ichartsPath}' isn't exists");
                return;
            }

            if (histDataDir != string.Empty && !Directory.Exists(histDataDir)) {
                _messageService.ShowError($@"Directory '{histDataDir}' isn't exists");
                return;
            }


            if (ichartsPath != _managerIni.FdSettings.IchartsPath) {
                _ichartsDownloader.SetIchartsPath(ichartsPath);
                _issuersManager.SetIchartsPath(ichartsPath);
                _mainForm.SetIssuerCount(_issuersManager.GetIssuersCount());
            }
            _issuersManager.HistDataDir = histDataDir;

            _managerIni.SaveSettings(histDataDir, ichartsPath, fAutoUpdate);
        }


        private void Logging(string message) {
            if (_fFormNotClosed) {
                _mainForm.Logging(message);
            }
        }
    }
}
