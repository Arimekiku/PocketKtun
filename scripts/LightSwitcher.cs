using Godot;

namespace Scripts.Gameplay;

public partial class LightSwitcher : InteractableTarget
{
    [Export] private Light3D _connectedLight;
    [Export] private GpuParticles3D _connectedParticles;
    
    public override void OnInteract()
    {
        _connectedLight.Visible = !_connectedLight.Visible;
        _connectedParticles.SetEmitting(_connectedLight.Visible);
    }
}