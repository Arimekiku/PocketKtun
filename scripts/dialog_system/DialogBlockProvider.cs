using System;
using System.Collections.Generic;

namespace Scripts.DialogSystem;

public class DialogBlockProvider : IDialogBlockProvider
{
    private Dictionary<string, DialogBlock> _dialogBlocks = new Dictionary<string, DialogBlock>();

    public DialogBlock GetDialogBlock(string blockId) => _dialogBlocks[blockId];
}