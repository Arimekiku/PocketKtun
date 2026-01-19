using Godot;
using Scripts.InteractSystem;

namespace Scripts.Gameplay;

public partial class LightSwitcher : BaseInteractReaction
{
    [Export] private MeshChanger _meshChanger;
    [Export] private Light3D _connectedLight;
    [Export] private GpuParticles3D _connectedParticles;
    
    public override void InteractReaction()
    {
        _connectedLight.Visible = !_connectedLight.Visible;
        _connectedParticles.SetEmitting(_connectedLight.Visible);
        
        _meshChanger.InteractReaction();
    }
}