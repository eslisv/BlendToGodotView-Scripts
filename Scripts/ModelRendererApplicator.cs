using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ModelRendererApplicator : Node3D
{
    private ShaderMaterial _material;

    public void ApplyShaderMaterial(Shader shader)
    {
        _material = GD.Load<ShaderMaterial>("res://Resources/ShaderMaterial/CelShading.tres");
        Godot.Collections.Array<Node> childNodes = GetChildren();
        GD.Print(childNodes.Count);
        foreach (Node node in childNodes)
        {
            MeshInstance3D instance = node as MeshInstance3D;
            instance.SetSurfaceOverrideMaterial(0, _material);
        }
    }
}
