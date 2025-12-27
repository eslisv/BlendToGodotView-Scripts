using Godot;

namespace AM.ModelViewerTool
{
    public sealed class InputRuntime
    {
        private readonly InputDefinition _definition;
        private InputEvent _keyboardInput;
        private InputEvent _controllerInput;

        public InputRuntime(InputDefinition definition)
        {
            _definition = definition;
            _keyboardInput = _definition.KeyboardInput;
            _controllerInput = _definition.ControllerInput;

            StringName actionName = _definition.Name;
            InputMap.AddAction(actionName, _definition.Deadzone);
            InputMap.ActionAddEvent(actionName, _keyboardInput);
            InputMap.ActionAddEvent(actionName, _controllerInput);
        }

        public bool IsPressed() => Input.IsActionPressed(_definition.Name);

        public bool IsJustPressed() => Input.IsActionJustPressed(_definition.Name);

        public bool IsJustReleased() => Input.IsActionJustReleased(_definition.Name);

        public void OverrideKeyboardInput(InputEvent keyboardInput)
        {
            InputMap.ActionEraseEvent(_definition.Name, _keyboardInput);
            _keyboardInput = keyboardInput;
            InputMap.ActionAddEvent(_definition.Name, _keyboardInput);
        }

        public void OverrideControllerInput(InputEvent controllerInput)
        {
            InputMap.ActionEraseEvent(_definition.Name, _controllerInput);
            _controllerInput = controllerInput;
            InputMap.ActionAddEvent(_definition.Name, _controllerInput);
        }

        public void ResetKeyboardInputToDefault()
        {
            OverrideKeyboardInput(_definition.KeyboardInput);
        }

        public void ResetControllerInputToDefault()
        {
            OverrideControllerInput(_definition.ControllerInput);
        }

        public void ResetAllToDefault()
        {
            ResetKeyboardInputToDefault();
            ResetControllerInputToDefault();
        }

        public override string ToString()
        {
            return $"({_definition.Name})";
        }
    }
}