﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
            const string dir = @"d:\SyncDirs\main\pdata\visualstudio\FinamDownloader\icharts\";
            const string fn = "icharts.js";


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
                    if (File.Exists(dir + fn))
                    {
                        File.Move(dir + fn, dir + $"{DateTime.Now:yyyy.MM.dd_HH;mm;ss}_{fn}");
                    }
                    
                    DownloadIcharts(dir + fn);
                    Console.WriteLine(@"Файл icharts.js загружен");
                }

            } while (ch != "1" && ch != "2" &&
                     ch != "3" && ch != "4" &&
                     ch != "5" && ch != "6" && 
                     ch != "0");



            var icharts = new Icharts(dir + fn);
            var issuers = icharts.Issuers;

            switch (Convert.ToInt32(ch))
            {
                case 1:
                    FindIssuers(issuers);
                    break;

                case 2:
                    FindIssuers(issuers, true, false);
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

        private static void DownloadIcharts(string fn)
        {
            const string ichartsUrl = @"https://www.finam.ru/cache/icharts/icharts.js";
            while (!TryDownload(ichartsUrl, fn))
            {
            }
        }

        // требуем равенство
        private static void FindIssuers(List<FinamIssuer> issuers, bool equalityRequired = false, bool fullDescr = true)
        {
            Console.Write(@"enter name of issuer: ");
            var name = Console.ReadLine();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name));

            var issuersList = issuers.FindAll(issuer =>
                equalityRequired ? name == issuer.Name : issuer.Name.Contains(name));
            Console.WriteLine($@"issuersList.Count  = {issuersList.Count}");

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
                Thread.Sleep(2000);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                res = false;
            }

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
