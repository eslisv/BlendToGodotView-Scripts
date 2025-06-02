using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace EVHelpers
{
    public static class FileSearchHelper
    {
        public static IReadOnlyCollection<string> SearchPathForFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        public static string FindExecutableInstallPath(string executableName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Check if executable is listed in uninstall registry
                string appPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey localMachine = Registry.LocalMachine.OpenSubKey(appPath);
                foreach (string key in localMachine.GetSubKeyNames())
                {
                    RegistryKey subKey = localMachine.OpenSubKey(key);
                    string displayName = (string)subKey.GetValue("DisplayName");
                    if (displayName == executableName)
                    {
                        return (string)subKey.GetValue("InstallLocation");
                    }
                }

                // Check if Steam is installed on device (As programs installed via Steam are not always listed in the search above)
                string steamAppPath = @"SOFTWARE\WOW6432Node\Valve\Steam";
                RegistryKey steamRegKey = Registry.LocalMachine.OpenSubKey(steamAppPath);

                if (steamRegKey == null) { return string.Empty; }

                string steamInstallPath = (string)steamRegKey.GetValue("InstallPath") + @"\steamapps";
                // Read the libraryfolders.vdf file which contains the all of the paths where linked steamapp folders are.
                string[] libraryFoldersFile = File.ReadAllLines(steamInstallPath + @"\libraryfolders.vdf");
                List<string> steamappFolders = new List<string>();
                foreach (string line in libraryFoldersFile)
                {
                    if (line.Contains("\"path\""))
                    {
                        steamappFolders.Add(line.Trim().Substring(6).Replace('\"', ' ').Trim() + "\\steamapps\\common");
                    }
                }
                foreach (string path in steamappFolders)
                {
                    string[] folderNames = Directory.GetDirectories(path);
                    foreach (string folderName in folderNames)
                    {
                        // The saved paths generally are formatted as {PATHTOSTEAMFOLDER}/steamapps/common/{ProgramName}
                        string baseFolderName = folderName.Split("\\").Last();
                        if (baseFolderName == executableName)
                        {
                            return folderName;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
