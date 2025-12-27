using EVLibrary.FileIO;
using EVLibrary.FileIO.Extensions;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EVLibrary.Godot
{
    public static class GodotResourceHelper
    {
        public static string GetScriptName(GodotObject obj)
        {
            if (obj is not Node) { return null; }
            return GetScriptName((Node)obj);
            //Array<Dictionary> properties = obj.GetPropertyList();
            //string scriptName = "";
            //for (int i = properties.Count - 1; i >= 0; --i)
            //{
            //    if (!properties[i].TryGetValue("name", out Variant value))
            //    {
            //        continue;
            //    }
            //    string strValue = value.AsString();
            //    if (strValue != "script")
            //    {
            //        continue;
            //    }
            //    if (i + 1 >= properties.Count)
            //    {
            //        return string.Empty;
            //    }
            //    properties[i + 1].TryGetValue("name", out Variant scriptNameVariantValue);
            //    scriptName = scriptNameVariantValue.AsString();
            //    break;
            //}
            //return scriptName;
        }

        public static string GetScriptName(Node node)
        {
            if (node == null) { return null; }
            Script nodeScript = node.GetScript().As<Script>();
            if (nodeScript == null) { return null; }
            string nodeScriptPath = nodeScript.ResourcePath;
            if (nodeScriptPath == null || nodeScriptPath == string.Empty) { return null; }
            string nodeScriptName = Path.GetFileNameWithoutExtension(nodeScriptPath);
            return nodeScriptName;
        }

        public static IDictionary<string, object> GetResourcePathsOfResourceContainingProperty(string basePath, string propertyName)
        {
            if (string.IsNullOrEmpty(basePath)) { return null; }
            if (string.IsNullOrEmpty(propertyName)) { return null; }

            if (basePath.StartsWith("res://"))
            {
                basePath = ProjectSettings.GlobalizePath(basePath);
            }
            IReadOnlyCollection<string> files = FileSearchHelper.SearchPathForFilesWithExtension(
                basePath,
                FileExtension.TRES.AsGenericFilter(),
                SearchOption.AllDirectories
            );

            GD.Print(basePath);
            GD.Print(FileExtension.TRES.AsGenericFilter());

            IDictionary<string, object> filePaths = new Dictionary<string, object>();
            foreach (string file in files)
            {
                string localFilePath = ProjectSettings.LocalizePath(file);
                Resource res = ResourceLoader.Load(file);
                IReadOnlyList<PropertyInfo> properties = Properties.GetPropertiesRecursive(res, customNestedTypes: typeof(Resource));
                filePaths.Add(localFilePath, false);
                foreach (PropertyInfo propInfo in properties)
                {
                    if (propInfo.Name != propertyName) { continue; }
                    filePaths[localFilePath] = true;
                }
            }
            return filePaths;
        }
    }
}
