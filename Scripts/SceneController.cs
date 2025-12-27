using AM.ModelViewerTool.Rendering;
using Godot;

namespace AM.ModelViewerTool
{
    public sealed partial class SceneController : Node
    {
        [Export] private ModelLoadingController _modelLoadingController;
        [Export] private GameUIController _gameUiController;
        [Export] private InputManager _inputManager;

        [Export] private ShaderMaterial _sharedSolidMaterial;
        [Export] private ShaderMaterial _genericToonMaterial;

        public override void _Ready()
        {
            base._Ready();

            // Not using _EnterTree because we need children to exist and this should only ExitTree when destroyed.
            RenderUtil.Setup(_sharedSolidMaterial, _genericToonMaterial);
            SettingsController.Setup();
            _modelLoadingController.Setup();
            _gameUiController.Setup(_modelLoadingController, ApplyTextureToModel);
            _inputManager.Setup();
            
        }

        private void ApplyTextureToModel(string texturePath)
        {
            _modelLoadingController.ApplyTexture(texturePath);
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            _modelLoadingController.Cleanup();
        }

        public override void _Notification(int notificationId)
        {
            base._Notification(notificationId);

            switch ((long)notificationId)
            {
                case NotificationWMCloseRequest:
                    Notification_Close();
                    break;
                default:
                    break;
            }
        }

        private void Notification_Close()
        {
            GD.Print("Bye");
            // Do anything else that needs to be done before closing here.
            SettingsController.SaveSettingsData(PathReferences.GetAbsolutePathToSettingsSavePathFile());
        }
    }
}