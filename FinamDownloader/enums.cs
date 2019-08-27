// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local


namespace FinamDownloader
{
    internal partial class Program
    {
        /// <summary>
        /// Таймфрейм (Tick, M1, M5, M10, M15, M30, H1, Day, Week, Month)
        /// </summary>
        private enum TimeFrame
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
        /// Разделитель полей (Comma, Dot, Semicolon, Tab, Space)
        /// </summary>
        private enum FieldSeparator
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
        /// Разделитель разрядов (None, Dot, Comma, Space, Quote)
        /// </summary>
        private enum BitSeparator
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
        /// Формат записи в файл (CandleAllParam, CandleOptimal, TickOptimal).
        /// </summary>
        private enum DataFormat
        {
            /// <summary>
            /// TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
            /// </summary>
            CandleAllParam = 1,
            /// <summary>
            /// DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
            /// </summary>
            CandleOptimal = 5,
            /// <summary>
            /// DATE, TIME, LAST, VOL, ID, OPER
            /// </summary>
            TickOptimal = 12,

            /* При парсе icharts.js находится только 11 форматов представления данных. 12 там нет.
               В то же время при запросе тиковых данных через сайт url получается с datf=12.
               2019.08.24 экспериментально выяснил, что параметр datf может быть от 1 до 12, все остальные
               варианты (перебрал от 0 до 18) аналогичны datf=10 (но при этом отсутствует заголовок).
               Оставил только 3 формата, потому что использовать другие нет смысла.
               
               Полная картина:
               1  TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL -- CandleAllParam
               2  TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE
               3  TICKER, PER, DATE, TIME, CLOSE, VOL
               4  TICKER, PER, DATE, TIME, CLOSE
               5  DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL              -- CandleOptimal
               6  TICKER, PER, DATE, TIME, LAST, VOL
               7  TICKER, DATE, TIME, LAST, VOL
               8  TICKER, DATE, TIME, LAST
               9  DATE, TIME, LAST, VOL
               10 DATE, TIME, LAST
               11 DATE, TIME, LAST, VOL, ID
               12 DATE, TIME, LAST, VOL, ID, OPER                      -- TickOptimal
             */
        }

        /// <summary>
        /// Время свечи (Open - время начала свечи, Close - время завершения свечи)
        /// </summary>
        private enum CandleTime
        {
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
        /// Формат даты (YYYYMMDD, YYMMDD, DDMMYY, DD_MM_YY, MM_DD_YY)
        /// </summary>
        private enum DateFormat
        {
            /// <summary>
            /// ГГГГММДД
            /// </summary>
            YYYYMMDD = 1,
            /// <summary>
            /// ГГММДД
            /// </summary>
            YYMMDD = 2,
            /// <summary>
            /// ДДММГГ
            /// </summary>
            DDMMYY = 3,
            /// <summary>
            /// ДД/ММ/ГГ
            /// </summary>
            DD_MM_YY = 4,
            /// <summary>
            /// ММ/ДД/ГГ
            /// </summary>
            MM_DD_YY = 5,
        }

        /// <summary>
        /// Формат времени (HHMMSS, HHMM, HH_MM_SS, HH_MM)
        /// </summary>
        private enum TimeFormat
        {
            /// <summary>
            /// ЧЧММСС
            /// </summary>
            HHMMSS = 1,
            /// <summary>
            /// ЧЧММ
            /// </summary>
            HHMM = 2,
            /// <summary>
            /// ЧЧ:ММ:СС
            /// </summary>
            HH_MM_SS = 3,
            /// <summary>
            /// ЧЧ:ММ
            /// </summary>
            HH_MM = 4,
        }

        /// <summary>
        /// Нужны ли заголовки столбцов ?
        /// </summary>
        private enum ColumnHeaderNeed {
            No = 0,
            Yes = 1,
        }
    }
}
