using System.Globalization;
using System.Xml.Linq;

namespace EVLibrary.Xml
{
    public sealed class FloatXml : DataTypeXml
    {
        public static readonly string FloatType = FLOAT_TYPE;

        private const string FLOAT_TYPE = "Float";
        private const float FLOAT_DEFAULT_VALUE = 0.0f;

        // Include CultureInfo to ensure that a floating point number is converted correctly.
        public float Value => float.TryParse(
            Element.Value,
            NumberStyles.Float | NumberStyles.AllowThousands,
            CultureInfo.InvariantCulture.NumberFormat,
            out float value
        ) ? value : default;

        public FloatXml() : base(nameof(FloatXml), FLOAT_TYPE, FLOAT_DEFAULT_VALUE) { }
        public FloatXml(string name) : base(name, FLOAT_TYPE, FLOAT_DEFAULT_VALUE) { }
        public FloatXml(string name, float value) : base(name, FLOAT_TYPE, value) { }
        public FloatXml(XName name) : this(name.LocalName) { }
        public FloatXml(XName name, float value) : this(name.LocalName, value) { }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
