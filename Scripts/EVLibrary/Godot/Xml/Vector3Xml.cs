using EVLibrary.Xml;
using Godot;
using System.Xml.Linq;

namespace EVLibrary.Godot.XML
{

    public sealed class Vector3Xml : DataTypeXml
    {
        public const string VECTOR3_X_ELEMENT_NAME = "X";
        public const string VECTOR3_Y_ELEMENT_NAME = "Y";
        public const string VECTOR3_Z_ELEMENT_NAME = "Z";

        private const string VECTOR3_TYPE = "Vector3";
        public Vector3 Vector => new Vector3(X.Value, Y.Value, Z.Value);

        public FloatXml X { get; }
        public FloatXml Y { get; }
        public FloatXml Z { get; }

        public Vector3Xml() : base(nameof(Vector3Xml), VECTOR3_TYPE)
        {
            X = new FloatXml(VECTOR3_X_ELEMENT_NAME, default);
            Y = new FloatXml(VECTOR3_Y_ELEMENT_NAME, default);
            Z = new FloatXml(VECTOR3_Z_ELEMENT_NAME, default);

            Element.Add(X.Element, Y.Element, Z.Element);
        }

        public Vector3Xml(string name) : this()
        {
            Element.Name = name;
        }

        public Vector3Xml(string name, string x, string y, string z) : this(name)
        {
            X.Element.Value = x;
            Y.Element.Value = y;
            Z.Element.Value = z;
        }

        public Vector3Xml(string name, Vector3 vector) : this(name, vector.X.ToString(), vector.Y.ToString(), vector.Z.ToString()) { }

        public Vector3Xml(string name, float x, float y, float z) : this(name, x.ToString(), y.ToString(), z.ToString()) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }

    public static class Vector3XMLExtensions
    {
        public const string VECTOR3_X_ELEMENT_NAME = "X";
        public const string VECTOR3_Y_ELEMENT_NAME = "Y";
        public const string VECTOR3_Z_ELEMENT_NAME = "Z";

        /// <summary>
        /// Converts an Vector3Xml XElement into a <seealso cref="Vector3Xml"/>.
        /// 
        /// 
        /// <code>
        /// An example of a Vector3Xml XElement would look like this.
        /// &lt;Position&gt;<br/>
        ///   &lt;X&gt;0&lt;/X&gt;<br/>
        ///   &lt;Y&gt;0&lt;/Y&gt;<br/>
        ///   &lt;Z&gt;0&lt;/Z&gt;<br/>
        /// &lt;/Position&gt;
        /// </code>
        /// </summary>
        /// <param name="vectorElement">The root node of a Vector3Xml</param>
        /// <returns></returns>
        public static Vector3Xml ConvertToVector3XML(this XElement vectorElement)
        {
            string rootName = vectorElement.Name.ToString();
            string x = vectorElement.Element(VECTOR3_X_ELEMENT_NAME).Value;
            string y = vectorElement.Element(VECTOR3_Y_ELEMENT_NAME).Value;
            string z = vectorElement.Element(VECTOR3_Z_ELEMENT_NAME).Value;

            return new Vector3Xml(rootName, x, y, z);
        }
    }
}
