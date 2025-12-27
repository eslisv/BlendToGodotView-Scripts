using System.Globalization;
using System.Xml.Linq;

namespace EVLibrary.Xml
{
    public sealed class IntXml : DataTypeXml
    {
        public static readonly string IntType = INT_TYPE;

        private const string INT_TYPE = "Integer";

        public int Value => int.TryParse(
            Element.Value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture.NumberFormat,
            out int value
        ) ? value : default;


        public IntXml() : base(nameof(BoolXml), INT_TYPE, false) { }
        public IntXml(string name) : base(name, INT_TYPE, false) { }
        public IntXml(string name, int value) : base(name, INT_TYPE, value) { }
        public IntXml(XName name) : this(name.LocalName) { }
        public IntXml(XName name, int value) : this(name.LocalName, value) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
