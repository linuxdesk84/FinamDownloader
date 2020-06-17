using System;
using System.IO;
using System.Linq;
using System.Net;


//https://github.com/rickyah/ini-parser


namespace FinamDownloader {
    interface IIchartsDownloader {
        bool TryDownloadAndMark(out string message);
        bool TryAutoUpdate(out string message);
    }


    /// <summary>
    /// Класс, выполняющий работу по загрузке файла icharts.js
    /// </summary>
    public class IchartsDownloader : IIchartsDownloader {
        /// <summary>
        /// Путь до файла icharts.js
        /// </summary>
        private string _ichartsPath;

        /// <summary>
        /// путь до файла check.txt, по которому определяется необходимость
        /// автоматического обновления файла icharts.js
        /// </summary>
        private string _fileCheckPath;

        /// <summary>
        /// корневой url сайта финама
        /// </summary>
        private const string FinamUrl = @"https://www.finam.ru/";

        /// <summary>
        /// страница, по которой определяется ссылка на скачивание файла icharts.js
        /// </summary>
        private const string SberExportPageUrl =
            FinamUrl + @"profile/moex-akcii/sberbank/export/";

        private readonly IDownloadService _downloadService;
        private readonly IHttpWebRequestService _httpWebRequestService;

        public IchartsDownloader(string ichartsPath) {
            _downloadService = new DownloadService();
            _httpWebRequestService = new HttpWebRequestService();

            SetIchartsPath(ichartsPath);

        }

        public void SetIchartsPath(string ichartsPath) {
            if (File.Exists(ichartsPath)) {
                _ichartsPath = ichartsPath;
                _fileCheckPath = Directory.GetParent(_ichartsPath) + "\\check.txt";
            } else {
                _ichartsPath = string.Empty;
                _fileCheckPath = string.Empty;
            }
        }


        private bool TryGetIchartsUrl(out string ichartsUrlOrError) {
            ichartsUrlOrError = "";

            var fb = _httpWebRequestService.TryGetResponse(
                SberExportPageUrl, out var responseOrError);
            if (!fb) {
                ichartsUrlOrError = responseOrError;
                return false;
            }

            var strings = responseOrError.Split('\n');

            foreach (var str in strings) {
                if (!str.Contains("icharts.js"))
                    continue;

                // пример строки: "\t\t<script src=\"/cache/N72Hgd54/icharts/icharts.js\" type=\"text/javascript\"></script>\r"
                var buf = str.Split('"');
                foreach (var subStr in buf) {

                    // пример строки: "/cache/N72Hgd54/icharts/icharts.js"
                    if (subStr.Contains("icharts.js")) {
                        ichartsUrlOrError = FinamUrl + subStr;
                        return true;
                    }
                }
            }

            ichartsUrlOrError = "response is recieved, but \"icharts.js\" not found";
            return false;
        }

        private bool TryDownloadIcharts(out string message) {
            message = "";

            if (!TryGetIchartsUrl(out var ichartsUrlOrError)) {
                message = ichartsUrlOrError;
                return false;
            }

            var ichartsDir = Path.GetDirectoryName(_ichartsPath) + "\\";
            var fnIcharts = Path.GetFileNameWithoutExtension(_ichartsPath);

            // адрес для нового скачиваемого файла icharts
            var ichartsTmpFilePath = ichartsDir + $@"{fnIcharts}_{DateTime.Now.Ticks}";

            // количество попыток скачать
            var numOfAttempts = 5;

            while (numOfAttempts-- > 0) {
                if (_downloadService.TryDownload(ichartsUrlOrError, ichartsTmpFilePath, out message)) {
                    break;
                }
            }

            if (message != string.Empty) {
                return false;
            }


            if (File.Exists(_ichartsPath)) {
                // если файлы одинаковые, то перезаписывать не надо
                if (new FileInfo(_ichartsPath).Length == new FileInfo(ichartsTmpFilePath).Length &&
                    File.ReadAllBytes(_ichartsPath).SequenceEqual(File.ReadAllBytes(ichartsTmpFilePath))) {
                    File.Delete(ichartsTmpFilePath);
                    message = @"downloading and existing files are equals";
                    return true;
                }

                // backup existing icharts.js
                var backupName = ichartsDir + $"{fnIcharts}_{DateTime.Now:yyyy.MM.dd_HH;mm;ss}" +
                                 Path.GetExtension(_ichartsPath);
                File.Move(_ichartsPath, backupName);
            }


            File.Move(ichartsTmpFilePath, _ichartsPath);
            message = @"icharts.js is downloaded (updated)";
            return true;
        }

        public bool TryDownloadAndMark(out string message) {
            if (_ichartsPath == string.Empty) {
                message = @"не определен путь до файла icharts.js";
                return false;
            }

            if (!TryDownloadIcharts(out message)) {
                return false;
            }

            // время модификации файла ffnIcharts будем менять только при успешной загрузке файла ffnIcharts
            var fileCheckPath = Directory.GetParent(_ichartsPath) + "\\check.txt";
            using (var writer = new StreamWriter(fileCheckPath, append: true)) {
                writer.WriteLine($"{DateTime.Now:yyyy.MM.dd HH:mm:ss}");
            }

            return true;
        }

        public bool TryAutoUpdate(out string message) {
            message = "";

            // по времени модификации файла check.txt будем определять надо ли закачивать новый icharts.js
            // если разница с текущим временем более 1 дня, то пытаемся обновить автоматически.


            if (!File.Exists(_fileCheckPath) ||
                DateTime.Now - File.GetLastWriteTime(_fileCheckPath) > new TimeSpan(24, 0, 0)) {
                if (!TryDownloadAndMark(out message)) {
                    return false;
                }
            }

            return true;
        }
    }
}
