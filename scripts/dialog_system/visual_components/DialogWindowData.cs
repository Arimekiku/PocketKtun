using Scripts.WindowSystem;
using System;

namespace Scripts.DialogSystem.Visual;

public class DialogWindowData : WindowData
{
    public readonly string StartDialogBlockId;
    public readonly Action<DialogWindowControl> OnInitializeDialogWindowControl;
    public readonly Action OnDeinitializeDialogWindowControl;
    
    public DialogWindowData(WindowIds windowId, string startDialogBlockId,
                            Action<DialogWindowControl> onInitializeDialogWindowControl = null,
                            Action onDeinitializeDialogWindowControl = null) : base(windowId)
    {
        StartDialogBlockId = startDialogBlockId;
        OnInitializeDialogWindowControl = onInitializeDialogWindowControl;
        OnDeinitializeDialogWindowControl = onDeinitializeDialogWindowControl;
    }
}