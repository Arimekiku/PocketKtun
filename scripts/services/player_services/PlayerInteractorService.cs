using System;
using Godot;

namespace Scripts.Gameplay.Services;

public class PlayerInteractorService : IPlayerInteractorService
{
    public event Action<CollisionObject3D> OnFocusChangedEvent;
    public event Action<MeshInstance3D> OnMeshChangedEvent;

    public void RaiseWithRaycast(CollisionObject3D trigger)
    {
        OnFocusChangedEvent!.Invoke(trigger);
    }

    public void RaiseWithMesh(MeshInstance3D trigger)
    {
        OnMeshChangedEvent!.Invoke(trigger);
    }
}