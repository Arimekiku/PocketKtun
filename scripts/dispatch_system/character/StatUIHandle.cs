using Godot;
using Scripts.UI;

namespace Scripts.DispatchSystem;

public partial class StatUIHandle : Control
{
    [Export] private Label _nameLabel;
    [Export] private FollowProgressBar _progressBar;

    public void FillStat(string statName, int statValue)
    {
        _nameLabel.Text = statName;
        _progressBar.Value = statValue;
    }
}