using System.Xml.Linq;

namespace EVLibrary.Xml
{
    public sealed class BoolXml : DataTypeXml
    {
        public static readonly string BoolType = BOOL_TYPE;

        private const string BOOL_TYPE = "Bool";

        //  TryParse returns true if parsing the string was successful. So we need to check against the actual state.
        public bool Bool => bool.TryParse(Element.Value, out bool state) && state;


        public BoolXml() : base(nameof(BoolXml), BOOL_TYPE, false) { }
        public BoolXml(string name) : base(name, BOOL_TYPE, false) { }
        public BoolXml(string name, bool state) : base(name, BOOL_TYPE, state) { }
        public BoolXml(XName name) : this(name.LocalName) { }
        public BoolXml(XName name, bool state) : this(name.LocalName, state) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }

    public static class BoolXMLExtensions
    {
        public static BoolXml ConvertToBoolXML(this XElement boolElement)
        {
            XAttribute attribute = boolElement.Attribute(DataTypeXml.TYPE_ATTRIBUTE_NAME);
            if (attribute == null || attribute.Value != BoolXml.BoolType)
            {
                throw new System.Exception($"{typeof(BoolXMLExtensions)}.{nameof(ConvertToBoolXML)}: ({nameof(boolElement)}) {boolElement} does not contain attribute {BoolXml.BoolType}");
            }

            bool state = boolElement.Value == bool.TrueString;

            return new BoolXml(boolElement.Name, state);
        }
    }
}
