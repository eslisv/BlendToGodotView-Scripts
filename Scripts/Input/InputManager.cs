using Godot;

namespace AM.ModelViewerTool
{
    public partial class InputManager : Node
    {
        public static MenuInput MenuInput => _menuInput;
        public static DebugInput DebugInput => _debugInput;

        [Export] private InputDefinition _panLeft;
        [Export] private InputDefinition _panRight;
        [Export] private InputDefinition _panUp;
        [Export] private InputDefinition _panDown;
        [Export] private InputDefinition _spinLeft;
        [Export] private InputDefinition _spinRight;
        [Export] private InputDefinition _openViewportPopup;
        [Export] private InputDefinition _growWindow_Debug;
        [Export] private InputDefinition _shrinkWindow_Debug;

        private static MenuInput _menuInput;
        private static DebugInput _debugInput;

        public void Setup()
        {
            InputRuntime panLeft = new InputRuntime(_panLeft);
            InputRuntime panRight = new InputRuntime(_panRight);
            InputRuntime panUp = new InputRuntime(_panUp);
            InputRuntime panDown = new InputRuntime(_panDown);

            InputRuntime spinLeft = new InputRuntime(_spinLeft);
            InputRuntime spinRight = new InputRuntime(_spinRight);

            InputRuntime openViewportPopup = new InputRuntime(_openViewportPopup);

            InputRuntime growWindow_Debug = new InputRuntime(_growWindow_Debug);
            InputRuntime shrinkWindow_Debug = new InputRuntime(_shrinkWindow_Debug);

            _menuInput = new MenuInput(panLeft, panRight, panUp, panDown, spinLeft, spinRight, openViewportPopup);
            _debugInput = new DebugInput(growWindow_Debug, shrinkWindow_Debug);
        }

        public void Cleanup()
        {
            _menuInput = null;
        }
	}
}
