using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Engine;
using NUnit.Framework;
using FinDownEntity;


namespace FinamDownloader
{
    internal partial class Program
    {
        /// <summary>
        /// Загрузка тиков по фьючам
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fNeedPeriod">запрашиваем период</param>
        /// <param name="fOverwrite">перезапись</param>
        /// <param name="fSkipUnfinished">skip loading unfinished futures</param>
        private static void DownloadFutures(List<FinamIssuer> issuers, bool fNeedPeriod = false,
            bool fOverwrite = false, bool fSkipUnfinished = true)
        {
            var dtBeg = DateTime.MinValue;
            var dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // намеренно игнорируем номер дня

            if (fNeedPeriod)
            {
                Console.Write("Enter dtBeg as 'year, month' (for example: '2017, 11'): ");
                var bufDt1 = Console.ReadLine()?.Split(',');
                Assert.IsTrue(bufDt1 != null && bufDt1.Length == 2);
                var y1 = Convert.ToInt32(bufDt1[0]);
                var m1 = Convert.ToInt32(bufDt1[1]);
                dtBeg = new DateTime(y1, m1, 1);

                Console.Write("Enter dtEnd as 'year, month' (for example: '2018, 12'): ");
                var bufDt2 = Console.ReadLine()?.Split(',');
                Assert.IsTrue(bufDt2 != null && bufDt2.Length == 2);
                var y2 = Convert.ToInt32(bufDt2[0]);
                var m2 = Convert.ToInt32(bufDt2[1]);
                dtEnd = new DateTime(y2, m2, 1);
            }


            const string historyDataDir = @"c:\Users\admin\Documents\SyncDocs\HistoryData\";


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
            var writer = new StreamWriter(fdUrlsLog, false) { AutoFlush = true };

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
                if (fSkipUnfinished && curDt < dtT || dtT < dtBeg || dtEnd < dtT)
                {
                    continue;
                }


                // имя результирующего файла (д.б. без расширения) для дневных свечей по каждому фьючерсу
                var rezultFnD1 = $"{code}_{dtF:yyMMdd}_{dtT:yyMMdd}";

                var ffn = saveDirBase + rezultFnD1 + ".txt";

                if (fOverwrite || !File.Exists(ffn))
                {
                    Console.Write(code);

                    var url = GetUrl(dtF, dtT, rezultFnD1, code, fut.Market, fut.Id, ETimeFrame.Day,
                        DataFormat.CandleOptimal);
                    writer.WriteLine(url);

                    while (!TryDownload(url, ffn))
                    { }
                }


                // теперь надо прочитать скачанный файл, и скачать за каждую доступную дату тики
                using (var reader = new StreamReader(ffn))
                {
                    var saveDir2 = saveDirBase + $@"{code}\";
                    Directory.CreateDirectory(saveDir2);

                    // игнорируем первую строку
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var str = reader.ReadLine();

                        // пустая строка не предусмотрена
                        Assert.IsNotNull(str);

                        var strDate = str.Split('\t')[0];

                        var dtTick = strDate.Contains('.')
                            ? DateTime.Parse(strDate, CultureInfo.CurrentCulture)
                            : new DateTime(
                                Convert.ToInt32(strDate.Substring(0, 4)),
                                Convert.ToInt32(strDate.Substring(4, 2)),
                                Convert.ToInt32(strDate.Substring(6, 2)));


                        // скачиваем тиковые данные за только за завершенные периоды
                        if (dtTick >= curDt)
                        {
                            continue;
                        }

                        // имя результирующего файла для тиковых данных за конкретную дату
                        var rezultFnTick = $"{code}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";

                        var urlTick = GetUrl(dtTick, dtTick, rezultFnTick, code, fut.Market, fut.Id, ETimeFrame.Tick,
                            DataFormat.TickOptimal);
                        writer.WriteLine(urlTick);

                        var ffnTick = saveDir2 + rezultFnTick + ".txt";
                        var ffnTick7Z = saveDir2 + rezultFnTick + ".7z";
                        if (fOverwrite || (!File.Exists(ffnTick) && !File.Exists(ffnTick7Z)))
                        {
                            while (!TryDownload(urlTick, ffnTick))
                            { }

                            Console.Write(".");
                        }
                    }

                    Console.Write('\n');
                }
            }

            writer.Close();
        }
    }
}