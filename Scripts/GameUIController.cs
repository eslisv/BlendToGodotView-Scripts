using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed partial class GameUIController : Node
    {
        public MultiSubViewportsUIController MultiSubViewportsController => _multiSubViewportsController;
        public ToolbarUIController ToolbarController => _toolbarController;

        [Export] private MultiSubViewportsUIController _multiSubViewportsController;
        [Export] private ToolbarUIController _toolbarController;
        [Export] private MultiSubViewportCamerasManager _multiSubViewportCamerasManager;

        public void Setup(ModelLoadingController modelLoadingController, Action<string> applyTextureCallback)
        {
            _multiSubViewportsController.Setup();
            _toolbarController.Setup(modelLoadingController, _multiSubViewportsController.Args, SaveCameraLayoutPreset, LoadCameraLayoutPreset, applyTextureCallback);
            _multiSubViewportCamerasManager.Setup(AddViewport, CloseViewport);
            LoadCameraLayoutPreset(SettingsController.Settings.CameraLayoutFilePath);
        }

        private void AddViewport()
        {
            _multiSubViewportsController.AddViewport();
        }

        private void CloseViewport(int viewportIndex)
        {
            _multiSubViewportsController.CloseViewport(viewportIndex);
        }

        private void SaveCameraLayoutPreset(string filePath)
        {
            _multiSubViewportCamerasManager.SavePreset(filePath);
        }

        private void LoadCameraLayoutPreset(string name)
        {
            _multiSubViewportCamerasManager.LoadPreset(name);
        }
    }
}