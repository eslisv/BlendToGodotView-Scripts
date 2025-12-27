using Godot;
using System;

namespace AM.ModelViewerTool
{

    public sealed class CustomPopupMenu_SingleEntry_Args
    {
        public PopupMenu SubMenuPopupMenu { get; }
        public string Label { get; set; }
        public string MetaData { get; }
        public bool IsDisabled { get; set; }
        public bool IsSubMenu { get; }
        public bool IsCheckable { get; }
        public bool IsChecked { get; set; }
        public Action SelectedCallback { get; }

        public CustomPopupMenu_SingleEntry_Args(string label, Action selectedCallback, PopupMenu subMenuPopupMenu = null, string metaData = "", bool isSubMenu = false, bool isDisabled = false, bool isCheckable = false, bool isChecked = false)
        {
            Label = label;
            SelectedCallback = selectedCallback;
            SubMenuPopupMenu = subMenuPopupMenu;
            MetaData = metaData;
            IsSubMenu = isSubMenu;
            IsDisabled = isDisabled;
            IsCheckable = isCheckable;
            IsChecked = isChecked;
        }
    }
}
