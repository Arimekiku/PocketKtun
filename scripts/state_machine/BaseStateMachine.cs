using Godot;
using Scripts.Utils;
using System;
using System.Collections.Generic;

namespace Scripts.StateMachine;

public abstract partial class BaseStateMachine<TState> : Node where TState : Enum
{
    private IState _currentState;
    private Dictionary<TState, IState> _states;
    
    public TState CurrentState { get; private set; }
    
    private bool IsStateMachineCreated => _states != null;

    protected abstract void CreateStateMachine();
    
    public override void _Process(double delta)
    {
        if (!IsStateMachineCreated)
            return;
        
        if (_currentState == null)
            return;
        
        _currentState.Update(delta);
    }
    
    protected void InitializeStateMachine()
    {
        _states = new Dictionary<TState, IState>();

        for (var i = 0; Enum.IsDefined(typeof(TState), i); ++i)
            _states.Add(EnumConverter<TState>.ToEnum(i), null);
    }

    protected void InitializeState(TState stateType, Action onEnter = null, Action onExit = null, Action<double> onUpdate = null)
    {
        InitializeState(stateType, new BaseState(onEnter, onExit, onUpdate));
    }

    protected void InitializeState(TState stateType, IState state)
    {
        if (!_states.ContainsKey(stateType))
            throw new Exception($"State {stateType} not found. Maybe state machine is not initialized.");

        _states[stateType] = state;
    }
    
    protected void SetState(TState stateType)
    {
        ExceptionsUtils.ThrowIfNull(_states, $"State machine {Name} is not initialized");
        
        _currentState?.ExitState();

        CurrentState = stateType;
        _currentState = _states[CurrentState];
        _currentState.EnterState();
    }
}