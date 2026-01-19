using Godot;
using Godot.Collections;

namespace Scripts.Gameplay;

[GlobalClass]
public partial class MeshChanger : InteractableTarget
{
    [Export] private Node3D _targetMesh;
    [Export] private Array<PackedScene> _meshes;

    private int _currentMeshIndex;

    public override void OnInteract()
    {
        _targetMesh?.QueueFree();

        _currentMeshIndex = (_currentMeshIndex + 1) % _meshes.Count;
        _targetMesh = _meshes[_currentMeshIndex].Instantiate<Node3D>();
        AddChild(_targetMesh);
    }
}