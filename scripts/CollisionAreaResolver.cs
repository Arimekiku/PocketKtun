using Godot;

public partial class CollisionAreaResolver : Node
{
	[Export] private Node _collisionContext;

	public Node Context => _collisionContext;
}
