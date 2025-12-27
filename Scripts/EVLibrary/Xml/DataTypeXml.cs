using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace EVLibrary.Xml
{
    public class DataTypeXml
    {
        public const string TYPE_ATTRIBUTE_NAME = "type";

        public XAttribute TypeAttribute { get; }
        public XElement Element { get; }

        public DataTypeXml(string name, string type, object value = null)
        {
            TypeAttribute = new XAttribute(TYPE_ATTRIBUTE_NAME, type);
            Element = new XElement(name, TypeAttribute);

            if (value == null) { return; }
            if (value is IFormattable formattable)
            {
                Element.Value = formattable.ToString(null, CultureInfo.InvariantCulture);
                return;
            }
            Element.Value = value.ToString();
        }

        protected DataTypeXml(XName name, string type, object value) : this(name.LocalName, type, value) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
