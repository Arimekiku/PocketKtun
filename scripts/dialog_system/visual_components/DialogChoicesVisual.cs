using Godot;
using Scripts.Utils;
using System;
using System.Collections.Generic;

namespace Scripts.DialogSystem.Visual;

[GlobalClass]
public partial class DialogChoicesVisual : Node
{
    public event Action<int> OnChoiceButtonPressedEvent;

    [Export] private Container _buttonContainer;
    [Export] private ObjectProvider2D _objectProvider;
    
    private readonly List<DialogChoice> _dialogChoices = new List<DialogChoice>();
    private readonly List<DialogChoiceButton> _choiceButtons = new List<DialogChoiceButton>();
    

    public void Initialize(DialogBlock dialogBlock)
    {
        _dialogChoices.AddRange(dialogBlock.DialogueChoices);
        
        SetDialogBlock();
    }

    public void Deinitialize()
    {
        foreach (var choiceButton in _choiceButtons)
        {
            choiceButton.ButtonPressedEvent -= ChoiceButtonsPressedListener;
            choiceButton.Deinitialize();
            _objectProvider.ReturnObject(choiceButton);
        }
        
        _dialogChoices.Clear();
        _choiceButtons.Clear();
    }

    private void SetDialogBlock()
    {
        for (var i = 0; i < _dialogChoices.Count; ++i)
        {
            var choiceButton = _objectProvider.GetObject<DialogChoiceButton>();
            choiceButton.Initialize(_dialogChoices[i].DialogText, i);
            choiceButton.ButtonPressedEvent += ChoiceButtonsPressedListener;
            choiceButton.Reparent(_buttonContainer);
            
            _choiceButtons.Add(choiceButton);
        }
    }

    private void ChoiceButtonsPressedListener(int buttonIndex)
    {
        OnChoiceButtonPressedEvent?.Invoke(buttonIndex);
    }
}
