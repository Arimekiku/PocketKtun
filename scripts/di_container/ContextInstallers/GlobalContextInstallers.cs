using Godot;
using Scripts.Utils;

namespace Scripts.DIContainer;

[GlobalClass]
internal partial class GlobalContextInstallers : GodotContextInstallers
{
    public override object ContextObject => null;
}