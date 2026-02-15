using Scripts.DialogSystem.Visual;
using Scripts.DIContainer;
using Scripts.Services;
using Scripts.WindowSystem;
using System;

namespace Scripts.DialogSystem;

public class DialogControl : IDialogControl
{
    public event Action OnDialogEndedEvent;
    public event Action OnDialogForceEndedEvent;
    
    private IWindowControl _windowControl;
    private ILogger _logger;

    private DialogWindowControl _dialogWindowControl;
    
    public bool IsDialogActive { get; private set; }

    [Inject]
    public DialogControl(IWindowControl windowControl, ILogger logger)
    {
        _windowControl = windowControl;
        _logger = logger;
    }
    
    public void StartDialog(string startBlockId)
    {
        if (IsDialogActive)
        {
            _logger.LogWarning("Dialog already active.");
            return;
        }

        if (string.IsNullOrEmpty(startBlockId))
        {
            _logger.LogError("StartBlockId string is null or empty.");
            return;
        }
        
        _logger.Log($"Dialog started with id: {startBlockId}");
        var dialogWindowData = new DialogWindowData(WindowIds.TestDialogWindow, startBlockId, OnInitializeDialogWindowControl);
        _windowControl.OpenWindow(dialogWindowData);
        
        IsDialogActive = true;
    }
    
    public void ForceEndDialog()
    {
        _windowControl.CloseWindow(WindowIds.TestDialogWindow);
        
        _dialogWindowControl.OnDialogEndedEvent -= EndDialog;
        _dialogWindowControl = null;
        
        IsDialogActive = false;
        OnDialogForceEndedEvent?.Invoke();
    }

    
    private void EndDialog()
    {
        _windowControl.CloseWindow(WindowIds.TestDialogWindow);
        
        _dialogWindowControl.OnDialogEndedEvent -= EndDialog;
        _dialogWindowControl = null;
        
        IsDialogActive = false;
        OnDialogEndedEvent?.Invoke();
    }
    
    private void OnInitializeDialogWindowControl(DialogWindowControl dialogWindowControl)
    {
        _dialogWindowControl = dialogWindowControl;
        _dialogWindowControl.OnDialogEndedEvent += EndDialog;
    }
}