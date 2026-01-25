#if TOOLS
using Godot;
using System;

[Tool]
public partial class WindowIdsPlugin : EditorPlugin
{
    private Control _panel;
    
    public override void _EnterTree()
    {
        _panel = GD.Load<PackedScene>("res://addons/window_ids/window_ids_panel.tscn").Instantiate() as Control;
        
        EditorInterface.Singleton.GetEditorMainScreen().AddChild(_panel);
    }

    public override void _ExitTree()
    {
        _panel.QueueFree();
    }

    public override bool _HasMainScreen() => true;

    public override string _GetPluginName() => "WindowIds";
}
#endif