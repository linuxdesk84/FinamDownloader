using System;

namespace FinDownEntity {
    public class Collector {
        public string TICKER { get; }
        public string PER    { get; }
        public string DATE   { get; }
        public string TIME   { get; }
        public string OPEN   { get; }
        public string HIGH   { get; }
        public string LOW    { get; }
        public string CLOSE  { get; }
        public string VOL    { get; }
        public string LAST   { get; }
        public string ID     { get; }
        public string OPER   { get; }

        private Collector() {
            TICKER =
                PER =
                    DATE =
                        TIME =
                            OPEN =
                                HIGH =
                                    LOW =
                                        CLOSE =
                                            VOL =
                                                LAST =
                                                    ID =
                                                        OPER = string.Empty;
        }

        public Collector(string[] buf, DataFormat df) : this() {
            switch (df) {
                case DataFormat.CandleAllParam:
                    TICKER = buf[0];
                    PER = buf[1];
                    DATE = buf[2];
                    TIME = buf[3];
                    OPEN = buf[4];
                    HIGH = buf[5];
                    LOW = buf[6];
                    CLOSE = buf[7];
                    VOL = buf[8];
                    break;

                case DataFormat.Candle_TP_DT_OHLC:
                    TICKER = buf[0];
                    PER = buf[1];
                    DATE = buf[2];
                    TIME = buf[3];
                    OPEN = buf[4];
                    HIGH = buf[5];
                    LOW = buf[6];
                    CLOSE = buf[7];
                    break;

                case DataFormat.Candle_TP_DT_C_V:
                    TICKER = buf[0];
                    PER = buf[1];
                    DATE = buf[2];
                    TIME = buf[3];
                    CLOSE = buf[4];
                    VOL = buf[5];
                    break;

                case DataFormat.Candle_TP_DT_C:
                    TICKER = buf[0];
                    PER = buf[1];
                    DATE = buf[2];
                    TIME = buf[3];
                    CLOSE = buf[4];
                    break;

                case DataFormat.Candle_DT_OHLC_V:
                    DATE = buf[0];
                    TIME = buf[1];
                    OPEN = buf[2];
                    HIGH = buf[3];
                    LOW = buf[4];
                    CLOSE = buf[5];
                    VOL = buf[6];
                    break;

                case DataFormat.Tick_TP_DT_LV:
                    TICKER = buf[0];
                    PER = buf[1];
                    DATE = buf[2];
                    TIME = buf[3];
                    LAST = buf[4];
                    VOL = buf[5];
                    break;

                case DataFormat.Tick_T_DT_LV:
                    TICKER = buf[0];
                    DATE = buf[1];
                    TIME = buf[2];
                    LAST = buf[3];
                    VOL = buf[4];
                    break;

                case DataFormat.Tick_T_DT_L:
                    TICKER = buf[0];
                    DATE = buf[1];
                    TIME = buf[2];
                    LAST = buf[3];
                    break;

                case DataFormat.Tick_DT_LV:
                    DATE = buf[0];
                    TIME = buf[1];
                    LAST = buf[2];
                    VOL = buf[3];
                    break;

                case DataFormat.TickMinimal:
                    DATE = buf[0];
                    TIME = buf[1];
                    LAST = buf[2];
                    break;

                case DataFormat.Tick_DT_LV_I:
                    DATE = buf[0];
                    TIME = buf[1];
                    LAST = buf[2];
                    VOL = buf[3];
                    ID = buf[4];
                    break;

                case DataFormat.TickOptimal:
                    DATE = buf[0];
                    TIME = buf[1];
                    LAST = buf[2];
                    VOL = buf[3];
                    ID = buf[4];
                    OPER = buf[5];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(df), df, null);
            }
        }
    }
}