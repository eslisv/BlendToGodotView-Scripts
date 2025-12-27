using EVLibrary.FileIO;
using EVLibrary.Godot.IO;
using Godot;
using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed partial class ToolBar_Option_ReferenceImage_PopupMenuUIController : Node
    {
        private const string REGISTER_REFERENCE_IMAGE_LABEL = "Add Reference Image";

        [Export] private CustomPopupMenuUIController _customPopupMenuUIController;

        private List<CustomPopupMenu_SingleEntry_Args> _singleArgs = new();

        public void Setup()
        {
            _singleArgs.Add(new CustomPopupMenu_SingleEntry_Args(
                label: REGISTER_REFERENCE_IMAGE_LABEL,
                selectedCallback: RegisterReferenceImage)
            );
            RefreshArgs();
        }

        public void RefreshArgs()
        {
            CustomPopupMenu_Args args = new(_singleArgs);
            _customPopupMenuUIController.SetArgs(args);
        }

        private void RegisterReferenceImage()
        {
            FileDialogHelper fileDialogHelper = new(
                FileDialog.FileModeEnum.OpenFile,
                FileDialog.AccessEnum.Filesystem,
                FileSelected,
                null,
                null,
                filters: new string[] { FileExtensionUtil.AsFilterString(FileExtension.PNG, FileExtension.JPG) }
            );

            FileDialog fileDialog = fileDialogHelper.Dialog;
            AddChild(fileDialog);
            fileDialog.PopupCenteredRatio();
        }

        private void FileSelected(string path)
        {
            _singleArgs.Add(new CustomPopupMenu_SingleEntry_Args(
                label: path.GetFile(),
                selectedCallback: null,
                isDisabled: true)
            );
            RefreshArgs();
            ReferenceImageTracking.AddReferenceImagePath(path);
        }
    }
}