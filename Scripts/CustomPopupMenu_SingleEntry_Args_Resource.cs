using Godot;
using System;

namespace AM.ModelViewerTool
{
    [GlobalClass]
    public sealed partial class CustomPopupMenu_SingleEntry_Args_Resource : Resource
    {
        public string Label => _label;
        public string MetaData => _metaData;
        public bool StartDisabled => _startsDisabled;
        public bool IsSubMenu => _isSubMenu;
        public bool IsCheckable => _isCheckable;
        public bool IsChecked => _isChecked;

        [Export] private string _label;
        [Export] private string _metaData;
        [Export] private bool _startsDisabled;
        [Export] private bool _isSubMenu;
        [Export] private bool _isCheckable;
        [Export] private bool _isChecked;

        public CustomPopupMenu_SingleEntry_Args CreateArgs(Action selectedCallback, PopupMenu subMenu = null)
        {
            return new CustomPopupMenu_SingleEntry_Args(
                label: Label,
                subMenuPopupMenu: subMenu,
                selectedCallback: selectedCallback,
                metaData: MetaData,
                isSubMenu: IsSubMenu,
                isDisabled: StartDisabled,
                isCheckable: IsCheckable,
                isChecked: IsChecked
            );
        }
    }
}
