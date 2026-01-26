using Godot;

namespace Scripts.Systems.DispatchSystem;

public partial class CharacterUIHandle : Control
{
    [Export] public Label NameLabel;
    [Export] public TextureRect PortraitImage;
    [Export] public VBoxContainer StatContainer;
    [Export] public PackedScene StatRowTemplate;

    public void UpdateDisplay(DispatchUnit3D unit)
    {
        NameLabel.Text = unit.Data.Name;
        PortraitImage.Texture = unit.Data.Portrait;
        
        foreach (var child in StatContainer.GetChildren())
            child.QueueFree();

        foreach (var stat in unit.Data.Stats)
        {
            var row = StatRowTemplate.Instantiate<StatUIHandle>();
            row.FillStat(stat.Key.ToString(), stat.Value);
            StatContainer.AddChild(row);
        }
    }
}