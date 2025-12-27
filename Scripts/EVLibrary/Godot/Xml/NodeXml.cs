using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EVLibrary.Godot.XML
{
    public class NodeXml
    {
        public const string TYPE_ATTRIBUTE_NAME = "type";
        public const string SCRIPT_ATTRIBUTE_NAME = "script";

        public XAttribute TypeAttribute { get; protected set; }
        public XAttribute ScriptAttribute { get; protected set; }
        public XElement Element { get; protected set; }

        public NodeXml(string name = nameof(Node), string type = nameof(Node))
        {
            TypeAttribute = new XAttribute(TYPE_ATTRIBUTE_NAME, type);
            ScriptAttribute = new XAttribute(SCRIPT_ATTRIBUTE_NAME, string.Empty);
            Element = new XElement(
                name,
                TypeAttribute,
                ScriptAttribute
            );
        }

        public NodeXml(Node node) : this(name: node.Name) { }

        public NodeXml(XElement element) : this()
        {
            Element = element;
        }

        public override string ToString()
        {
            return Element.ToString();
        }
    }

    public static class NodeXmlExtensions
    {
        public static NodeXml Create(Node node)
        {
            return node switch
            {
                Camera3D camera3d => new Camera3DXml(camera3d),
                Node3D node3d => new Node3DXml(node3d),
                Control control => new ControlXml(control),
                null => throw new ArgumentNullException(nameof(node)),
                _ => new NodeXml(node)
            };
        }

        public static NodeXml ConvertNodeToXmlTree(this Node node)
        {
            return new NodeXml(BuildNodeXmlTree(node));
        }

        private static XElement BuildNodeXmlTree(Node node)
        {
            NodeXml xml = Create(node);
            string scriptName = GodotResourceHelper.GetScriptName(node);
            if (scriptName == null)
            {
                xml.ScriptAttribute.Remove();
            }
            else
            {
                xml.ScriptAttribute.Value = scriptName;
            }

            IEnumerable<Node> children = node.GetChildren();
            foreach (Node child in children)
            {
                xml.Element.Add(BuildNodeXmlTree(child));
            }
            return xml.Element;
        }
    }
}
