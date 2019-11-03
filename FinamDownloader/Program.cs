﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Engine;
using NUnit.Framework;
using FinDownEntity;


namespace FinamDownloader
{
    internal partial class Program
    {
        const string HistoryDataDir = @"c:\Users\admin\Documents\SyncDocs\HistoryData\";

        private static void Main()
        {
            // The request was aborted: Could not create SSL/TLS secure channel.
            // Запрос был прерван: Не удалось создать защищенный канал SSL/TLS.
            // http://qaru.site/questions/45913/the-request-was-aborted-could-not-create-ssltls-secure-channel

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            const string ffnIcharts = @"d:\SyncDirs\main\pdata\visualstudio\FinamDownloader\icharts\icharts.js";


            var ch = "";
            do
            {
                Console.WriteLine(@"1 - поиск инструмента (содержит подстроку)");
                Console.WriteLine(@"2 - поиск инструмента (равно строке)");

                Console.WriteLine(@"3 - загрузка / обновление фьючерса без перезаписи (базовое имя)");
                Console.WriteLine(@"4 - загрузка фьючерса за период с перезаписью (базовое имя)");

                Console.WriteLine(@"5 - загрузка / обновление акций без перезаписи");
                Console.WriteLine(@"6 - загрузка акций за период с перезаписью");

                Console.WriteLine(@"9 - загрузка icharts.js");
                Console.WriteLine(@"0 - выход");

                ch = Console.ReadLine();

                if (ch == "9")
                {
                    DownloadIcharts(ffnIcharts);
                }

            } while (ch != "1" && ch != "2" &&
                     ch != "3" && ch != "4" &&
                     ch != "5" && ch != "6" && 
                     ch != "0");



            var icharts = new Icharts(ffnIcharts);
            var issuers = icharts.Issuers;

            switch (Convert.ToInt32(ch))
            {
                case 1:
                    FindIssuers(issuers);
                    break;

                case 2:
                    FindIssuers(issuers, true);
                    break;

                case 3:
                    DownloadFutures(issuers, false);
                    break;

                case 4:
                    DownloadFutures(issuers, true, true);
                    break;

                case 5:
                    DownloadShares(issuers);
                    break;

                case 6:
                    DownloadShares(issuers, true, true);
                    break;

                case 0:
                    break;

                default:
                    break;
            }

            if (ch == "0")
               return;

            Console.WriteLine(@"Complete");
            Console.ReadKey();
        }


        private static string MyGetResponse(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192))
            {
                return reader.ReadToEnd();
            }
        }

        private static bool GetIchartsUrl(out string ichartsUrl)
        {
            ichartsUrl = "";

            const string finamUrl = @"https://www.finam.ru/";
            const string sberPage = finamUrl + @"profile/moex-akcii/sberbank/export/";

            var sberPageResponse = MyGetResponse(sberPage);

            var strings = sberPageResponse.Split('\n');

            foreach (var str in strings)
            {
                if (!str.Contains("icharts.js"))
                    continue;

                var buf = str.Split('"');
                foreach (var subStr in buf)
                {
                    if (subStr.Contains("icharts.js"))
                    {
                        ichartsUrl = finamUrl + subStr;
                        return true;
                    }
                }
            }
            return false;
        }


        private static void DownloadIcharts(string ffnIcharts)
        {
            var resGet = GetIchartsUrl(out var ichartsUrl);
            if (!resGet)
            {
                Console.WriteLine("error getting ichartsUrl");
                return;
            }

            var ichartsDir = Path.GetDirectoryName(ffnIcharts) + "\\";
            var fn = Path.GetFileNameWithoutExtension(ffnIcharts);

            var ichartsTmpFile = $@"a:\{fn}_{DateTime.Now.Ticks}"; // ichartsDir + $"{DateTime.Now.Ticks}";

            // количество попыток скачать
            var numOfAttempts = 5;
            resGet = false;

            while (numOfAttempts > 0)
            {
                if (TryDownload(ichartsUrl, ichartsTmpFile))
                {
                    resGet = true;
                    break;
                }

                numOfAttempts--;
            }

            if (!resGet)
            {
                Console.WriteLine("error downloading icharts");
                return;
            }


            if (File.Exists(ffnIcharts))
            {
                // если файлы одинаковые, то перезаписывать не надо
                if (new FileInfo(ffnIcharts).Length == new FileInfo(ichartsTmpFile).Length &&
                    File.ReadAllBytes(ffnIcharts).SequenceEqual(File.ReadAllBytes(ichartsTmpFile)))
                {
                    Console.WriteLine("downloading and existing files are equals");
                    File.Delete(ichartsTmpFile);
                    return;
                }

                // backup existing icharts.js
                var backupName = ichartsDir + $"{fn}_{DateTime.Now:yyyy.MM.dd_HH;mm;ss}" +
                                 Path.GetExtension(ffnIcharts);
                File.Move(ffnIcharts, backupName);
            }


            File.Move(ichartsTmpFile, ffnIcharts);
            Console.WriteLine(@"icharts.js is downloading");
        }


        /// <summary>
        /// поиск эмитента
        /// </summary>
        /// <param name="issuers">имя эмитента</param>
        /// <param name="equalityRequired">требование полного совпадения строки поиска и имени эмитента</param>
        private static void FindIssuers(List<FinamIssuer> issuers, bool equalityRequired = false)
        {
            Console.Write(@"enter name of issuer: ");
            var name = Console.ReadLine();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name));

            var issuersList = issuers.FindAll(issuer =>
                equalityRequired ? name == issuer.Name : issuer.Name.Contains(name));
            Console.WriteLine($@"issuersList.Count  = {issuersList.Count}");

            // если найдено не более 5 эмитентов, то молча печатаем с полным описанием
            const int max = 5;
            var fullDescr = true;

            // если найдено более 5 эмитентов, то спрашиваем надо ли печатать
            if (issuersList.Count > max)
            {
                var bCh = "";
                do
                {
                    Console.WriteLine(@"do you want to print all of issuersList names?");
                    Console.WriteLine(@"1 - OK (min description), 2 - OK (full description), 0 - No");
                    bCh = Console.ReadLine();
                } while (bCh != "1" && bCh != "2" && bCh != "0");

                if (bCh == "0")
                    return;

                // нужно ли полное описание?
                fullDescr = bCh == "2";
            }

            
            foreach (var issuer in issuersList)
            {
                Console.WriteLine(issuer.GetDescription(fullDescr));
            }
        }


        /// <summary>
        /// Попытка выполнить загрузку (true - успешно, false - ошибка)
        /// </summary>
        /// <param name="url">сгенерированный url</param>
        /// <param name="fn">имя результирующего файла</param>
        /// <returns></returns>
        public static bool TryDownload(string url, string fn)
        {
            

            // результат выполнения загрузки
            var res = true;

            var wc = new WebClient();
            try
            {
                wc.DownloadFile(url, fn);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                res = false;
            }
            Thread.Sleep(1000);

            return res;
        }


        //issuer.Market, issuer.Id
        private static string GetUrl(DateTime dtF, DateTime dtT, string rezultFn, string code, string market, string id, ETimeFrame tf,
            DataFormat datf = DataFormat.CandleAllParam,
            DateFormat dtf = DateFormat.DD_MM_YY,
            TimeFormat tmf = TimeFormat.HH_MM_SS,
            CandleTime ct = CandleTime.Open,
            FieldSeparator fs = FieldSeparator.Tab,
            BitSeparator bs = BitSeparator.None,
            ColumnHeaderNeed at = ColumnHeaderNeed.Yes)
        {
            Assert.IsTrue(tf == ETimeFrame.Tick && datf == DataFormat.TickOptimal ||
                          tf != ETimeFrame.Tick && datf != DataFormat.TickOptimal);


            // генерируем url
            var url = "http://export.finam.ru/" +
                      $"{rezultFn}.txt?" +
                      $"market={market}&" + // Номер рынка
                      $"em={id}&" + // Номер инструмента
                      $"code={code}&" + // Тикер инструмента
                      $"apply=0&" + // ?

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