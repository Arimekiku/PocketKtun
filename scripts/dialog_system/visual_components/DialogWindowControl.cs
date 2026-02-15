using Godot;
using Scripts.DIContainer;
using Scripts.Services;
using System;
using System.Linq;

namespace Scripts.DialogSystem.Visual;

[GlobalClass]
public partial class DialogWindowControl : Node
{
    public event Action OnDialogEndedEvent;
    
    [Export] private DialogChoicesVisual _dialogChoicesVisual;
    [Export] private DialogNpcLineVisual _npsLineVisual;
    
    private DialogBlock _currentDialogBlock;
    
    private ILogger _logger;
    private IDialogBlockProvider _dialogBlockProvider;
    
    [Inject]
    public void Construct(ILogger logger, IDialogBlockProvider dialogBlockProvider)
    {
        _dialogBlockProvider = dialogBlockProvider;
        _logger = logger;
    }
    
    public void Initialize(string startDialogBlockId)
    {
        _dialogChoicesVisual.OnChoiceButtonPressedEvent += SelectChoiceListener;

        _currentDialogBlock = _dialogBlockProvider.GetDialogBlock(startDialogBlockId);
        SetDialogBlock(_currentDialogBlock);
    }

    public void Deinitialize()
    {
        _dialogChoicesVisual.OnChoiceButtonPressedEvent -= SelectChoiceListener;

        _currentDialogBlock = null;
        _dialogChoicesVisual.Deinitialize();
        _npsLineVisual.Deinitialize();
    }
    
    private void SetDialogBlock(DialogBlock dialogBlock)
    {
        _dialogChoicesVisual.Deinitialize();
        _npsLineVisual.Deinitialize();
        
        _dialogChoicesVisual.Initialize(dialogBlock);
        _npsLineVisual.Initialize(dialogBlock);
    }

    private void SelectChoiceListener(int choiceIndex)
    {
        var choice = _currentDialogBlock.DialogueChoices.ElementAt(choiceIndex);
        var nextBlockId = choice.NextBlockId;
        
        _logger.Log($"Choice {choiceIndex} was selected, next Block Id {nextBlockId}");

        if (string.IsNullOrEmpty(nextBlockId))
        {
            OnDialogEndedEvent?.Invoke();
            return;
        }
        
        _currentDialogBlock = _dialogBlockProvider.GetDialogBlock(nextBlockId);
        
        SetDialogBlock(_currentDialogBlock);
    }
}
