using Godot;
using Scripts.DIContainer;
using Scripts.Systems.WindowSystem;

namespace Scripts.Installers;

[GlobalClass]
public partial class WindowSystemInstaller : Installer 
{
    public override void ProcessCreateBinds()
    {
        CreateBind<IWindowControl, WindowControl>().AsSingle();
        CreateBind<IWindowRegistry, WindowRegistry>().AsSingle();
    }
}