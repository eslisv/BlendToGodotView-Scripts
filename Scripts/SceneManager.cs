using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EVHelpers;
using EVHelpers.Godot;
using Microsoft.Win32;

public partial class SceneManager : Node
{
    public string FilePath;

    public GltfLoader GltfLoader => _gltfLoader;
    private GltfLoader _gltfLoader;
    private Node3D _gltfNode;
    private Resource _applicatorScript;
    private Shader _shaderMaterial;

    public override void _Ready()
    {
        string savedPath = Path.GetTempPath().PathJoin(@"\BlenderImporterViewer\DefaultPath.txt");
        if (File.Exists(savedPath))
        {
            savedPath = File.ReadAllText(savedPath);
            if (File.Exists(savedPath))
            {
                FilePath = savedPath.GetBaseDir();
            }
            else
            {
                FilePath = savedPath;
            }
        }
        else if (OS.HasFeature("editor"))
        {
            FilePath = ProjectSettings.GlobalizePath(@"res://Models").Replace('/', '\\');
        }
        else
        {
            FilePath = OS.GetExecutablePath();
        }
        GD.Print($"Loaded path: {FilePath}");
        Setup(savedPath);
        GD.Print(FileSearchHelper.FindExecutableInstallPath("Blender"));
    }

    public void Setup(string modelPath)
    {
        _gltfNode = new Node3D();
        _gltfLoader = new GltfLoader();
        _gltfLoader.Setup(FilePath);
        _applicatorScript = GD.Load(@"res://Scripts/ModelRendererApplicator.cs");
        _shaderMaterial = GD.Load<Shader>(@"res://Shaders/PixelOutline.gdshader");
        _gltfLoader.OnOpenedFileChanged += GenerateModelNodeOutsideTree;
        if (!File.Exists(modelPath)) { return; }
        GenerateModelNode(modelPath);
    }

    public void GenerateModelNodeOutsideTree(string path)
    {
        Reset();
        _gltfLoader.LoadFile(path);
        _gltfNode = _gltfLoader.GenerateNode();
        CallDeferred(Node3D.MethodName.AddChild, _gltfNode);
        ulong instanceId = _gltfNode.GetInstanceId();
        _gltfNode.SetScript(_applicatorScript); // Some error comes up here saying to use CallDeferred (doesnt work btw) but I don't know how to solve it.
        _gltfNode = (Node3D)InstanceFromId(instanceId);
        ModelRendererApplicator applicator = _gltfNode as ModelRendererApplicator;
        applicator.ApplyShaderMaterial(_shaderMaterial);
    }

    public void GenerateModelNode(string path)
    {
        _gltfLoader.LoadFile(path);
        _gltfNode = _gltfLoader.GenerateNode();
        AddChild(_gltfNode);
        ulong instanceId = _gltfNode.GetInstanceId();
        _gltfNode.SetScript(_applicatorScript);
        _gltfNode = (Node3D)InstanceFromId(instanceId);
        ModelRendererApplicator applicator = _gltfNode as ModelRendererApplicator;
        applicator.ApplyShaderMaterial(_shaderMaterial);
    }

    public void Reset(bool resetLoader = true)
    {
        if (resetLoader)
        {
            _gltfLoader.Reset();
        }
        if (_gltfNode != null)
        {
            _gltfNode.QueueFree();
            _gltfNode = null;
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            _gltfLoader.OnOpenedFileChanged -= GenerateModelNodeOutsideTree;
        }
    }
}
