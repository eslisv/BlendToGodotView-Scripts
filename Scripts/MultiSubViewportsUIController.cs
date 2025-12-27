using EVLibrary.Godot.Extensions;
using Godot;
using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed partial class MultiSubViewportsUIController : Control
    {
        private const string BORDER_LINE_COLOR_HEX_CODE = "323232c8";

        public IReadOnlyMultiSubViewportsUIController_Args Args => _args;

        [Export] private PackedScene _subViewportControllerPrefab;
        [Export] private SubViewportRightClickMenuUIController _rightClickMenuUIController;

        private readonly MultiSubViewportsUIController_Args _args = new();
        private readonly List<int> _viewportIndexHolder = new();
        private MultiSubViewportsUIController_Formatter _subViewportsFormatter;
        private MultiSubViewportsUIController_PopupController _popupController;

        private bool _isFocusing;

        public override void _Process(double delta)
        {
            base._Process(delta);

            MenuInput input = InputManager.MenuInput;

            TryOpenPopupMenu(input);
        }

        public void Setup()
        {
            _rightClickMenuUIController.Setup(
                addViewportCallback: PopupMenu_OnAddViewport,
                toggleFocusViewportCallback: PopupMenu_OnToggleFocusViewport,
                closeViewportCallback: PopupMenu_OnCloseViewport,
                toggleCameraControlCallback: PopupMenu_ToggleCameraControl
            );

            _args.SubViewportControllers = this.GetAllChildrenInNodeOfType<SubViewportUIController>();
            foreach (SubViewportUIController subViewportController in _args.SubViewportControllers)
            {
                subViewportController.Setup();
            }

            _subViewportsFormatter = new MultiSubViewportsUIController_Formatter(_args, this, new Color(BORDER_LINE_COLOR_HEX_CODE));
            _popupController = new MultiSubViewportsUIController_PopupController(_args, _rightClickMenuUIController);
            FormatView();
        }

        private void PopupMenu_OnAddViewport()
        {
            AddViewport();
        }

        private void PopupMenu_OnToggleFocusViewport(int viewportIndex)
        {
            ToggleFocusViewport(viewportIndex);
        }

        private void PopupMenu_OnCloseViewport(int viewportIndex)
        {
            CloseViewport(viewportIndex);
        }

        private void PopupMenu_ToggleCameraControl(int viewportIndex)
        {
            ToggleCameraControl(viewportIndex);
        }

        public void AddViewport()
        {
            SubViewportUIController subViewportControllerInstance = _subViewportControllerPrefab.Instantiate<SubViewportUIController>();
            AddChild(subViewportControllerInstance);
            subViewportControllerInstance.Setup();
            _args.SubViewportControllers.Add(subViewportControllerInstance);
            FormatView();
        }

        private void ToggleFocusViewport(int focusViewportIndex)
        {
            if (_isFocusing)
            {
                UnfocusViewport();
                return;
            }
            FocusViewport(focusViewportIndex);
        }

        private void FocusViewport(int viewportIndex)
        {
            int activeViewportCount = _args.DetermineActiveViewportCount();
            if (activeViewportCount <= 1) { return; }

            for (int i = 0, j = 0; i < _args.SubViewportControllers.Count; i++)
            {
                SubViewportUIController subViewportController = _args.SubViewportControllers[i];
                SubViewportContainer viewportContainer = subViewportController.ViewportContainer;
                if (!viewportContainer.Visible) { continue; }

                if (j != viewportIndex)
                {
                    viewportContainer.Hide();
                    _viewportIndexHolder.Add(i);
                }
                else
                {
                    _args.FocusedSubViewport = subViewportController;
                }
                j++;
            }
            FormatView();

            _isFocusing = true;
        }

        private void UnfocusViewport()
        {
            foreach (int index in _viewportIndexHolder)
            {
                _args.SubViewportControllers[index].ViewportContainer.Show();
            }
            _args.FocusedSubViewport = null;
            _viewportIndexHolder.Clear();
            FormatView();

            _isFocusing = false;
        }

        public void CloseViewport(int viewportIndex)
        {
            if (_args.SubViewportControllers.Count == 1) { return; }
            SubViewportUIController subViewportToClose = _args.SubViewportControllers[viewportIndex];
            if (subViewportToClose.CameraController == _args.EnabledCameraController)
            {
                _args.EnabledCameraController = null;
            }
            subViewportToClose.Cleanup();
            subViewportToClose.QueueFree();
            _args.SubViewportControllers.RemoveAt(viewportIndex);
            FormatView();
        }

        private void ToggleCameraControl(int viewportIndex)
        {
            MultiSubViewportCameraController camera = _args.SubViewportControllers[viewportIndex].CameraController;
            if (camera != _args.EnabledCameraController)
            {
                GD.Print("Toggling camera");
                _args.EnabledCameraController?.SetIsEnabled(false);
                camera.SetIsEnabled(true);
                _args.EnabledCameraController = camera;
                return;
            }
            GD.Print("Disabling camera");
            _args.EnabledCameraController?.SetIsEnabled(false);
            _args.EnabledCameraController = null;
        }

        private void FormatView()
        {
            FormatSubViewportsContext context = CreateFormatSubViewportsContext();
            _subViewportsFormatter.Refresh(context);
        }

        private void TryOpenPopupMenu(MenuInput input)
        {
            if (!input.OpenViewportPopup.IsJustPressed()) { return; }
            
            Vector2 mouseLocalPosition = GetLocalMousePosition();
            int selectedViewportIndex = GetViewportIndexFromMousePosition(mouseLocalPosition);
            if (selectedViewportIndex < 0) { return; }

            SubViewportUIController selectedViewportController = null;
            if (selectedViewportIndex < _args.SubViewportControllers.Count)
            {
                selectedViewportController = _args.SubViewportControllers[selectedViewportIndex];
            }
            _popupController.TryOpenPopup(mouseLocalPosition, selectedViewportController, selectedViewportIndex);
        }

        private int GetViewportIndexFromMousePosition(Vector2 localMousePosition)
        {
            FormatSubViewportsContext context = CreateFormatSubViewportsContext();
            int activeViewportCount = context.ActiveViewportCount;
            Vector2 screenSize = context.ScreenSize;
            Vector2I gridDimensions = context.GridDimensions;
            if (activeViewportCount == 2)
            {
                if (localMousePosition.X <= screenSize.X * 0.5f)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            float subViewportSizeX = screenSize.X / gridDimensions.X;
            float subViewportSizeY = screenSize.Y / gridDimensions.Y;

            int viewportPosX = Mathf.FloorToInt(localMousePosition.X / subViewportSizeX);
            int viewportPosY = Mathf.FloorToInt(localMousePosition.Y / subViewportSizeY);

            return viewportPosX + viewportPosY * gridDimensions.X;
        }

        private FormatSubViewportsContext CreateFormatSubViewportsContext()
        {
            int activeViewportCount = _args.DetermineActiveViewportCount();
            Vector2I gridDimensions = DetermineGridDimensions(activeViewportCount);
            Vector2 screenSize = GetViewportRect().Size;
            Vector2 viewportSize = new Vector2(screenSize.X / gridDimensions.X, screenSize.Y / gridDimensions.Y);
            return new FormatSubViewportsContext(activeViewportCount, gridDimensions, screenSize, viewportSize);
        }

        private static Vector2I DetermineGridDimensions(int activeViewportCount)
        {
            switch (activeViewportCount)
            {
                case 1: return Vector2I.One;
                case 2: return new Vector2I(2, 1);
                default:
                    int smallestSquare = Mathf.CeilToInt(Mathf.Sqrt(activeViewportCount));
                    return new Vector2I(smallestSquare, smallestSquare);
            }
        }
    }
}