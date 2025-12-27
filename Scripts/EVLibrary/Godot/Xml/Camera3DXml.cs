using Godot;
using System.Xml.Linq;

namespace EVLibrary.Godot.XML
{
    public sealed class Camera3DXml : Node3DXml
    {
        public Camera3DXml(Camera3D node) : base(node)
        {
            TypeAttribute.Value = nameof(Camera3D);
        }

        public Camera3DXml(Node3D node) : base(node) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
