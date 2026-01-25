using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Scripts.DispatchSystem;

[GlobalClass]
public partial class EmergencyResourceDefinition : Resource
{
    [Export] public string Name { get; private set; } = "New Emergency";
    [Export] public float TimeToResolve { get; set; } = 5f;
    [Export] public Texture2D Icon { get; set; }
    [Export] private Godot.Collections.Dictionary<StatType, int> _stats = [];
    
    public override void _ValidateProperty(Godot.Collections.Dictionary property)
    {
        if (property["name"].AsString() == nameof(_stats))
            EnsureKeys();
    }
    
    public Emergency ToEmergency()
    {
        return new Emergency(TimeToResolve, _stats.ToDictionary());
    }

    private void EnsureKeys()
    {
        var values = Enum.GetValues(typeof(StatType)) as StatType[];
        
        foreach (var statType in values!)
            _stats.TryAdd(statType, 0);
    }
}