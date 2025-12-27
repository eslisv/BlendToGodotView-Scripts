using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed partial class MultiSubViewportCameraController : Node3D, IMultiSubViewportCameraController
    {
        private static readonly Vector3 _cameraUpDirection = Vector3.Forward;
        private static readonly Vector3 _cameraDownDirection = Vector3.Back;
        private static readonly Vector3 _cameraLeftDirection = Vector3.Left;
        private static readonly Vector3 _cameraRightDirection = Vector3.Right;

        public Node3D Node3D => this;
        public string CameraName => GetParent().GetParent().Name;
        public bool IsEnabled => _isEnabled;

        [Export] private float _cameraMoveSpeed = 0.5f;
        [Export] private float _cameraRotateTime = 0.25f;
        [Export(PropertyHint.Range, "0, 360")] private float _rotateIncrement = 45.0f;
        [Export] private Label _cameraControlEnabledLabel;

        private Tween _rotateTween;
        private bool _isEnabled;

        public override void _Process(double delta)
        {
            if (!_isEnabled) { return; }
            if(!GetViewport().GetWindow().HasFocus()) { return; }
            if(!Node3D.IsVisibleInTree()) { return; }

            MenuInput input = InputManager.MenuInput;

            TryRotateCameraFromInput(input);
            TryTranslateCameraFromInput(input);
        }

        public void Setup()
        {
            MultiSubViewportCamerasRegistration.RegisterCamera(this);
        }

        public void Cleanup()
        {
            MultiSubViewportCamerasRegistration.UnregisterCamera(this);

            if (_rotateTween != null)
                _rotateTween.Finished -= ClearTween;
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
            _cameraControlEnabledLabel.Visible = isEnabled;
        }

        private void TryTranslateCameraFromInput(MenuInput input)
        {
            TryTranslateCameraVerticalFromInput(input);
            TryTranslateCameraHorizontalFromInput(input);
        }

        private bool TryTranslateCameraVerticalFromInput(MenuInput input)
        {
            if (input == null) { return false; }
            if (input.PanUp.IsPressed())
            {
                TranslateCamera(_cameraUpDirection);
                return true;
            }
            if (input.PanDown.IsPressed())
            {
                TranslateCamera(_cameraDownDirection);
                return true;
            }
            return false;
        }

        private bool TryTranslateCameraHorizontalFromInput(MenuInput input)
        {
            if (input == null) { return false; }
            if (input.PanLeft.IsPressed())
            {
                TranslateCamera(_cameraLeftDirection);
                return true;
            }
            if (input.PanRight.IsPressed())
            {
                TranslateCamera(_cameraRightDirection);
                return true;
            }
            return false;
        }

        private bool TryRotateCameraFromInput(MenuInput input)
        {
            if (input == null) { return false; }
            if (IsRotating()) { return false; }
            if (input.SpinLeft.IsPressed())
            {
                RotateCamera(-_rotateIncrement);
                return true;
            }
            if (input.SpinRight.IsPressed())
            {
                RotateCamera(_rotateIncrement);
                return true;
            }
            return false;
        }

        private void TranslateCamera(Vector3 vel)
        {
            TranslateObjectLocal(vel * _cameraMoveSpeed);
        }

        private void RotateCamera(float degrees)
        {
            if (IsRotating()) { return; }

            _rotateTween = TweenUtil.TweenYRotation(parent: this, Mathf.DegToRad(degrees), _cameraRotateTime, isRelative: true);
            _rotateTween.Finished += ClearTween;
        }

        private void ClearTween()
        {
            _rotateTween.Finished -= ClearTween;
            _rotateTween = null;
        }

        private bool IsRotating()
        {
            return _rotateTween?.IsRunning() ?? false;
        }
    }
}