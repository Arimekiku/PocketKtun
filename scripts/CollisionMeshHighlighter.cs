using Godot;

namespace Scripts.Gameplay;

public partial class CollisionMeshHighlighter : Node, IRaycastable
{
	[Export] private MeshInstance3D _mesh;
	
	public void OnRaycastIn()
	{
		var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
		mat?.SetShaderParameter("outline_width", 2.0f);
	}

	public void OnRaycastOut()
	{
		var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
		mat?.SetShaderParameter("outline_width", 0.0f);
	}
}
