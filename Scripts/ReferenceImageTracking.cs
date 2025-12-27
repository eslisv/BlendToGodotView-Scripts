using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public static class ReferenceImageTracking
    {
        public static IReadOnlyList<string> ReferenceImagePaths => _referenceImagePaths.AsReadOnly();

        private static readonly List<string> _referenceImagePaths = new();

        public static void AddReferenceImagePath(string referenceImagePath)
        {
            // TODO: Make this saved in settings after implementing removal of reference images
            _referenceImagePaths.Add(referenceImagePath);
        }
    }
}