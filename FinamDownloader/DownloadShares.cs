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
        private static void GetPeriod(string shareDirD1, bool fNeedPeriod, out DateTime dtFrom, out DateTime dtTo)
        {
            var dtFinamMin = new DateTime(1979, 1, 1);
            var dtNow = DateTime.Now.AddHours(-2);

            dtFrom = dtFinamMin;
            dtTo = dtNow;


            var filesD1 = Directory.GetFiles(shareDirD1);
            foreach (var fD1 in filesD1)
            {
                var fn = Path.GetFileNameWithoutExtension(fD1); // SBER_790101_191101
                var strDtTo = fn.Split('_')[2];

                var y = Convert.ToInt32(strDtTo.Substring(0, 2));
                y += y > 70 ? 1900 : 2000;

                var m = Convert.ToInt32(strDtTo.Substring(2, 2));
                var d = Convert.ToInt32(strDtTo.Substring(4, 2));

                var dt = new DateTime(y, m, d);
                Assert.IsTrue(dtFinamMin < dt && dt <= dtNow);

                if (dtFrom < dt)
                {
                    dtFrom = dt;
                }
            }


            if (fNeedPeriod)
            {
                Console.Write("Enter dtFrom as 'dd/MM/yyyy': ");
                var bufDt1 = Console.ReadLine();
                if (bufDt1 != "")
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(bufDt1));
                    var dt1 = DateTime.Parse(bufDt1, CultureInfo.CurrentCulture);

                    Assert.IsTrue(dtFinamMin < dt1 && dt1 <= dtNow);
                    dtFrom = dt1;
                }


                Console.Write("Enter dtTo as 'dd/MM/yyyy': ");
                var bufDt2 = Console.ReadLine();
                if (bufDt2 != "")
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(bufDt2));
                    var dt2 = DateTime.Parse(bufDt2, CultureInfo.CurrentCulture);

                    Assert.IsTrue(dtFrom <= dt2 && dt2 <= dtNow);
                    dtTo = dt2;
                }
            }
        }

        /// <summary>
        /// Загрузка тиков по акциям
        /// </summary>
        /// <param name="issuers"></param>
        /// <param name="fNeedPeriod">запрашиваем период</param>
        /// <param name="fOverwrite">перезапись</param>
        private static void DownloadShares(List<FinamIssuer> issuers, bool fNeedPeriod = false,
            bool fOverwrite = false)
        {
            Console.Write(@"Enter 'Name,Market,Id': ");
            var shareName = Console.ReadLine();
            Assert.IsFalse(string.IsNullOrWhiteSpace(shareName));


            var market = "-1";
            var id = "-1";

            if (shareName.Contains(','))
            {
                var buf = shareName.Split(',');
                Assert.IsTrue(buf.Length == 3);

                shareName = buf[0];
                market = buf[1];
                id = buf[2];

                // Нельзя требовать, чтобы такой инструмент был в issuers в единственном экземпляре. это не работает!
                // с сайта финам уже давно загружается SBER,market=1,id=3, но в обновляемых icharts.js такого SBER нет
                // var tmpIssuers = issuers.FindAll(iss => iss.Name == shareName && iss.Id == id && iss.Market == market);
                // Assert.IsTrue(tmpIssuers != null && tmpIssuers.Count == 1);
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



            GetPeriod(shareDirD1, fNeedPeriod, out var dtBeg, out var dtEnd);


            // в файл urlsWriter будут записываться сформированные url
            var curDt = DateTime.Now.AddHours(-2);

            var logDir = shareDir + "logs\\";
            Directory.CreateDirectory(logDir);


            var fdUrlsLog = logDir + $"urls_{curDt:yyyy.MM.dd_HH;mm;ss}.txt";
            var urlsWriter = new StreamWriter(fdUrlsLog, false) { AutoFlush = true };



            if (market == "-1" && id == "-1")
            {
                // акций с одним и тем же именем может быть несколько (например, SBER)
                // в таком случае выбираем ту у которой url содержит @"moex-classica/"
                var shares = issuers.FindAll(s => s.Name == shareName);
                if (shares.Count == 0)
                {
                    Console.WriteLine("Акции с таким кодом не найдены");
                    return;
                }

                if (shares.Count > 1)
                {
                    const string moexClassica = @"moex-classica/";
                    var shares2 = shares.FindAll(s => s.Url.Contains(moexClassica));

                    if (shares2.Count == 1)
                    {
                        market = shares[0].Market;
                        id = shares[0].Id;
                    }
                    else
                    {
                        // если выбор по moexClassica не удался, то выбираем вручную
                        Console.WriteLine($"найдено акций с таким кодом: {shares.Count}");
                        for (var i = 0; i < shares.Count; i++)
                        {
                            var issuer = shares[i];
                            Console.WriteLine($"{i}\t{issuer.GetDescription()}");
                        }

                        Console.WriteLine($@"Введите порядковый номер акции которую скачать, либо иное - для выхода: ");
                        var fTry = int.TryParse(Console.ReadLine(), out var num);
                        if (fTry && 0 <= num && num < shares.Count)
                        {
                            market = shares[num].Market;
                            id = shares[num].Id;
                        }
                        else
                        {
                            Console.WriteLine($"порядковый номер акции не найден");
                            return;
                        }
                    }
                }
            }

            Assert.IsTrue(market != "-1" || id != "-1");

            // имя результирующего файла для дневных свечей. имя передается без расширения.
            var fnD1 = $"{shareName}_{dtBeg:yyMMdd}_{dtEnd:yyMMdd}";
            var fnD1Txt = fnD1 + ".txt";

            var ffnD1 = shareDirD1 + fnD1Txt;

            var url = GetUrl(dtBeg, dtEnd, fnD1, shareName, market, id, ETimeFrame.Day, DataFormat.CandleOptimal);
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


            DownloadTicksFromD1File(true, urlsWriter, shareDir, ffnD1,
                shareName, market, id, fOverwrite);


            urlsWriter.Close();
        }

        /// <summary>
        /// загрузка тиков по датам из файла
        /// </summary>
        /// <param name="isShares">Это акция? для фьючей тики не раскладываются по годам</param>
        /// <param name="urlsWriter">открытый поток куда сохраняем urls</param>
        /// <param name="saveDir">каталог, куда сохраняем тики</param>
        /// <param name="ffnD1">полный путь до файла с дневными свечами</param>
        /// <param name="issuerName">security</param>
        /// <param name="issuerMarket"></param>
        /// <param name="issuerId"></param>
        /// <param name="fOverwrite"></param>
        private static void DownloadTicksFromD1File(bool isShares, TextWriter urlsWriter, string saveDir,
            string ffnD1, string issuerName, string issuerMarket, string issuerId, bool fOverwrite = false)
        {
            Assert.IsTrue(File.Exists(ffnD1));

            // todo Date
            var curDt = DateTime.Now.AddHours(-2); // корректировка на мск

            // читаем файл со свечами D1, и скачиваем за каждую доступную дату тики
            using (var reader = new StreamReader(ffnD1))
            {
                // игнорируем первую строку
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var str = reader.ReadLine();

                    // пустая строка не предусмотрена
                    Assert.IsFalse(string.IsNullOrWhiteSpace(str));

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
                        continue; // or break;
                    }


                    // только для акций: все тиковые данные будем хранить по годам
                    if (isShares)
                    {
                        saveDir += $"{dtTick:yyyy}\\";
                        Directory.CreateDirectory(saveDir);
                    }
                    

                    // имя результирующего файла для тиковых данных за конкретную дату
                    var rezultFnTick = $"{issuerName}_{dtTick:yyMMdd}_{dtTick:yyMMdd}";

                    var urlTick = GetUrl(dtTick, dtTick, rezultFnTick, issuerName, issuerMarket, issuerId,
                        ETimeFrame.Tick, DataFormat.TickOptimal);
                    urlsWriter.WriteLine(urlTick);

                    var ffnTick = saveDir + rezultFnTick + ".txt";
                    var ffnTick7Z = saveDir + rezultFnTick + ".7z";

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

                        Console.WriteLine(rezultFnTick);
                    }
                }
                Console.WriteLine(string.Empty);
            }
        }




        /// <summary>
        /// Попытка найти не нулевые акции из shares с кодом shareName
        /// Вырезано из DownloadShares()
        /// </summary>
        /// <param name="issuers"></param>
        private void TryToFindNoEmptyIssuers(List<FinamIssuer> issuers)
        {
            var shareName = "SBER";
            var shares = issuers.FindAll(iss => iss.Name == shareName);


            var dtBeg = new DateTime(1979, 1, 1);
            var dtEnd = DateTime.Now.AddHours(-2);
            var shareDirD1 = "";
            var urlsWriter = new StreamWriter("");
            var shareDir = "";


            // 
            // запросим дневные свечи за период [dtBeg; dtEnd] для каждого shares[i]
            var nonEmptyShares = new List<int>();
            var nonEmptyFns = new List<string>();


            for (var i = 0; i < shares.Count; i++)
            {
                var issuer = shares[i];

                //имя результирующего файла для дневных свечей. имя передается без расширения
                var fnD1 = $"{shareName}_{dtBeg:yyMMdd}_{dtEnd:yyMMdd}";
                var fnD1Txt = fnD1 + ".txt";



                var ffnD1 = shareDirD1 + fnD1Txt;


                var url = GetUrl(dtBeg, dtEnd, fnD1, shareName, issuer.Market, issuer.Id, ETimeFrame.Day,
                    DataFormat.CandleOptimal);
                urlsWriter.WriteLine(url);

                while (!TryDownload(url, ffnD1))
                { }


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

                DownloadTicksFromD1File(true, urlsWriter, shareDir, fn,
                    shareName, issuer.Market, issuer.Id);
            }
        }
    }
}
