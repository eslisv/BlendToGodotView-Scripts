using Godot;
using System;

public partial class CameraController : Node3D
{
    private Tween rotateTween;

    [Export] private float cameraMoveSpeed = 0.5f;
    [Export] private float cameraRotateTime = 0.25f;

    public override void _Process(double delta)
    {
        if (!Input.IsAnythingPressed()) { return; }
        if (rotateTween?.IsRunning() ?? false) { return; }
        if (Input.IsActionPressed("CameraUp"))
        {
            TranslateCamera(Vector3.Forward);
        }
        if (Input.IsActionPressed("CameraDown"))
        {
            TranslateCamera(Vector3.Back);
        }
        if (Input.IsActionPressed("CameraLeft"))
        {
            TranslateCamera(Vector3.Left);
        }
        if (Input.IsActionPressed("CameraRight"))
        {
            TranslateCamera(Vector3.Right);
        }
        if (Input.IsActionPressed("CameraRotateLeft"))
        {
            RotateCamera(-45f);
        }
        if (Input.IsActionPressed("CameraRotateRight"))
        {
            RotateCamera(45f);
        }
    }

    public void RotateCamera(float degrees)
    {
        if (rotateTween?.IsRunning() ?? false) { return; }

        rotateTween = this.CreateTween();
        rotateTween.TweenProperty(this, "rotation:y", Mathf.DegToRad(degrees), cameraRotateTime).AsRelative();
        rotateTween.Finished += ClearTween;
    }

    public void TranslateCamera(Vector3 vel)
    {
        TranslateObjectLocal(vel * cameraMoveSpeed);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            if (rotateTween == null) { return; }
            rotateTween.Finished -= ClearTween;
        }
    }

    private void ClearTween()
    {
        rotateTween.Finished -= ClearTween;
        rotateTween = null;
    }
}
