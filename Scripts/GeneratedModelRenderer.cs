using AM.ModelViewerTool.Rendering;
using EVLibrary.FileIO.Extensions;
using EVLibrary.Godot.Extensions;
using Godot;
using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed partial class GeneratedModelRenderer : Node3D
    {
        private const string ALBEDO_TEXTURE_NAME = "{0} Base Color.png";

        [Export] private ShaderMaterial _pixelOutlineShaderMaterial;

        private Node3D _modelNode;

        public void SetModel(Node3D modelNode, bool isOutsideTree)
        {
            _modelNode = modelNode;
            if (isOutsideTree)
            {
                CallDeferred(MethodName.AddChild, _modelNode);
            }
            else
            {
                AddChild(_modelNode);
            }

            List<MeshInstance3D> childNodes = _modelNode.GetAllChildrenInNodeOfType<MeshInstance3D>();
            GD.Print(childNodes.Count);
            foreach (MeshInstance3D mesh in childNodes)
            {
                string albedoTextureName = string.Format(ALBEDO_TEXTURE_NAME, mesh.Name);
                string generatedTexturePath = SettingsController.Settings.GltfModelFilePath.GetBaseDir().PathJoin(albedoTextureName).StandardizeSlashesToBackslash();
                GD.Print($"Generated Texture Path = {generatedTexturePath}");
                ImageTexture albedoTexture = null;
                Image albedoTextureImage = new();
                Error loadError = albedoTextureImage.Load(generatedTexturePath);
                if (loadError != Error.Ok)
                {
                    GD.PrintErr($"Failed to load albedo texture at path ({generatedTexturePath}). Error: ({loadError})");
                }
                else
                {
                    albedoTexture = ImageTexture.CreateFromImage(albedoTextureImage);
                }

                for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); ++i)
                {
                    ShaderMaterial toonMaterialCopy = RenderUtil.GenericToonMaterial.Duplicate() as ShaderMaterial;
                    if (albedoTexture != null)
                    {
                        toonMaterialCopy.SetShaderParameter("texture_albedo", albedoTexture);
                    }
                    ShaderMaterial uniqueMaterial = RenderUtil.CreateUniqueOutlineMaterial(toonMaterialCopy, out _);
                    mesh.SetSurfaceOverrideMaterial(i, uniqueMaterial);
                }
            }
        }

        public void SetModelTexture(string texturePath)
        {
            List<MeshInstance3D> childNodes = _modelNode.GetAllChildrenInNodeOfType<MeshInstance3D>();
            GD.Print(childNodes.Count);
            foreach (MeshInstance3D mesh in childNodes)
            {
                ImageTexture albedoTexture = null;
                Image albedoTextureImage = new();
                albedoTextureImage.Load(texturePath);
                albedoTexture = ImageTexture.CreateFromImage(albedoTextureImage);
                for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); ++i)
                {
                    ShaderMaterial toonMaterialCopy = RenderUtil.GenericToonMaterial.Duplicate() as ShaderMaterial;
                    if (albedoTexture != null)
                    {
                        toonMaterialCopy.SetShaderParameter("texture_albedo", albedoTexture);
                    }
                    ShaderMaterial uniqueMaterial = RenderUtil.CreateUniqueOutlineMaterial(toonMaterialCopy, out _);
                    mesh.SetSurfaceOverrideMaterial(i, uniqueMaterial);
                }
            }
        }

        public void DestroyModelNode()
        {
            if (_modelNode == null)
                return;
            _modelNode.QueueFree();
            _modelNode = null;
        }
    }
}