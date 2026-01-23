using Godot;

namespace Scripts.Gameplay;

[GlobalClass, Tool]
public partial class PlayerGrabberRibbon : MeshInstance3D
{
    [ExportGroup("Nodes")]
    [Export] public Node3D StartNode;
    [Export] public Node3D EndNode;

    [ExportGroup("Visuals")]
    [Export] public float BaseRadius = 0.05f;
    [Export] public int Segments = 16;
    [Export] public int RadialSegments = 6;
    [Export] public Curve WidthCurve; 
    
    [ExportGroup("Curve Settings")]
    [Export] public float CurveHeight = 0.5f;
    [Export] public Vector3 CurveDirection = Vector3.Up;

    private ImmediateMesh _immMesh;

    public override void _Ready()
    {
        _immMesh = new ImmediateMesh();
        Mesh = _immMesh;
    }

    public override void _Process(double delta)
    {
        _immMesh.ClearSurfaces();
        
        if (EndNode == null || StartNode == null)
            return;
        if (!IsInstanceValid(EndNode) || !IsInstanceValid(StartNode))
            return;
        
        var start = StartNode.GlobalPosition - GlobalPosition;
        var end = EndNode.GlobalPosition - GlobalPosition;
        var mid = (start + end) * 0.5f;
        var control = mid + (CurveDirection.Normalized() * CurveHeight);

        _immMesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

        var prevRing = new Vector3[RadialSegments];
        for (var i = 0; i <= Segments; i++)
        {
            var t = (float)i / Segments;
            var center = start.BezierInterpolate(control, control, end, t);
            
            var radiusFactor = WidthCurve?.Sample(t) ?? 1.0f;
            var currentRadius = BaseRadius * radiusFactor;

            var nextT = (i < Segments)
                ? start.BezierInterpolate(control, control, end, (i + 1f) / Segments)
                : center + (center - start.BezierInterpolate(control, control, end, (i - 1f) / Segments));

            var forward = (nextT - center).Normalized();
            var right = forward.Cross(Mathf.Abs(forward.Y) > 0.9f ? Vector3.Right : Vector3.Up).Normalized();
            var up = right.Cross(forward).Normalized();

            var currRing = new Vector3[RadialSegments];
            for (var r = 0; r < RadialSegments; r++)
            {
                var angle = (float)r / RadialSegments * Mathf.Pi * 2.0f;
                currRing[r] = center + (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * currentRadius;
            }

            if (i > 0)
            {
                for (var r = 0; r < RadialSegments; r++)
                {
                    var nextR = (r + 1) % RadialSegments;
                    var uPrev = (float)(i - 1) / Segments;
                    var uCurr = (float)i / Segments;
                    var v = (float)r / RadialSegments;
                    var vNext = (float)(r + 1) / RadialSegments;

                    _immMesh.SurfaceSetUV(new Vector2(uPrev, v));
                    _immMesh.SurfaceAddVertex(prevRing[r]);
                    _immMesh.SurfaceSetUV(new Vector2(uCurr, v));
                    _immMesh.SurfaceAddVertex(currRing[r]);
                    _immMesh.SurfaceSetUV(new Vector2(uCurr, vNext));
                    _immMesh.SurfaceAddVertex(currRing[nextR]);

                    _immMesh.SurfaceSetUV(new Vector2(uPrev, v));
                    _immMesh.SurfaceAddVertex(prevRing[r]);
                    _immMesh.SurfaceSetUV(new Vector2(uCurr, vNext));
                    _immMesh.SurfaceAddVertex(currRing[nextR]);
                    _immMesh.SurfaceSetUV(new Vector2(uPrev, vNext));
                    _immMesh.SurfaceAddVertex(prevRing[nextR]);
                }
            }
            
            prevRing = currRing;
        }

        _immMesh.SurfaceEnd();
    }
}