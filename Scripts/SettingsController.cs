using EVLibrary.FileIO.Extensions;
using System.IO;
using System.Text.Json;

namespace AM.ModelViewerTool
{
    public static class SettingsController
    {
        public static SettingsData Settings => _settings;

        private static SettingsData _settings;

        // NOTE: This should only be called once.
        public static void Setup()
        {
            DetermineSettingsFilePath(out string settingsPath, out string settingsFolderPath);
            CreateSettingsFileIfNone(settingsPath, settingsFolderPath);
            _settings = LoadSettingsFromFile(settingsPath);
        }

        public static void SaveSettingsData(string settingsPath)
        {
            FileStream settingsFileStream = File.Create(settingsPath);
            byte[] serializedSettings = JsonSerializer.SerializeToUtf8Bytes(_settings);
            settingsFileStream.Write(serializedSettings);
            settingsFileStream.Close();
        }

        private static void CreateSettingsFileIfNone(string settingsPath, string settingsFolderPath)
        {
            if (!Directory.Exists(settingsFolderPath))
            {
                Directory.CreateDirectory(settingsFolderPath);
            }
            if (!File.Exists(settingsPath))
            {
                _settings = SettingsData.CreateDefault();
                SaveSettingsData(settingsPath);
            }
        }

        private static void DetermineSettingsFilePath(out string settingsPath, out string settingsFolderPath)
        {
            settingsPath = PathReferences.GetAbsolutePathToSettingsSavePathFile();
            settingsFolderPath = Path.GetDirectoryName(settingsPath).StandardizeSlashesToBackslash();
        }

        private static SettingsData LoadSettingsFromFile(string settingsPath)
        {
            FileStream fileStream = File.Open(settingsPath, FileMode.Open);
            SettingsData settings = JsonSerializer.Deserialize<SettingsData>(fileStream);
            fileStream.Close();
            return settings;
        }
    }
}