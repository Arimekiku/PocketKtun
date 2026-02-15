using Godot;
using System;

namespace Scripts.DialogSystem;

[GlobalClass]
public partial class DialogChoiceButton : Button
{
    public event Action<int> ButtonPressedEvent;
    
    private int _buttonIndex;
    
    public void Initialize(string buttonText, int buttonIndex)
    {
        _buttonIndex = buttonIndex;
        
        Text = buttonText;
        Text = buttonText;
        Pressed += ButtonPressedListener;
    }

    public void Deinitialize()
    {
        Pressed -= ButtonPressedListener;
    }

    private void ButtonPressedListener()
    {
        ButtonPressedEvent?.Invoke(_buttonIndex); 
    }
}