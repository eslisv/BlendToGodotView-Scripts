using EVLibrary.FileIO;
using Godot;
using System;
using System.IO;
using SysFile = System.IO.File;

namespace AM.ModelViewerTool
{
    public sealed class GltfLoader
    {
        private readonly Action<string> _openFileChangedCallback;
        private readonly string _watchingBlenderFolderPath;
        private readonly string _watchingGltfFolderPath;
        private GltfDocument _document;
        private GltfState _state;

        public GltfLoader(string watchingBlenderFolderPath, string watchingGltfFolderPath, Action<string> openFileChangedCallback)
        {
            _openFileChangedCallback = openFileChangedCallback;
            _watchingBlenderFolderPath = watchingBlenderFolderPath;
            _watchingGltfFolderPath = watchingGltfFolderPath;
            _document = new();
            _state = new();

            GD.Print($"FILE PATH = {_watchingBlenderFolderPath}");
            GD.Print($"MODEL PATH = {_watchingGltfFolderPath}");

            FileWatcherUtil.AddFileWatcherToList(
                watcherKey: EWatcher.BASE_FOLDER,
                path: _watchingBlenderFolderPath,
                filter: FileExtension.BLEND.AsGenericFilter(),
                notifyFilters: NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                includeSubdirectories: true,
                enableRaisingEvents: true,
                changeEvent: OnFileChanged,
                createEvent: OnFileCreated,
                deleteEvent: OnFileDeleted,
                renameEvent: OnFileRenamed
            );
            FileWatcherUtil.AddFileWatcherToList(
                watcherKey: EWatcher.MODEL_FILE,
                path: _watchingGltfFolderPath,
                filter: FileExtension.GLTF.AsGenericFilter(),
                changeEvent: OnLoadFileIntoScene
            );
        }

        public void Cleanup()
        {
            // TODO: Make FileWatcherUtil's functionality here and in constructor into a new-up-able class that gets its setup (constructor potentially) / cleanup called by this.
            // OR: Make a FileWatcherUtil.RemoveFileWatcherFromList function and use that here instead.
            FileWatcherUtil.Cleanup();
        }

        public void LoadFile(string path)
        {
            if (!SysFile.Exists(path)) { return; }
            _state.SetHandleBinaryImage((int)GltfState.HandleBinaryEmbedAsUncompressed);
            Error err = _document.AppendFromFile(path, _state);
            if (!err.Equals(Error.Ok))
            {
                GD.PrintErr(err);
            }
        }

        public Node3D GenerateNode()
        {
            return (Node3D)_document.GenerateScene(_state);
        }

        public void Reset()
        {
            _document.Dispose();
            _state.Dispose();
            _document = new GltfDocument();
            _state = new GltfState();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs args)
        {
            if (args.FullPath.EndsWith(".blend1")) { return; }
            if (args.ChangeType != WatcherChangeTypes.Changed) { return; }
            GD.Print($"{args.FullPath} CHANGED");
            GD.Print($"Making file at path {args.FullPath.GetBaseName()}");
            GltfUtil.GenerateGltfFromBlendFile(args.FullPath.GetBaseName());
        }

        private void OnFileCreated(object sender, FileSystemEventArgs args)
        {
            if (args.FullPath.EndsWith(".blend1")) { return; }
            if (args.ChangeType != WatcherChangeTypes.Created) { return; }
            GD.Print($"{args.FullPath} CREATED");
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs args)
        {
            if (args.FullPath.EndsWith(".blend1")) { return; }
            if (args.ChangeType != WatcherChangeTypes.Deleted) { return; }
            GD.Print($"{args.FullPath} DELETED");
        }

        private void OnFileRenamed(object sender, RenamedEventArgs args)
        {
            if (args.FullPath.EndsWith(".blend1")) { return; }
            if (args.ChangeType != WatcherChangeTypes.Renamed) { return; }
            GD.Print($"Path:{args.OldFullPath} RENAMED to {args.FullPath}");
            GD.Print(@$"Making file at path {args.FullPath.GetBaseName()}\{Path.GetFileNameWithoutExtension(args.FullPath)}.{GltfUtil.GLTF_FILE_EXTENSION_NAME}");
            GltfUtil.GenerateGltfFromBlendFile(@$"{args.FullPath.GetBaseName()}\{Path.GetFileNameWithoutExtension(args.FullPath)}.{GltfUtil.GLTF_FILE_EXTENSION_NAME}");
        }

        private void OnLoadFileIntoScene(object sender, FileSystemEventArgs args)
        {
            _openFileChangedCallback?.Invoke(args.FullPath);
        }
    }
}