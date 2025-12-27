using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public static class MultiSubViewportCamerasRegistration
    {
        public static IEnumerable<IMultiSubViewportCameraController> Cameras => _cameras;

        private static readonly HashSet<IMultiSubViewportCameraController> _cameras = new();

        public static void RegisterCamera(IMultiSubViewportCameraController camera)
        {
            _cameras.Add(camera);
        }

        public static void UnregisterCamera(IMultiSubViewportCameraController camera)
        {
            _cameras.Remove(camera);
        }
    }
}