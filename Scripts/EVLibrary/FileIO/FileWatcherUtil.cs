using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EVLibrary.FileIO
{
    public static class FileWatcherUtil
    {
        private const string CHANGED_NAME = "Changed";
        private const string CREATED_NAME = "Created";
        private const string DELETED_NAME = "Deleted";
        private const string RENAMED_NAME = "Renamed";
        public static Dictionary<EWatcher, FileSystemWatcher> DirectoryWatchers => _directoryWatchers;
        private static Dictionary<EWatcher, FileSystemWatcher> _directoryWatchers = new Dictionary<EWatcher, FileSystemWatcher>();
        private static Dictionary<EWatcher, Dictionary<string, object>> _subscriptionDict = new Dictionary<EWatcher, Dictionary<string, object>>();

        public static void AddFileWatcherToList(EWatcher watcherKey, string path, string filter = "*.*", NotifyFilters notifyFilters = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime, bool includeSubdirectories = true, bool enableRaisingEvents = true, FileSystemEventHandler changeEvent = null, FileSystemEventHandler createEvent = null, FileSystemEventHandler deleteEvent = null, RenamedEventHandler renameEvent = null)
        {
            if (!Directory.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            FileSystemWatcher tempWatcher = new FileSystemWatcher(path, filter);
            tempWatcher.IncludeSubdirectories = includeSubdirectories;
            tempWatcher.EnableRaisingEvents = enableRaisingEvents;
            tempWatcher.Changed += changeEvent;
            tempWatcher.Created += createEvent;
            tempWatcher.Deleted += deleteEvent;
            tempWatcher.Renamed += renameEvent;
            tempWatcher.NotifyFilter = notifyFilters;

            Dictionary<string, object> tempSubscriptionDict = new Dictionary<string, object>();
            if (changeEvent != null)
            {
                tempSubscriptionDict.Add(CHANGED_NAME, changeEvent);
            }
            if (createEvent != null)
            {
                tempSubscriptionDict.Add(CREATED_NAME, createEvent);
            }
            if (deleteEvent != null)
            {
                tempSubscriptionDict.Add(DELETED_NAME, deleteEvent);
            }
            if (renameEvent != null)
            {
                tempSubscriptionDict.Add(RENAMED_NAME, renameEvent);
            }

            _directoryWatchers.Add(watcherKey, tempWatcher);
            _subscriptionDict.Add(watcherKey, tempSubscriptionDict);
        }

        public static void Cleanup()
        {
            foreach (EWatcher key in _directoryWatchers.Keys)
            {
                DeactivateFileWatcher(key);
            }
            _subscriptionDict.Clear();
            foreach (FileSystemWatcher watcher in _directoryWatchers.Values)
            {
                watcher.Dispose();
            }
            _directoryWatchers.Clear();
        }

        #region Getters
        public static FileSystemWatcher GetWatcher(EWatcher key)
        {
            return _directoryWatchers[key];
        }

        public static IReadOnlyList<string> GetFilesFromWatcher(EWatcher key)
        {
            return FileSearchHelper.SearchPathForFilesWithExtension(_directoryWatchers[key].Path, _directoryWatchers[key].Filter).ToList();
        }
        #endregion

        #region Removers
        public static void RemoveFileWatcher(EWatcher key)
        {
            DeactivateFileWatcher(key);
            _directoryWatchers[key].Dispose();
            _directoryWatchers.Remove(key);
        }
        #endregion

        #region Setters
        public static void SetPathOfWatcher(EWatcher key, string path)
        {
            _directoryWatchers[key].Path = path;
        }

        public static void SetFilter(EWatcher key, string filter)
        {
            _directoryWatchers[key].Filter = filter;
        }
        #endregion

        #region Deactivator
        private static void DeactivateFileWatcher(EWatcher key)
        {
            foreach (string subKey in _subscriptionDict[key].Keys)
            {
                switch (subKey)
                {
                    case CHANGED_NAME:
                        _directoryWatchers[key].Changed -= (FileSystemEventHandler)_subscriptionDict[key][subKey];
                        break;
                    case CREATED_NAME:
                        _directoryWatchers[key].Created -= (FileSystemEventHandler)_subscriptionDict[key][subKey];
                        break;
                    case DELETED_NAME:
                        _directoryWatchers[key].Deleted -= (FileSystemEventHandler)_subscriptionDict[key][subKey];
                        break;
                    case RENAMED_NAME:
                        _directoryWatchers[key].Renamed -= (RenamedEventHandler)_subscriptionDict[key][subKey];
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}