using EVLibrary.FileIO;
using EVLibrary.FileIO.Extensions;
using Godot;
using System.Collections.Generic;
using System.IO;
using SysFile = System.IO.File;

namespace AM.ModelViewerTool
{
    public sealed partial class ToolBar_Option_Models_PopupMenuUIController : Node
    {
        [Export] private CustomPopupMenuUIController _customPopupMenuUIController;

        private ModelLoadingController _modelLoadingController;

        public void Setup(ModelLoadingController modelLoadingController)
        {
            _modelLoadingController = modelLoadingController;
            RefreshArgs();
        }

        public void RefreshArgs()
        {
            IReadOnlyList<string> modelPaths = FileWatcherUtil.GetFilesFromWatcher(EWatcher.BASE_FOLDER);
            if (modelPaths.Count == 0)
            {
                GD.PrintErr("No models found");
            }

            List<CustomPopupMenu_SingleEntry_Args> popupMenuSingleArgs = new();
            for (int i = 0; i < modelPaths.Count; i++)
            {
                string modelName = modelPaths[i].GetBaseName();
                popupMenuSingleArgs.Add(new CustomPopupMenu_SingleEntry_Args(
                    label: modelName,
                    selectedCallback: () => GenerateModelWithName(modelName)
                ));
            }

            CustomPopupMenu_Args args = new(popupMenuSingleArgs);
            _customPopupMenuUIController.SetArgs(args);
        }

        private void GenerateModelWithName(string modelName)
        {
            // TODO: potentially later, if this script grows, move this function to something else like into the model loading controller
            _modelLoadingController.Reset();
            string gltfPath = modelName.PathJoin($"{Path.GetFileNameWithoutExtension(modelName)}.{GltfUtil.GLTF_FILE_EXTENSION_NAME}").StandardizeSlashesToBackslash();
            GD.Print($"GLTF PATH = {gltfPath}");
            if (!SysFile.Exists(gltfPath))
            {
                GltfUtil.GenerateGltfFromBlendFile(gltfPath);
            }
            SettingsController.Settings.GltfModelFilePath = gltfPath;
            _modelLoadingController.GenerateModelNode(gltfPath, false);
            FileWatcherUtil.SetPathOfWatcher(EWatcher.MODEL_FILE, modelName);
        }
    }
}