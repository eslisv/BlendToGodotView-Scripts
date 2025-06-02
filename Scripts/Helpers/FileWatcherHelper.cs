using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EVHelpers
{
    public static class FileWatcherHelper
    {
        private const string CHANGED_NAME = "Changed";
        private const string CREATED_NAME = "Created";
        private const string DELETED_NAME = "Deleted";
        private const string RENAMED_NAME = "Renamed";
        public static List<FileSystemWatcher> DirectoryWatchers => _directoryWatchers;
        private static List<FileSystemWatcher> _directoryWatchers = new List<FileSystemWatcher>();
        private static List<Dictionary<string, object>> _subscriptionDict = new List<Dictionary<string, object>>();

        public static void AddFileWatcherToList(string path, string filter = "*.*", NotifyFilters notifyFilters = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime, bool includeSubdirectories = true, bool enableRaisingEvents = true, FileSystemEventHandler changeEvent = null, FileSystemEventHandler createEvent = null, FileSystemEventHandler deleteEvent = null, RenamedEventHandler renameEvent = null)
        {
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

            _directoryWatchers.Add(tempWatcher);
            _subscriptionDict.Add(tempSubscriptionDict);
        }

        #region Getters
        public static FileSystemWatcher GetWatcherFromIndex(int index)
        {
            return _directoryWatchers[index];
        }

        public static FileSystemWatcher GetWatcherFromPath(string path)
        {
            int index = GetIndexOfWatcherWatchingPath(path);
            return _directoryWatchers[index];
        }

        public static int GetIndexOfWatcherWatchingPath(string path)
        {
            foreach (FileSystemWatcher watcher in _directoryWatchers)
            {
                if (watcher.Path == path)
                {
                    return _directoryWatchers.IndexOf(watcher);
                }
            }
            return -1;
        }

        public static IReadOnlyList<string> GetFilesOfWatcherFromIndex(int index)
        {

            return FileSearchHelper.SearchPathForFiles(_directoryWatchers[index].Path, _directoryWatchers[index].Filter).ToList();
        }
        #endregion

        #region Removers
        public static void RemoveFileWatcherFromList(FileSystemWatcher fileSystemWatcher)
        {
            int index = _directoryWatchers.IndexOf(fileSystemWatcher);
            _directoryWatchers.RemoveAt(index);
            DeactivateFileWatcherAtIndex(index);
        }

        public static void RemoveFileWatcherAtIndex(int index)
        {
            _directoryWatchers.RemoveAt(index);
            DeactivateFileWatcherAtIndex(index);
        }

        public static void RemoveFileWatcherLookingAtPath(string path)
        {
            int index = GetIndexOfWatcherWatchingPath(path);
            RemoveFileWatcherAtIndex(index);
        }
        #endregion

        #region Setters
        public static void SetPathOfWatcherAtIndex(int index, string path)
        {
            _directoryWatchers[index].Path = path;
        }

        public static void SetFilterAtIndex(int index, string filter)
        {
            _directoryWatchers[index].Filter = filter;
        }
        #endregion

        #region Deactivator
        private static void DeactivateFileWatcherAtIndex(int index)
        {
            foreach (string key in _subscriptionDict[index].Keys)
            {
                switch (key)
                {
                    case CHANGED_NAME:
                        _directoryWatchers[index].Changed -= (FileSystemEventHandler)_subscriptionDict[index][key];
                        break;
                    case CREATED_NAME:
                        _directoryWatchers[index].Created -= (FileSystemEventHandler)_subscriptionDict[index][key];
                        break;
                    case DELETED_NAME:
                        _directoryWatchers[index].Deleted -= (FileSystemEventHandler)_subscriptionDict[index][key];
                        break;
                    case RENAMED_NAME:
                        _directoryWatchers[index].Renamed -= (RenamedEventHandler)_subscriptionDict[index][key];
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Deactivate()
        {
            for (int i = 0; i < _directoryWatchers.Count; i++)
            {
                DeactivateFileWatcherAtIndex(i);
            }
            _subscriptionDict.Clear();
            foreach (FileSystemWatcher watcher in _directoryWatchers)
            {
                watcher.Dispose();
            }
            _directoryWatchers.Clear();
        }
        #endregion
    }
}