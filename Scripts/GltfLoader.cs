using EVHelpers;
using EVHelpers.Godot;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class GltfLoader
{
    public IReadOnlyDictionary<string, bool> FilePathNeedsReload => _filePathNeedsReload;
    private Dictionary<string, bool> _filePathNeedsReload;
    private GltfDocument _document = new GltfDocument();
    private GltfState _state = new GltfState();
    private string _loadedPath = string.Empty;

    public Action<string> OnOpenedFileChanged;

    public void Setup(string filePath)
    {
        FileWatcherHelper.AddFileWatcherToList(
            path: filePath,
            filter: "*.blend",
            notifyFilters: NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
            includeSubdirectories: true,
            enableRaisingEvents: true,
            changeEvent: FileChanged,
            createEvent: FileCreated,
            deleteEvent: FileDeleted,
            renameEvent: FileRenamed
            );
        FileWatcherHelper.AddFileWatcherToList(
            path: filePath,
            filter: $"{filePath.GetFile()}",
            changeEvent: LoadFileIntoScene
            );
        _filePathNeedsReload = new Dictionary<string, bool>();
        GD.Print(filePath);
        string[] files = FileSearchHelper.SearchPathForFiles(path: filePath, searchPattern: "*.gltf").ToArray();
        foreach (string file in files)
        {
            _filePathNeedsReload.Add(file, false);
        }
        GodotPrintHelper.PrintDictionary(_filePathNeedsReload);
    }

    public void LoadFile(string path)
    {
        _document.AppendFromFile(path, _state);
    }

    public Node3D GenerateNode()
    {
        return (Node3D)_document.GenerateScene(_state);
    }

    public void Reset()
    {
        _loadedPath = string.Empty;
        _filePathNeedsReload.Clear();
        _document.Dispose();
        _state.Dispose();
        _document = new GltfDocument();
        _state = new GltfState();
    }

    private void FileChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed) { return; }
        if (!_filePathNeedsReload.ContainsKey(e.FullPath)) { return; }
        _filePathNeedsReload[e.FullPath] = true;
        if (_loadedPath == e.FullPath)
        {
            Reset();
            LoadFile(e.FullPath);
            _filePathNeedsReload[e.FullPath] = false;
        }
        GD.Print($"{e.FullPath} CHANGED");
        GodotPrintHelper.PrintDictionary(_filePathNeedsReload);
        GenerateGltfFromBlendFile(e.FullPath.GetBaseName());
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Created) { return; }
        if (_filePathNeedsReload.ContainsKey(e.FullPath)) { return; }
        _filePathNeedsReload.Add(e.FullPath, true);
        if (_loadedPath == e.FullPath)
        {
            LoadFile(e.FullPath);
            _filePathNeedsReload[e.FullPath] = false;
        }
        GD.Print($"{e.FullPath} CREATED");
        GodotPrintHelper.PrintDictionary(_filePathNeedsReload);
    }

    private void LoadFileIntoScene(object sender, FileSystemEventArgs e)
    {
        OnOpenedFileChanged?.Invoke(e.FullPath);
    }

    private void FileDeleted(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Deleted) { return; }
        if (!_filePathNeedsReload.ContainsKey(e.FullPath)) { return; }
        _filePathNeedsReload.Remove(e.FullPath);
        if (_loadedPath == e.FullPath)
        {
            LoadFile(e.FullPath);
            _filePathNeedsReload[e.FullPath] = false;
        }
        GD.Print($"{e.FullPath} DELETED");
        GodotPrintHelper.PrintDictionary(_filePathNeedsReload);
    }

    private void FileRenamed(object sender, RenamedEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Renamed) { return; }
        if (_filePathNeedsReload.ContainsKey(e.OldFullPath))
        {
            _filePathNeedsReload.Remove(e.OldFullPath);
        }
        if (e.FullPath.EndsWith(".gltf"))
        {
            _filePathNeedsReload.Add(e.FullPath, true);
        }
        if (_loadedPath == e.FullPath)
        {
            LoadFile(e.FullPath);
            _filePathNeedsReload[e.FullPath] = false;
        }
        GD.Print($"Path:{e.OldFullPath} RENAMED to {e.FullPath}");
        GodotPrintHelper.PrintDictionary(_filePathNeedsReload);
        GenerateGltfFromBlendFile(e.FullPath.GetBaseName());
    }

    public void GenerateGltfFromBlendFile(string exportFilePath)
    {
        FileWatcherHelper.SetPathOfWatcherAtIndex(1, exportFilePath.GetBaseDir());
        FileWatcherHelper.SetFilterAtIndex(1, $"*.gltf");
        string pyScript = @$"import bpy

bpy.ops.export_scene.gltf(
    filepath = r""{exportFilePath}.gltf"",
    check_existing = False,
    export_format = 'GLTF_SEPARATE',
    use_visible = True,
    export_use_gltfpack = True
)";
        string pyFileDirectory = Path.GetTempPath() + @"BlenderImporterViewer";
        Directory.CreateDirectory(pyFileDirectory);
        File.WriteAllText(@$"{pyFileDirectory}\exportScript.py", pyScript);
        string blenderPath = FileSearchHelper.FindExecutableInstallPath("Blender").SimplifyPath();
        string blenderExportCommand = @$"blender -b ""{exportFilePath}.blend"" --python ""{pyFileDirectory}/exportScript.py""".Replace('/', '\\');
        GD.Print("Generating GLTF file...");
        GD.Print($"{blenderExportCommand}");
        WindowsCmdHelper.RunCommandInBackground(blenderExportCommand, blenderPath, out string output, out string error);
        GD.Print(output);
        GD.Print(error);
    }
}
