using EVLibrary.Godot.IO;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace AM.ModelViewerTool
{
    public sealed partial class SubViewportRightClickMenu_ReferenceImage_PopupMenuUIController : Node
    {
        public CustomPopupMenuUIController ReferenceImagePopupMenu => _referenceImagePopupMenu;

        [Export] private CustomPopupMenuUIController _referenceImagePopupMenu;
        [Export] private PackedScene _spriteInputMovementPrefab;

        public void RefreshArgs()
        {
            List<CustomPopupMenu_SingleEntry_Args> popupMenuSingleArgs = new();
            IReadOnlyList<string> referencePaths = ReferenceImageTracking.ReferenceImagePaths;
            for (int i = 0; i < referencePaths.Count(); ++i)
            {
                int index = i;
                popupMenuSingleArgs.Add(new CustomPopupMenu_SingleEntry_Args(
                    label: referencePaths[index].GetFile(),
                    selectedCallback: () => SubmenuItemPressed(referencePaths[index])
                ));
            }
            CustomPopupMenu_Args args = new(popupMenuSingleArgs);
            _referenceImagePopupMenu.SetArgs(args);
        }

        private void SubmenuItemPressed(string path)
        {
            var spriteInputMovement = _spriteInputMovementPrefab.Instantiate<SpriteInputMovement>();
            GetTree().Root.AddChild(spriteInputMovement);

            spriteInputMovement.Texture = GodotImageImportHelper.LoadImageFromPath(path);
            spriteInputMovement.TopLevel = true;
            spriteInputMovement.Position = GameConstants.UI_REFERENCE_RESOLUTION * 0.5f;
        }
    }
}