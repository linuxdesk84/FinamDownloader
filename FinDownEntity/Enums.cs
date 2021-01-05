using System.Diagnostics.CodeAnalysis;

namespace FinDownEntity {
    /// <summary>
    /// Таймфрейм (Tick, M1, M5, M10, M15, M30, H1, Day, Week, Month)
    /// </summary>
    public enum ETimeFrame
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
    public enum FieldSeparator {
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
    public enum BitSeparator {
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
    public enum DataFormat {
        /// <summary>
        /// (Candle_TP_DT_OHLC_V)
        /// TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
        /// </summary>
        CandleAllParam = 1,

        /// <summary>
        /// TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE
        /// </summary>
        Candle_TP_DT_OHLC = 2,

        /// <summary>
        /// TICKER, PER, DATE, TIME, CLOSE, VOL
        /// </summary>
        Candle_TP_DT_C_V = 3,

        /// <summary>
        /// TICKER, PER, DATE, TIME, CLOSE
        /// </summary>
        Candle_TP_DT_C = 4,

        /// <summary>
        /// DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
        /// </summary>
        Candle_DT_OHLC_V = 5,

        /// <summary>
        /// TICKER, PER, DATE, TIME, LAST, VOL
        /// </summary>
        Tick_TP_DT_LV = 6,

        /// <summary>
        /// TICKER, DATE, TIME, LAST, VOL
        /// </summary>
        Tick_T_DT_LV = 7,

        /// <summary>
        /// TICKER, DATE, TIME, LAST
        /// </summary>
        Tick_T_DT_L = 8,

        /// <summary>
        /// DATE, TIME, LAST, VOL
        /// </summary>
        Tick_DT_LV = 9,

        /// <summary>
        /// (Tick_DT_L)
        /// DATE, TIME, LAST
        /// </summary>
        TickMinimal = 10,

        /// <summary>
        /// DATE, TIME, LAST, VOL, ID
        /// </summary>
        Tick_DT_LV_I = 11,


        /// <summary>
        /// (Candle_DT_OHLC_V)
        /// DATE, TIME, LAST, VOL, ID, OPER
        /// </summary>
        TickOptimal = 12,

        /* При парсе icharts.js находится только 11 форматов представления данных. 12 там нет.
           В то же время, при запросе тиковых данных через сайт, сгенерированный url содержит "datf=12".
           2019.08.24 экспериментально выяснил, что параметр datf может быть от 1 до 12, все остальные
           варианты (перебрал от 0 до 18) аналогичны datf=10 (но при этом отсутствует заголовок (header)).
           
           Полная картина:
           1  TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL -- C
           2  TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE      -- C
           3  TICKER, PER, DATE, TIME, CLOSE, VOL                  -- C
           4  TICKER, PER, DATE, TIME, CLOSE                       -- C
           5  DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL              -- CandleOptimal = 5
           6  TICKER, PER, DATE, TIME, LAST, VOL                   -- T
           7  TICKER, DATE, TIME, LAST, VOL                        -- T
           8  TICKER, DATE, TIME, LAST                             -- T
           9  DATE, TIME, LAST, VOL                                -- T
           10 DATE, TIME, LAST                                     -- T
           11 DATE, TIME, LAST, VOL, ID                            -- T
           12 DATE, TIME, LAST, VOL, ID, OPER                      -- TickOptimal = 12
         */
    }

    /// <summary>
    /// Время свечи (Open - время начала свечи, Close - время завершения свечи)
    /// </summary>
    public enum CandleTime {
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
    public enum DateFormat {
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
    public enum TimeFormat {
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
    public enum ColumnHeaderNeed {
        No = 0,
        Yes = 1,
    }
}