using System;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;

namespace FinamDownloader
{
    public class ManagerIni {
        private readonly string _fileIniPath;
        public Settings FdSettings { get; }

        public event Action SettingsLoaded;
        public event Action<string> Inform;

        public ManagerIni(string fileIniPath) {
            FdSettings = new Settings();
            FdSettings.Inform += FdSettings_Inform;

            _fileIniPath = fileIniPath;
            if (File.Exists(_fileIniPath)) {
                LoadSettings();
            }
        }

        private void FdSettings_Inform(string message) {
            Inform?.Invoke(message);
        }

        private void LoadSettings() {
            //Create an instance of a ini file parser
            FileIniDataParser fileIniData = new FileIniDataParser();

            //Parse the ini file
            IniData parsedData = fileIniData.ReadFile(_fileIniPath, Encoding.Default);

            var histDataDir = parsedData["general"]["histDataDir"];
            var ichartsPath = parsedData["general"]["ichartsPath"];
            var sAutoUpdate = parsedData["general"]["fAutoUpdate"];

            var parseResult = bool.TryParse(sAutoUpdate, out var fAutoUpdate);
            if (!parseResult) {
                fAutoUpdate = true;
            }

            SetSettings(histDataDir, ichartsPath, fAutoUpdate);

            SettingsLoaded?.Invoke();
        }

        /// <summary>
        /// сохраняем настройки в файл
        /// </summary>
        public void SaveSettings(string histDataDir, string ichartsPath, bool fAutoUpdate) {
            SetSettings(histDataDir, ichartsPath, fAutoUpdate);

            var fileIniData = new FileIniDataParser();

            const string sectionName = "general";
            IniData settingsIniData = new IniData();
            settingsIniData.Sections.AddSection(sectionName);
            settingsIniData.Sections.GetSectionData(sectionName)
                .Comments.Add("Основные настройки");

            settingsIniData.Sections.GetSectionData(sectionName).Keys.AddKey("histDataDir", FdSettings.HistDataDir);
            settingsIniData.Sections.GetSectionData(sectionName).Keys.GetKeyData("histDataDir")
                .Comments.Add("каталог сохранения скачиваемых файлов");

            settingsIniData.Sections.GetSectionData(sectionName).Keys.AddKey("ichartsPath", FdSettings.IchartsPath);
            settingsIniData.Sections.GetSectionData(sectionName).Keys.GetKeyData("ichartsPath")
                .Comments.Add("путь до файла 'icharts.js'");

            settingsIniData.Sections.GetSectionData(sectionName).Keys.AddKey("fAutoUpdate", FdSettings.FAutoUpdate.ToString());
            settingsIniData.Sections.GetSectionData(sectionName).Keys.GetKeyData("fAutoUpdate")
                .Comments.Add("флаг автоматического обновления 'icharts.js'. периодичность - 24ч.");

            fileIniData.WriteFile(_fileIniPath, settingsIniData);
        }

        /// <summary>
        /// применяем настройки
        /// </summary>
        public void SetSettings(string histDataDir, string ichartsPath, bool fAutoUpdate) {
            FdSettings.HistDataDir = histDataDir;
            FdSettings.IchartsPath = ichartsPath;
            FdSettings.FAutoUpdate = fAutoUpdate;
        }

    }
}
