using System.Net;
using System.Threading;

namespace FinamDownloader {
    /// <summary>
    /// интерфейс класса, реализующего возможность загрузки данных
    /// </summary>
    interface IDownloadService {
        bool TryDownload(string url, string filePath, out string strErr);
    }

    /// <summary>
    /// Класс, реализующий возможность загрузки данных
    /// </summary>
    class DownloadService : IDownloadService {
        /// <summary>
        /// Попытка выполнить загрузку данных по адресу url, и сохранить в файл fn.
        /// После выполнения попытки поток спит 1000 мс
        /// </summary>
        /// <param name="url">сгенерированный url</param>
        /// <param name="filePath">полный путь имя результирующего файла</param>
        /// <param name="strErr">строка, содержащая подробности ошибки; в случае успеха - пустая</param>
        /// <returns>результат выполнения загрузки: true - успешно, false - ошибка</returns>
        public bool TryDownload(string url, string filePath, out string strErr) {
            strErr = string.Empty;
            var res = true; // результат выполнения загрузки

            var wc = new WebClient();
            try {
                wc.DownloadFile(url, filePath);
            }
            catch (WebException webException) {
                strErr = webException.Message;
                res = false;
            }

            Thread.Sleep(1000);
            return res;
        }

    }
}