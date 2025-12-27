using System.Collections.Generic;
using System.Linq;

namespace EVLibrary.FileIO
{
    public readonly struct FileExtension
    {
        public static readonly FileExtension GLTF = new("gltf");
        public static readonly FileExtension PNG = new("png");
        public static readonly FileExtension JPG = new("jpg");
        public static readonly FileExtension PYTHON = new("py");
        public static readonly FileExtension XML = new("xml");
        public static readonly FileExtension JSON = new("json");
        public static readonly FileExtension TXT = new("txt");
        public static readonly FileExtension ASTERISK = new("*");
        public static readonly FileExtension TSCN = new("tscn"); // Godot Scene
        public static readonly FileExtension TRES = new("tres"); // Godot Resource
        public static readonly FileExtension BLEND = new("blend"); // Blender File
        public static readonly FileExtension BLEND1 = new("blend1"); // Backup Blender File

        private readonly string _name;
        private FileExtension(string name) => _name = name;

        public static FileExtension[] GetExtensions(params FileExtension[] extensions)
        {
            return extensions;
        }

        public override string ToString() => _name;

        public static bool operator ==(FileExtension left, FileExtension right)
        {
            return left._name == right._name;
        }

        public static bool operator !=(FileExtension left, FileExtension right)
        {
            return left._name != right._name;
        }
    }

    public static class FileExtensionUtil
    {
        /// <summary>
        /// Returns a string containing ".extension"
        /// </summary>
        public static string AsFileExtension(this FileExtension ext)
        {
            return "." + ext;
        }

        /// <summary>
        /// Returns a string containing "*.extension"
        /// </summary>
        public static string AsGenericFilter(this FileExtension ext)
        {
            return "*." + ext;
        }

        public static string AsFilterString(params FileExtension[] ext)
        {
            string returnString = "";
            foreach (FileExtension extension in ext)
            {
                returnString += extension.AsGenericFilter();
                if (extension != ext.Last())
                {
                    returnString += ", ";
                }
            }
            return returnString;
        }

        public static string[] AsFilterArray(this FileExtension[] ext)
        {
            List<string> list = new();
            foreach (FileExtension extension in ext)
            {
                list.Add(extension.AsGenericFilter());
            }
            return list.ToArray();
        }
    }
}
