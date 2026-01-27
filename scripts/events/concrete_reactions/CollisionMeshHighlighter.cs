using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.Systems.InteractSystem;

namespace Scripts.Gameplay;

public partial class CollisionMeshHighlighter : BaseInteractReaction
{
    [Inject] private IPlayerInteractorService _playerInteractor;
    
    [Export] private MeshInstance3D _mesh;
    
    public override void FocusReaction()
    {
        _playerInteractor.RaiseWithMesh(_mesh);
    }

    public override void UnfocusReaction()
    {
        _playerInteractor.RaiseWithMesh(null);
    }
}
