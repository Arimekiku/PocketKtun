namespace Scripts.StateMachine;

public interface IState
{
    public void EnterState();
    public void ExitState();
    public void Update(double delta);
}