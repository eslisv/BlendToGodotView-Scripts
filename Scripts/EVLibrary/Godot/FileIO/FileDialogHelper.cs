using AM.ModelViewerTool;
using Godot;
using System;
using System.IO;
using System.Linq;
using static Godot.FileDialog;

namespace EVLibrary.Godot.IO
{
    public class FileDialogHelper
    {
        public FileDialog Dialog => _dialog;

        private FileDialog _dialog;

        private FileDialogHelper()
        {
            _dialog = new FileDialog()
            {
                UseNativeDialog = SettingsController.Settings.UseNativeFileDialog,
                MinSize = new Vector2I(200, 500),
                FileMode = FileModeEnum.OpenAny,
                Access = AccessEnum.Filesystem,
                CurrentFile = ""
            };
            _dialog.TreeExiting += L_ExitDelegate;

            void L_ExitDelegate()
            {
                GD.Print("Closing File Dialog");
                _dialog.TreeExiting -= L_ExitDelegate;
                _dialog.Dispose();
            }
        }

        private FileDialogHelper(FileModeEnum fileMode,
                                 AccessEnum fileAccess,
                                 string startingPath,
                                 string[] filters) : this()
        {
            _dialog.FileMode = fileMode;
            _dialog.Access = fileAccess;

            string formattedPath = startingPath;
            if (formattedPath != string.Empty)
            {
                if (System.IO.File.Exists(formattedPath))
                {
                    GD.PrintRich(GodotPrintHelper.BuildRichText("Is File: ", "[color=yellow]", "[b]", formattedPath, $"[url={formattedPath}]"));
                    _dialog.CurrentPath = formattedPath;
                }
                else if (Directory.Exists(formattedPath))
                {
                    if (formattedPath.Last() != '\\')
                    {
                        formattedPath += '\\';
                    }
                    GD.PrintRich(GodotPrintHelper.BuildRichText("Is Directory: ", "[color=yellow]", "[b]", formattedPath, $"[url={startingPath}]"));
                    _dialog.CurrentPath = formattedPath;
                }
            }

            if (filters != null)
            {
                _dialog.Filters = filters;
            }
        }

        public FileDialogHelper(FileModeEnum fileMode,
                                AccessEnum fileAccess,
                                FileSelectedEventHandler fileSelectedCallback,
                                FilesSelectedEventHandler filesSelectedCallback,
                                DirSelectedEventHandler dirSelectedCallback,
                                string startingPath = "",
                                string[] filters = null) : this(fileMode, fileAccess, startingPath, filters)
        {
            #region Checks
            switch (fileMode)
            {
                case FileModeEnum.OpenAny:
                    if (fileSelectedCallback == null) { throw new ArgumentNullException("FileDialogHelper.fileSelectedCallback should not be null when FileModeEnum is set to OpenAny."); }
                    if (filesSelectedCallback == null) { throw new ArgumentNullException("FileDialogHelper.filesSelectedCallback should not be null when FileModeEnum is set to OpenAny."); }
                    if (dirSelectedCallback == null) { throw new ArgumentNullException("FileDialogHelper.dirSelectedCallback should not be null when FileModeEnum is set to OpenAny."); }
                    break;
                case FileModeEnum.OpenFile:
                    if (fileSelectedCallback == null)
                        throw new ArgumentNullException("FileDialogHelper.fileSelectedCallback should not be null when FileModeEnum is set to OpenFile.");
                    break;
                case FileModeEnum.OpenFiles:
                    if (filesSelectedCallback == null)
                        throw new ArgumentNullException("FileDialogHelper.filesSelectedCallback should not be null when FileModeEnum is set to OpenFiles.");
                    break;
                case FileModeEnum.OpenDir:
                    if (dirSelectedCallback == null)
                        throw new ArgumentNullException("FileDialogHelper.dirSelectedCallback should not be null when FileModeEnum is set to OpenDir.");
                    break;
                default:
                    break;
            }
            #endregion
            if (fileSelectedCallback != null) { _dialog.FileSelected += fileSelectedCallback; }
            if (filesSelectedCallback != null) { _dialog.FilesSelected += filesSelectedCallback; }
            if (dirSelectedCallback != null) { _dialog.DirSelected += dirSelectedCallback; }
            _dialog.VisibilityChanged += L_ExitDelegate;

            void L_ExitDelegate()
            {
                if (_dialog.Visible) { return; }
                if (fileSelectedCallback != null) { _dialog.FileSelected -= fileSelectedCallback; }
                if (filesSelectedCallback != null) { _dialog.FilesSelected -= filesSelectedCallback; }
                if (dirSelectedCallback != null) { _dialog.DirSelected -= dirSelectedCallback; }
                _dialog.VisibilityChanged -= L_ExitDelegate;
                _dialog.QueueFree();
            }
        }
    }
}
