using System;
using Godot;

namespace Scripts.Systems.DispatchSystem;

[GlobalClass]
public partial class DispatchUnit3D : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public Marker3D[] PatrolPoints;
    [Export] public NavigationAgent3D NavAgent;
    [Export] public CharacterResourceDefinition CharacterResource;

    public event Action<Vector3, Emergency> OnDispatchedEvent; 
    
    public bool IsBusy { get; set; }
    
    public Character Data;
    
    public override void _Ready()
    {
        Data = CharacterResource.ToCharacter();
    }

    public void DispatchTo(Vector3 pos, Emergency incident)
    {
        OnDispatchedEvent!.Invoke(pos, incident);
    }
}
