using Godot;
using System;

namespace AM.ModelViewerTool
{
    public sealed partial class CustomPopupMenuUIController : PopupMenu
    {
        private CustomPopupMenu_Args _args;

        public override void _EnterTree()
        {
            base._EnterTree();

            IndexPressed += OptionPressed;
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            IndexPressed -= OptionPressed;
        }

        public void SetArgs(CustomPopupMenu_Args args)
        {
            Clear();
            _args = args;
            // Prints out all properties in CustomPopupMenu_Args
            //GodotPrintHelper.PrintProperties($"{this.Name}{nameof(_args)}", _args);
            for (int i = 0; i < _args.Entries.Count; ++i)
            {
                CustomPopupMenu_SingleEntry_Args entry = _args.Entries[i];
                if (entry.IsSubMenu)
                {
                    AddSubmenuNodeItem(entry.Label, entry.SubMenuPopupMenu);
                }
                else
                {
                    AddItem(entry.Label);
                }
                SetItemMetadata(i, entry.MetaData);
                SetItemDisabled(i, entry.IsDisabled);
                SetItemAsCheckable(i, entry.IsCheckable);
                if (entry.IsChecked)
                {
                    SetItemChecked(i, entry.IsChecked);
                }
            }
        }

        private void OptionPressed(long index)
        {
            if (_args == null)
            {
                throw new NullReferenceException("CustomPopupMenu_Args has not been set.");
            }
            if (index < 0 && index >= _args.Entries.Count)
            {
                throw new ArgumentOutOfRangeException($"Index exceeds arg entries. ({index})");
            }
            if (_args.Entries[(int)index].IsCheckable)
            {
                SetItemChecked((int)index, !IsItemChecked((int)index));
            }
            _args.Entries[(int)index].SelectedCallback.Invoke();
        }
    }
}
