using System;

namespace Scripts.DialogSystem;

public interface IDialogControl
{
    public event Action OnDialogForceEndedEvent;
    public event Action OnDialogEndedEvent;
    
    public bool IsDialogActive { get; }

    public void StartDialog(string startBlockId);
    public void ForceEndDialog();
}