using System;

namespace Scripts.InteractSystem;

public interface IInteractProcess
{
    public event Action OnInteractProcessEvent;

    public void InteractProcess();
}