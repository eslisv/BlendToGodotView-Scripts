using EVLibrary.FileIO.Extensions;
using Godot;

namespace AM.ModelViewerTool
{
    public sealed class SettingsData
    {
        private const string EDITOR_FEATURE_NAME = "editor";

        public string WatchingFolderPath
        { 
            get
            {
                return _watchingFolderPath;
            }
            set
            {
                _watchingFolderPath = value;
            }
        }
        public string GltfModelFilePath
        {
            get
            {
                return _gltfModelFilePath;
            }
            set
            {
                _gltfModelFilePath = value;
            }
        }
        public string CameraLayoutFilePath
        {
            get
            {
                return _cameraLayoutFilePath;
            }
            set
            {
                _cameraLayoutFilePath = value;
            }
        }
        public bool UseNativeFileDialog
        {
            get
            {
                return _useNativeFileDialog;
            }
            set
            {
                _useNativeFileDialog = value;
            }
        }

        private string _watchingFolderPath;
        private string _gltfModelFilePath;
        private string _cameraLayoutFilePath;
        private bool _useNativeFileDialog;

        public SettingsData(string watchingFolderPath, string gltfModelFilePath, string cameraLayoutFilePath, bool useNativeFileDialog)
        {
            _watchingFolderPath = watchingFolderPath;
            _gltfModelFilePath = gltfModelFilePath;
            _cameraLayoutFilePath = cameraLayoutFilePath;
            _useNativeFileDialog = useNativeFileDialog;
        }

        public static SettingsData CreateDefault()
        {
            string watchingFolderDefault;
            // This may not work even after exporting. Refer to -> https://forum.godotengine.org/t/84105
            if (OS.HasFeature(EDITOR_FEATURE_NAME))
            {
                watchingFolderDefault = PathReferences.GetAbsolutePathToModelsPathFolder();
            }
            else
            {
                watchingFolderDefault = OS.GetExecutablePath();
            }
            return new SettingsData(watchingFolderDefault.StandardizeSlashesToBackslash(), string.Empty, string.Empty, false);
        }
    }
}