using EVLibrary.Godot;
using EVLibrary.Godot.XML;
using EVLibrary.Xml;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AM.ModelViewerTool
{
    [Tool]
    public sealed partial class DebugSettingsViewer : Node
    {
        private XElement _debugSettingsXML;

        public override Array<Dictionary> _GetPropertyList()
        {
            if (!File.Exists(PathReferences.GetAbsolutePathToDebugSettingsPathFile()))
            {
                return base._GetPropertyList();
            }

            try
            {
                _debugSettingsXML = XElement.Load(PathReferences.GetAbsolutePathToDebugSettingsPathFile()); 
            }
            catch
            {
                return base._GetPropertyList();
            }
            Godot.Collections.Array<Dictionary> props = new();

            IEnumerable<XElement> elementList = _debugSettingsXML.Elements();
            foreach (XElement element in elementList)
            {
                BoolXml boolXML = element.ConvertToBoolXML();
                props.Add(
                    new Godot.Collections.Dictionary()
                    {
                        {"name", "bool_" + boolXML.Element.Name.ToString()},
                        {"type", (int)Godot.Variant.Type.Bool}
                    }
                );
            }
            return props;
        }

        public override Variant _Get(StringName property)
        {
            if (_debugSettingsXML == null) { return default; }
            string propertyName = property.ToString();
            string propertyNameNoSuffix = propertyName.Substring("bool_".Length);
            if (propertyName.StartsWith("bool_"))
            {
                string stringValue = _debugSettingsXML.Element(propertyNameNoSuffix).Value;
                bool value = bool.Parse(stringValue);
                return value;
            }

            return default;
        }

        public override bool _Set(StringName property, Variant value)
        {
            if (_debugSettingsXML == null) { return false; }
            string settingsPath = PathReferences.GetAbsolutePathToDebugSettingsPathFile();
            string propertyName = property.ToString();
            string propertyNameNoSuffix = propertyName.Substring("bool_".Length);
            if (propertyName.StartsWith("bool_"))
            {
                _debugSettingsXML.Element(propertyNameNoSuffix).Value = value.AsBool().ToString();

                GD.Print($"Setting {propertyNameNoSuffix} to {value.ToString()}");
                File.WriteAllBytes(settingsPath, _debugSettingsXML.ToString().ToUtf8Buffer());
                bool state = value.As<bool>();
                return true;
            }

            return false;
        }
    }
}