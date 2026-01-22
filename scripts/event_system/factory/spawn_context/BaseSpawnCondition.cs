using Godot;

namespace Scripts.EventSystem;

[GlobalClass]
public abstract partial class BaseSpawnCondition : Resource, IEventSpawnCondition
{
    public abstract bool IsMet();
}