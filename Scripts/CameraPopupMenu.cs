using Godot;
using System;

public partial class CameraPopupMenu : PopupMenu
{
    [Export] private Node3D[] cameraNodes;

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            IdPressed -= OptionPressed;
        }
    }

    public override void _Ready()
    {
        IdPressed += OptionPressed;
    }

    private void OptionPressed(long id)
    {
        foreach (Node3D node in cameraNodes)
        {
            switch (id) // Reset position
            {
                case 0:
                    node.Position = Vector3.Zero;
                    node.GlobalPosition = Vector3.Zero;
                    break;
                case 1:
                    node.Rotation = Vector3.Zero;
                    node.GlobalRotation = Vector3.Zero;
                    break;
                case 2:
                    node.Position = Vector3.Zero;
                    node.GlobalPosition = Vector3.Zero;
                    node.Rotation = Vector3.Zero;
                    node.GlobalRotation = Vector3.Zero;
                    break;
                default:
                    break;
            }
        }
    }
}
