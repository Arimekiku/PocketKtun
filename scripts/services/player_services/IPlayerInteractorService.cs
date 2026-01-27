using System;
using Godot;

namespace Scripts.Gameplay.Services;

public interface IPlayerInteractorService
{
    public event Action<CollisionObject3D> OnFocusChangedEvent;
    public event Action<MeshInstance3D> OnMeshChangedEvent;
    
    public void RaiseWithRaycast(CollisionObject3D trigger);
    public void RaiseWithMesh(MeshInstance3D trigger);
}