using AM.ModelViewerTool;
using EVLibrary.Godot.XML;
using Godot;
using System;
using System.IO;

public partial class GenerateHierarchyXml : Node
{
    [Export] Node rootNode;

    public override void _Ready()
    {
        base._Ready();

        NodeXml xml = rootNode.ConvertNodeToXmlTree();
        File.WriteAllText($"{PathReferences.GetAbsolutePathToUserPathFolder()}Hierarchy.xml", xml.Element.ToString());
    }
}
