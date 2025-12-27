using Godot;

namespace AM.ModelViewerTool
{
    public sealed partial class SubViewportUIController : Control
    {
        public SubViewportContainer ViewportContainer => _viewportContainer;
        public MultiSubViewportCameraController CameraController => _cameraController;

        [Export] private SubViewportContainer _viewportContainer;
        [Export] private MultiSubViewportCameraController _cameraController;

        public void Setup()
        {
            _cameraController.Setup();
        }

        public void Cleanup()
        {
            _cameraController.Cleanup();
        }

        public bool TryReformat(Vector2 position, Vector2 size, int stretchShrink)
        {
            if (!_viewportContainer.Visible)
                return false;

            _viewportContainer.Position = position;
            _viewportContainer.Size = size;
            _viewportContainer.StretchShrink = stretchShrink;
            return true;
        }
    }
}