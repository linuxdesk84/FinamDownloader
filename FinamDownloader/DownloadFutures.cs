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
            var dtBeg = new DateTime(1979, 1, 1);
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


            const int futBaseCodeLen = 2; // for example: BR, MX, MM, SR
            const int futCodeLen = 4; // for example: BRU9, MXZ7, etc.


            Console.Write(@"Enter future base Code (for example: BR, MX, MM, SR, etc.");
            var futBaseCode = Console.ReadLine();
            Assert.IsTrue(futBaseCode != null && futBaseCode.Length >= futBaseCodeLen);

            var futBaseName = futBaseCode;
            switch (futBaseCode) {
                case "MX":
                    futBaseName = "MIX";
                    break;
                case "MM":
                    futBaseName = "MXI";
                    break;
            }
            futBaseName += '-'; // "BR" -> "BR-" or "MIX" -> "MIX-"

            var futList = issuers.FindAll(issuer =>
                issuer.Code.Length == futCodeLen &&
                issuer.Code.Substring(0, futBaseCode.Length) == futBaseCode &&
                issuer.Name.Length >= futBaseName.Length &&
                issuer.Name.Substring(0, futBaseName.Length) == futBaseName &&
                true);


            var futBaseDir = HistoryDataDir + futBaseCode + "\\";
            Directory.CreateDirectory(futBaseDir);
            

            var curDt = DateTime.Now.AddHours(-2);

            foreach (var fut in futList)
            {
                /* приставка "SPFB.", которую автоматически формирует сайт финама при запросе,
                 не обязательна для корректного скачивания по сформированным urls */
                var name = fut.Name.Split('(')[0]; // "BR-1.09(BRF9)" -> "BR-1.09"

                // дата экспирации
                var expDateStr = name.Substring(futBaseName.Length).Split('.'); // "BR-1.09" -> "1.09" -> { "1", "09" }
                var expDateM = Convert.ToInt32(expDateStr[0]);
                var expDateY = 2000 + Convert.ToInt32(expDateStr[1]);

                var futName = futBaseCode + "-" + $"{expDateY}.{expDateM:D2}"; // BR-2009.01

                // для каждого фьюча запросим его дневные свечи за период [3 года до экспирации; 1 мес после экспирации]
                var expDt = new DateTime(expDateY, expDateM, 1);
                var dtF = expDt.AddYears(-3); // Начальная дата: 3 года до экспирации
                var dtT = expDt.AddMonths(1); // Конечная дата: следующий месяц после экспирации


                // если фьючерс еще не завершен, то пропускаем загрузку
                // До тех пор пока мы не скачиваем не завершенные фьючерсы - их не надо докачивать
                if (fSkipUnfinished && curDt < dtT || dtT < dtBeg || dtEnd < dtT)
                {
                    continue;
                }



                // create dir futDir
                var futDir = futBaseDir + $"{futName}\\";
                Directory.CreateDirectory(futDir);


                var futDirLog = futDir + "logs\\";
                Directory.CreateDirectory(futDirLog);
                var futUrlsLog = futDirLog + $"urls_{curDt:yyyy.MM.dd_HH;mm;ss}.txt";

                using (var futLog = new StreamWriter(futUrlsLog))
                {
                    // create dir futDirD1
                    var futDirD1 = futDir + "D1\\";
                    Directory.CreateDirectory(futDirD1);

                    // имя результирующего файла для дневных свечей (д.б. без расширения)
                    var futD1Name = $"{futName}_{dtF:yyMMdd}_{dtT:yyMMdd}";


                    var futD1Path = futDirD1 + futD1Name + ".txt";

                    if (!File.Exists(futD1Path) || fOverwrite)
                    {
                        Console.WriteLine(futName);

                        var url = GetUrl(dtF, dtT, futD1Name, futName, fut.Market, fut.Id, ETimeFrame.Day,
                            DataFormat.CandleOptimal);
                        futLog.WriteLine(url);

                        while (!TryDownload(url, futD1Path))
                        { }


                        // читаем futD1Path, и скачиваем тики за каждую доступную дату
                        DownloadTicksFromD1File(false, futLog, futDir, futD1Path, futName, fut.Market, fut.Id,
                            fOverwrite);
                    }
                }
            }
        }
    }
}