using System.Diagnostics.CodeAnalysis;

namespace FinDownEntity
{
    /// <summary>
    /// Разделитель полей (Comma, Dot, Semicolon, Tab, Space)
    /// </summary>
    public enum FieldSeparator
    {
        /// <summary>
        /// запятая.
        /// Лучше не использовать, т.к. является разделителем дробной части в русской локали
        /// </summary>
        Comma = 1,

        /// <summary>
        /// точка.
        /// Лучше не использовать, т.к. является разделителем дробной части в английской локали
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
    public enum BitSeparator
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
    public enum DataFormat
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
        /// DATE, TIME, LAST
        /// </summary>
        TickMinimal = 10,

        /// <summary>
        /// DATE, TIME, LAST, VOL, ID, OPER
        /// </summary>
        TickOptimal = 12,

        /* При парсе icharts.js находится только 11 форматов представления данных. 12 там нет.
           В то же время, при запросе тиковых данных через сайт, сгенерированный url содержит "datf=12".
           2019.08.24 экспериментально выяснил, что параметр datf может быть от 1 до 12, все остальные
           варианты (перебрал от 0 до 18) аналогичны datf=10 (но при этом отсутствует заголовок).
           Оставил только 4 формата, потому что использовать другие нет смысла.
           
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
           10 DATE, TIME, LAST                                     -- TickMinimal
           11 DATE, TIME, LAST, VOL, ID
           12 DATE, TIME, LAST, VOL, ID, OPER                      -- TickOptimal
         */
    }

    /// <summary>
    /// Время свечи (Open - время начала свечи, Close - время завершения свечи)
    /// </summary>
    public enum CandleTime
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
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum DateFormat
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
        // ReSharper disable once InconsistentNaming
        DD_MM_YY = 4,

        /// <summary>
        /// ММ/ДД/ГГ
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MM_DD_YY = 5,
    }

    /// <summary>
    /// Формат времени (HHMMSS, HHMM, HH_MM_SS, HH_MM)
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TimeFormat
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
    public enum ColumnHeaderNeed
    {
        No = 0,
        Yes = 1,
    }

}