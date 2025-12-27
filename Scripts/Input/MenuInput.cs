
namespace AM.ModelViewerTool
{
	public sealed class MenuInput
	{
		public InputRuntime PanLeft => _panLeft;
		public InputRuntime PanRight => _panRight;
		public InputRuntime PanUp => _panUp;
		public InputRuntime PanDown => _panDown;
        public InputRuntime SpinLeft => _spinLeft;
        public InputRuntime SpinRight => _spinRight;
        public InputRuntime OpenViewportPopup => _openViewportPopup;

        private InputRuntime _panLeft;
		private InputRuntime _panRight;
		private InputRuntime _panUp;
		private InputRuntime _panDown;
        private InputRuntime _spinLeft;
        private InputRuntime _spinRight;
        private InputRuntime _openViewportPopup;

        public MenuInput(InputRuntime panLeft, InputRuntime panRight, InputRuntime panUp, InputRuntime panDown, InputRuntime spinLeft, InputRuntime spinRight, InputRuntime openViewportPopup)
		{
			_panLeft = panLeft;
			_panRight = panRight;
			_panUp = panUp;
			_panDown = panDown;
            _spinLeft = spinLeft;
            _spinRight = spinRight;
			_openViewportPopup = openViewportPopup;
        }
	}
}
