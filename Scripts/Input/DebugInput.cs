
namespace AM.ModelViewerTool
{
    public sealed class DebugInput
    {
        public InputRuntime GrowWindow => _growWindow;
        public InputRuntime ShrinkWindow => _shrinkWindow;

        private InputRuntime _growWindow;
        private InputRuntime _shrinkWindow;

        public DebugInput(InputRuntime growWindow, InputRuntime shrinkWindow)
        {
            _growWindow = growWindow;
            _shrinkWindow = shrinkWindow;
        }
    }
}
