using EVLibrary.Godot;

namespace AM.ModelViewerTool
{
    public interface IMultiSubViewportCameraController : IHaveNode3D
    {
        string CameraName { get; }
    }
}