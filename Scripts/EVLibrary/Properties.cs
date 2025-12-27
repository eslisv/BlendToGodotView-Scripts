using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EVLibrary
{
    public static class Properties
    {
        public static IReadOnlyList<PropertyInfo> GetProperties(object obj, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return obj.GetType().GetProperties(bindingFlags).ToList().AsReadOnly();
        }

        public static IReadOnlyList<PropertyInfo> GetPropertiesRecursive(object obj, BindingFlags bindingflags = BindingFlags.Public | BindingFlags.Instance, params Type[] customNestedTypes)
        {
            if (obj == null) { return null; }

            Type type = obj.GetType();
            IReadOnlyList<PropertyInfo> props = GetProperties(obj).ToList();
            List<PropertyInfo> returnList = new List<PropertyInfo>();

            foreach (PropertyInfo property in props)
            {
                object propValue = property.GetValue(obj, null);
                if (propValue == null) { continue; }
                Type valueType = propValue.GetType();
                bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string);

                if (isEnumerable)
                {
                    foreach (var item in (IEnumerable)propValue)
                    {
                        returnList.Add(property);
                        returnList.AddRange(GetPropertiesRecursive(item, bindingflags, customNestedTypes));
                    }
                    continue;
                }
                bool isCustomType = false;
                foreach (Type customType in customNestedTypes)
                {
                    if (customType.IsAssignableFrom(valueType))
                    {
                        isCustomType = true;
                        returnList.Add(property);
                        returnList.AddRange(GetPropertiesRecursive(propValue, bindingflags, customNestedTypes));
                        break;
                    }
                }
                if (isCustomType) { continue; }
                returnList.Add(property);
            }
            return returnList.AsReadOnly();
        }
    }
}
