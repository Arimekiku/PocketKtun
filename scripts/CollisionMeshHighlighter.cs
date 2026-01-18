using Godot;

namespace Scripts.Gameplay;

public partial class CollisionMeshHighlighter : RaycastTarget
{
	[Export] private MeshInstance3D _mesh;
	
	public override void OnRaycastIn()
	{
		var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
		mat?.SetShaderParameter("outline_width", 2.0f);
	}

	public override void OnRaycastOut()
	{
		var mat = _mesh.GetActiveMaterial(0).NextPass as ShaderMaterial;
		mat?.SetShaderParameter("outline_width", 0.0f);
	}
}
