using Godot;

namespace Scripts.Gameplay;

public partial class AnimationLooper : AnimationPlayer
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (!IsPlaying())
		{
			Play("mixamo_com");
		}
	}
}
