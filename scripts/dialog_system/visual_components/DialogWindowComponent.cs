using Godot;
using Scripts.DIContainer;
using Scripts.Localization;
using Scripts.WindowSystem;

namespace Scripts.DialogSystem.Visual;

[GlobalClass]
public partial class DialogWindowComponent : Node, IOpenWindowComponent, ICloseWindowComponent
{
    [Export] private DialogWindowControl _dialogWindowControl;
    
    private DialogWindowData _dialogWindowData;
    
    public void Open(WindowData windowData)
    {
        _dialogWindowData = windowData as DialogWindowData;
        
        if (_dialogWindowData == null)
            return;
        
        if (_dialogWindowData.WindowId != WindowIds.TestDialogWindow)
            return;
        
        _dialogWindowControl.Initialize(_dialogWindowData.StartDialogBlockId);
        
        _dialogWindowData.OnInitializeDialogWindowControl?.Invoke(_dialogWindowControl);
    }

    public void Close()
    {
        _dialogWindowControl.Deinitialize();
        _dialogWindowData.OnDeinitializeDialogWindowControl?.Invoke();
        
        _dialogWindowData = null;
    }
}