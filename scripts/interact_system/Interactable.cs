using Godot;
using Scripts.DIContainer;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scripts.InteractSystem;

public partial class Interactable : Node, IInteractable
{
    public event Action<IInteractable> OnInteractEvent;
    public event Action<IInteractable> OnFocusEvent;
    public event Action<IInteractable> OnUnfocusEvent;

    [Export] private float _cooldownInterval;
    [Export] private BaseInteractProcess[] _interactProcesses;
    [Export] private BaseInteractReaction[] _reactions;
    [Export] private bool _needAllTriggers = true;
    [Export] private BaseFocusTrigger[] _triggers;
    
    private readonly Stopwatch _cooldownStopwatch = new Stopwatch();
    private readonly HashSet<IFocusTrigger> _activatedTriggers = new HashSet<IFocusTrigger>();
    
    private ILogger _logger;
    

    public bool IsInFocus => _needAllTriggers && _activatedTriggers.Count == _triggers.Length ||
                             !_needAllTriggers &&  _activatedTriggers.Count != 0;
    
    private bool IsCooldown => _cooldownStopwatch.ElapsedMilliseconds < _cooldownInterval * 1000f;
    
    [Inject]
    public void Constructor(ILogger logger)
    {
        _logger = logger;
    }

    public override void _EnterTree()
    {
        Subscribe();
    }

    public override void _ExitTree()
    {
        Unsubscribe();
    }

    public void Interact()
    {
        _cooldownStopwatch.Restart();
        
        _logger.Log($"Interactable {Name} was interacted");
        
        OnInteractEvent?.Invoke(this);
        
        foreach (var reaction in _reactions)
            reaction.InteractReaction();
    }

    public void Focus()
    {
        _logger.Log($"Interactable {Name} in focus");
        
        OnFocusEvent?.Invoke(this);
        
        foreach (var reaction in _reactions)
            reaction.FocusReaction();
    }

    public void Unfocus()
    {
        _logger.Log($"Interactable {Name} in unfocus");
        
        OnUnfocusEvent?.Invoke(this);
        
        foreach (var reaction in _reactions)
            reaction.UnfocusReaction();
    }

    private void FocusProcess(IFocusTrigger trigger)
    {
        _activatedTriggers.Add(trigger);
        
        if (!IsInFocus)
            return;
        
        Focus();
    }

    private void UnfocusProcess(IFocusTrigger trigger)
    {
        _activatedTriggers.Remove(trigger);
        
        if (IsInFocus)
            return;
        
        Unfocus();
    }
    
    private void TryInteract()
    {
        if (IsCooldown)
        {
            _logger.Log($"Interactable {Name} is cooldown");
            return;
        }
        
        if (!IsInFocus)
        {
            _logger.Log($"Interactable {Name} in unfocus");
            return;
        }
        
        Interact();
    }
    
    private void Subscribe()
    {
        foreach (var interactProcess in _interactProcesses)
            interactProcess.OnInteractProcessEvent += TryInteract;

        foreach (var trigger in _triggers)
        {
            trigger.OnFocusEvent += FocusProcess;
            trigger.OnUnfocusEvent += UnfocusProcess;
        }
    }
    
    private void Unsubscribe()
    {
        foreach (var interactProcess in _interactProcesses)
            interactProcess.OnInteractProcessEvent -= TryInteract;

        foreach (var trigger in _triggers)
        {
            trigger.OnFocusEvent -= FocusProcess;
            trigger.OnUnfocusEvent -= UnfocusProcess;
        }
    }
}