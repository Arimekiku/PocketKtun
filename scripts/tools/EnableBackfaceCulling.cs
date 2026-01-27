using Godot;

[Tool]
public partial class EnableBackfaceCulling : EditorScript
{
    public override void _Run()
    {
        var root = EditorInterface.Singleton.GetEditedSceneRoot();
        if (root == null)
        {
            GD.PrintErr("No scene loaded.");
            return;
        }

        var modified = 0;
        ProcessNode(root, ref modified);
        GD.Print($"Backface culling fixed on {modified} material(s).");
    }

    private void ProcessNode(Node node, ref int modified)
    {
        if (node is MeshInstance3D { Mesh: not null } meshInstance)
        {
            if (meshInstance.MaterialOverride != null)
            {
                var duplicated = DuplicateAndFix(meshInstance.MaterialOverride, ref modified);
                meshInstance.MaterialOverride = duplicated;
            }
            else
            {
                for (var i = 0; i < meshInstance.Mesh.GetSurfaceCount(); i++)
                {
                    var mat = meshInstance.Mesh.SurfaceGetMaterial(i);
                    if (mat == null)
                        continue;

                    var duplicated = DuplicateAndFix(mat, ref modified);
                    meshInstance.Mesh.SurfaceSetMaterial(i, duplicated);
                }
            }
        }

        foreach (var child in node.GetChildren())
            ProcessNode(child, ref modified);
    }

    private Material DuplicateAndFix(Material source, ref int modified)
    {
        var copy = source.Duplicate(true) as Material;
        if (copy is not BaseMaterial3D baseMat)
            return copy;
        
        baseMat.CullMode = BaseMaterial3D.CullModeEnum.Back;
        modified++;
        return copy;
    }
}