using System;

namespace Scripts.Systems.InteractSystem;

public interface IInteractProcess
{
    public event Action OnInteractProcessEvent;

    public void InteractProcess();
}