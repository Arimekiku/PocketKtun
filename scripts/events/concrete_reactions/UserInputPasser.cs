using Godot;
using GodotInterfaceExport;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.Systems.InteractSystem;

namespace Scripts.Gameplay;

public partial class UserInputPasser : BaseInteractReaction
{
    [Export, ExportInterface(typeof(IInputReceiver))] private Node InputReceiver { get; set; }

    [Inject] private IPlayerInputReceiverService _inputReceiverService;
    
    private IInputReceiver _receiver;

    private bool _passed;
    
    public override void _Ready()
    {
        _receiver = InputReceiver as IInputReceiver;
    }

    public override void InteractReaction()
    {
        _passed = _receiver.ToggleReceive();
        _inputReceiverService.ToggleReceive();
    }
}