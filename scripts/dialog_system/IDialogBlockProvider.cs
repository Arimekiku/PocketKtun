namespace Scripts.DialogSystem;

public interface IDialogBlockProvider
{
    public DialogBlock GetDialogBlock(string blockId);
}