using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;

namespace Scripts.Gameplay;

public partial class MeshHighlighter : MeshInstance3D
{
    [Inject] private IPlayerInteractorService _playerInteractor;

    private MeshInstance3D _target;

    public override void _Notification(int what)
    {
        if (what == NotificationReady)
            _playerInteractor.OnMeshChangedEvent += Highlight;
        
        if (what == NotificationPredelete)
            _playerInteractor.OnMeshChangedEvent -= Highlight;
    }

    private void Highlight(MeshInstance3D targetMesh)
    {
        var mat = MaterialOverride as ShaderMaterial;
        _target = targetMesh;
        
        if (targetMesh == null)
        {
            mat!.SetShaderParameter("outline_width", 0.0);
            return;
        }
        
        Mesh = targetMesh.Mesh;
        
        Reparent(targetMesh);
        Position = Vector3.Zero;
        Rotation = Vector3.Zero;
        Scale = Vector3.One;
        
        mat!.SetShaderParameter("outline_width", 1.0);
    }
}