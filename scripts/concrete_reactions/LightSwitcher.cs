using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Messages;
using Scripts.InteractSystem;
using Scripts.MessageManager;

namespace Scripts.Gameplay;

public partial class LightSwitcher : BaseInteractReaction
{
    [Export] private MeshChanger _meshChanger;
    [Export] private Light3D _connectedLight;
    [Export] private GpuParticles3D _connectedParticles;
    
    [Inject] private IMessageManager _messageManager;
    
    public override void InteractReaction()
    {
        _connectedLight.Visible = !_connectedLight.Visible;
        _connectedParticles.SetEmitting(_connectedLight.Visible);
        _meshChanger.InteractReaction();

        var message = new GameMessage(GameMessages.LightSwitchToggled)
        {
            Bool = _connectedLight.Visible
        };
        _messageManager.Publish(message);
    }
}