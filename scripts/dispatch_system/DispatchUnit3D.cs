using Godot;
using Scripts.StateMachine;

namespace Scripts.DispatchSystem;

[GlobalClass]
public partial class DispatchUnit3D : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public Marker3D[] PatrolPoints;
    [Export] public NavigationAgent3D NavAgent;
    [Export] public CharacterResourceDefinition CharacterResource;
    
    public bool IsBusy => _currentState is not PatrolState;
    
    public Character Data;
    
    private BaseState _currentState;

    public override void _Ready()
    {
        Data = CharacterResource.ToCharacter();
        
        ChangeState(new PatrolState());
    }

    public void ChangeState(BaseState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Init(this);
        _currentState.Enter();
    }

    public override void _Process(double delta) => _currentState?.Process(delta);
    public override void _PhysicsProcess(double delta) => _currentState?.PhysicsProcess(delta);

    public void DispatchTo(Vector3 pos, Emergency incident)
    {
        ChangeState(new RespondingState(pos, incident));
    }
}
