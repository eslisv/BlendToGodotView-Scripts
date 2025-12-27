using EVLibrary.FileIO;
using EVLibrary.FileIO.Extensions;
using EVLibrary.Godot.IO;
using Godot;
using System;
using System.Diagnostics;

namespace AM.ModelViewerTool
{

    public sealed partial class ToolBar_Option_File_PopupMenuUIController : Node
    {
        private enum EFilePopupOptions
        {
            SelectFolder,
            OpenSelectedFolder,
            ToggleNativeDialog,
            Exit
        }

        private const string EXPLORER_EXE_NAME = "explorer.exe";

        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _selectEntryArgs;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _openSelectedFolderEntryArgs;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _toggleNativeDialogArgs;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _exitEntryArgs;

        public Action OnDirectorySelectedCallback;

        [Export] private CustomPopupMenuUIController _customPopupMenuUIController;

        private ModelLoadingController _modelLoadingController;
        private CustomPopupMenu_Args _args;

        public override void _ExitTree()
        {
            if (OnDirectorySelectedCallback == null) { return; }
            Delegate[] delegateList = OnDirectorySelectedCallback.GetInvocationList();
            foreach (Delegate del in delegateList)
            {
                OnDirectorySelectedCallback -= (del as Action);
            }
        }

        public void Setup(ModelLoadingController modelLoadingController, Action onDirectorySelectedCallback)
        {
            _modelLoadingController = modelLoadingController;
            OnDirectorySelectedCallback += onDirectorySelectedCallback;

            _args = new CustomPopupMenu_Args(
                _selectEntryArgs.CreateArgs(selectedCallback: SelectFolder),
                _openSelectedFolderEntryArgs.CreateArgs(selectedCallback: OpenSelectedFolder),
                _toggleNativeDialogArgs.CreateArgs(selectedCallback: ToggleNativeDialog),
                _exitEntryArgs.CreateArgs(selectedCallback: ExitGame)
            );
            _args.Entries[(int)EFilePopupOptions.ToggleNativeDialog].IsChecked = SettingsController.Settings.UseNativeFileDialog;
            _customPopupMenuUIController.SetArgs(_args);
        }

        private void SelectFolder()
        {
            FileDialogHelper fileDialogHelper = new(
                FileDialog.FileModeEnum.OpenDir,
                FileDialog.AccessEnum.Filesystem,
                null,
                null,
                DirectorySelected,
                startingPath: PathReferences.GetAbsolutePathToProjectPathFolder()
            );

            FileDialog fileDialog = fileDialogHelper.Dialog;
            AddChild(fileDialog);
            fileDialog.PopupCenteredRatio();
        }

        private void OpenSelectedFolder()
        {
            GD.Print(FileWatcherUtil.DirectoryWatchers[EWatcher.BASE_FOLDER].Path);
            Process.Start(EXPLORER_EXE_NAME, $"{FileWatcherUtil.DirectoryWatchers[EWatcher.BASE_FOLDER].Path}");
        }

        private void ToggleNativeDialog()
        {
            bool currentState = _args.Entries[(int)EFilePopupOptions.ToggleNativeDialog].IsChecked;
            _args.Entries[(int)EFilePopupOptions.ToggleNativeDialog].IsChecked = !currentState;
            SettingsController.Settings.UseNativeFileDialog = !currentState;
        }

        private void ExitGame()
        {
            // TODO: Move this to a helper/util class?
            SceneTree tree = GetTree();
            // Note: Quit does not auto call notification close request. Thats weird
            tree.Root.PropagateNotification((int)NotificationWMCloseRequest);
            tree.Quit();
        }

        private void DirectorySelected(string dir)
        {
            _modelLoadingController.Reset();

            string fixedDir = dir.StandardizeSlashesToBackslash();
            FileWatcherUtil.SetPathOfWatcher(EWatcher.BASE_FOLDER, fixedDir);
            OnDirectorySelectedCallback.Invoke();

            SettingsController.Settings.WatchingFolderPath = fixedDir;
        }
    }
}