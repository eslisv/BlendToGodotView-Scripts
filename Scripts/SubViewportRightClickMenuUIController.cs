using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed class SubViewportRightClickMenuUIController_ActivateOptions
    {
        public bool ShouldShowAddNew => _shouldShowAddNew;
        public bool ShouldShowClose => _shouldShowClose;
        public bool ShouldShowFocus => _shouldShowFocus;
        public bool ShouldShowToggleCameraControl => _shouldShowToggleCameraControl;
        public bool IsCameraControlEnabled => _isCameraControlEnabled;
        public bool IsFocusedOnViewport => _isFocusedOnViewport;

        private bool _shouldShowAddNew;
        private bool _shouldShowClose;
        private bool _shouldShowFocus;
        private bool _shouldShowToggleCameraControl;
        private bool _isCameraControlEnabled;
        private bool _isFocusedOnViewport;

        public void Set(bool shouldShowAddNew, bool shouldShowClose, bool shouldShowFocus, bool shouldShowToggleCameraControl, bool isCameraControlEnabled, bool isFocusedOnViewport)
        {
            _shouldShowAddNew = shouldShowAddNew;
            _shouldShowClose = shouldShowClose;
            _shouldShowFocus = shouldShowFocus;
            _shouldShowToggleCameraControl = shouldShowToggleCameraControl;
            _isCameraControlEnabled = isCameraControlEnabled;
            _isFocusedOnViewport = isFocusedOnViewport;
        }
    }

    public sealed partial class SubViewportRightClickMenuUIController : Node
    {
        [Export] private CustomPopupMenuUIController _rightClickPopupMenu;
        [Export] private SubViewportRightClickMenu_ReferenceImage_PopupMenuUIController _referenceImageUIController;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _addViewportArgResource;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _closeViewportArgResource;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _focusViewportArgResource;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _unfocusViewportArgResource;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _toggleCameraControlArgResource;
        [Export] private CustomPopupMenu_SingleEntry_Args_Resource _referenceImageSubMenuArgResource;

        private SubViewportUIController _selectedSubViewportController;
        private CustomPopupMenu_SingleEntry_Args _addViewport;
        private CustomPopupMenu_SingleEntry_Args _closeViewport;
        private CustomPopupMenu_SingleEntry_Args _focusViewport;
        private CustomPopupMenu_SingleEntry_Args _unfocusViewport;
        private CustomPopupMenu_SingleEntry_Args _toggleCameraControl;
        private CustomPopupMenu_SingleEntry_Args _referenceImageSubMenu;
        private CustomPopupMenu_Args _args;
        private int _subViewportIndex;
        private Action _addViewportCallback;
        private Action<int> _toggleFocusViewportCallback;
        private Action<int> _closeViewportCallback;
        private Action<int> _toggleCameraControlCallback;

        public void Setup(Action addViewportCallback, Action<int> toggleFocusViewportCallback, Action<int> closeViewportCallback, Action<int> toggleCameraControlCallback)
        {
            _addViewportCallback = addViewportCallback;
            _toggleFocusViewportCallback = toggleFocusViewportCallback;
            _closeViewportCallback = closeViewportCallback;
            _toggleCameraControlCallback = toggleCameraControlCallback;

            _addViewport = _addViewportArgResource.CreateArgs(OptionPressed_AddNewViewport);
            _closeViewport = _closeViewportArgResource.CreateArgs(OptionPressed_CloseViewport);
            _focusViewport = _focusViewportArgResource.CreateArgs(OptionPressed_ToggleFocus);
            _unfocusViewport = _unfocusViewportArgResource.CreateArgs(OptionPressed_ToggleFocus);
            _toggleCameraControl = _toggleCameraControlArgResource.CreateArgs(OptionPressed_ToggleCameraControl);
            _referenceImageSubMenu = _referenceImageSubMenuArgResource.CreateArgs(
                () => GD.Print("Reference Image"),
                _referenceImageUIController.ReferenceImagePopupMenu
            );

            _args = new(
                _addViewport,
                _closeViewport,
                _focusViewport,
                _unfocusViewport,
                _toggleCameraControl,
                _referenceImageSubMenu
            );
            RefreshArgs();
        }

        private void RefreshArgs()
        {
            _rightClickPopupMenu.SetArgs(_args);
        }

        public void Activate(Vector2 mouseLocalPosition, SubViewportUIController selectedViewportController, SubViewportRightClickMenuUIController_ActivateOptions options, int subViewportIndex)
        {
            _selectedSubViewportController = selectedViewportController;
            _subViewportIndex = subViewportIndex;

            _addViewport.IsDisabled = !options.ShouldShowAddNew;
            _closeViewport.IsDisabled = !options.ShouldShowClose;
            _focusViewport.IsDisabled = !options.ShouldShowFocus;
            _unfocusViewport.IsDisabled = !options.IsFocusedOnViewport;
            _toggleCameraControl.IsDisabled = !options.ShouldShowToggleCameraControl;

            RefreshArgs();
            _referenceImageUIController.RefreshArgs();

            _rightClickPopupMenu.Position = CalculateGoToPosition(mouseLocalPosition);
            _rightClickPopupMenu.Show();
        }

        private void OptionPressed(long id)
        {
            switch (id)
            {
                case 0:
                    OptionPressed_AddNewViewport();
                    break;
                case 1:
                    OptionPressed_ToggleFocus();
                    break;
                case 2:
                    OptionPressed_CloseViewport();
                    break;
                case 3:
                    OptionPressed_ToggleCameraControl();
                    break;
                default:
                    break;
            }
        }

        private void OptionPressed_AddNewViewport()
        {
            _addViewportCallback.Invoke();
        }

        private void OptionPressed_ToggleFocus()
        {
            _toggleFocusViewportCallback.Invoke(_subViewportIndex);
        }

        private void OptionPressed_CloseViewport()
        {
            _closeViewportCallback.Invoke(_subViewportIndex);
        }

        private void OptionPressed_ToggleCameraControl()
        {
            _toggleCameraControlCallback.Invoke(_subViewportIndex);
        }

        private Vector2I CalculateGoToPosition(Vector2 mouseLocalPosition)
        {
            return (Vector2I)mouseLocalPosition;
        }
    }
}