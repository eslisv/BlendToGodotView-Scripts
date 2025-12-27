using EVLibrary.Xml;
using Godot;

namespace EVLibrary.Godot.XML
{
    public sealed class Vector2Xml : DataTypeXml
    {
        public const string VECTOR2_X_ELEMENT_NAME = "X";
        public const string VECTOR2_Y_ELEMENT_NAME = "Y";

        private const string VECTOR2_TYPE = "Vector2";
        public Vector2 Vector => new Vector2(X.Value, Y.Value);

        public FloatXml X { get; }
        public FloatXml Y { get; }

        public Vector2Xml() : base(nameof(Vector2Xml), VECTOR2_TYPE)
        {
            X = new FloatXml(VECTOR2_X_ELEMENT_NAME, default);
            Y = new FloatXml(VECTOR2_Y_ELEMENT_NAME, default);

            Element.Add(X.Element, Y.Element);
        }

        public Vector2Xml(string name) : this()
        {
            Element.Name = name;
        }

        public Vector2Xml(string name, string x, string y) : this(name)
        {
            X.Element.Value = x;
            Y.Element.Value = y;
        }

        public Vector2Xml(string name, Vector2 vector) : this(name, vector.X.ToString(), vector.Y.ToString()) { }

        public Vector2Xml(string name, float x, float y) : this(name, x.ToString(), y.ToString()) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
