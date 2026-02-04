using Godot;
using Scripts.DialogSystem;
using Scripts.DIContainer;

namespace Scripts.Installers;

[GlobalClass]
public partial class DialogSystemInstaller : Installer
{
    public override void ProcessCreateBinds()
    {
        CreateBind<IDialogBlockProvider, DialogBlockProvider>().AsSingle();
        CreateBind<IDialogControl, DialogControl>().AsSingle();
    }
}