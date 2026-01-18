using Godot;

namespace Scripts.Gameplay;

public partial class NodeRotator : Node3D
{
	[Export] private float _degreesPerSecond = 15f;
	
	public override void _Process(double delta)
	{
		var deltaDegrees = _degreesPerSecond * (float)delta;
		Rotate(Vector3.Up, Mathf.DegToRad(deltaDegrees));
	}
}
