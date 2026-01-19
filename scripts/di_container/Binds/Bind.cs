using Godot;
using System;

namespace Scripts.DIContainer;

public class Bind
{
    public Type ContractType;
    public Type RealizationType;

    private BindCondition _bindCondition;
    private BindInstanceCreator _bindInstanceCreator;
    
    public BindCondition BindCondition => _bindCondition ??= new BindCondition();
    public BindInstanceCreator BindInstanceCreator => _bindInstanceCreator ??= new BindInstanceCreator();
    public bool HaveCondition =>  _bindCondition?.IsHaveCondition ?? false;
}