using System.Collections.Generic;

namespace Scripts.DialogSystem;

public class DialogBlock
{
    public readonly string BlockId;
    public readonly string NpcLine;
    private readonly DialogueChoice[] _dialogueChoices;
    
    public IReadOnlyCollection<DialogueChoice> DialogueChoices => _dialogueChoices;

    public DialogBlock(string blockId, string npcLine, DialogueChoice[] dialogueChoices)
    {
        BlockId = blockId;
        NpcLine = npcLine;
        _dialogueChoices = dialogueChoices;
    }
}