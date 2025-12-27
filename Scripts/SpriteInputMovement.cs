using Godot;

namespace AM.ModelViewerTool
{
    public partial class SpriteInputMovement : Sprite2D
    {
        private bool _inputEnabled = true;
        private bool _isDragging = false;
        private bool _shiftPressed = false;
        private Vector2 _offset;
        private Vector2 _originalPosition;

        public override void _Process(double delta)
        {
            if (!_inputEnabled) { return; }
            if (!_isDragging) { return; }
            Position = GetGlobalMousePosition() - _offset;
        }

        public override void _Input(InputEvent @event)
        {
            // TODO REPLACE STEAL INPUT HANDLING FROM ETERNAL WAR AND USE HERE
            if (!_inputEnabled) { return; }
            if (@event is InputEventWithModifiers modifierEvent)
            {
                _shiftPressed = modifierEvent.ShiftPressed;
            }
            if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
            {
                if (keyEvent.Keycode == Key.W) { Translate(Vector2.Up); }
                if (keyEvent.Keycode == Key.S) { Translate(Vector2.Down); }
                if (keyEvent.Keycode == Key.A) { Translate(Vector2.Left); }
                if (keyEvent.Keycode == Key.D) { Translate(Vector2.Right); }
            }
            if (@event is InputEventMouseButton mouseEvent)
            {
                if (mouseEvent.ButtonIndex != MouseButton.Left) { return; }
                if (mouseEvent.IsPressed() && GetRect().HasPoint(ToLocal(mouseEvent.Position)))
                {
                    _originalPosition = ToGlobal(GetRect().GetCenter());
                    _offset = ToLocal(mouseEvent.Position);
                    _isDragging = true;
                }
                if (mouseEvent.IsReleased())
                {
                    _isDragging = false;
                }
            }
        }

        public void SetInput(bool inputEnabled)
        {
            _inputEnabled = inputEnabled;
        }
    }
}