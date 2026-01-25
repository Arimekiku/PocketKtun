using System.Collections.Generic;
using Godot;

namespace Scripts.DispatchSystem;

public class Character
{
    public string Name { get; private set; }
    public Texture2D Portrait { get; private set; }
    public Dictionary<StatType, int> Stats { get; private set; }
    
    public Character(string name, Texture2D portrait, Dictionary<StatType, int> stats)
    {
        Name = name;
        Portrait = portrait;
        Stats = stats;
    }
}