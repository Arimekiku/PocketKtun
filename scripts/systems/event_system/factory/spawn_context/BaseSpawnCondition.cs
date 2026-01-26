using Godot;

namespace Scripts.Systems.EventSystem;

[GlobalClass]
public abstract partial class BaseSpawnCondition : Resource, IEventSpawnCondition
{
    public abstract bool IsMet();
}