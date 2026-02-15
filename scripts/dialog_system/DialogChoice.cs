using Godot;
using System;

namespace Scripts.DialogSystem;

public class DialogChoice
{
    public readonly string DialogText;
    public readonly string NextBlockId;
    public readonly string ExecuteEventId;
    public readonly string VisibleConditionId;
    
    public bool IsFinishChoice => string.IsNullOrEmpty(NextBlockId);

    public DialogChoice(string dialogText, string nextBlockId)
    {
        DialogText = dialogText;
        NextBlockId = nextBlockId;
    }

    public DialogChoice(string dialogText, string nextBlockId, string executeEventId, string visibleConditionId)
    {
        DialogText = dialogText;
        NextBlockId = nextBlockId;
        ExecuteEventId = executeEventId;
        VisibleConditionId = visibleConditionId;
    }
    
    public override string ToString() => DialogText;
}