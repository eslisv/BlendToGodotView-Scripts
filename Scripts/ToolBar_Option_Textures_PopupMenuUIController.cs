using EVLibrary.FileIO;
using EVLibrary.Godot.IO;
using Godot;
using System;

namespace AM.ModelViewerTool
{
    public partial class ToolBar_Option_Textures_PopupMenuUIController : Node
    {
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _applyTextureArgs;

        [Export] private CustomPopupMenuUIController _customPopupMenuUIController;

        public Action<string> ApplyTextureCallback;

        public void Setup(Action<string> applyTextureCallback)
        {
            ApplyTextureCallback = applyTextureCallback;

            CustomPopupMenu_Args args = new(
                _applyTextureArgs.CreateArgs(selectedCallback: ApplyTexture)
            );

            _customPopupMenuUIController.SetArgs(args);
        }

        private void ApplyTexture()
        {
            FileDialogHelper fileDialogHelper = new(
                FileDialog.FileModeEnum.OpenFile,
                FileDialog.AccessEnum.Filesystem,
                L_ApplyTextureCallback,
                null,
                null,
                SettingsController.Settings.WatchingFolderPath,
                new string[] { FileExtensionUtil.AsFilterString(FileExtension.PNG, FileExtension.JPG) }
            );

            FileDialog fileDialog = fileDialogHelper.Dialog;
            AddChild(fileDialog);
            fileDialog.PopupCenteredRatio();

            void L_ApplyTextureCallback(string texturePath)
            {
                ApplyTextureCallback.Invoke(texturePath);
            }
        }
    }
}
