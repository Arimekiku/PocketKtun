#if TOOLS
using Godot;
using Scripts.Utils.SheetParser;

[Tool]
public partial class ParserMenu : EditorPlugin
{
    private MenuButton _myMenuButton;

    public override void _EnterTree()
    {
        _myMenuButton = new MenuButton();
        _myMenuButton.Text = "Parser";

        var popup = _myMenuButton.GetPopup();
        popup.AddItem("BalanceParser", 0);
        popup.AddItem("LocalizationParser", 1);

        popup.IdPressed += OnIdPressed;

        AddControlToContainer(CustomControlContainer.Toolbar, _myMenuButton);
    }

    public override void _ExitTree()
    {
        if (_myMenuButton == null) 
            return;
        RemoveControlFromContainer(CustomControlContainer.Toolbar, _myMenuButton);
        _myMenuButton.QueueFree();
    }

    private void OnIdPressed(long id)
    {
        if (id == 0)
        {
            var parser = new BalanceDataParser(new CredentialProvider());
            parser.Initialize();
            parser.ParseData();
            return;
        }
        
        if (id == 1)
        {
            var parser = new LocalizationParser(new CredentialProvider());
            parser.Initialize();
            parser.ParseData();
            return;
        }
    }
}
#endif
