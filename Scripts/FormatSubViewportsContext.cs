using Godot;

namespace AM.ModelViewerTool
{
    public sealed class FormatSubViewportsContext
    {
        public int ActiveViewportCount => _activeViewportCount;
        public Vector2I GridDimensions => _gridDimensions;
        public Vector2 ScreenSize => _screenSize;
        public Vector2 ViewportSize => _viewportSize;

        private readonly int _activeViewportCount;
        private readonly Vector2I _gridDimensions;
        private readonly Vector2 _screenSize;
        private readonly Vector2 _viewportSize;

        public FormatSubViewportsContext(int activeViewportCount, Vector2I gridDimensions, Vector2 screenSize, Vector2 viewportSize)
        {
            _activeViewportCount = activeViewportCount;
            _gridDimensions = gridDimensions;
            _screenSize = screenSize;
            _viewportSize = viewportSize;
        }
    }
}