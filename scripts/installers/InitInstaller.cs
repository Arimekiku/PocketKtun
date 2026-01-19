using Godot;
using Scripts.DIContainer;
using Scripts.Services;

namespace Scripts.Installers;

[GlobalClass]
public partial class InitInstaller : Installer
{
    public override void ProcessCreateBinds()
    {
        CreateBind<ILogger, BaseLogger>().AsSingle();
    }
}