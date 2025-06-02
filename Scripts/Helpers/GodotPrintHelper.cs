using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EVHelpers.Godot
{
    public static class GodotPrintHelper
    {
        public static void PrintDictionary(IDictionary dict)
        {
            GD.Print("--- Dictionary START ---");
            foreach (var key in dict.Keys)
            {
                GD.Print($"{key.ToString()} : {dict[key].ToString()}");
            }
            GD.Print("--- Dictionary END ---");
        }
    }
}