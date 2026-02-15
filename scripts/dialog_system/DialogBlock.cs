using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scripts.DialogSystem;

public class DialogBlock
{
    public readonly string BlockId;
    public readonly DialogLine DialogLine;
    private readonly DialogChoice[] _dialogueChoices;
    
    public IReadOnlyCollection<DialogChoice> DialogueChoices => _dialogueChoices;
    
    public DialogBlock(string blockId, DialogLine dialogLine, DialogChoice[] dialogueChoices)
    {
        BlockId = blockId;
        DialogLine = dialogLine;
        _dialogueChoices = dialogueChoices;
    }
}