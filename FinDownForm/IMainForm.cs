using System;

namespace FinDownForm {
    interface IMainForm {
        #region tabMain

        string IssuerName { get; }
        string IssuerMarket { get; }
        string IssuerId { get; }

        bool fEqualName { get; }
        bool fAllTime { get; }
        bool IsFutures { get; }
        bool fSkipUnfinished { get; }
        bool fOverwrite { get; }

        DateTime DtPeriodMin { set; } // минимально возможная к выбору дата - определяется из модели
        DateTime DtPeriodMax { set; } // максимально возможная к выбору дата - определяется из модели

        DateTime DtPeriodFrom { get; }
        DateTime DtPeriodTo { get; }

        void Logging(string str);
        void ClearLog();

        void SetIssuerCount(int count);
        void SetDownloadedCount(int count);

        event EventHandler SearchIssuerClick;
        event EventHandler DownloadIssuerClick;

        #endregion

        #region tabSettings

        string IChartsPath { get; set; }
        string HistDataDir { get; set; }

        bool fAutoUpdate { get; }

        event EventHandler UpdateIChartsClick;
        event EventHandler SaveSettingsClick;

        #endregion
    }
}