using Godot;
using System.Collections.Generic;
using System.Linq;

namespace AM.ModelViewerTool
{
    public interface IReadOnlyMultiSubViewportsUIController_Args
    {
        IReadOnlyList<SubViewportUIController> GetSubViewportControllers();
        SubViewportUIController FocusedSubViewport { get; }
        MultiSubViewportCameraController EnabledCameraController { get; }
    }

    public sealed class MultiSubViewportsUIController_Args : IReadOnlyMultiSubViewportsUIController_Args
    {
        public List<SubViewportUIController> SubViewportControllers { get; set; }
        public SubViewportUIController FocusedSubViewport { get; set; }
        public MultiSubViewportCameraController EnabledCameraController { get; set; }

        public IReadOnlyList<SubViewportUIController> GetSubViewportControllers()
        {
            return SubViewportControllers.AsReadOnly();
        }
    }

    public static class MultiSubViewportsUIController_ArgsExtensions
    {
        public static int DetermineActiveViewportCount(this MultiSubViewportsUIController_Args args)
        {
            return args.SubViewportControllers.Count(viewportController => viewportController.ViewportContainer.Visible);
        }
    }
}