namespace Scripts.DialogSystem;

public interface IDialogControl
{
    public void StartDialog(string startBlockId);
    public void EndDialog();
    public void ForceEndDialog();
}