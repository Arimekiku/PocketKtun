using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Scripts.Systems.DispatchSystem;

[GlobalClass]
public partial class CharacterResourceDefinition : Resource
{
    [Export] private string _characterName = "New Character";
    [Export] private Texture2D _characterPortrait;
    [Export] private Godot.Collections.Dictionary<StatType, int> _stats = [];
    
    public override void _ValidateProperty(Godot.Collections.Dictionary property)
    {
        if (property["name"].AsString() == nameof(_stats))
            EnsureKeys();
    }

    public Character ToCharacter()
    {
        var value = new Character(_characterName, _characterPortrait, _stats.ToDictionary());
        
        return value;
    }
    
    private void EnsureKeys()
    {
        var values = Enum.GetValues(typeof(StatType)) as StatType[];
        
        foreach (var statType in values!)
            _stats.TryAdd(statType, 0);
    }
}