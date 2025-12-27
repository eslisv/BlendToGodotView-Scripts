using Godot;
using System;
using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed class MultiSubViewportsUIController_PopupController
    {
        private readonly SubViewportRightClickMenuUIController_ActivateOptions _popupOptions = new();
        private readonly IReadOnlyMultiSubViewportsUIController_Args _parentControllerArgs;
        private readonly SubViewportRightClickMenuUIController _viewportPopupMenu;

        public MultiSubViewportsUIController_PopupController(IReadOnlyMultiSubViewportsUIController_Args parentControllerArgs, SubViewportRightClickMenuUIController viewportPopupMenu)
        {
            _parentControllerArgs = parentControllerArgs;
            _viewportPopupMenu = viewportPopupMenu;
        }

        public void TryOpenPopup(Vector2 mouseLocalPosition, SubViewportUIController selectedViewportController, int selectedViewportIndex)
        {
            //if (_viewportPopupMenu.Visible) { return; }

            _popupOptions.Set(
                shouldShowAddNew: CanAddNewViewport(),
                shouldShowClose: CanCloseViewport(selectedViewportIndex),
                shouldShowFocus: CanFocusOnViewport(selectedViewportIndex),
                shouldShowToggleCameraControl: CanToggleCameraControl(selectedViewportIndex),
                isCameraControlEnabled: IsCameraControlEnabledForViewport(selectedViewportIndex),
                isFocusedOnViewport: IsFocusedOnAnyViewport()
                );
            _viewportPopupMenu.Activate(mouseLocalPosition, selectedViewportController, _popupOptions, selectedViewportIndex);
        }

        private bool IsFocusedOnAnyViewport()
        {
            return _parentControllerArgs.FocusedSubViewport != null;
        }

        private bool CanAddNewViewport()
        {
            // Can add a new viewport when not focused
            return !IsFocusedOnAnyViewport();
        }

        private bool CanFocusOnViewport(int viewportIndex)
        {
            // Didn't right click on a viewport
            if (!IsViewportIndexValid(viewportIndex)) { return false; }
            // If already focusing on a viewport
            if (IsFocusedOnAnyViewport()) { return false; }
            // No reason to ever focus if there is only 1 viewport. Its basically already focused.
            if (_parentControllerArgs.GetSubViewportControllers().Count <= 1) { return false; }
            return true;
        }

        private bool CanCloseViewport(int viewportIndex)
        {
            // Didn't right click on a viewport
            if (!IsViewportIndexValid(viewportIndex)) { return false; }
            // Don't allow closing if there is only 1 viewport
            if (_parentControllerArgs.GetSubViewportControllers().Count <= 1) { return false; }
            // Can't close if you're focused on a viewport
            if (IsFocusedOnAnyViewport()) { return false; }
            return true;
        }

        private bool CanToggleCameraControl(int viewportIndex)
        {
            // Didn't right click on a viewport
            if (!IsViewportIndexValid(viewportIndex)) { return false; }
            return true;
        }

        private bool IsCameraControlEnabledForViewport(int viewportIndex)
        {
            // Didn't right click on a viewport
            if (!IsViewportIndexValid(viewportIndex)) { return false; }
            // If the enabled camera controller is nothing, there obviously isn't control enabled.
            if (_parentControllerArgs.EnabledCameraController == null) { return false; }
            // There is camera control, but is for the current viewport.
            IReadOnlyList<SubViewportUIController> subViewportControllers = _parentControllerArgs.GetSubViewportControllers();
            SubViewportUIController desiredSubViewport = subViewportControllers[viewportIndex];
            if (_parentControllerArgs.EnabledCameraController != desiredSubViewport.CameraController) { return false; }
            return true;
        }

        private bool IsViewportIndexValid(int viewportIndex)
        {
            return viewportIndex >= 0 && viewportIndex < _parentControllerArgs.GetSubViewportControllers().Count;
        }
    }
}