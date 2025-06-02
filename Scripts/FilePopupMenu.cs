using EVHelpers;
using Godot;
using System;
using System.Diagnostics;

public partial class FilePopupMenu : PopupMenu
{
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            IdPressed -= OptionPressed;
        }
    }

    public override void _Ready()
	{
		IdPressed += OptionPressed;
	}

    private void OptionPressed(long id)
    {
		switch (id) // Select Folder
		{
			case 0:
                FileDialog fileDialog = new FileDialog();
				fileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
				fileDialog.Access = FileDialog.AccessEnum.Filesystem;
				fileDialog.DirSelected += DirectorySelected;
				fileDialog.Visible = true;
				fileDialog.Size = new Vector2I(640, 360);
				AddChild(fileDialog);
                fileDialog.VisibilityChanged += exitDelegate;

                void exitDelegate() {
                    if (fileDialog.Visible) { return; }
					GD.Print("Closing File Dialog");
					fileDialog.VisibilityChanged -= exitDelegate;
                    fileDialog.DirSelected -= DirectorySelected;
                    fileDialog?.Dispose();
                };
                break;
            case 1:
                GD.Print(FileWatcherHelper.DirectoryWatchers[0].Path);
                Process.Start("explorer.exe", @$"{FileWatcherHelper.DirectoryWatchers[0].Path}");
                break;
			default:
                GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
				break;
		}
    }

    private void DirectorySelected(string dir)
    {
        string fixedDir = dir.Replace("/", "\\");
		SceneManager loader = (SceneManager)GetTree().Root.GetChild(0);
		loader.FilePath = fixedDir;
		loader.Reset();
		FileWatcherHelper.SetPathOfWatcherAtIndex(0, fixedDir);
    }
}
