using Godot;
using Scripts.Utils;

namespace Scripts.DIContainer;

[GlobalClass]
internal partial class GlobalContext : GodotContext
{
    public override object ContextObject => null;
}