using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TrEngine.HistoryData;

namespace FinDownEntity {
    public static class FinDown {
        /// <summary>
        /// Определяет разделитель полей
        /// </summary>
        /// <param name="str">строка по которой определяется разделитель</param>
        /// <returns></returns>
        public static char GetSeparator(string str) {
            var fieldSeparators = new[] {'\t', ';', ' ', ',', '.',};
            var sep = fieldSeparators.FirstOrDefault(str.Contains);
            Assert.AreNotEqual(sep, '\0');

            // если в качестве разделителя определилась ',' или '.', то
            // дополнительно проверим, что найденный символ встречается большее количество раз
            if (sep == ',' || sep == '.') {
                var oth = sep == ',' ? '.' : ',';
                var qtySep = 0;
                var qtyOth = 0;

                foreach (var ch in str) {
                    if (ch == sep) {
                        qtySep++;
                    } else if (ch == oth) {
                        qtyOth++;
                    }
                }

                Assert.IsTrue(qtySep > qtyOth);
            }

            return sep;
        }


        /// <summary>
        /// проверка наличия финамовского заголовка файла
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckHeader(string str) {
            // Примеры заголовков:
            //"<DATE>	<TIME>	<LAST>	<VOL>	<ID>	<OPER>"
            //"<DATE>,<TIME>,<LAST>,<VOL>,<ID>,<OPER>"
            //"<DATE>;<TIME>;<LAST>"

            // параметры DATE, TIME (а также символы '<' и '>') есть для всех типов DataFormat,
            // так что наличие header можно определить по ним
            return str.Contains('<') && str.Contains('>') &&
                   str.Contains("DATE") && str.Contains("TIME");
        }


        /// <summary>
        /// Парсим заголовок для finam-файлов
        /// </summary>
        /// <param name="str">заголовок</param>
        /// <param name="sep">возвращаем разделитель полей</param>
        /// <param name="dataF">возвращаем FinDownEntity.DataFormat</param>
        public static void ParseHeader(string str, out char sep, out DataFormat dataF) {
            sep = GetSeparator(str);

            // требуем наличия заголовка
            Assert.IsTrue(CheckHeader(str));

            // все форматы данных в собственном форматировании
            var dataFormats = new[] {
                "TICKER,PER,DATE,TIME,OPEN,HIGH,LOW,CLOSE,VOL", // 1 - CandleAllParam
                "TICKER,PER,DATE,TIME,OPEN,HIGH,LOW,CLOSE",     // 2
                "TICKER,PER,DATE,TIME,CLOSE,VOL",               // 3
                "TICKER,PER,DATE,TIME,CLOSE",                   // 4
                "DATE,TIME,OPEN,HIGH,LOW,CLOSE,VOL",            // 5 - CandleOptimal
                "TICKER,PER,DATE,TIME,LAST,VOL",                // 6
                "TICKER,DATE,TIME,LAST,VOL",                    // 7
                "TICKER,DATE,TIME,LAST",                        // 8
                "DATE,TIME,LAST,VOL",                           // 9
                "DATE,TIME,LAST",                               // 10 - TickMinimal
                "DATE,TIME,LAST,VOL,ID",                        // 11
                "DATE,TIME,LAST,VOL,ID,OPER",                   // 12 - TickOptimal
            };

            // теперь надо определить какой это DataFormat
            var headline = str.Replace("<", "").Replace(">", "").Replace(sep, ',');
            var df = -1;

            for (var i = 0; i < dataFormats.Length; i++) {
                if (headline != dataFormats[i])
                    continue;

                df = i + 1;
                break;
            }

            Assert.IsTrue(1 <= df && df <= 12);
            dataF = (DataFormat) df;
        }

        /// <summary>
        /// попытка определить формат даты из строки даты
        /// </summary>
        /// <param name="sDate">строка даты</param>
        /// <returns></returns>
        public static bool TryGetDateFormat(string sDate, out DateFormat df) {
            // присваиваем любое значение
            df = DateFormat.YYYYMMDD;

            Assert.IsTrue(!string.IsNullOrWhiteSpace(sDate)
                          && (sDate.Length == 6 || sDate.Length == 8));

            if (sDate.Length == 6) {
                // при длине 6, считаем что форматы "ГГММДД" и "ДДММГГ" - программно не различимы
                Assert.IsTrue(false);
                return false;
            }

            if (sDate.Contains('/')) {
                // считаем что форматы "ДД/ММ/ГГ" и "ММ/ДД/ГГ" - программно не различимы
                Assert.IsTrue(false);
                return false;
            }

            df = DateFormat.YYYYMMDD;
            return true;
        }

        /// <summary>
        /// попытка определить формат времени из строки времени
        /// </summary>
        /// <param name="sTime">строка времени</param>
        /// <returns></returns>
        public static bool TryGetTimeFormat(string sTime, out TimeFormat tf) {
            tf = TimeFormat.HHMMSS;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sTime));
            var len = sTime.Length;

            Assert.IsTrue(len == 4 || len == 5 || len == 6 || len == 8);

            switch (len) {
                case 6:
                    tf = TimeFormat.HHMMSS;
                    break;
                case 4:
                    tf = TimeFormat.HHMM;
                    break;
                case 8:
                    tf = TimeFormat.HH_MM_SS;
                    break;
                default:
                    tf = TimeFormat.HH_MM;
                    break;
            }

            return true;
        }

        /// <summary>
        /// парсим строку даты с известным форматом в дату 
        /// </summary>
        /// <param name="sDate">строка даты</param>
        /// <param name="df">формат даты</param>
        /// <returns></returns>
        public static DateTime ParseDate(string sDate, DateFormat df) {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sDate)
                          && (sDate.Length == 6 || sDate.Length == 8));

            string sY;
            string sM;
            string sD;

            switch (df) {
                case DateFormat.YYYYMMDD:
                    sY = sDate.Substring(0, 4);
                    sM = sDate.Substring(4, 2);
                    sD = sDate.Substring(6, 2);
                    break;

                case DateFormat.YYMMDD:
                    sY = sDate.Substring(0, 2);
                    sM = sDate.Substring(2, 2);
                    sD = sDate.Substring(4, 2);
                    break;

                case DateFormat.DDMMYY:
                    sD = sDate.Substring(0, 2);
                    sM = sDate.Substring(2, 2);
                    sY = sDate.Substring(4, 2);
                    break;

                case DateFormat.DD_MM_YY:
                    var buf = sDate.Split('/');
                    sD = buf[0];
                    sM = buf[1];
                    sY = buf[2];
                    break;

                case DateFormat.MM_DD_YY:
                    buf = sDate.Split('/');
                    sM = buf[0];
                    sD = buf[1];
                    sY = buf[2];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(df), df, null);
            }

            var f1 = int.TryParse(sY, out var y);
            var f2 = int.TryParse(sM, out var m);
            var f3 = int.TryParse(sD, out var d);
            Assert.IsTrue(f1 && f2 && f3);

            if (y < 1900) {
                y += y < 70 ? 2000 : 1900;
            }

            

            return new DateTime(y, m, d);
        }

        /// <summary>
        /// парсим строку времени в TimeSpan от начала суток
        /// </summary>
        /// <param name="sTime">строка времени</param>
        /// <param name="tf">формат времени</param>
        /// <returns></returns>
        public static TimeSpan ParseTime(string sTime, TimeFormat tf) {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sTime) &&
                          (sTime.Length == 4 || sTime.Length == 5 || sTime.Length == 6 || sTime.Length == 8));

            string sH;
            string sM;
            string sS = "0";

            switch (tf) {
                case TimeFormat.HHMMSS:
                    sH = sTime.Substring(0, 2);
                    sM = sTime.Substring(2, 2);
                    sS = sTime.Substring(4, 2);
                    break;

                case TimeFormat.HHMM:
                    sH = sTime.Substring(0, 2);
                    sM = sTime.Substring(2, 2);
                    break;

                case TimeFormat.HH_MM_SS:
                    var buf = sTime.Split(':');
                    sH = buf[0];
                    sM = buf[1];
                    sS = buf[2];
                    break;

                case TimeFormat.HH_MM:
                    buf = sTime.Split(':');
                    sH = buf[0];
                    sM = buf[1];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(tf), tf, null);
            }

            var f1 = int.TryParse(sH, out var h);
            var f2 = int.TryParse(sM, out var m);
            var f3 = int.TryParse(sS, out var s);
            Assert.IsTrue(f1 && f2 && f3);

            return new TimeSpan(0, h, m, s);
        }

        /// <summary>
        /// возвращаем tms тика из строк sDate и sTime и известных форматах df и tf
        /// </summary>
        /// <param name="sDate">строка даты</param>
        /// <param name="df">формат даты</param>
        /// <param name="sTime">строка времени</param>
        /// <param name="tf">формат времени</param>
        /// <returns></returns>
        public static DateTime GetTms(string sDate, DateFormat df, string sTime, TimeFormat tf) {
            return ParseDate(sDate, df) + ParseTime(sTime, tf);
        }

        //public ParseStringToTick(string In,
        //out DateTime tms, out decimal price, out int volume, out Direction direction)
        //{
        //    //20170307,095931,162.410000000,10,Sell,0,
        //    var sIn = In.Split(',');

        //    var year = Convert.ToInt32(sIn[0].Substring(0, 4));
        //    var month = Convert.ToInt32(sIn[0].Substring(4, 2));
        //    var day = Convert.ToInt32(sIn[0].Substring(6, 2));

        //    var hour = Convert.ToInt32(sIn[1].Substring(0, 2));
        //    var minute = Convert.ToInt32(sIn[1].Substring(2, 2));
        //    var second = Convert.ToInt32(sIn[1].Substring(4, 2));

        //    Tms = new DateTime(year, month, day, hour, minute, second);

        //    Price = Convert.ToDecimal(sIn[2].Replace(".", ","));
        //    Volume = Convert.ToInt32(sIn[3]);

        //    Direction = sIn[4] == "Sell" ? Direction.Sell : Direction.Buy;
        //}

        public static bool ParseStringToTick(string str, char sep, DataFormat dataF, DateFormat df, TimeFormat tf,
            out DateTime tms, out decimal price, out int volume, out long id, out Direction direction)
        {
            int dfNum = (int) dataF;
            Assert.IsTrue(6 <= dfNum && dfNum <= 12);

            string[] buf = str.Split(sep);
            Assert.IsTrue(dfNum == 6  && buf.Length == 6 || // 6  TICKER, PER, DATE, TIME, LAST, VOL              - 6 
                          dfNum == 7  && buf.Length == 5 || // 7  TICKER,      DATE, TIME, LAST, VOL              - 5 
                          dfNum == 8  && buf.Length == 4 || // 8  TICKER,      DATE, TIME, LAST                   - 4 
                          dfNum == 9  && buf.Length == 4 || // 9               DATE, TIME, LAST, VOL              - 4 
                          dfNum == 10 && buf.Length == 3 || // 10              DATE, TIME, LAST                   - 3 
                          dfNum == 11 && buf.Length == 5 || // 11              DATE, TIME, LAST, VOL, ID          - 5 
                          dfNum == 12 && buf.Length == 6);  // 12              DATE, TIME, LAST, VOL, ID, OPER    - 6

            int shift = 0;
            if (6 <= dfNum && dfNum <= 8) {
                string ticker = buf[0];
                shift++;

                if (dfNum == 6) {
                    string per = buf[1];
                    shift++;
                }
            }

            string sDate = buf[0 + shift];
            string sTime = buf[1 + shift];
            tms = GetTms(sDate, df, sTime, tf);

            string sPrice = buf[2 + shift];
            price = Convert.ToDecimal(sPrice.Replace(".", ","));

            string sVol = dfNum == 8 || dfNum == 10 ? "-1" : buf[3 + shift];
            volume = Convert.ToInt32(sVol);

            string sId = dfNum <= 10 ? "-1" : buf[4 + shift];
            id = Convert.ToInt64(sId);

            direction = dfNum != 12 ? Direction.None : /*EngineConvert.*/ConvertToDirection(buf[5 + shift]);
            return true;
        }

        private static Direction ConvertToDirection(string str) {
            Assert.IsTrue(str == "" || str == "B" || str == "S" || str == "Buy" || str == "Sell");

            if (str == "B" || str == "Buy") {
                return Direction.Buy;
            }

            if (str == "S" || str == "Sell") {
                return Direction.Sell;
            }

            return Direction.None;
        }


        /// <summary>
        /// Формируем url, понимаемый финамом, для передачи его загрузчику
        /// </summary>
        /// <param name="dtF">Начальная дата</param>
        /// <param name="dtT">Конечная дата</param>
        /// <param name="rezultFn">Имя сформированного файла</param>
        /// <param name="code">issuer.Name</param>
        /// <param name="market">issuer.Market</param>
        /// <param name="id">issuer.Id</param>
        /// <param name="tf">Таймфрейм</param>
        /// <param name="datf">Формат записи в файл. По умолчанию: DataFormat.CandleOptimal</param>
        /// <param name="dtf">формат даты. По умолчанию: DateFormat.DD_MM_YY</param>
        /// <param name="tmf">формат времени. По умолчанию: TimeFormat.HH_MM_SS</param>
        /// <param name="ct">Время свечи (0 - open; 1 - close). По умолчанию: CandleTime.Open</param>
        /// <param name="fs">Разделитель полей. По умолчанию: FieldSeparator.Tab</param>
        /// <param name="bs">Разделитель разрядов. По умолчанию: BitSeparator.None</param>
        /// <param name="at">Нужны ли заголовки столбцов (0 - нет, 1 - да). По умолчанию: ColumnHeaderNeed.Yes</param>
        /// <returns></returns>
        public static string GetUrl(DateTime dtF, DateTime dtT, string rezultFn,
            string code, string market, string id,
            ETimeFrame tf,
            DataFormat datf = DataFormat.Candle_DT_OHLC_V,
            DateFormat dtf = DateFormat.DD_MM_YY,
            TimeFormat tmf = TimeFormat.HH_MM_SS,
            CandleTime ct = CandleTime.Open,
            FieldSeparator fs = FieldSeparator.Tab,
            BitSeparator bs = BitSeparator.None,
            ColumnHeaderNeed at = ColumnHeaderNeed.Yes)
        {
            // для запроса тиковых данных требуем формат данных TickOptimal
            Assert.IsTrue(tf == ETimeFrame.Tick && datf == DataFormat.TickOptimal ||
                          tf != ETimeFrame.Tick && datf != DataFormat.TickOptimal);


            // генерируем url
            var url = "http://export.finam.ru/" +
                      $"{rezultFn}.txt?" +
                      $"market={market}&" +
                      $"em={id}&" +
                      $"code={code}&" +
                      $"apply=0&" + // ?

                      $"df={dtF.Day}&" +
                      $"mf={dtF.Month - 1}&" + // номер месяца (0-11)
                      $"yf={dtF.Year}&" +
                      $"from={dtF:dd.MM.yyyy}&" + // Начальная дата в формате "ДД.ММ.ГГГГ" (здесь месяцы от 1 до 12)

                      $"dt={dtT.Day}&" +
                      $"mt={dtT.Month - 1}&" + // номер месяца (0-11)
                      $"yt={dtT.Year}&" +
                      $"to={dtT:dd.MM.yyyy}&" + // Конечная дата в формате "ДД.ММ.ГГГГ" (здесь месяцы от 1 до 12)

                      $"p={(int)tf}&" +
                      $"f={rezultFn}&" +
                      $"e=.txt&" + // Расширение сформированного файла: ".txt" или ".csv"
                      $"cn={code}&" +

                      $"dtf={(int)dtf}&" +
                      $"tmf={(int)tmf}&" +
                      $"MSOR={(int)ct}&" +

                      $"mstime=on&" + // Московское время	( "on", "off")
                      $"mstimever=1&" + // Коррекция часового пояса
                      $"sep={(int)fs}&" +
                      $"sep2={(int)bs}&" +
                      $"datf={(int)datf}&" +
                      $"at={(int)at}";

            return url;
        }

        public static ETimeFrame GetETimeFrameFromPer(string per) {
            List<string> pers = new[] {
                "0",  // tick
                "1",  // M1
                "5",  // M5
                "10", // M10
                "15", // M15
                "30", // M30
                "60", // H1
                "D",  // day
                "W",  // week
                "M",  // month
            }.ToList();

            Assert.IsTrue(pers.Contains(per));


            int idx = -1;
            for (int i = 0; i < pers.Count; i++) {
                if (pers[i] == per) {
                    idx = i;
                    break;
                }
            }
            Assert.IsTrue(idx >= 0);

            return (ETimeFrame) (idx + 1);

        }
    }
}
