using EVLibrary.FileIO;
using EVLibrary.FileIO.Extensions;
using EVLibrary.Godot;
using Godot;

namespace AM.ModelViewerTool
{
    public static class PathReferences
    {
        public const string GODOT_USER_PATH_FOLDER = @"user://";
        public const string GODOT_PROJECT_PATH_FOLDER = @"res://";
        public const string RELATIVE_PROJECT_TO_MODEL_PATH_FOLDER = @"Model";
        public const string RELATIVE_PROJECT_TO_SCRIPTS_PATH_FOLDER = @"Scripts";
        public const string RELATIVE_PROJECT_TO_RESOURCES_PATH_FOLDER = @"Resources";
        public const string RELATIVE_PROJECT_TO_DEBUG_SETTINGS_PATH_FILE = @"SettingsData/DebugSettings.xml";
        public const string RELATIVE_SETTINGS_SAVE_TO_PATH_FILE = @$"SettingsData/settings.txt";
        public const string RELATIVE_CAMERA_LAYOUT_TO_PATH_FOLDER = @$"CameraLayout/";

        private const string BLENDER_EXE_NAME = "Blender";

        public static string BlenderDirectory => FileSearchHelper.FindExecutableInstallPath(BLENDER_EXE_NAME);

        // Some of these may break when exporting. If it does, try using OS.has_feature to detect if in editor (not guaranteed to work)?
        public static string GetAbsolutePathToUserPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_USER_PATH_FOLDER)}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToSettingsSavePathFile()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_USER_PATH_FOLDER)}{RELATIVE_SETTINGS_SAVE_TO_PATH_FILE}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToCameraLayoutPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_USER_PATH_FOLDER)}{RELATIVE_CAMERA_LAYOUT_TO_PATH_FOLDER}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToProjectPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_PROJECT_PATH_FOLDER)}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToModelsPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_PROJECT_PATH_FOLDER)}{RELATIVE_PROJECT_TO_MODEL_PATH_FOLDER}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToScriptsPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_PROJECT_PATH_FOLDER)}{RELATIVE_PROJECT_TO_SCRIPTS_PATH_FOLDER}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToResourcesPathFolder()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_PROJECT_PATH_FOLDER)}{RELATIVE_PROJECT_TO_RESOURCES_PATH_FOLDER}".StandardizeSlashesToBackslash();
        }

        public static string GetAbsolutePathToDebugSettingsPathFile()
        {
            return $"{ProjectSettings.GlobalizePath(GODOT_USER_PATH_FOLDER)}{RELATIVE_PROJECT_TO_DEBUG_SETTINGS_PATH_FILE}".StandardizeSlashesToBackslash();
        }

        public static string GetNonLocalPath(string path)
        {
            string absolutePath = path;
            if (path.StartsWith(GODOT_USER_PATH_FOLDER)
                || path.StartsWith(GODOT_PROJECT_PATH_FOLDER))
            {
                absolutePath = ProjectSettings.GlobalizePath(path);
            }

            GD.PrintRich(GodotPrintHelper.BuildRichText("Loading file: ", absolutePath, "[color=red]", "[u]", $"[url={absolutePath}]"));
            return absolutePath;
        }
    }
}