using Godot;
using System.IO;

namespace AM.ModelViewerTool
{
    public sealed partial class ModelLoadingController : Node
    {
        [Export] private PackedScene _generatedModelPrefab;

        private GltfLoader _modelLoader;
        private GeneratedModelRenderer _modelRenderer;

        public void Setup()
        {
            SettingsData settingsData = SettingsController.Settings;
            _modelLoader = new GltfLoader(settingsData.WatchingFolderPath, settingsData.WatchingFolderPath, OnOpenFileChangedCallback);

            _modelRenderer = _generatedModelPrefab.Instantiate<GeneratedModelRenderer>();
            AddChild(_modelRenderer);

            if (!Directory.Exists(settingsData.WatchingFolderPath)) { return; }
            if (!File.Exists(settingsData.GltfModelFilePath)) { return; }
            GenerateModelNode(settingsData.GltfModelFilePath, false);
        }

        public void Cleanup()
        {
            _modelLoader.Cleanup();
        }

        public void GenerateModelNode(string modelPath, bool isOutsideTree)
        {
            Reset(isOutsideTree);
            _modelLoader.LoadFile(modelPath);
            Node3D modelNode = _modelLoader.GenerateNode();
            _modelRenderer.SetModel(modelNode, isOutsideTree);
        }

        public void Reset(bool shouldResetLoader = true)
        {
            if (shouldResetLoader)
            {
                _modelLoader.Reset();
            }
            _modelRenderer.DestroyModelNode();
        }

        private void OnOpenFileChangedCallback(string pathToChangedFile)
        {
            GenerateModelNode(pathToChangedFile, true);
        }

        public void ApplyTexture(string texturePath)
        {
            _modelRenderer.SetModelTexture(texturePath);
        }
    }
}