using EVLibrary.FileIO;
using EVLibrary.Godot.IO;
using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed partial class ToolBar_Option_Camera_PopupMenuUIController : Node
    {
        private enum ECameraPopupOptions
        {
            ResetPosition,
            ResetRotation,
            ResetTransform,
            SavePreset,
            LoadPreset
        }

        [Export] private CustomPopupMenu_SingleEntry_Args_Resource[] _singleEntryArgs;
        [Export] private CustomPopupMenuUIController _customPopupMenuUIController;

        private IReadOnlyMultiSubViewportsUIController_Args _multiSubViewportArgs;
        private CustomPopupMenu_Args _args;

        public Action<string> SaveCameraLayoutCallback;
        public Action<string> LoadCameraLayoutCallback;

        public void Setup(IReadOnlyMultiSubViewportsUIController_Args multiSubViewportArgs, Action<string> saveCameraLayoutCallback, Action<string> loadCameraLayoutCallback)
        {
            _multiSubViewportArgs = multiSubViewportArgs;
            SaveCameraLayoutCallback = saveCameraLayoutCallback;
            LoadCameraLayoutCallback = loadCameraLayoutCallback;

            _args = new CustomPopupMenu_Args(
                _singleEntryArgs[(int)ECameraPopupOptions.ResetPosition].CreateArgs(L_ResetPosition),
                _singleEntryArgs[(int)ECameraPopupOptions.ResetRotation].CreateArgs(L_ResetRotation),
                _singleEntryArgs[(int)ECameraPopupOptions.ResetTransform].CreateArgs(L_ResetTransform),
                _singleEntryArgs[(int)ECameraPopupOptions.SavePreset].CreateArgs(SavePreset),
                _singleEntryArgs[(int)ECameraPopupOptions.LoadPreset].CreateArgs(LoadPreset)
            );
            _customPopupMenuUIController.SetArgs(_args);

            void L_ResetPosition()
            {
                ResetPosition();
            }

            void L_ResetRotation()
            {
                ResetRotation();
            }

            void L_ResetTransform()
            {
                ResetTransform();
            }
        }

        private void ResetPosition()
        {
            _multiSubViewportArgs.EnabledCameraController.GlobalPosition = Vector3.Zero;
        }

        private void ResetRotation()
        {
            _multiSubViewportArgs.EnabledCameraController.GlobalRotation = Vector3.Zero;
        }

        private void ResetTransform()
        {
            ResetPosition();
            ResetRotation();
        }

        private void SavePreset()
        {
            FileDialogHelper fileDialogHelper = new(
                FileDialog.FileModeEnum.SaveFile,
                FileDialog.AccessEnum.Userdata,
                L_SavePresetFile,
                null,
                null,
                PathReferences.GetAbsolutePathToCameraLayoutPathFolder(),
                new string[] { FileExtensionUtil.AsFilterString(FileExtension.XML) }
            );

            FileDialog fileDialog = fileDialogHelper.Dialog;
            AddChild(fileDialog);
            fileDialog.PopupCenteredRatio();

            void L_SavePresetFile(string filePath)
            {
                string absolutePath = PathReferences.GetNonLocalPath(filePath);
                SaveCameraLayoutCallback.Invoke(absolutePath);
            }
        }

        private void LoadPreset()
        {
            FileDialogHelper fileDialogHelper = new(
                FileDialog.FileModeEnum.OpenFile,
                FileDialog.AccessEnum.Userdata,
                L_LoadPresetFile,
                null,
                null,
                PathReferences.GetAbsolutePathToCameraLayoutPathFolder(),
                new string[] { FileExtensionUtil.AsFilterString(FileExtension.JSON, FileExtension.XML, FileExtension.TXT) }
            );

            FileDialog fileDialog = fileDialogHelper.Dialog;
            AddChild(fileDialog);
            fileDialog.PopupCenteredRatio();

            void L_LoadPresetFile(string filePath)
            {
                string absolutePath = PathReferences.GetNonLocalPath(filePath);
                LoadCameraLayoutCallback.Invoke(absolutePath);
            }
        }
    }
}