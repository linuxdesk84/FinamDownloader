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
        /// Загрузка тиков по акциям
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fNeedPeriod">запрашиваем период</param>
        /// <param name="fOverwrite">перезапись</param>
        private static void DownloadShares(List<FinamIssuer> issuers, bool fNeedPeriod = false, bool fOverwrite = false)
        {
            void GetPeriod(bool fNeedPeriod2, out DateTime dtFrom, out DateTime dtTo)
            {
                dtFrom = new DateTime(1979, 1, 1);
                dtTo = DateTime.Now.AddHours(-2);

                if (!fNeedPeriod2)
                    return;


                Console.Write("Enter dtBeg as 'ДД/ММ/ГГГГ': ");
                var bufDt1 = Console.ReadLine();
                if (bufDt1 != "")
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(bufDt1));
                    var dt1 = DateTime.Parse(bufDt1, CultureInfo.CurrentCulture);
                    if (dtFrom < dt1)
                    {
                        dtFrom = dt1;
                    }
                }


                Console.Write("Enter dtEnd as 'ДД/ММ/ГГГГ': ");
                var bufDt2 = Console.ReadLine();
                if (bufDt2 != "")
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(bufDt2));
                    var dt2 = DateTime.Parse(bufDt2, CultureInfo.CurrentCulture);
                    if (dt2 < dtTo)
                    {
                        dtTo = dt2;
                    }
                }
            }


            GetPeriod(fNeedPeriod, out var dtBeg, out var dtEnd);



            Console.Write(@"Enter 'Name,Id,MarketNum': ");
            var shareName = Console.ReadLine();
            Assert.IsFalse(string.IsNullOrWhiteSpace(shareName));


            var id = "-1";
            var marketNum = "-1";

            if (shareName.Contains(','))
            {
                var buf = shareName.Split(',');
                Assert.IsTrue(buf.Length == 3);

                shareName = buf[0];
                id = buf[1];
                marketNum = buf[2];
            }


            var shareDir = HistoryDataDir + shareName + "\\";
            if (!Directory.Exists(shareDir))
            {
                Directory.CreateDirectory(shareDir);
            }


            var shareDirD1 = shareDir + "D1\\";
            if (!Directory.Exists(shareDirD1))
            {
                Directory.CreateDirectory(shareDirD1);
            }



            // в файл urlsWriter будут записываться сформированные url
            var curDt = DateTime.Now.AddHours(-2);

            var logDir = shareDir + "logs\\";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }


            var fdUrlsLog = logDir + $"urls_{curDt:yyyy.MM.dd_HH;mm;ss}.txt";
            var urlsWriter = new StreamWriter(fdUrlsLog, false) { AutoFlush = true };



            if (id == "-1" && marketNum == "-1")
            {
                // акций с одним и тем же именем может быть несколько (например, SBER)
                var shares = issuers.FindAll(s => s.Name == shareName);
                if (shares.Count == 0)
                {
                    Console.WriteLine("Акции с таким кодом не найдены");
                    return;
                }

                if (shares.Count > 1)
                {
                    Console.WriteLine($"найдено акций с таким кодом: {shares.Count}");
                    foreach (var issuer in shares)
                    {
                        Console.WriteLine($"{issuer.GetDescription()}");
                    }
                }



                // запросим дневные свечи за период [dtBeg; dtEnd] для каждого shares[i]
                var nonEmptyShares = new List<int>();
                var nonEmptyFns = new List<string>();


                for (var i = 0; i < shares.Count; i++)
                {
                    var issuer = shares[i];

                    // имя результирующего файла для дневных свечей. имя передается без расширения.
                    var fnD1 = $"{shareName}({issuer.Id},{issuer.Market})_{dtBeg:yyMMdd}_{dtEnd:yyMMdd}";
                    var fnD1Txt = fnD1 + ".txt";

                    var ffnD1 = shareDirD1 + fnD1Txt;

                    var url = GetUrl(dtBeg, dtEnd, fnD1, shareName, issuer.Market, issuer.Id, ETimeFrame.Day, DataFormat.CandleOptimal);
                    urlsWriter.WriteLine(url);

                    while (!TryDownload(url, ffnD1))
                    {
                    }


                    // ищем не пустые файлы
                    var length = new FileInfo(ffnD1).Length;
                    if (length > 0)
                    {
                        nonEmptyShares.Add(i);
                        nonEmptyFns.Add(fnD1Txt);
                    }
                    else
                    {
                        File.Delete(ffnD1);
                    }
                }



                if (nonEmptyShares.Count == 0)
                {
                    Console.WriteLine($"Для всех issuer получены пустые файлы с датами");
                    return;
                }

                foreach (var idx in nonEmptyShares)
                {
                    var issuer = shares[idx];
                    var fn = nonEmptyFns[idx];

                    if (nonEmptyShares.Count == 1)
                    {
                        var fnNew = $"{shareName}_{dtBeg:yyMMdd}_{dtEnd:yyMMdd}";
                        File.Move(shareDir + fn, shareDir + fnNew);
                        fn = fnNew;
                    }

                    DownloadShareFromFile(urlsWriter, shareDir, fn,
                        shareName, issuer.Market, issuer.Id, fOverwrite);
                }

            }
            else
            {
                // имя результирующего файла для дневных свечей. имя передается без расширения.
                var fnD1 = $"{shareName}({id},{marketNum})_{dtBeg:yyMMdd}_{dtEnd:yyMMdd}";
                var fnD1Txt = fnD1 + ".txt";

                var ffnD1 = shareDirD1 + fnD1Txt;

                var url = GetUrl(dtBeg, dtEnd, fnD1, shareName, marketNum, id, ETimeFrame.Day, DataFormat.CandleOptimal);
                urlsWriter.WriteLine(url);

                while (!TryDownload(url, ffnD1))
                {
                }


                // ищем не пустые файлы
                var length = new FileInfo(ffnD1).Length;
                if (length == 0)
                {
                    File.Delete(ffnD1);
                    Console.WriteLine("получен пустой файлы с датами");
                    return;
                }


                DownloadShareFromFile(urlsWriter, shareDir, ffnD1,
                    shareName, marketNum, id, fOverwrite);
            }


            urlsWriter.Close();
        }

        /// <summary>
        /// загрузка тиков по датам из файла
        /// </summary>
        /// <param name="urlsWriter"></param>
        /// <param name="shareDir">папка акции</param>
        /// <param name="ffnD1">полный путь до файла с дневными свечами</param>
        /// <param name="shareName"></param>
        /// <param name="shareMarket"></param>
        /// <param name="shareId"></param>
        /// <param name="fOverwrite"></param>
        private static void DownloadShareFromFile(TextWriter urlsWriter, string shareDir, string ffnD1,
            string shareName, string shareMarket, string shareId, bool fOverwrite = false)
        {
            var curDt = DateTime.Now.AddHours(-2); // корректировка на мск

            Assert.IsTrue(File.Exists(ffnD1));
            var fn = Path.GetFileName(ffnD1);
            Assert.IsTrue(fn != null && fn.Contains('_'));

            var fnShareName = fn.Split('_')[0];

            // читаем файл со свечами D1, и скачиваем за каждую доступную дату тики
            using (var reader = new StreamReader(ffnD1))
            {
                // игнорируем первую строку
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var str = reader.ReadLine();

                    // пустая строка не предусмотрена
                    Assert.IsNotNull(str);

                    var strDate = str.Split('\t')[0];

                    var dtTick = strDate.Contains('.') || strDate.Contains('/')
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


                    // все тиковые данные будем хранить по годам
                    var strYear = $"{dtTick:yyyy}";
                    var yearDir = shareDir + strYear + "\\";
                    if (!Directory.Exists(yearDir))
                    {
                        Directory.CreateDirectory(yearDir);
                    }



                    // имя результирующего файла для тиковых данных за конкретную дату
                    var rezultFnTick = $"{fnShareName}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";

                    var urlTick = GetUrl(dtTick, dtTick, rezultFnTick, shareName, shareMarket, shareId, ETimeFrame.Tick, DataFormat.TickOptimal);
                    urlsWriter.WriteLine(urlTick);

                    var ffnTick = yearDir + rezultFnTick + ".txt";
                    var ffnTick7Z = yearDir + rezultFnTick + ".7z";

                    var isExistsTick = File.Exists(ffnTick);
                    var isExistsTick7Z = File.Exists(ffnTick7Z);

                    // если перезапись включена, то удаляем существующие файлы
                    if (fOverwrite)
                    {
                        if (isExistsTick)
                        {
                            File.Delete(ffnTick);
                            isExistsTick = false;
                        }

                        // а может архив не удалять?
                        if (isExistsTick7Z)
                        {
                            File.Delete(ffnTick7Z);
                            isExistsTick7Z = false;
                        }
                    }


                    if (fOverwrite || (!isExistsTick && !isExistsTick7Z))
                    {
                        while (!TryDownload(urlTick, ffnTick))
                        { }

                        Console.Write(rezultFnTick);
                    }
                }
                Console.Write('\n');
            }
        }
    }
}
