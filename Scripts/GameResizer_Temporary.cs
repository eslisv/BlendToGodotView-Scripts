using Godot;
// Original Authors - Wyatt Senalik

namespace AM.ModelViewerTool
{
    public sealed partial class GameResizer_Temporary : Node
    {
        private static readonly Vector2I SMALLEST_RESOLUTION = new Vector2I(640, 360);

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);

            if (@event is not InputEventKey keyEvent)
                return;
            if (!keyEvent.IsPressed())
                return;
            if (keyEvent.Keycode == Key.Equal)
            {
                TryGrowWindow();
                return;
            }
            if (keyEvent.Keycode == Key.Minus)
            {
                TryShrinkWindow();
                return;
            }
        }

        private void TryFixSize()
        {
            if (!ShouldFixSize(out int newScale))
                return;
            SetResolutionToScale(newScale);
        }

        private void TryGrowWindow()
        {
            Vector2I currentScale = GetCurrentScale();
            int scale = Mathf.Max(currentScale.X, currentScale.Y);
            int maxScale = GetMaxScale();
            if (scale >= maxScale)
            {
                GD.Print($"Cannot grow anymore");
                return;
            }

            int newScale = scale + 1;
            GD.Print($"Growing to {newScale}");
            SetResolutionToScale(newScale);
        }

        private void TryShrinkWindow()
        {
            Vector2I currentScale = GetCurrentScale();
            int scale = Mathf.Min(currentScale.X, currentScale.Y);
            if (scale <= 1)
            {
                GD.Print($"Cannot shrink anymore");
                return;
            }

            int newScale = scale - 1;
            GD.Print($"Shrinking to {newScale}");
            SetResolutionToScale(newScale);
        }

        private Vector2I GetCurrentScale()
        {
            Vector2I currentSize = DisplayServer.WindowGetSize();
            int scaleX = currentSize.X / SMALLEST_RESOLUTION.X;
            int scaleY = currentSize.Y / SMALLEST_RESOLUTION.Y;
            return new Vector2I(scaleX, scaleY);
        }

        private bool ShouldFixSize(out int desiredScale)
        {
            Vector2I currentSize = DisplayServer.WindowGetSize();
            Vector2I currentScale = GetCurrentScale();
            desiredScale = Mathf.Min(currentScale.X, currentScale.Y);
            if (currentScale.X * SMALLEST_RESOLUTION.X != currentSize.X)
                return true;
            if (currentScale.Y * SMALLEST_RESOLUTION.Y != currentSize.Y)
                return true;
            return false;
        }

        private void SetResolutionToScale(int scale)
        {
            Vector2I smallestResolution = SMALLEST_RESOLUTION;
            Vector2I newSize = scale * smallestResolution;
            bool fullscreen = scale == GetMaxScale();
            DisplayServer.WindowMode mode = fullscreen ? DisplayServer.WindowMode.ExclusiveFullscreen : DisplayServer.WindowMode.Windowed;

            Vector2I previousSize = DisplayServer.WindowGetSize();
            bool wasFullscreen = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.ExclusiveFullscreen;
            DisplayServer.WindowSetMode(mode);
            DisplayServer.WindowSetSize(newSize);

            if (fullscreen)
            {
                // Don't matter, its fullscreen.
                DisplayServer.WindowSetPosition(Vector2I.Zero);
                return;
            }
            if (wasFullscreen)
            {
                // If we were previously fullscreen, we need to recenter the window
                Vector2I screenSize = DisplayServer.ScreenGetSize();
                Vector2I emptySpace = screenSize - newSize;
                Vector2I recenteringPosition = emptySpace / 2;
                DisplayServer.WindowSetPosition(recenteringPosition);
                return;
            }
            Vector2I sizeDifference = newSize - previousSize;
            Vector2I previousPosition = DisplayServer.WindowGetPosition();
            Vector2I newPosition = previousPosition - sizeDifference / 2;
            GD.Print($"Previous={previousPosition}; New={newPosition}");
            DisplayServer.WindowSetPosition(newPosition);
            return;
        }

        private int GetMaxScale()
        {
            Vector2I monitorSize = DisplayServer.ScreenGetSize();

            int maxScaleX = monitorSize.X / SMALLEST_RESOLUTION.X;
            int maxScaleY = monitorSize.Y / SMALLEST_RESOLUTION.Y;
            return Mathf.Min(maxScaleX, maxScaleY);
        }
    }
}
