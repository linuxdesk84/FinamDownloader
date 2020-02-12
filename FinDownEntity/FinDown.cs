using System;
using System.Linq;
using NUnit.Framework;

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
        public static DateFormat TryGetDateFormat(string sDate) {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sDate)
                          && (sDate.Length == 6 || sDate.Length == 8));

            if (sDate.Length == 6) {
                // при длине 6, считаем что форматы "ГГММДД" и "ДДММГГ" не различимы
                Assert.IsTrue(false);
            }

            if (sDate.Contains('/')) {
                // считаем что формат "ДД/ММ/ГГ" более вероятен, чем  "ММ/ДД/ГГ"
                return DateFormat.DD_MM_YY;
            }

            return DateFormat.YYYYMMDD;
        }

        /// <summary>
        /// попытка определить формат времени из строки времени
        /// </summary>
        /// <param name="sTime">строка времени</param>
        /// <returns></returns>
        public static TimeFormat TryGetTimeFormat(string sTime) {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(sTime));
            var len = sTime.Length;

            Assert.IsTrue(len == 4 || len == 5 || len == 6 || len == 8);

            TimeFormat tf;
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

            return tf;
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

            y += y < 70 ? 2000 : 1900;

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
    }
}