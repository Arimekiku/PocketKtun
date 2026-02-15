using Godot;
using Scripts.StateMachine;
using Scripts.Utils;
using System.Collections.Generic;

namespace Scripts.WindowSystem;

[GlobalClass]
public partial class Window : BaseStateMachine<Window.WindowStates>, IWindow
{
    public enum WindowStates
    {
        Init,
        Close,
        Open,
        Focus,
        Unfocus,
    }
    
    [Export] private WindowIds _windowId;
    [Export] private CanvasItem _visual;
    
    private WindowData _currentData;
    private BaseZStrategy _zStrategy;
    private List<IOpenWindowComponent>  _openWindowComponents;
    private List<ICloseWindowComponent> _closeWindowComponents;
    private List<IFocusWindowComponent> _focusWindowComponents;
    private List<IUnfocusWindowComponent> _unfocusWindowComponents;
    
    public WindowIds Id => _windowId;
    public float VisualZ { get; private set; }
    
    protected override void CreateStateMachine()
    {
        InitializeStateMachine();
        InitializeState(WindowStates.Init, onEnter: InitEnter);
        InitializeState(WindowStates.Close, onEnter: CloseEnter);
        InitializeState(WindowStates.Open, onEnter: OpenEnter);
        InitializeState(WindowStates.Focus, onEnter: FocusEnter);
        InitializeState(WindowStates.Unfocus, onEnter: UnfocusEnter);
    }

    private void InitEnter()
    {
        _openWindowComponents = this.FindChildren<IOpenWindowComponent>();
        _closeWindowComponents = this.FindChildren<ICloseWindowComponent>();
        _focusWindowComponents = this.FindChildren<IFocusWindowComponent>();
        _unfocusWindowComponents = this.FindChildren<IUnfocusWindowComponent>();
        
        _zStrategy = this.FindChild<BaseZStrategy>();
    }
    
    public void Initialize()
    {
        CreateStateMachine();
        SetState(WindowStates.Init);
    }
    
    public void OpenWindow(WindowData windowData)
    {
        _currentData = windowData;
        SetState(WindowStates.Open);
    }
    public void CloseWindow()
    {
        _currentData = null;
        SetState(WindowStates.Close);
    }
    public void FocusWindow() => SetState(WindowStates.Focus);
    
    public void UnfocusWindow() => SetState(WindowStates.Unfocus);
    
    private void OpenEnter()
    {
        VisualZ = _zStrategy?.GetZ() ?? 0;
        _visual.ZIndex = (int)VisualZ;
        _visual.Visible = true;
        
        if (_openWindowComponents.IsNullOrEmpty())
            return;
        
        foreach (var component in _openWindowComponents)
            component.Open(_currentData);
    }
    
    private void CloseEnter()
    {
        _visual.Visible = false;
        VisualZ = 0;
        
        if (_openWindowComponents.IsNullOrEmpty())
            return;
        
        foreach (var component in _closeWindowComponents)
            component.Close();
    }

    private void FocusEnter()
    {
        if (_focusWindowComponents.IsNullOrEmpty())
            return;
        
        foreach (var component in _focusWindowComponents)
            component.Focus();
    }

    private void UnfocusEnter()
    {
        if (_unfocusWindowComponents.IsNullOrEmpty())
            return;
        
        foreach (var component in _unfocusWindowComponents)
            component.Unfocus();
    }
}