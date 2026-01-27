using System;
using Godot;
using Scripts.Systems.InteractSystem;

namespace Scripts.Gameplay.Services;

public interface IPlayerInteractorService
{
    public event Action<CollisionObject3D> OnFocusChangedEvent;
    
    public void RaiseWithRaycast(CollisionObject3D trigger);
}