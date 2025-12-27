using EVLibrary.Xml;
using Godot;

namespace EVLibrary.Godot.XML
{
    public class ControlXml : NodeXml
    {
        public const string SIZE_ELEMENT_NAME = "Size";
        public const string POSITION_ELEMENT_NAME = "Position";
        public const string ROTATION_ELEMENT_NAME = "Rotation";
        public const string SCALE_ELEMENT_NAME = "Scale";
        public const string VISIBLE_ELEMENT_NAME = "Visible";
        public const string PIVOT_OFFSET_ELEMENT_NAME = "Pivot-Offset";

        public Vector2Xml Size { get; }
        public Vector2Xml Position { get; }
        public FloatXml Rotation { get; }
        public Vector2Xml Scale { get; }
        public BoolXml Visible { get; }
        public Vector2Xml PivotOffset { get; }

        public ControlXml() : base(type: nameof(Control))
        {
            Size = new Vector2Xml(SIZE_ELEMENT_NAME);
            Position = new Vector2Xml(POSITION_ELEMENT_NAME);
            Rotation = new FloatXml(ROTATION_ELEMENT_NAME);
            Scale = new Vector2Xml(SCALE_ELEMENT_NAME);
            Visible = new BoolXml(VISIBLE_ELEMENT_NAME, true);
            PivotOffset = new Vector2Xml(PIVOT_OFFSET_ELEMENT_NAME);

            Element.Add(Size.Element, Position.Element, Rotation.Element, Scale.Element, Visible.Element, PivotOffset.Element);
        }

        public ControlXml(string name) : this()
        {
            Element.Name = name;
        }

        public ControlXml(string name, Vector2 size, Vector2 position, float rotation, Vector2 scale, Vector2 pivotOffset) : this(name)
        {
            Size.X.Element.Value = size.X.ToString();
            Size.Y.Element.Value = size.Y.ToString();

            Position.X.Element.Value = position.X.ToString();
            Position.Y.Element.Value = position.Y.ToString();

            Rotation.Element.Value = rotation.ToString();

            Scale.X.Element.Value = scale.X.ToString();
            Scale.Y.Element.Value = scale.Y.ToString();

            Scale.X.Element.Value = scale.X.ToString();
            Scale.Y.Element.Value = scale.Y.ToString();

            PivotOffset.X.Element.Value = pivotOffset.X.ToString();
            PivotOffset.Y.Element.Value = pivotOffset.Y.ToString();
        }

        public ControlXml(Control node) : this(node.Name, node.Size, node.Position, node.Rotation, node.Scale, node.PivotOffset) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
