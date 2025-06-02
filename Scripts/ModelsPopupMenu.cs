using EVHelpers;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class ModelsPopupMenu : PopupMenu
{
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            IdPressed -= OptionPressed;
            AboutToPopup -= GenerateItems;
        }
    }

    public override void _Ready()
    {
        IdPressed += OptionPressed;
        AboutToPopup += GenerateItems;
    }

    public void GenerateItems()
    {
        Clear();
        IReadOnlyList<string> modelPaths = FileWatcherHelper.GetFilesOfWatcherFromIndex(0);
        for (int i = 0; i < modelPaths.Count; i++)
        {
            AddItem(modelPaths[i].GetBaseName(), i);
            SetItemMetadata(i, modelPaths[i]);
        }
    }

    public void Reset()
    {
        Clear();
    }

    private void OptionPressed(long id)
    {
        int itemIndex = GetItemIndex((int)id);
        string itemPath = (string)GetItemMetadata(itemIndex);
        SceneManager loader = (SceneManager)GetTree().Root.GetChild(0);
        loader.Reset();
        string gltfPath = Path.ChangeExtension(itemPath, ".gltf");
        if (!File.Exists(gltfPath))
        {
            loader.GltfLoader.GenerateGltfFromBlendFile(gltfPath.GetBaseName());
        }
        loader.GenerateModelNode(gltfPath);
        File.WriteAllText(Path.GetTempPath().PathJoin(@"BlenderImporterViewer\DefaultPath.txt"), gltfPath);
    }
}
