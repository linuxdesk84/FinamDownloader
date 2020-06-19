using System;
using System.IO;

namespace FinamDownloader {
    /// <summary>
    /// Настройки программы.
    /// </summary>
    public class Settings {
        private string _histDataDir;
        private string _ichartsPath;

        public event Action<string> Inform;

        public string HistDataDir {
            get => _histDataDir;
            set {
                if (!Directory.Exists(value)) {
                    if (value != string.Empty) {
                        Inform?.Invoke($@"Directory {value} isn't exists");
                    }
                    _histDataDir = string.Empty;
                } else {
                    _histDataDir = value;
                }
            }
        }

        public string IchartsPath {
            get => _ichartsPath;
            set {
                if (!File.Exists(value)) {
                    if (value != string.Empty) {
                        Inform?.Invoke($@"File {value} isn't exists");
                    }
                    _ichartsPath = string.Empty;

                } else {
                    _ichartsPath = value;
                }
            }
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