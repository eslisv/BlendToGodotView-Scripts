using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EVLibrary.Xml.XPath
{
    public static class XPathSearch
    {
        private const string QUERY = @"//*[contains({0}, '{1}')]";
        private const string CLASS_IDENTIFIER = "@class";
        private const string TYPE_IDENTIFIER = "@type";
        private const string TEXT_IDENTIFIER = "text()";

        public static IEnumerable<XElement> ContainsClass(this XContainer element, string query)
        {
            return element.XPathSelectElements(string.Format(QUERY, CLASS_IDENTIFIER, query));
        }

        public static IEnumerable<XElement> ContainsType(this XContainer element, string type)
        {
            return element.XPathSelectElements(string.Format(QUERY, TYPE_IDENTIFIER, type));
        }

        public static IEnumerable<XElement> TextMatch(this XContainer element, string matchString)
        {
            return element.XPathSelectElements(string.Format(QUERY, TEXT_IDENTIFIER, matchString));
        }
    }
}
