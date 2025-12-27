using Godot;
using System;
using System.IO;
using SysFile = System.IO.File;

namespace EVLibrary.Godot.IO
{
    public enum EImageType
    {
        PNG,
        JPG,
        BMP,
        WEBP,
        SVG,
        KTX,
        TGA,
        UNDEFINED
    }

    /// <summary>
    /// Helper class for importing images into Godot during runtime.
    /// </summary>
    public static class GodotImageImportHelper
    {
        public static ImageTexture LoadImageFromPath(string texturePath)
        {
            Image img = new Image();
            ImageTexture tex = new ImageTexture();
            EImageType imageType = DetectImageType(texturePath);
            byte[] buffer = SysFile.ReadAllBytes(texturePath);
            switch (imageType)
            {
                case EImageType.PNG:
                    img.LoadPngFromBuffer(buffer);
                    break;
                case EImageType.JPG:
                    img.LoadJpgFromBuffer(buffer);
                    break;
                case EImageType.BMP:
                    img.LoadBmpFromBuffer(buffer);
                    break;
                case EImageType.WEBP:
                    img.LoadWebpFromBuffer(buffer);
                    break;
                case EImageType.SVG:
                    img.LoadSvgFromBuffer(buffer);
                    break;
                case EImageType.KTX:
                    img.LoadKtxFromBuffer(buffer);
                    break;
                case EImageType.TGA:
                    img.LoadTgaFromBuffer(buffer);
                    break;
                case EImageType.UNDEFINED: // Included just for clarification
                default:
                    throw new Exception($"Unknown image type at path: ({texturePath})");
            }
            tex.SetImage(img);
            return tex;
        }
        private static EImageType DetectImageType(string texturePath)
        {
            string extension = Path.GetExtension(texturePath);
            switch (extension)
            {
                case ".png":
                    return EImageType.PNG;
                case ".jpg":
                    return EImageType.JPG;
                case ".bmp":
                    return EImageType.BMP;
                case ".webp":
                    return EImageType.WEBP;
                case ".svg":
                    return EImageType.SVG;
                case ".ktx":
                    return EImageType.KTX;
                case ".tga":
                    return EImageType.TGA;
            }
            return EImageType.UNDEFINED;
        }
    }
}
