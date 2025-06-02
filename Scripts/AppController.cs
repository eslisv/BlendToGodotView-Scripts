using EVHelpers;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class AppController : Node
{
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            ExitSequence();
            GetTree().Quit();
        }
    }

    private void ExitSequence()
    {
        GD.Print("QUITTING GAME");
        string savedFilePath = Path.GetTempPath().PathJoin(@"\BlenderImporterViewer\DefaultPath.txt");
        if (File.Exists(savedFilePath))
        {
            string fileText = File.ReadAllText(savedFilePath);
            if (fileText.GetBaseDir() != FileWatcherHelper.DirectoryWatchers[0].Path)
            {
                File.WriteAllText(savedFilePath, FileWatcherHelper.DirectoryWatchers[0].Path);
            }
        }
        FileWatcherHelper.Deactivate();
    }
}
