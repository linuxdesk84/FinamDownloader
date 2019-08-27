using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using NUnit.Framework;


namespace FinamDownloader
{
    internal partial class Program
    {
        private static void Main()
        {
            const string dir = @"d:\SyncDirs\main\pdata\visualstudio\FinamDownloader\icharts_analyze\";
            const string fn = "icharts.js";

            var icharts = new Icharts(dir + fn);
            var issuers = icharts.Issuers;


            var ch = "";
            do
            {
                Console.WriteLine(@"1 - поиск инструмента (содержит подстроку)");
                Console.WriteLine(@"2 - поиск инструмента (равно строке)");
                Console.WriteLine(@"3 - загрузка фьючерса");

                ch = Console.ReadLine();
            } while (ch != "1" && ch != "2" && ch != "3");

            switch (Convert.ToInt32(ch))
            {
                case 1:
                    FindIssuers(issuers);
                    break;

                case 2:
                    FindIssuers(issuers, true, false);
                    break;

                case 3:
                    DownloadFuture(issuers, true);
                    break;

                default:
                    break;
            }


            Console.ReadKey();
        }

        // требуем равенство
        private static void FindIssuers(List<FinamIssuer> issuers, bool equalityRequired = false, bool fullDescr = true)
        {
            Console.Write(@"enter name of issuer: ");
            var name = Console.ReadLine();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name));

            var issuersList = issuers.FindAll(issuer =>
                equalityRequired ? name == issuer.Name : issuer.Name.Contains(name));
            Console.WriteLine($"issuersList.Count  = {issuersList.Count}");

            var bCh = "";
            const int max = 50;

            if (issuersList.Count > max)
            {
                Console.WriteLine(@"do you want to print all of issuersList names? (Y = yes, anyKey = no)");
                bCh = Console.ReadLine()?.ToUpper();
            }

            if (issuersList.Count <= max || bCh == "Y")
            {
                foreach (var issuer in issuersList)
                {
                    Console.WriteLine(issuer.GetDescription(fullDescr));
                }
            }
        }

        /// <summary>
        /// Загрузка тиков по фьючам
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fSkipUnfinished">skip loading unfinished futures</param>
        private static void DownloadFuture(List<FinamIssuer> issuers, bool fSkipUnfinished)
        {
            const string historyDataDir = @"c:\Users\admin\Documents\HistoryData\";

            Console.Write(@"Enter future name (for example: BR, MX, MM, SR, etc.): ");
            var futNameBase = Console.ReadLine();
            Assert.IsTrue(futNameBase != null && futNameBase.Length == 2);

            const int futCodeLen = 4; // for example: BRU9, MXZ7, etc.

            futNameBase += '-'; // "BR" -> "BR-"
            var futList = issuers.FindAll(issuer =>
                issuer.Name.Length >= futNameBase.Length &&
                issuer.Name.Substring(0, futNameBase.Length) == futNameBase &&
                futCodeLen == issuer.Code.Length);


            var saveDirBase = historyDataDir + futNameBase + "\\";
            Directory.CreateDirectory(saveDirBase);

            // в файл writer будут записываться сформированные urls
            var curDt = DateTime.Now;
            var fdUrlsLog = historyDataDir + $"FD_urls_{curDt:yyyyMMdd_HHmmss}.txt";
            var writer = new StreamWriter(fdUrlsLog, false) {AutoFlush = true};

            foreach (var fut in futList)
            {
                /* приставка "SPFB.", которую автоматически формирует сайт финама при запросе,
                 не обязательна для корректного скачивания по сформированным urls */
                var code = fut.Name.Split('(')[0]; // "BR-1.09(BRF9)" -> "BR-1.09"

                // дата экспирации
                var expDateStr = code.Substring(futNameBase.Length).Split('.'); // "BR-1.09" -> "1.09" -> { "1", "09" }
                var expDateM = Convert.ToInt32(expDateStr[0]);
                var expDateY = 2000 + Convert.ToInt32(expDateStr[1]);

                // для каждого фьюча запросим его дневные свечи за период [3 года до экспирации; 1 мес после экспирации]
                var expDt = new DateTime(expDateY, expDateM, 1);
                var dtF = expDt.AddYears(-3); // Начальная дата: 3 года до экспирации
                var dtT = expDt.AddMonths(1); // Конечная дата: следующий месяц после экспирации


                // если фьючерс еще не завершен, то пропускаем загрузку
                if (fSkipUnfinished && curDt < dtT)
                {
                    continue;
                }


                // имя результирующего файла (д.б. без расширения) для дневных свечей по каждому фьючерсу
                var rezultFnD1 = $"{code}_{dtF:yyMMdd}_{dtT:yyMMdd}";


                var url = GetUrl(dtF, dtT, rezultFnD1, code, fut, TimeFrame.Day, DataFormat.CandleOptimal, at: ColumnHeaderNeed.No);
                writer.WriteLine(url);

                var ffn = saveDirBase + rezultFnD1 + ".txt";
                while (!TryDownload(url, ffn))
                {
                }
                Console.Write(code);


                // теперь надо прочитать скачанный файл, и скачать за каждую доступную дату тики
                using (var reader = new StreamReader(ffn))
                {
                    var saveDir2 = saveDirBase + $@"{code}\";
                    Directory.CreateDirectory(saveDir2);

                    //var writer2 = new StreamWriter(saveDir2 + "urls.txt", false) { AutoFlush = true };

                    while (!reader.EndOfStream)
                    {
                        var str = reader.ReadLine();

                        // пустая строка не предусмотрена
                        Assert.IsNotNull(str);

                        var strDate = str.Split('\t')[0];

                        var year = Convert.ToInt32(strDate.Substring(0, 4)); // "2019" -> 2019
                        var month = Convert.ToInt32(strDate.Substring(4, 2)); // "06" -> 6
                        var day = Convert.ToInt32(strDate.Substring(6, 2)); // "01 -> 1

                        var dtTick = new DateTime(year, month, day);

                        // скачиваем тиковые данные за только за завершенные периоды
                        if (curDt <= dtTick)
                        {
                            continue;
                        }

                        // имя результирующего файла для тиковых данных за конкретную дату
                        var rezultFnTick = $"{code}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";

                        var urlTick = GetUrl(dtTick, dtTick, rezultFnTick, code, fut, TimeFrame.Tick, DataFormat.TickOptimal);
                        writer.WriteLine(urlTick);

                        var ffn2 = saveDir2 + rezultFnTick + ".txt";
                        while (!TryDownload(urlTick, ffn2))
                        {
                        }
                        Console.Write(".");
                    }
                    Console.Write('\n');
                }
            }

            writer.Close();
        }

        /// <summary>
        /// Попытка выполнить загрузку (true - успешно, false - ошибка)
        /// </summary>
        public static bool TryDownload(string url, string fn)
        {
            return true;

            // результат выполнения загрузки
            var res = true;

            var wc = new WebClient();
            try
            {
                wc.DownloadFile(url, fn);
                Thread.Sleep(2000);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                res = false;
            }

            return res;
        }

        private static string GetUrl(DateTime dtF, DateTime dtT, string rezultFn, string code, FinamIssuer issuer, TimeFrame tf,
            DataFormat datf = DataFormat.CandleAllParam,
            DateFormat dtf = DateFormat.YYYYMMDD,
            TimeFormat tmf = TimeFormat.HHMMSS,
            CandleTime ct = CandleTime.Open,
            FieldSeparator fs = FieldSeparator.Tab,
            BitSeparator bs = BitSeparator.None,
            ColumnHeaderNeed at = ColumnHeaderNeed.Yes)
        {
            Assert.IsTrue(tf == TimeFrame.Tick && datf == DataFormat.TickOptimal ||
                          tf != TimeFrame.Tick && datf != DataFormat.TickOptimal);


            // генерируем url
            var url = "http://export.finam.ru/" +
                      $"{rezultFn}.txt?" +
                      $"market={issuer.Market}&" + // Номер рынка
                      $"em={issuer.Id}&" + // Номер инструмента
                      $"code={code}&" + // Тикер инструмента
                      $"apply=0&" + // todo ?

                      $"df={dtF.Day}&" + // Начальная дата, номер дня (1-31)
                      $"mf={dtF.Month - 1}&" + // Начальная дата, номер месяца (0-11)
                      $"yf={dtF.Year}&" + // Начальная дата, год
                      $"from={dtF:dd.MM.yyyy}&" + // Начальная дата в формате "ДД.ММ.ГГГГ" (здесь месяцы от 1 до 12)

                      $"dt={dtT.Day}&" + // Конечная дата, номер дня (1-31)
                      $"mt={dtT.Month - 1}&" + // Конечная дата, номер месяца (0-11)
                      $"yt={dtT.Year}&" + // Конечная дата, год
                      $"to={dtT:dd.MM.yyyy}&" + // Конечная дата в формате "ДД.ММ.ГГГГ" (здесь месяцы от 1 до 12)

                      $"p={(int) tf}&" + // Таймфрейм
                      $"f={rezultFn}&" + // Имя сформированного файла
                      $"e=.txt&" + // Расширение сформированного файла: ".txt" или ".csv"
                      $"cn={code}&" + // Имя контракта

                      $"dtf={(int) dtf}&" + // Номер формата дат
                      $"tmf={(int) tmf}&" + // Номер формата времени
                      $"MSOR={(int) ct}&" + // Время свечи (0 - open; 1 - close)

                      $"mstime=on&" + // Московское время	( "on", "off")
                      $"mstimever=1&" + // Коррекция часового пояса
                      $"sep={(int) fs}&" + // Разделитель полей
                      $"sep2={(int) bs}&" + // Разделитель разрядов
                      $"datf={(int) datf}&" + // Формат записи в файл
                      $"at={(int) at}"; // Нужны ли заголовки столбцов (0 - нет, 1 - да)

            return url;
        }
    }
}
