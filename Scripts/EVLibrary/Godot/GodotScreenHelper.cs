using Godot;
using System;


namespace EVLibrary.Windows
{
    public static class GodotScreenHelper
    {
        public static void GetTotalScreenSize()
        {
            int count = DisplayServer.GetScreenCount();
            Vector2I[] screenSizes = new Vector2I[count];
            for (int i = 0; i < count; i++)
            {
                screenSizes[i] = DisplayServer.ScreenGetSize(i);
            }
        }
    }
}
