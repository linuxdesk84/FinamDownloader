using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using FinDownEntity;
using NUnit.Framework;


namespace FinamDownloader {
    /// <summary>
    /// Класс, выполняющий работу по загрузке и поиску эмитентов
    /// </summary>
    public class IssuersManager {
        /// <summary>
        /// Флаг отмены загрузки по требованию
        /// </summary>
        private bool _fCancelled;

        /// <summary>
        /// распарсенный файл icharts.js
        /// </summary>
        private readonly IchartsData _ichartsData;

        /// <summary>
        /// текущее московское время
        /// </summary>
        private readonly DateTime _currentDt;

        /// <summary>
        /// Минимально возможная начальная точка
        /// </summary>
        public DateTime DtPeriodMin { get; }

        /// <summary>
        /// Максимально возможная конечная точка
        /// </summary>
        public DateTime DtPeriodMax { get; }

        /// <summary>
        /// Загрузчик
        /// </summary>
        private readonly IDownloadService _downloadService;

        
        /// <summary>
        /// Задаем путь _ichartsData.ichartsPath.
        /// При необходимости читаем файл charts.js заново
        /// </summary>
        public void SetIchartsPath(string ichartsPath) {
            _ichartsData.IchartsPath = ichartsPath;
        }


        /// <summary>
        /// Возвращаем количество эммитентов
        /// </summary>
        public int GetIssuersCount() {
            return _ichartsData.Issuers.Count;
        }

        /// <summary>
        /// Корневой каталог, куда будем загружать сделки
        /// </summary>
        public string HistDataDir { get; set; }

        /// <summary>
        /// Сообщаем служебную информацию
        /// </summary>
        public event Action<string> Inform;

        /// <summary>
        /// Сообщаем, что загружен новый файл
        /// </summary>
        public event Action FileDownloaded;

        /// <summary>
        /// Сообщаем, что загрузка завершена
        /// </summary>
        public event Action<bool> DownloadComplete;

        public IssuersManager(string ichartsPath, string histDataDir) {
            _ichartsData = new IchartsData(ichartsPath);
            _ichartsData.Inform += IchartssData_Inform;
            HistDataDir = histDataDir;

            
            //HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Time Zones\Russian Standard Time
            var msk = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

            // текущее московское время
            _currentDt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, msk);

            // самая ранняя дата, доступная в finam
            DtPeriodMin = new DateTime(1979, 1, 1);

            // за сегодняшнюю дату закачивать нельзя. самое позднее - за вчера
            DtPeriodMax = _currentDt.AddDays(-1).Date;


            _downloadService = new DownloadService();
        }

        /// <summary>
        /// Проброс события IchartsData.Inform
        /// </summary>
        private void IchartssData_Inform(string message) {
            Inform?.Invoke(message);
        }


        /// <summary>
        /// поиск эмитента
        /// </summary>
        public string FindIssuers(string issuerName, string issuerMarket, string issuerId,
            bool fExactMatchName, bool fMatchCase, bool isFutures)
        {
            if (_ichartsData.Issuers.Count == 0)
            {
                Inform?.Invoke($@"Не задан путь к файлу icharts.js");
                return "";
            }

            var issuersList = SearchEngine(issuerName, issuerMarket, issuerId,
                fExactMatchName, fMatchCase, isFutures);

            var str = new StringBuilder();
            str.AppendLine($@"Найдено эмитентов: {issuersList.Count}");


            // если найдено не более max эмитентов, то печатаем их с полным описанием
            const int max = 200;

            if (0 < issuersList.Count && issuersList.Count < max)
            {
                const bool fFullDescr = true;
                str.AppendLine(FinamIssuer.GetDescriptionHead(fFullDescr));
                foreach (var issuer in issuersList)
                {
                    str.AppendLine(issuer.GetDescription(fFullDescr));
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// Поисковый движок должен является общим для режимов поиска и скачивания,
        /// чтобы обеспечить однозначность получаемых данных.
        /// Что находится в режиме поиска, то и должно скачиваться.
        /// </summary>
        /// <returns></returns>
        private List<FinamIssuer> SearchEngine(string issuerName, string issuerMarket, string issuerId,
            bool fExactMatchName, bool fMatchCase, bool isFutures)
        {
            // для фьючерсов: требуем длину имени = 2
            Assert.IsTrue(isFutures == false || issuerName.Length == 2);

            // код должен быть длины 4, for example: BRU9, MXZ7, etc.
            const int futCodeLen = 4;


            // поиск можно уточнять при помощи Market и Id
            var fMarket = !string.IsNullOrWhiteSpace(issuerMarket);
            var fId = !string.IsNullOrWhiteSpace(issuerId);


            if (fMatchCase == false) {
                issuerName = issuerName.ToLower();
            }


            var issuersList = new List<FinamIssuer>();
            foreach (var issuer in _ichartsData.Issuers) {
                // отфильтровываем по Market и Id
                if (fId && issuerId != issuer.Id ||
                    fMarket && issuerMarket != issuer.Market) {
                    continue;
                }

                var name = fMatchCase ? issuer.Name : issuer.Name.ToLower();

                bool comparison;
                if (isFutures) {
                    var code = fMatchCase ? issuer.Code : issuer.Code.ToLower();

                    // Name: "BR-1.09(BRF9)", Code: "BRF9"
                    comparison = futCodeLen == issuer.Code.Length &&
                                 code.Substring(0, issuerName.Length) == issuerName &&
                                 name.Contains("-") && name.Contains(code) &&
                                 name.Contains(".") && name.Contains("(") && name.Contains(")");
                } else {
                    comparison = fExactMatchName ? issuerName == name : name.Contains(issuerName);
                }

                if (comparison) {
                    issuersList.Add(issuer);
                }
            }

            return issuersList;
        }

        /// <summary>
        /// Отмена загрузки
        /// </summary>
        public void DowloadCancel() {
            _fCancelled = true;
        }

        /// <summary>
        /// Метод, вызываемый при создании нового потока из GUI
        /// </summary>
        /// <param name="obj"></param>
        public void DownloadIssuers(object obj) {
            _fCancelled = false;

            var downloadParams = (DownloadParams) obj;

            var issuerName      = downloadParams.IssuerName;
            var issuerMarket    = downloadParams.IssuerMarket;
            var issuerId        = downloadParams.IssuerId;

            var fExactMatchName = downloadParams.FExactMatchName;
            var fMatchCase      = downloadParams.FMatchCase;
            var isFutures       = downloadParams.IsFutures;

            var fAllTime        = downloadParams.FAllTime;
            var dtPeriodBeg     = downloadParams.DtPeriodBeg;
            var dtPeriodEnd     = downloadParams.DtPeriodEnd;

            var fOverwrite      = downloadParams.FOverwrite;
            var fSkipUnfinished = downloadParams.FSkipUnfinished;

            DownloadIssuers( issuerName, issuerMarket, issuerId, fExactMatchName, fMatchCase, isFutures,
             fAllTime, dtPeriodBeg, dtPeriodEnd, fOverwrite, fSkipUnfinished);

            DownloadComplete?.Invoke(_fCancelled);
        }


        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <param name="issuerName"></param>
        /// <param name="issuerMarket"></param>
        /// <param name="issuerId"></param>
        /// <param name="fExactMatchName"></param>
        /// <param name="fMatchCase"></param>
        /// <param name="isFutures"></param>
        /// <param name="fAllTime"></param>
        /// <param name="dtPeriodBeg"></param>
        /// <param name="dtPeriodEnd"></param>
        /// <param name="fOverwrite">перезапись</param>
        /// <param name="fSkipUnfinished">skip loading unfinished futures. Данный флаг иммеет отношение
        /// только к фьючерсам, но не к конкретным дням. не завершенные дни мы не загружаем всегда</param>
        public void DownloadIssuers(string issuerName, string issuerMarket, string issuerId,
            bool fExactMatchName, bool fMatchCase, bool isFutures,
            bool fAllTime, DateTime dtPeriodBeg, DateTime dtPeriodEnd,
            bool fOverwrite, bool fSkipUnfinished)
        {
            if (_ichartsData.Issuers.Count == 0) {
                Inform?.Invoke($@"Не задан путь к файлу icharts.js");
                return;
            }

            if (HistDataDir == string.Empty) {
                Inform?.Invoke($@"Не задан путь к каталогу historyDataDir");
                return;
            }

            if (fAllTime) {
                dtPeriodBeg = DtPeriodMin;
                dtPeriodEnd = DtPeriodMax;
            }

            if (dtPeriodBeg < DtPeriodMin) {
                dtPeriodBeg = DtPeriodMin;
            }

            if (dtPeriodEnd > DtPeriodMax) {
                dtPeriodEnd = DtPeriodMax;
            }

            var issuersList = SearchEngine(issuerName, issuerMarket, issuerId,
                fExactMatchName, fMatchCase, isFutures);

            if (issuersList.Count == 0) {
                Inform?.Invoke("Не найдено ни одного эмитента");
                return;
            }



            if (isFutures) {
                DownloadFutures(issuerName, issuersList, true, 
                    dtPeriodBeg, dtPeriodEnd, fOverwrite, fSkipUnfinished);
            } else {
                if (issuersList.Count != 1) {
                    Inform?.Invoke("Найдено больше одного эмитента. Необходимо изменить условия поиска");
                    return;
                }

                // Загрузка тиков по акции
                var issuer = issuersList[0];
                DownloadIssuer(issuer.Name, issuer.Market, issuer.Id,
                    false, dtPeriodBeg, dtPeriodEnd, HistDataDir, fOverwrite);
            }
        }



        /// <summary>
        /// Загрузка тиков по фьючам
        /// </summary>
        private void DownloadFutures(string futBaseName, List<FinamIssuer> futList, bool isFutures,
            DateTime dtPeriodBeg, DateTime dtPeriodEnd, bool fOverwrite, bool fSkipUnfinished)
        {
            if (fSkipUnfinished) {
                // намеренно игнорируем номер дня
                var dt = new DateTime(DtPeriodMax.Year, DtPeriodMax.Month, 1);

                if (dt < dtPeriodEnd) {
                    dtPeriodEnd = dt;
                }
            }


            futBaseName += '-'; // "BR" -> "BR-"


            // базовая директория фьючерса. 
            var futBaseDir = HistDataDir + futBaseName + "\\";
            Directory.CreateDirectory(futBaseDir);


            foreach (var fut in futList) {
                /* приставка "SPFB.", которую автоматически формирует сайт финама при запросе,
                 не обязательна для корректного скачивания по сформированным urls */
                var datePart = fut.Name.Split('(')[0]; // "BR-1.09(BRF9)" -> "BR-1.09"

                // дата экспирации
                var sDate = datePart.Split('-')[1]; // // "BR-1.09" -> "1.09"
                var strExpDate = sDate.Split('.'); // "1.09" -> { "1", "09" }
                var expDateMonth = Convert.ToInt32(strExpDate[0]);
                var expDateYear = 2000 + Convert.ToInt32(strExpDate[1]);


                // для каждого фьюча запросим его дневные свечи за период [3 года до экспирации; 1 мес после экспирации]
                var futExpDate = new DateTime(expDateYear, expDateMonth, 1);
                var futDateFrom = futExpDate.AddYears(-3); // Начальная дата: 3 года до экспирации
                var futDateTo = futExpDate.AddMonths(1); // Конечная дата: 1 число месяца, следующего после экспирации


                var futName = futBaseName + $"{expDateYear}.{expDateMonth:D2}"; // BR-2009.01

                // пропускаем фьючерсы вне запрашиваемого диапазона
                if (futDateTo < dtPeriodBeg || dtPeriodEnd < futDateFrom) {
                    Inform?.Invoke($@"Фьючерс {futName} вне запрашиваемого диапазона");
                    continue;
                }

                // если fSkipUnfinished и фьючерс еще не завершен, то пропускаем загрузку
                if (fSkipUnfinished && _currentDt < futDateTo) {
                    Inform?.Invoke($@"Фьючерс {futName} еще не завершен. Пропускаем загрузку");
                    continue;
                }


                // сообщаем, что будем закачивать фьючерс futName
                Inform?.Invoke($@"Загружаем {futName}, диапазон: {futDateFrom} - {futDateTo}");


                DownloadIssuer(futName, fut.Market, fut.Id,
                    isFutures, futDateFrom, futDateTo, futBaseDir, fOverwrite);

                if (_fCancelled) {
                    return;
                }
            } // foreach
        }


        /// <summary>
        /// Загружаем файл D1, и читая его загружаем тики по отдельным датам
        /// </summary>
        private void DownloadIssuer(string issuerName, string issuerMarket, string issuerId,
            bool isFutures, DateTime dtFrom, DateTime dtTo, string baseDir, bool fOverwrite)
        {
            // директория, куда будем сохранять загруженные данные
            var issuerDir = baseDir + issuerName + "\\";
            Directory.CreateDirectory(issuerDir);

            
            var logDir = issuerDir + "logs\\";
            Directory.CreateDirectory(logDir);

            // в файл urlsLog будут записываться сформированные url
            var urlsLog = logDir + $"urls_{_currentDt:yyyy.MM.dd_HH;mm;ss}.txt";
            using (var urlsWriter = new StreamWriter(urlsLog, append: false) {AutoFlush = true}) {
                var fileD1Dir = issuerDir + "D1\\";
                Directory.CreateDirectory(fileD1Dir);


                // имя результирующего файла для дневных свечей (д.б. без расширения)
                var fileD1Name = $"{issuerName}_{dtFrom:yyMMdd}_{dtTo:yyMMdd}";
                var fileD1Path = fileD1Dir + fileD1Name + ".txt";

                /* Для акций файл D1 можно заказывать всегда.
             * Для фьючерсов заказывать надо в случаях:
             * 1) включена перезапись
             * 2) файл D1 не существует
             * 3) его дата записи раньше даты экспирации, т.е. история запрашивалась до экспирации
             */
                if (isFutures && !fOverwrite && File.Exists(fileD1Path) &&
                    File.GetLastWriteTime(fileD1Path) >= dtTo && new FileInfo(fileD1Path).Length > 0) {
                    Inform?.Invoke($@"Фьючерс {issuerName} уже загружен");
                    return ;
                }

                var url = FinDown.GetUrl(dtFrom, dtTo, fileD1Name,
                    issuerName, issuerMarket, issuerId, ETimeFrame.Day);
                urlsWriter.WriteLine(url);

                while (!_downloadService.TryDownload(url, fileD1Path, out var strErr)) {
                    Inform?.Invoke(strErr);
                    if (_fCancelled) {
                        return ;
                    }
                }

                // если полученный файл пустой, то ошибка
                var length = new FileInfo(fileD1Path).Length;
                if (length == 0) {
                    File.Delete(fileD1Path);
                    Inform?.Invoke($@"Пропускаем загрузку: получен пустой файл с датами");
                    return;
                }

                Inform?.Invoke($@"Загружен файл {fileD1Path}.");


                // читаем futD1Path, и скачиваем тики за каждую доступную дату
                DownloadTicksFromD1File(urlsWriter, issuerDir, fileD1Path,
                    issuerName, issuerMarket, issuerId, fOverwrite, isFutures);
            }
        }


        /// <summary>
        /// загрузка тиков по датам из файла
        /// </summary>
        /// <param name="urlsWriter">открытый поток куда сохраняем urls</param>
        /// <param name="saveDir">каталог, куда сохраняем тики</param>
        /// <param name="ffnD1">полный путь до файла с дневными свечами</param>
        /// <param name="issuerName">security</param>
        /// <param name="issuerMarket"></param>
        /// <param name="issuerId"></param>
        /// <param name="fOverwrite"></param>
        /// <param name="isFutures">Это акция? для фьючей тики не раскладываются по годам</param>
        private void DownloadTicksFromD1File(
            TextWriter urlsWriter, string saveDir, string ffnD1,
            string issuerName, string issuerMarket, string issuerId,
            bool fOverwrite, bool isFutures)
        {
            Assert.IsTrue(File.Exists(ffnD1));
            var lastYear = 0;
            var currentSaveDir = saveDir;

            // читаем файл со свечами D1, и скачиваем за каждую доступную дату тики
            using (var reader = new StreamReader(ffnD1)) {
                // игнорируем первую строку
                reader.ReadLine();

                while (!reader.EndOfStream) {
                    if (_fCancelled) {
                        return;
                    }

                    var str = reader.ReadLine();

                    // пустая строка не предусмотрена
                    Assert.IsFalse(string.IsNullOrWhiteSpace(str));

                    var strDate = str.Split('\t')[0];

                    var dtTick = strDate.Contains(".") || strDate.Contains("/")
                        ? DateTime.Parse(strDate, CultureInfo.CurrentCulture)
                        : new DateTime(
                            Convert.ToInt32(strDate.Substring(0, 4)),
                            Convert.ToInt32(strDate.Substring(4, 2)),
                            Convert.ToInt32(strDate.Substring(6, 2)));



                    // скачиваем тиковые данные за только за завершенные периоды
                    if (dtTick > DtPeriodMax) {
                        continue; // or break;
                    }


                    // только для акций: все тиковые данные будем хранить по годам
                    if (!isFutures && lastYear != dtTick.Year) {
                        lastYear = dtTick.Year;
                        currentSaveDir = saveDir + $"{lastYear:d4}\\";
                        Directory.CreateDirectory(currentSaveDir);
                    }


                    // имя результирующего файла для тиковых данных за конкретную дату
                    var rezultFnTick = $"{issuerName}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";
                    Inform?.Invoke(rezultFnTick);

                    var urlTick = FinDown.GetUrl(dtTick, dtTick, rezultFnTick, issuerName, issuerMarket, issuerId,
                        ETimeFrame.Tick, DataFormat.TickOptimal);
                    urlsWriter.WriteLine(urlTick);

                    var ffnTick = currentSaveDir + rezultFnTick + ".txt";
                    var ffnTick7Z = currentSaveDir + rezultFnTick + ".7z";

                    var isExistsTick = File.Exists(ffnTick);
                    var isExistsTick7Z = File.Exists(ffnTick7Z);

                    // если перезапись включена, то удаляем существующие файлы
                    if (fOverwrite) {
                        if (isExistsTick) {
                            Inform?.Invoke($@"Удаляем файл {ffnTick}");
                            File.Delete(ffnTick);
                            isExistsTick = false;
                        }

                        if (isExistsTick7Z) {
                            Inform?.Invoke($@"Удаляем файл {ffnTick7Z}");
                            File.Delete(ffnTick7Z);
                            isExistsTick7Z = false;
                        }
                    }


                    if (!fOverwrite && (isExistsTick || isExistsTick7Z)) {
                        Inform?.Invoke($@"Пропускаем загрузку: Файл(ы) {ffnTick} и/или {ffnTick7Z} уже скачен(ы)");
                        continue;
                    }

                    while (!_downloadService.TryDownload(urlTick, ffnTick, out var strErr)) {
                        Inform?.Invoke(strErr);
                        if (_fCancelled) {
                            return;
                        }
                    }

                    Inform?.Invoke(rezultFnTick + " - downloaded");
                    FileDownloaded?.Invoke();
                }

                Inform?.Invoke(string.Empty);
            }
        }
    }
}