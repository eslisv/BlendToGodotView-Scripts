using System.IO;

namespace EVLibrary.FileIO.Extensions
{
    public static class StringPathExtensions
    {
        public static string CombinePaths(string basePath, params string[] additionalPaths)
        {
            string tempPath = basePath;
            foreach (string path in additionalPaths)
            {
                tempPath = Path.Join(tempPath, path);
            }
            tempPath = tempPath.StandardizeSlashesToBackslash();
            return tempPath;
        }

        public static string StandardizeSlashesToBackslash(this string path) => path.Replace("/", "\\");
        public static string StandardizeSlashesToForwardslash(this string path) => path.Replace("\\", "/");
    }
}