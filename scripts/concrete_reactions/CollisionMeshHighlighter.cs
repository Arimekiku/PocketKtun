using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.InteractSystem;

namespace Scripts.Gameplay;

public partial class CollisionMeshHighlighter : BaseInteractReaction
{
    [Inject] private IPlayerInteractorService _playerInteractor;
    
    [Export] private MeshInstance3D _mesh;

    public override void FocusReaction()
    {
        var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
        mat?.SetShaderParameter("outline_width", 1.0);
    }

    public override void UnfocusReaction()
    {
        var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
        mat?.SetShaderParameter("outline_width", 0.0);
    }
}
