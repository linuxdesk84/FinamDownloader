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
        /* Возможные сценарии использования:
         *  1) докачать данные
         *      а) только завершенные фьючерсы, т.е. те для которых экспирация была в прошлом месяце.
         *      б) все доступные данные
         *  2) закачать всю историю:
         *      а)
         *      б)
         *  3) закачать за определенный период
         *      а)
         *      б)
         *      
         *     **
         *      б) 
         *
         */


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
            var dtPeriodBeg = new DateTime(1979, 1, 1);
            var dtPeriodEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // намеренно игнорируем номер дня

            // запрашиваем дату
            if (fNeedPeriod)
            {
                Console.Write("Enter dtPeriodBeg as 'year, month' (for example: '2017, 11'): ");
                var bufDt1 = Console.ReadLine()?.Split(',');
                Assert.IsTrue(bufDt1 != null && bufDt1.Length == 2);
                var y1 = Convert.ToInt32(bufDt1[0]);
                var m1 = Convert.ToInt32(bufDt1[1]);
                dtPeriodBeg = new DateTime(y1, m1, 1);

                Console.Write("Enter dtPeriodEnd as 'year, month' (for example: '2018, 12'): ");
                var bufDt2 = Console.ReadLine()?.Split(',');
                Assert.IsTrue(bufDt2 != null && bufDt2.Length == 2);
                var y2 = Convert.ToInt32(bufDt2[0]);
                var m2 = Convert.ToInt32(bufDt2[1]);
                dtPeriodEnd = new DateTime(y2, m2, 1);
            }


            Console.Write(@"Enter future Base name (for example: BR, MX, MM, SR, etc.): ");

            var futBaseName = Console.ReadLine();
            Assert.IsTrue(futBaseName != null && futBaseName.Length == 2);

            const int futCodeLen = 4; // for example: BRU9, MXZ7, etc.

            var futList = issuers.FindAll(issuer =>
                issuer.Name.Contains('-') && issuer.Name.Contains('.') &&
                futCodeLen == issuer.Code.Length &&
                issuer.Code.Substring(0, futBaseName.Length) == futBaseName);


            futBaseName += '-'; // "BR" -> "BR-"


            // базовая директория фьючерса. 
            var futBaseDir = HistoryDataDir + futBaseName + "\\";
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
                if (fSkipUnfinished && curDt < futDateTo || futDateTo < dtPeriodBeg || dtPeriodEnd < futDateTo) {
                    continue;
                }



                // create dir futDir
                var futName = futBaseName + $"{expDateYear}.{expDateMonth:D2}"; // BR-2009.01
                var futDir = futBaseDir + $"{futName}\\";
                Directory.CreateDirectory(futDir);


                var futDirLog = futDir + "logs\\";
                Directory.CreateDirectory(futDirLog);
                var futUrlsLog = futDirLog + $"urls_{curDt:yyyy.MM.dd_HH;mm;ss}.txt";

                using (var futLog = new StreamWriter(futUrlsLog, append: true))
                {
                    // create dir futDirD1
                    var futDirD1 = futDir + "D1\\";
                    Directory.CreateDirectory(futDirD1);

                    // имя результирующего файла для дневных свечей (!д.б. без расширения)
                    var futD1Name = $"{futName}_{futDateFrom:yyMMdd}_{futDateTo:yyMMdd}";


                    var futD1Path = futDirD1 + futD1Name + ".txt";

                    // файл futD1Path может быть ранее создан принудительно для не завершенного фьючерса
                    // надо проверять дату такого файла!

                    if (!File.Exists(futD1Path) || fOverwrite)
                    {
                        Console.WriteLine(futName);

                        var url = GetUrl(futDateFrom, futDateTo, futD1Name, futName, fut.Market, fut.Id, ETimeFrame.Day,
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