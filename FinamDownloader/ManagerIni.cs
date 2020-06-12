using System;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;

namespace FinamDownloader
{
    class ManagerIni {
        private readonly string _fileIniPath;
        public Settings FdSettings { get; }

        public event Action SettingsLoaded;

        public ManagerIni(string appDir) {
            FdSettings = new Settings();

            _fileIniPath = appDir + "FinamDowloader.ini";
            if (File.Exists(_fileIniPath)) {
                LoadSettings();
            }
        }

        private void LoadSettings() {
            //Create an instance of a ini file parser
            FileIniDataParser fileIniData = new FileIniDataParser();

            //Parse the ini file
            IniData parsedData = fileIniData.ReadFile(_fileIniPath, Encoding.Default);

            var histDataDir = parsedData["general"]["histDataDir"];
            var ichartsPath = parsedData["general"]["ichartsPath"];
            var fAutoUpdate = parsedData["general"]["fAutoUpdate"];

            var parseResult = bool.TryParse(fAutoUpdate, out var fAuto);
            if (!parseResult) {
                fAuto = true;
            }

            SetSettings(histDataDir, ichartsPath, fAuto);

            SettingsLoaded?.Invoke();
        }

        private void SaveSettings() {
            var fileIniData = new FileIniDataParser();
            var parsedData = fileIniData.ReadFile(_fileIniPath, Encoding.Default);

            parsedData["general"]["histDataDir"] = FdSettings.HistDataDir;
            parsedData["general"]["ichartsPath"] = FdSettings.IchartsPath;
            parsedData["general"]["fAutoUpdate"] = FdSettings.FAutoUpdate.ToString();

            fileIniData.WriteFile(_fileIniPath, parsedData);
        }

        public void SetSettings(string histDataDir, string ichartsPath, bool fAutoUpdate) {
            // применяем настройки
            FdSettings.HistDataDir = histDataDir;
            FdSettings.IchartsPath = ichartsPath;
            FdSettings.FAutoUpdate = fAutoUpdate;

            // сохраняем настройки в файл
            SaveSettings();
        }

    }
}
