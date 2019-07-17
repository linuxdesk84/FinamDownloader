using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace FinamDownloader {
    class Program {
        static void Main(string[] args) {

            
            var dtBeg = new DateTime(2008, 03, 05);

            var writer = new StreamWriter("urls7.2.txt", false);
            using (var reader = new StreamReader("sber_passed7.txt")) {
                while (!reader.EndOfStream) {
                    var date = reader.ReadLine();
                    if (date == null || date.Length != 8) {
                        throw new Exception("");
                    }

                    var dateShort = date.Substring(2, 6);


                    var strYear = date.Substring(0, 4);
                    var strMonth = date.Substring(4, 2);
                    var strDay = date.Substring(6, 2);

                    var year = Convert.ToInt32(strYear);
                    var month = Convert.ToInt32(strMonth);
                    var day = Convert.ToInt32(strDay);

                    var dtCur = new DateTime(year, month, day);
                    if (new DateTime(year, month, day) < dtBeg) {
                        continue;
                    }




                    var rezultFilename = string.Format("SBER_{0}_{0}", dateShort);

                    var url = string.Format("http://export.finam.ru/" +
                                            "{0}.txt?market=1&em=3&code=SBER&apply=0&" +
                                            "df={1}&mf={2}&yf={3}&from={4}.{5}.{6}&" +
                                            "dt={1}&mt={2}&yt={3}&to={4}.{5}.{6}&" +
                                            $"p={TimeFrame.Tick}&" + // Таймфрейм (для TICK = 1; M1=2; M5=3; M10=4; M15=5; M30=6; H1=7; D1=8; W1=9; M1=10)
                                            "f ={0}&" + // Имя сформированного файла GBPUSD_141201_141206
                                            "e=.txt&" + // Расширение сформированного файла: ".txt" или ".csv"
                                            "cn=SBER&" + // Имя контракта
                                            $"dtf={DateFormat.YYYYMMDD}&" + // Номер формата дат
                                            $"tmf={TimeFormat.HHMMSS}&" + // Номер формата времени
                                            $"MSOR={CandleTime.Open}&" + // Время свечи (0 - open; 1 - close)
                                            "mstime=on&" + // Московское время	on
                                            "mstimever=1&" + // Коррекция часового пояса
                                            $"sep={FieldSeparator.Tab}&" + // Разделитель полей (запятая - 1, точка - 2, точка с запятой - 3, табуляция - 4, пробел - 5)
                                            $"sep2={BitSeparator.None}&" + // Разделитель разрядов
                                            "datf=12&" + // Формат записи в файл
                                            "at=1", // Нужны ли заголовки столбцов
                        rezultFilename, day, month - 1, year, strDay, strMonth, strYear);

                    Console.WriteLine(rezultFilename);
                    writer.WriteLine(url);

                    var saveDir = @"c:\Users\admin\Documents\HistoryData\SBER\";
                    // wc.DownloadFile(url, saveDir + rezultFilename + ".txt");

                    while (!TryDownload(url, saveDir + rezultFilename + ".txt")) { }
                }
            }

            writer.Close();
        }

        public static bool TryDownload(string url, string fn)
        {
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

        /// <summary>
        /// Таймфрейм (для TICK = 1; M1=2; M5=3; M10=4; M15=5; M30=6; H1=7; D1=8; W1=9; M1=10)
        /// </summary>
        enum TimeFrame
        {
            /// <summary>
            /// Тики
            /// </summary>
            Tick = 1,
            /// <summary>
            /// 1 Минутный
            /// </summary>
            M1 = 2,
            /// <summary>
            /// 5 минутный
            /// </summary>
            M5 = 3,
            /// <summary>
            /// 10 минутный
            /// </summary>
            M10 = 4,
            /// <summary>
            /// 15 минутный
            /// </summary>
            M15 = 5,
            /// <summary>
            /// 30 минутный
            /// </summary>
            M30 = 6,
            /// <summary>
            /// Часовой
            /// </summary>
            H1 = 7,
            /// <summary>
            /// Дневной
            /// </summary>
            Day = 8,
            /// <summary>
            /// Недельный
            /// </summary>
            Week = 9,
            /// <summary>
            /// Месяц
            /// </summary>
            Month = 10
        }

        /// <summary>
        /// Разделитель полей (запятая - 1, точка - 2, точка с запятой - 3, табуляция - 4, пробел - 5)
        /// </summary>
        enum FieldSeparator
        {
            /// <summary>
            /// запятая
            /// </summary>
            Comma = 1,
            /// <summary>
            /// точка
            /// </summary>
            Dot = 2,
            /// <summary>
            /// точка с запятой
            /// </summary>
            Semicolon = 3,
            /// <summary>
            /// табуляция
            /// </summary>
            Tab = 4,
            /// <summary>
            /// пробел
            /// </summary>
            Space = 5
        }

        /// <summary>
        /// Разделитель разрядов
        /// </summary>
        enum BitSeparator
        {
            /// <summary>
            /// нет
            /// </summary>
            None = 1,
            /// <summary>
            /// точка (.)
            /// </summary>
            Dot = 2,
            /// <summary>
            /// запятая (,)
            /// </summary>
            Comma = 3,
            /// <summary>
            /// пробел ( )
            /// </summary>
            Space = 4,
            /// <summary>
            /// кавычка (')
            /// </summary>
            Quote = 5


        }

        /// <summary>
        /// Формат записи в файл (datf лучше всего задавать равным 11, для всех остальных таймфреймов – 5.)
        /// </summary>
        enum Template
        {
            /// <summary>
            /// TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
            /// </summary>
            CandleAllParam = 1,
            /// <summary>
            /// TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE
            /// </summary>
            CandleAllParamNoVol = 2,
            /// <summary>
            /// TICKER, PER, DATE, TIME, CLOSE, VOL
            /// </summary>
            CandleOnlyClose = 3,
            /// <summary>
            /// TICKER, PER, DATE, TIME, CLOSE
            /// </summary>
            CandleOnlyCloseNoVol = 4,
            /// <summary>
            /// DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
            /// </summary>
            CandleOptimal = 5,
            /// <summary>
            /// DATE, TIME, LAST, VOL, ID, OPER
            /// </summary>
            TickOptimal = 6
        }

        /// <summary>
        /// Время свечи (0 - open; 1 - close)
        /// </summary>
        enum CandleTime{
            /// <summary>
            /// Время свечи = времени ее начала
            /// </summary>
            Open,
            /// <summary>
            /// Время свечи = времени ее завершения
            /// </summary>
            Close
        }


        /// <summary>
        /// Формат даты
        /// </summary>
        enum DateFormat
        {
            /// <summary>
            /// ггггммдд
            /// </summary>
            YYYYMMDD = 1,
            /// <summary>
            /// ггммдд
            /// </summary>
            YYMMDD = 2,
            /// <summary>
            /// ддммгг
            /// </summary>
            DDMMYY = 3,
            /// <summary>
            /// дд/мм/гг
            /// </summary>
            DD_MM_YY = 4,
            /// <summary>
            /// мм/дд/гг
            /// </summary>
            MM_DD_YY = 5
        }

        /// <summary>
        /// Формат времени
        /// </summary>
        enum TimeFormat
        {
            /// <summary>
            /// ччммсс
            /// </summary>
            HHMMSS = 1,
            /// <summary>
            /// ччмм
            /// </summary>
            HHMM = 2,
            /// <summary>
            /// чч:мм:сс
            /// </summary>
            HH_MM_SS = 3,
            /// <summary>
            /// чч:мм
            /// </summary>
            HH_MM = 4
        }
    }
}


/*
 * Имя параметра	Назначение	Пример
market	Номер рынка	5
em	Номер инструмента	86
code	Тикер инструмента	GBPUSD
df	Начальная дата, номер дня (1-31)	1
mf	Начальная дата, номер месяца (0-11)	11
yf	Начальная дата, год	2014
from	Начальная дата	01.12.2014
dt	Конечная дата, номер дня	6
mt	Конечная дата, номер месяца	11
yt	Конечная дата, год	2014
to	Конечная дата	06.12.2014
 */

 /*
  * Месяцы нумеруются, начиная с 0; дни – с 1.
  * Если в форме поставить галочку Московское время, то в запросе появится параметр mstime=on,
  * если не поставить – данный параметр будет отсутствовать.
  * Аналогично для параметра at – он появится в строке запроса, только если поставить галочку Добавить заголовок файла.
  * Параметр p для тиковых данных должен быть равен 1; для минуток – 2; для 5-минуток – 3; 10-минуток – 4; ... ;
  * для часового таймфрейма – 7; дневного – 8; недельного – 9; месячного – 10.
  * Для тиковых данных параметр datf лучше всего задавать равным 11, для всех остальных таймфреймов – 5.
  * Параметр market (рынок) для мировых индексов равен 6, для мировых валют – 5, для фьючерсов США – 7,
  * для фьючерсов ФОРТС – 14 (но эксперименты показали, что данный параметр может принимать любое значение, полученный результат от этого не изменяется).
  */
