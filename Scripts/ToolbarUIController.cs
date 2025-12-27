using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed partial class ToolbarUIController : Control
    {
        public ToolBar_Option_File_PopupMenuUIController FilePopupMenu => _filePopupMenu;
        public ToolBar_Option_Models_PopupMenuUIController ModelsPopupMenu => _modelsPopupMenu;
        public ToolBar_Option_Camera_PopupMenuUIController CameraPopupMenu => _cameraPopupMenu;
        public ToolBar_Option_ReferenceImage_PopupMenuUIController ReferenceImagePopupController => _referenceImagePopupController;
        public ToolBar_Option_Textures_PopupMenuUIController TexturesPopupMenu => _texturesPopupMenu;

        [Export] private ToolBar_Option_File_PopupMenuUIController _filePopupMenu;
        [Export] private ToolBar_Option_Models_PopupMenuUIController _modelsPopupMenu;
        [Export] private ToolBar_Option_Camera_PopupMenuUIController _cameraPopupMenu;
        [Export] private ToolBar_Option_ReferenceImage_PopupMenuUIController _referenceImagePopupController;
        [Export] private ToolBar_Option_Textures_PopupMenuUIController _texturesPopupMenu;

        public void Setup(ModelLoadingController modelLoadingController, IReadOnlyMultiSubViewportsUIController_Args multiSubViewportArgs, Action<string> saveCameraLayoutCallback, Action<string> loadCameraLayoutCallback, Action<string> applyTextureCallback)
        {
            _filePopupMenu.Setup(modelLoadingController, ModelRefreshArgs);
            _modelsPopupMenu.Setup(modelLoadingController);
            _cameraPopupMenu.Setup(multiSubViewportArgs, saveCameraLayoutCallback, loadCameraLayoutCallback);
            _referenceImagePopupController.Setup();
            _texturesPopupMenu.Setup(applyTextureCallback);
        }

        private void ModelRefreshArgs()
        {
            _modelsPopupMenu.RefreshArgs();
        }
    }
}