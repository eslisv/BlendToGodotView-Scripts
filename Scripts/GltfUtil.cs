using EVLibrary.FileIO.Extensions;
using EVLibrary.Windows;
using Godot;
using System.IO;

namespace AM.ModelViewerTool
{
    public static class GltfUtil
    {
        public const string GLTF_FILE_EXTENSION_NAME = "gltf";

        public static void GenerateGltfFromBlendFile(string exportFilePath)
        {
            string generatedExportPath = $@"{exportFilePath.GetBaseDir()}\{Path.GetFileNameWithoutExtension(exportFilePath)}";
            Directory.CreateDirectory(generatedExportPath.GetBaseDir());
            GD.PrintErr($@"SET PATH TO {generatedExportPath.GetBaseDir()}");
            GD.PrintErr($@"LOADING FILE {generatedExportPath.GetBaseDir()}.blend");
            GD.PrintErr($@"CREATING FILE {exportFilePath}");

            // Don't change the formatting on the line below. It needs to stay like that or it'll error out.
            string pyScript = @$"import bpy
bpy.ops.export_scene.{GLTF_FILE_EXTENSION_NAME}(
    filepath = r""{exportFilePath}"",
    check_existing = False,
    export_unused_textures = True,
    export_format = 'GLTF_SEPARATE',
    use_visible = True
)";

            // Make the BlenderImporterViewer temp folder to hold the python script for exporting the gltf model.
            string pyFileDirectory = Path.GetTempPath() + @"BlenderImporterViewer";
            Directory.CreateDirectory(pyFileDirectory);
            File.WriteAllText(@$"{pyFileDirectory}\exportScript.py", pyScript);

            // Make the cmd command to run the generated python script.
            string blenderExportCommand = @$"blender -b ""{generatedExportPath.GetBaseDir()}.blend"" --python ""{pyFileDirectory}/exportScript.py""".StandardizeSlashesToBackslash();
            GD.Print("Generating GLTF file...");
            GD.Print($"{blenderExportCommand}");

            WindowsCmdHelper.RunCommandInBackground(blenderExportCommand, PathReferences.BlenderDirectory, out string output, out string error);
            GD.Print(output);
            GD.Print(error);
        }
    }
}