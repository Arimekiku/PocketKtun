using Godot;
using System;

namespace Scripts.DialogSystem;

public class DialogueChoice
{
    public readonly string DialogText;
    public readonly string NextBlockId;
    public readonly Action ChoiceAction; 
    
    private readonly Func<bool> VisibleCondition; 

    public bool IsFinishChoice => string.IsNullOrEmpty(NextBlockId);
    public bool IsVisible => VisibleCondition?.Invoke() ?? true;

    public DialogueChoice(string dialogText, string nextBlockId)
    {
        DialogText = dialogText;
        NextBlockId = nextBlockId;
        
        ChoiceAction = () => {GD.Print($"Player choice: {ToString()}");};
    }
    
    public override string ToString() => DialogText;
}