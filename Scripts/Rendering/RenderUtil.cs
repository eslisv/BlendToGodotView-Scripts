using AM.Helpers;
using Godot;
using System.Collections.Generic;
// Original Authors - Wyatt Senalik

namespace AM.ModelViewerTool.Rendering
{
    public static class RenderUtil
    {
        private const string SOLID_COLOR_PARAMETER_NAME = "solid_color_albedo";

        public static ShaderMaterial GenericToonMaterial => _genericToonMaterial;

        private static readonly IdLibrary _colorIdLibrary = new();
        private static ShaderMaterial _solidMaterial;
        private static ShaderMaterial _genericToonMaterial;

        public static void Setup(ShaderMaterial solidMaterial, ShaderMaterial genericToonMaterial)
        {
            _solidMaterial = solidMaterial;
            _genericToonMaterial = genericToonMaterial;
        }

        public static ShaderMaterial CreateUniqueOutlineMaterial(ShaderMaterial originalToonAndOutlineMaterial, out int colorId)
        {
            colorId = _colorIdLibrary.CheckoutID();
            Color uniqueColor = DetermineUniqueColor(colorId);
            return CreateNewOutlineMaterialWithColor(originalToonAndOutlineMaterial, uniqueColor);
        }

        public static int CheckoutColorId()
        {
            return _colorIdLibrary.CheckoutID();
        }

        public static ShaderMaterial CreateNewOutlineMaterialWithExistingId(ShaderMaterial originalToonAndOutlineMaterial, int colorId)
        {
            Color color = DetermineUniqueColor(colorId);
            return CreateNewOutlineMaterialWithColor(originalToonAndOutlineMaterial, color);
        }

        public static void ReturnColorIds(IEnumerable<int> materialIds)
        {
            foreach (int id in materialIds)
            {
                _colorIdLibrary.ReturnID(id);
            }
        }

        public static void ReturnColorIds(params int[] materialIds)
        {
            foreach (int id in materialIds)
            {
                _colorIdLibrary.ReturnID(id);
            }
        }

        public static void ReturnColorId(int materialId)
        {
            _colorIdLibrary.ReturnID(materialId);
        }

        private static ShaderMaterial CreateNewOutlineMaterialWithColor(ShaderMaterial originalToonAndOutlineMaterial, Color solidColor)
        {
            ShaderMaterial duplicateMaterialPass1_Solid = _solidMaterial.Duplicate() as ShaderMaterial;
            duplicateMaterialPass1_Solid.SetShaderParameter(SOLID_COLOR_PARAMETER_NAME, solidColor);

            ShaderMaterial duplicateMaterialPass2_Outline = originalToonAndOutlineMaterial.Duplicate() as ShaderMaterial;
            duplicateMaterialPass2_Outline.SetShaderParameter(SOLID_COLOR_PARAMETER_NAME, solidColor);
            duplicateMaterialPass1_Solid.NextPass = duplicateMaterialPass2_Outline;

            return duplicateMaterialPass1_Solid;
        }

        private static Color DetermineUniqueColor(int index)
        {
            float hue = MathUtil.VanDerCorput((uint)index);
            return Color.FromHsv(hue, 1.0f, 1.0f);
        }
    }
}