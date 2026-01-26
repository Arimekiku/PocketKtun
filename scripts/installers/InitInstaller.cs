using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.MessageManager;
using Scripts.Services;

namespace Scripts.Installers;

[GlobalClass]
public partial class InitInstaller : Installer
{
    public override void ProcessCreateBinds()
    {
        CreateBind<ILogger, BaseLogger>().AsSingle();
        CreateBind<IPlayerInteractorService, PlayerInteractorService>().AsSingle();
        CreateBind<IPlayerInputReceiverService, PlayerInputReceiverService>().AsSingle();
        CreateBind<IDispatcherMapStateService, DispatcherMapStateService>().AsSingle();
        CreateBind<IMessageManager, MessageManager.MessageManager>().AsSingle();
    }
}