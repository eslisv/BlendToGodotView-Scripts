using Godot;

namespace EVLibrary.Godot
{
    public static class GodotNodeHelper
    {
        public static T AddScript<T>(Node node, Resource script)
        {
            ulong instanceId = node.GetInstanceId();
            node.SetScript(script);
            return (T)(object)GodotObject.InstanceFromId(instanceId);
        }
    }
}
