using System.Linq;
using NUnit.Framework;

namespace FinDownEntity
{
    public class FinDown
    {
        /// <summary>
        /// Определяет разделитель полей
        /// </summary>
        /// <param name="str">строка по которой определяется разделитель</param>
        /// <returns></returns>
        public char GetSeparator(string str)
        {
            var fieldSeparators = new[] { '\t', ';', ' ', ',', '.', };
            var sep = fieldSeparators.FirstOrDefault(str.Contains);
            Assert.AreNotEqual(sep, '\0');

            // если в качестве разделителя определилась ',' или '.', то
            // дополнительно проверим, что найденный символ встречается большее количество раз
            if (sep == ',' || sep == '.')
            {
                var oth = sep == ',' ? '.' : ',';
                var qtySep = 0;
                var qtyOth = 0;

                foreach (var ch in str)
                {
                    if (ch == sep)
                    {
                        qtySep++;
                    }
                    else if (ch == oth)
                    {
                        qtyOth++;
                    }
                }

                Assert.IsTrue(qtySep > qtyOth);
            }

            return sep;
        }


        /// <summary>
        /// Парсим заголовок для finam-файлов
        /// </summary>
        /// <param name="str">заголовок</param>
        /// <param name="sep">возвращаем разделитель полей</param>
        /// <param name="df">возвращаем FinDownEntity.DataFormat</param>
        public void ParseHeader(string str, out char sep, out int df)
        {
            sep = GetSeparator(str);

            // Примеры заголовков:
            //"<DATE>	<TIME>	<LAST>	<VOL>	<ID>	<OPER>"
            //"<DATE>,<TIME>,<LAST>,<VOL>,<ID>,<OPER>"
            //"<DATE>;<TIME>;<LAST>"

            // параметры DATE, TIME (а также символы '<' и '>') есть для всех типов DataFormat,
            // так что наличие header можно определить по ним


            // требуем наличия заголовка
            Assert.IsTrue(str.Contains('<') && str.Contains('>') &&
                          str.Contains("DATE") && str.Contains("TIME"));


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
            df = -1;

            for (var i = 0; i < dataFormats.Length; i++)
            {
                if (headline != dataFormats[i])
                    continue;

                df = i + 1;
                break;
            }
            Assert.IsTrue(1 <= df && df <= 12);
        }
    }
}
