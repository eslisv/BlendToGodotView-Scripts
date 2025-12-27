using Godot;
using System;

namespace EVLibrary.Godot.IO
{
    public static class File
    {
        public static bool FileIsResourceType(string filePath, params Type[] types)
        {
            Resource resource = ResourceLoader.Load(filePath);
            Type resourceType = resource.GetType();
            foreach (Type type in types)
            {
                if (resourceType != type)
                {
                    continue;
                }
                return true;
            }
            return false;
        }
    }
}
