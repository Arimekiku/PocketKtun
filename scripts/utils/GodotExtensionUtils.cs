using Godot;
using System;

namespace Scripts.Utils;

public static class GodotExtensionUtils
{
    public static TScript FindAncestor<TScript>(this Node targetNode) where TScript : Node
    {
        var current = targetNode.GetParent();

        for (; current != null; current = current.GetParent())
        {
            if (current is TScript script)
                return script;
        }
        
        return null;
    }

    public static bool HasAncestor<TScript>(this Node targetNode) where TScript : Node => targetNode.FindAncestor<TScript>() != null;
}