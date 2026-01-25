using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utils;

public static class GodotExtensionUtils
{
    public static TScript FindParent<TScript>(this Node targetNode) where TScript : class
    {
        var current = targetNode.GetParent();

        for (; current != null; current = current.GetParent())
        {
            if (current is TScript script)
                return script;
        }
        
        return null;
    }

    public static bool HasParent<TScript>(this Node targetNode) where TScript : class => targetNode.FindParent<TScript>() != null;

    public static TScript FindChild<TScript>(this Node targetNode) where TScript : class
    {
        var children = targetNode.GetChildren();

        if (children.FirstOrDefault(node => node is TScript) is TScript script) 
            return script;
        
        foreach (var child in children)
        {
            var scriptInChild = child.FindChild<TScript>();
            
            if (scriptInChild == null)
                continue;

            return scriptInChild;
        }

        return null;
    }

    public static List<TScript> FindChildren<TScript>(this Node targetNode) where TScript : class
    {
        var children = targetNode.GetChildren();

        var findChildren = children.OfType<TScript>().ToList();

        foreach (var child in children)
            findChildren.AddRange(FindChildren<TScript>(child));
        
        return findChildren;
    }
}