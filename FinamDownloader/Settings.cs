using System.IO;

namespace FinamDownloader {
    class Settings {
        private string _histDataDir;
        private string _ichartsPath;

        public string HistDataDir {
            get => _histDataDir;
            set => _histDataDir = Directory.Exists(value) ? value : string.Empty;
        }

        public string IchartsPath {
            get => _ichartsPath;
            set => _ichartsPath = File.Exists(value) ? value : string.Empty;
        }

        public bool FAutoUpdate { get; set; }

        public Settings() : this(string.Empty, string.Empty, false) { }

        public Settings(string historyDataDir, string fileIChartsPath, bool fAutoUpdate) {
            HistDataDir = historyDataDir;
            IchartsPath = fileIChartsPath;
            FAutoUpdate = fAutoUpdate;
        }
    }
}