using Godot;

namespace AM.ModelViewerTool
{
	[GlobalClass]
	public partial class InputDefinition : Resource
	{
		public StringName Name => _name;
		public float Deadzone => _deadzone;
		public InputEvent KeyboardInput => _keyboardInput;
		public InputEvent ControllerInput => _controllerInput;

		[Export] private StringName _name;
		[Export] private float _deadzone = 0.5f;
		[Export] private InputEvent _keyboardInput;
		[Export] private InputEvent _controllerInput;
	}
}
