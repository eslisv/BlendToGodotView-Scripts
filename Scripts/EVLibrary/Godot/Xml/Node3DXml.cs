using EVLibrary.Xml;
using Godot;

namespace EVLibrary.Godot.XML
{
    public class Node3DXml : NodeXml
    {
        public const string POSITION_ELEMENT_NAME = "Position";
        public const string ROTATION_ELEMENT_NAME = "Rotation";
        public const string SCALE_ELEMENT_NAME = "Scale";
        public const string VISIBLE_ELEMENT_NAME = "Visible";

        public Vector3Xml Position { get; }
        public Vector3Xml Rotation { get; }
        public Vector3Xml Scale { get; }
        public BoolXml Visible { get; }

        public Node3DXml() : base(type: nameof(Node3D))
        {
            Position = new Vector3Xml(POSITION_ELEMENT_NAME);
            Rotation = new Vector3Xml(ROTATION_ELEMENT_NAME);
            Scale = new Vector3Xml(SCALE_ELEMENT_NAME);
            Visible = new BoolXml(VISIBLE_ELEMENT_NAME, true);

            Element.Add(Position.Element, Rotation.Element, Scale.Element, Visible.Element);
        }

        public Node3DXml(string name) : this()
        {
            Element.Name = name;
        }

        public Node3DXml(string name, Vector3 position, Vector3 rotation, Vector3 scale) : this(name)
        {
            Position.X.Element.Value = position.X.ToString();
            Position.Y.Element.Value = position.Y.ToString();
            Position.Z.Element.Value = position.Z.ToString();

            Rotation.X.Element.Value = rotation.X.ToString();
            Rotation.Y.Element.Value = rotation.Y.ToString();
            Rotation.Z.Element.Value = rotation.Z.ToString();

            Scale.X.Element.Value = scale.X.ToString();
            Scale.Y.Element.Value = scale.Y.ToString();
            Scale.Z.Element.Value = scale.Z.ToString();
        }

        public Node3DXml(Node3D node) : this(node.Name.ToString(), node.Position, node.Rotation, node.Scale)
        {
            Visible.Element.Value = node.Visible.ToString();
        }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}