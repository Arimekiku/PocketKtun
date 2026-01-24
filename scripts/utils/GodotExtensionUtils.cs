using Godot;
using System;

namespace Scripts.Utils;

public static class GodotExtensionUtils
{
    public static TScript FindParent<TScript>(this Node targetNode) where TScript : Node
    {
        var current = targetNode.GetParent();

        for (; current != null; current = current.GetParent())
        {
            if (current is TScript script)
                return script;
        }
        
        return null;
    }

    public static bool HasParent<TScript>(this Node targetNode) where TScript : Node => targetNode.FindParent<TScript>() != null;

    public static TScript FindChild<TScript>(this Node targetNode) where TScript : Node
    {
        throw new NotImplementedException();
    }
}