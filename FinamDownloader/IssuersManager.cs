using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using FinDownEntity;
using NUnit.Framework;


namespace FinamDownloader {
    /// <summary>
    /// Класс, выполняющий работу по загрузке и поиску эмитентов
    /// </summary>
    class IssuersManager {
        private List<FinamIssuer> issuers;

        private IchartsData _icharts;
        //C:\Users\admin\Documents\main\programming\visualstudio\FinamDownloader\FinamDownloader\IchartsData.cs
        DateTime dtPeriodMin;
        DateTime dtPeriodMax;

        private Settings _settings;


        private IDownloadService _downloadService;

        public IssuersManager(List<FinamIssuer> issuers) {
            this.issuers = issuers;

            // часовой пояс - мск
            var currentDt = DateTime.Now.AddHours(-2);

            var dtPeriodMin = new DateTime(1979, 1, 1); // todo дата с сайта финам???
            var dtPeriodMax =
                currentDt.AddDays(-1).Date; // за сегодняшнюю дату закачивать нельзя. самое позднее - за вчера



        }


        /* 
 * свалка:
 * List<FinamIssuer> issuers,
 * DateTime dtPeriodBeg, DateTime dtPeriodEnd,
 *
 * issuer.Name, issuer.Market, issuer.Id,
 * bool isFutures,
 * bool fOverwrite = false
 */

        bool DownloadIssuers(string issuerName, string issuerMarket, string issuerId,
            bool fAllTime, DateTime dtPeriodBeg, DateTime dtPeriodEnd,
            bool isFutures, bool fSkipUnfinished, bool fOverwrite) {

            if (fAllTime) {
                dtPeriodBeg = dtPeriodMin;
                dtPeriodEnd = dtPeriodMax;
            }

            if (dtPeriodBeg < dtPeriodMin) {
                dtPeriodBeg = dtPeriodMin;
            }

            if (dtPeriodEnd > dtPeriodMax) {
                dtPeriodEnd = dtPeriodMax;
            }


            return true;
        }

        bool DownloadFutures(string issuerName, string issuerMarket, string issuerId,
            bool fAllTime, DateTime dtPeriodBeg, DateTime dtPeriodEnd,
            bool isFutures, bool fSkipUnfinished, bool fOverwrite) {
            return true;
        }

        bool DownloadShare(string issuerName, string issuerMarket, string issuerId,
            bool fAllTime, DateTime dtPeriodBeg, DateTime dtPeriodEnd,
            bool isFutures, bool fSkipUnfinished, bool fOverwrite) {
            return true;
        }

        bool DownloadD1AndLower(string baseDir, DateTime dtFrom, DateTime dtTo,
            string issuerName, string issuerMarket, string issuerId,
            bool fOverwrite, bool isFutures) {
            return true;
        }


        /// <summary>
        /// Загрузка тиков по фьючам
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fNeedPeriod">запрашиваем период</param>
        /// <param name="fOverwrite">перезапись</param>
        /// <param name="fSkipUnfinished">skip loading unfinished futures. Данный флаг иммеет отношение
        /// только к фьючерсам, но не к конкретным дням. не завершенные дни мы не загружаем всегда</param>
        private void DownloadFutures(List<FinamIssuer> issuers, string futBaseName, bool fNeedPeriod = false,
            bool fOverwrite = false, bool fSkipUnfinished = true)
        {
            var dtPeriodBeg = new DateTime(1979, 1, 1); // самая ранняя дата, доступная в finam
            var dtPeriodEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // намеренно игнорируем номер дня


            Assert.IsTrue(futBaseName != null && futBaseName.Length == 2);

            const int futCodeLen = 4; // for example: BRU9, MXZ7, etc.

            var futList = issuers.FindAll(issuer =>
                issuer.Name.Contains("-") && issuer.Name.Contains(".") && // "BR-1.09(BRF9)"
                futCodeLen == issuer.Code.Length &&
                issuer.Code.Substring(0, futBaseName.Length) == futBaseName); // "BRF9"


            futBaseName += '-'; // "BR" -> "BR-"


            // базовая директория фьючерса. 
            var futBaseDir = _settings.HistDataDir + futBaseName + "\\";
            Directory.CreateDirectory(futBaseDir);


            var curDt = DateTime.Now.AddHours(-2);

            foreach (var fut in futList)
            {
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


                // если фьючерс еще не завершен, то пропускаем загрузку
                // До тех пор пока мы не скачиваем не завершенные фьючерсы - их не надо докачивать
                if (fSkipUnfinished && curDt < futDateTo || futDateTo < dtPeriodBeg || dtPeriodEnd < futDateTo)
                {
                    continue;
                }

                var futName = futBaseName + $"{expDateYear}.{expDateMonth:D2}"; // BR-2009.01



                // сообщаем в консоль, что будем закачивать фьючерс futName
                Console.WriteLine(futName); // todo


                var fb = DownloadIssuer(futBaseDir, futDateFrom, futDateTo,
                    futName, fut.Market, fut.Id, fOverwrite, isFutures: true);

                if (!fb)
                {
                    return;
                }
            }
        }


        /// <summary>
        /// поиск эмитента
        /// </summary>
        /// <param name="issuers">имя эмитента</param>
        /// <param name="equalityRequired">требование полного совпадения строки поиска и имени эмитента</param>
        private void FindIssuers(List<FinamIssuer> issuers, bool equalityRequired = false) {
            Console.Write(@"enter name of issuer: "); // todo
            var name = Console.ReadLine(); // todo
            Assert.IsFalse(string.IsNullOrWhiteSpace(name));

            var issuersList = issuers.FindAll(issuer =>
                equalityRequired ? name == issuer.Name : issuer.Name.Contains(name));
            Console.WriteLine($@"issuersList.Count  = {issuersList.Count}"); // todo

            // если найдено не более 5 эмитентов, то молча печатаем с полным описанием
            const int max = 5;
            var fullDescr = true;

            // если найдено более 5 эмитентов, то спрашиваем надо ли печатать
            if (issuersList.Count > max) {
                string bCh;
                do {
                    Console.WriteLine(@"do you want to print all of issuersList names?"); // todo
                    Console.WriteLine(@"1 - OK (min description), 2 - OK (full description), 0 - No"); // todo
                    bCh = Console.ReadLine();
                } while (bCh != "1" && bCh != "2" && bCh != "0");

                if (bCh == "0")
                    return;

                // нужно ли полное описание?
                fullDescr = bCh == "2";
            }

            Console.WriteLine(FinamIssuer.GetDescriptionHead(fullDescr)); // todo
            foreach (var issuer in issuersList) {
                Console.WriteLine(issuer.GetDescription(fullDescr)); // todo
            }
        }

        /// <summary>
        /// Загрузка тиков по акциям
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fNeedPeriod">запрашиваем период</param>
        /// <param name="fOverwrite">перезапись</param>
        private bool DownloadShare(
            List<FinamIssuer> issuers,
            DateTime dtBeg,
            DateTime dtEnd,
            string shareName,
            string shareMarket,
            string shareId,
            bool fOverwrite,
            bool fEqualName) {
            //GetPeriod(fNeedPeriod, out var dtBeg, out var dtEnd);





            var shares = issuers.FindAll(s =>
                (fEqualName && s.Name == shareName || s.Name.Contains(shareName)) &&
                (shareMarket == "" || s.Market == shareMarket) &&
                (shareId == "" || s.Id == shareId)
            );


            if (shares == null) {
                //logging("не найдено ни одного эмитента");
                //return false;

            }

            if (shares.Count > 1) {
                //logging("Найдено больше одного эмитента. Уточните условия поиска");
                //return false;

            }






            DownloadIssuer(_settings.HistDataDir, dtBeg, dtEnd,
                shareName, shareMarket, shareId, fOverwrite, isFutures: false);

            return true;
        }

        private bool DownloadIssuer(string baseDir, DateTime dtFrom, DateTime dtTo,
            string issuerName, string issuerMarket, string issuerId,
            bool fOverwrite, bool isFutures) {
            // директория, куда будем сохранять загруженные данные
            var issuerDir = baseDir + issuerName + "\\";
            Directory.CreateDirectory(issuerDir);

            // todo у Сани это работать будет не правильно
            // берем московское время
            var curDt = DateTime.Now.AddHours(-2);

            var logDir = issuerDir + "logs\\";
            Directory.CreateDirectory(logDir);

            // в файл urlsLog будут записываться сформированные url
            var urlsLog = logDir + $"urls_{curDt:yyyy.MM.dd_HH;mm;ss}.txt";
            var urlsWriter = new StreamWriter(urlsLog, append: false) {AutoFlush = true};

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
            if (!isFutures || fOverwrite ||
                !File.Exists(fileD1Path) || File.GetLastWriteTime(fileD1Path) < dtTo) {

                var url = FinDown.GetUrl(dtFrom, dtTo, fileD1Name,
                    issuerName, issuerMarket, issuerId,
                    ETimeFrame.Day, DataFormat.CandleOptimal);
                urlsWriter.WriteLine(url);

                while (!_downloadService.TryDownload(url, fileD1Path, out var strErr)) { }


                // если полученный файл пустой, то ошибка
                var length = new FileInfo(fileD1Path).Length;
                if (length == 0) {
                    File.Delete(fileD1Path);
                    Console.WriteLine("получен пустой файл с датами"); // todo
                    return false;
                }

                // читаем futD1Path, и скачиваем тики за каждую доступную дату
                DownloadTicksFromD1File(urlsWriter, issuerDir, fileD1Path,
                    issuerName, issuerMarket, issuerId, fOverwrite, isFutures);

                urlsWriter.Close();
            }

            return true;
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
            TextWriter urlsWriter,
            string saveDir,
            string ffnD1,
            string issuerName,
            string issuerMarket,
            string issuerId,
            bool fOverwrite,
            bool isFutures) {
            Assert.IsTrue(File.Exists(ffnD1));

            // todo Date
            var curDt = DateTime.Now.AddHours(-2); // корректировка на мск

            // читаем файл со свечами D1, и скачиваем за каждую доступную дату тики
            using (var reader = new StreamReader(ffnD1)) {
                // игнорируем первую строку
                reader.ReadLine();

                while (!reader.EndOfStream) {
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
                    if (dtTick >= curDt) {
                        continue; // or break;
                    }


                    // только для акций: все тиковые данные будем хранить по годам
                    if (!isFutures) {
                        saveDir += $"{dtTick:yyyy}\\";
                        Directory.CreateDirectory(saveDir);
                    }


                    // имя результирующего файла для тиковых данных за конкретную дату
                    var rezultFnTick = $"{issuerName}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";

                    var urlTick = FinDown.GetUrl(dtTick, dtTick, rezultFnTick, issuerName, issuerMarket, issuerId,
                        ETimeFrame.Tick, DataFormat.TickOptimal);
                    urlsWriter.WriteLine(urlTick);

                    var ffnTick = saveDir + rezultFnTick + ".txt";
                    var ffnTick7Z = saveDir + rezultFnTick + ".7z";

                    var isExistsTick = File.Exists(ffnTick);
                    var isExistsTick7Z = File.Exists(ffnTick7Z);

                    // если перезапись включена, то удаляем существующие файлы
                    if (fOverwrite) {
                        if (isExistsTick) {
                            File.Delete(ffnTick);
                            isExistsTick = false;
                        }

                        if (isExistsTick7Z) {
                            File.Delete(ffnTick7Z);
                            isExistsTick7Z = false;
                        }
                    }


                    if (fOverwrite || (!isExistsTick && !isExistsTick7Z)) {
                        while (!_downloadService.TryDownload(urlTick, ffnTick, out var strErr)) { }

                        Console.WriteLine(rezultFnTick); // todo
                    }
                }

                Console.WriteLine(string.Empty); // todo
            }
        }


    }

}