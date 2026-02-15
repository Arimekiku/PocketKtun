using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.MessageManager;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Installers;

[GlobalClass]
public partial class InitInstaller : Installer
{
    public override void ProcessCreateBinds()
    {
        CreateBind<ILogger, BaseLogger>().AsSingle();
        CreateBind<IPlayerInteractorService, PlayerInteractorService>().AsSingle();
        CreateBind<IMessageManager, MessageManager.MessageManager>().AsSingle();
    }
}