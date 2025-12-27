using EVLibrary;
using EVLibrary.Extensions;
using EVLibrary.FileIO.Extensions;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EVLibrary.Godot
{
    public static class GodotPrintHelper
    {
        /// <summary>
        /// Build a rich text string. Strings beginning with '[' and ending with ']' are detected as a tag for rich text.<br/>
        /// Using multiple tags in one string does not work.<br/>
        /// Instead, passing multiple tags as individual tags will apply all tags to the previous string that was not a tag.<br/>
        /// <br/><br/>
        /// Also, see here for documentation on what tags are usable within Godot.<br/>
        /// <see href="https://docs.godotengine.org/en/4.4/classes/class_%40globalscope.html#class-globalscope-method-print-rich"/>
        /// 
        /// <code>
        /// // An example of building a string with rich text.
        /// string variableName = "name";
        /// string variableValue = "value";
        /// string typeName = "string";
        /// BuildRichText(
        ///     variableName, "[color=yellow]", // Adding a singular tag to a string.
        ///     " = ", // No additional tags.
        ///     variableValue, "[color=white]", "[u]", // Adding multiple tags to a string.
        ///     " : ",
        ///     $"{typeName}", "[color=red]"
        /// );
        /// 
        /// Output: "[color=yellow]name[/color] = [u][color=white]value[/color][/u] : [color=red]string[/color]"
        /// </code>
        /// </summary>
        /// <param name="args">Tags must start and end with '[' and ']' and must come after the string that it is being added to.</param>
        /// <returns></returns>
        public static string BuildRichText(params string[] args)
        {
            List<string> stringList = new();
            foreach (string arg in args)
            {
                if (arg.StartsWith('[') && arg.EndsWith("]"))
                {
                    string openTag = arg;
                    string closeTag = openTag.Split('=')[0].Replace("[", "[/");
                    if (!closeTag.EndsWith("]"))
                    {
                        closeTag += ']';
                    }
                    stringList[stringList.Count - 1] = openTag + stringList[stringList.Count - 1] + closeTag;
                    continue;
                }
                stringList.Add(arg);
            }
            string compiledString = string.Empty;
            foreach (string str in stringList)
            {
                compiledString += str;
            }
            return compiledString.Trim();
        }

        public static void PrintIDictionary<T1, T2>(IDictionary<T1, T2> dict, int indentDepth = 0)
        {
            string[] valueParams = { "[color=white]", "[u]" };
            foreach (KeyValuePair<T1, T2> entry in dict)
            {
                string[] printParams = {
                    entry.Key.ToString(), "[color=yellow]",
                    " : ",
                    entry.Value.ToString()
                };
                if (entry.Value.GetType() == typeof(bool))
                {
                    if (entry.Value.ToString() == bool.TrueString)
                    {
                        valueParams = new string[] { "[color=green]" };
                    }
                    else
                    {
                        valueParams = new string[] { "[color=red]" };
                    }
                }
                printParams = printParams.Concat(valueParams).ToArray();
                GD.PrintRich(BuildRichText(printParams).Indent(indentDepth));
            }
        }

        public static void PrintIDictionary(IDictionary dict, int indentDepth = 0)
        {
            string[] valueParams = { "[color=white]", "[u]" };
            foreach (DictionaryEntry entry in dict)
            {
                string[] printParams = {
                    entry.Key.ToString(), "[color=yellow]",
                    " : ",
                    entry.Value.ToString()
                };
                if (entry.Value.GetType() == typeof(bool))
                {
                    if (entry.Value.ToString() == bool.TrueString)
                    {
                        valueParams = new string[] { "[color=green]" };
                    }
                    else
                    {
                        valueParams = new string[] { "[color=red]" };
                    }
                }
                printParams = printParams.Concat(valueParams).ToArray();
                GD.PrintRich(BuildRichText(printParams).Indent(indentDepth));
            }
        }

        public static void PrintList(IList list, int indentDepth = 0)
        {
            foreach (var item in list)
            {
                GD.Print($"{item.ToString()}".VisibleIndent(indentDepth));
            }
        }

        public static void PrintIEnumerable(IEnumerable enumerable, int indentDepth = 0)
        {
            foreach (var item in enumerable)
            {
                GD.Print($"{item.ToString()}".VisibleIndent(indentDepth));
            }
        }

        public static void PrintArray<T>(T[] array, int indentDepth = 0)
        {
            foreach (T item in array)
            {
                GD.Print($"{item.ToString()}".VisibleIndent(indentDepth));
            }
        }

        public static void PrintNodeList(IList list, int indentDepth = 0)
        {
            foreach (Node item in list)
            {
                GD.Print($"{item.Name}".VisibleIndent(indentDepth));
            }
        }

        public static void PrintToGodot(string strToPrint, int indentDepth = 0)
        {
            GD.Print($"{strToPrint}".VisibleIndent(indentDepth));
        }

        public static void PrintErrToGodot(string strToPrint, int indentDepth = 0)
        {
            GD.Print($"{strToPrint}".VisibleIndent(indentDepth));
        }

        public static void PrintVariable<T>(string variableName, T variable, int indentDepth = 0)
        {
            if (variable == null)
            {
                GD.PrintRich(BuildRichText(variableName, "[color=yellow]", " = ", "null", "[color=white]", "[u]", " : ", "null", "[color=red]").VisibleIndent(indentDepth));
                return;
            }

            string typeName = variable.GetType().Name;

            if (variable.GetType() == typeof(Action))
            {
                GD.PrintRich(BuildRichText(variableName, "[color=yellow]", " = ", (variable as Action).Method.Name, "[color=white]", "[u]", " : ", "Action", "[color=red]").VisibleIndent(indentDepth));
                return;
            }

            if (variable is IEnumerable && variable is not string)
            {
                string enumberableSubType = DetermineVariableTypeName(variable);
                GD.PrintRich(BuildRichText(variableName, "[color=yellow]", " : ", enumberableSubType, "[color=red]").VisibleIndent(indentDepth));
            }

            if (variable is Node)
            {
                GD.PrintRich(BuildRichText((variable as Node).Name, "[color=yellow]", " : ", $"{variable.GetType().Name}", "[color=red]").VisibleIndent(indentDepth));
                return;
            }

            if (typeof(T).GenericTypeArguments.Contains(typeof(Node)) && !(variable is IDictionary))
            {
                PrintNodeList(variable as IList);
                return;
            }

            if (variable is IDictionary)
            {
                PrintIDictionary(variable as IDictionary);
                return;
            }

            if (variable is IEnumerable && variable.GetType() != typeof(string))
            {
                PrintIEnumerable(variable as IEnumerable);
                return;
            }
            GD.PrintRich(BuildRichText(variableName, "[color=yellow]", " = ", variable.ToString(), "[color=white]", "[u]", " : ", $"{typeName}", "[color=red]").VisibleIndent(indentDepth));
        }

        public static void PrintProperties(string variableName, object variableToPrint, int indentDepth = 0, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance, bool printNullValues = true)
        {
            if (variableToPrint == null)
            {
                return;
            }

            Type type = variableToPrint.GetType();
            IReadOnlyList<PropertyInfo> props = Properties.GetProperties(variableToPrint);

            GD.PrintRich(BuildRichText(variableName, "[color=yellow]", " : ", $"{variableToPrint.GetType().Name}", "[color=red]").VisibleIndent(indentDepth));

            foreach (PropertyInfo property in props)
            {
                if (property.Name == "FunctionGroup")
                {
                    GD.PrintRich(BuildRichText(property.Name, "[color=green]"));
                }

                object propValue = property.GetValue(variableToPrint, null);

                if (!printNullValues && propValue == null) { continue; }
                if (propValue == null)
                {
                    PrintVariable(property.Name, propValue, indentDepth + 1);
                    continue;
                }

                Type valueType = propValue.GetType();
                bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string);
                bool isResource = typeof(Resource).IsAssignableFrom(valueType);
                if (isEnumerable)
                {
                    int index = 0;
                    foreach (var item in (IEnumerable)propValue)
                    {
                        PrintProperties($"{property.Name} (Elem {index})", item, indentDepth + 1, printNullValues: printNullValues);
                        ++index;
                    }
                    continue;
                }
                else if (isResource)
                {
                    PrintProperties(property.Name, (Resource)propValue, indentDepth + 1, printNullValues: printNullValues);
                    continue;
                }
                switch (property.DeclaringType.Name)
                {
                    case nameof(Resource):
                        if (property.Name == "ResourcePath" && !propValue.ToString().Contains("::Resource_"))
                            PrintVariable("ResourceName", Path.GetFileName(propValue.ToString()), indentDepth);
                        continue;
                    case nameof(GodotObject):
                        continue;
                }
                PrintVariable(property.Name, propValue, indentDepth + 1);
            }
            if (indentDepth == 0)
            {
                GD.Print();
            }
        }

        private static string DetermineVariableTypeName<T>(T varType)
        {
            Type[] argTypes = typeof(T).GenericTypeArguments;
            string typeName = typeof(T).Name.Split('`')[0];

            if (argTypes.Length >= 1)
            {
                typeName += "<";
            }
            foreach (Type type in argTypes)
            {
                typeName += $"{type.Name}";
                if (type == argTypes.Last())
                {
                    typeName += ">";
                    break;
                }
                typeName += ", ";
            }
            return typeName;
        }
    }
}