using Godot;

namespace Scripts.Gameplay;

public partial class AnimationLooper : AnimationPlayer
{
    [Export] private string _animationName = "mixamo_com";
    
    public override void _Ready()
    {
        if (!IsPlaying())
        {
            Play(_animationName);
        }
    }

    public override void _Process(double delta)
    {
        if (!IsPlaying())
        {
            Play(_animationName);
        }
    }
}
