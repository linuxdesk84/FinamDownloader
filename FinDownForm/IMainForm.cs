using System;

namespace FinDownForm {
    interface IMainForm {
        event EventHandler FormClosingClick;

        #region tabMain

        string IssuerName { get; }
        string IssuerMarket { get; }
        string IssuerId { get; }

        bool FExactMatchName { get; }
        bool FMatchCase { get; }
        bool fAllTime { get; }
        bool IsFutures { get; }
        bool fSkipUnfinished { get; }
        bool fOverwrite { get; }

        /// <summary>
        /// минимально возможная к выбору дата - определяется из модели
        /// </summary>
        DateTime DtPeriodMin { set; }

        /// <summary>
        /// максимально возможная к выбору дата - определяется из модели
        /// </summary>
        DateTime DtPeriodMax { set; }

        DateTime DtPeriodFrom { get; set; }
        DateTime DtPeriodTo { get; set; }

        void Logging(string str);
        //void ClearLog();

        void FlipButtons(bool fEnable);


        void SetIssuerCount(int count);
        void SetDownloadedCount(int count);

        event EventHandler SearchIssuerClick;
        event EventHandler DownloadIssuerClick;
        event EventHandler DownloadCancelClick;

        #endregion

        #region tabSettings

        string IchartsPath { get; set; }
        string HistDataDir { get; set; }

        bool FAutoUpdate { get; set; }

        event EventHandler UpdateIChartsClick;
        event Action SaveSettings;

        #endregion
    }
}