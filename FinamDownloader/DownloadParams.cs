using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinamDownloader
{
    public class DownloadParams
    {
        public DownloadParams(string issuerName, string issuerMarket, string issuerId, bool fExactMatchName, bool fMatchCase, bool isFutures, bool fAllTime, DateTime dtPeriodFrom, DateTime dtPeriodTo, bool fOverwrite, bool fSkipUnfinished)
        {
            IssuerName = issuerName;
            IssuerMarket = issuerMarket;
            IssuerId = issuerId;
            FExactMatchName = fExactMatchName;
            FMatchCase = fMatchCase;
            IsFutures = isFutures;
            FAllTime = fAllTime;
            DtPeriodBeg = dtPeriodFrom;
            DtPeriodEnd = dtPeriodTo;
            FOverwrite = fOverwrite;
            FSkipUnfinished = fSkipUnfinished;
        }

        public string IssuerName { get; }
        public string IssuerMarket { get; }
        public string IssuerId { get; }

        public bool FExactMatchName { get; }
        public bool FMatchCase { get; }
        public bool IsFutures { get; }

        public bool FAllTime { get; }
        public DateTime DtPeriodBeg { get; }
        public DateTime DtPeriodEnd { get; }

        public bool FOverwrite { get; }
        public bool FSkipUnfinished { get; }
    }
}
