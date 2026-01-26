using Godot;

namespace Scripts.Gameplay;

[GlobalClass, Tool]
public partial class PlayerGrabberRibbon : MeshInstance3D
{
    [ExportGroup("Nodes")]
    [Export] private Node3D _startNode;
    [Export] private Node3D _endNode;

    [ExportGroup("Visuals")]
    [Export] private float _baseRadius = 0.05f;
    [Export] private int _segments = 16;
    [Export] private int _radialSegments = 6;
    [Export] private Curve _widthCurve; 
    
    [ExportGroup("Curve Settings")]
    [Export] private float _curveHeight = 0.5f;
    [Export] private Vector3 _curveDirection = Vector3.Up;

    private ImmediateMesh _immMesh;

    public override void _Ready()
    {
        _immMesh = new ImmediateMesh();
        Mesh = _immMesh;
    }

    public override void _Process(double delta)
    {
        _immMesh.ClearSurfaces();
        
        if (_endNode == null || _startNode == null)
            return;
        if (!IsInstanceValid(_endNode) || !IsInstanceValid(_startNode))
            return;
        
        var start = _startNode.GlobalPosition - GlobalPosition;
        var end = _endNode.GlobalPosition - GlobalPosition;
        var mid = (start + end) * 0.5f;
        var control = mid + (_curveDirection.Normalized() * _curveHeight);

        _immMesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

        var prevRing = new Vector3[_radialSegments];
        for (var i = 0; i <= _segments; i++)
        {
            var t = (float)i / _segments;
            var center = start.BezierInterpolate(control, control, end, t);
            
            var radiusFactor = _widthCurve?.Sample(t) ?? 1.0f;
            var currentRadius = _baseRadius * radiusFactor;

            var nextT = (i < _segments)
                ? start.BezierInterpolate(control, control, end, (i + 1f) / _segments)
                : center + (center - start.BezierInterpolate(control, control, end, (i - 1f) / _segments));

            var forward = (nextT - center).Normalized();
            var right = forward.Cross(Mathf.Abs(forward.Y) > 0.9f ? Vector3.Right : Vector3.Up).Normalized();
            var up = right.Cross(forward).Normalized();

            var currRing = new Vector3[_radialSegments];
            for (var r = 0; r < _radialSegments; r++)
            {
                var angle = (float)r / _radialSegments * Mathf.Pi * 2.0f;
                currRing[r] = center + (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * currentRadius;
            }

            if (i > 0)
            {
                for (var r = 0; r < _radialSegments; r++)
                {
                    var nextR = (r + 1) % _radialSegments;
                    var uPrev = (float)(i - 1) / _segments;
                    var uCurr = (float)i / _segments;
                    var v = (float)r / _radialSegments;
                    var vNext = (float)(r + 1) / _radialSegments;

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
    
    public void ChangeTarget(Node3D newTarget) 
    {
        _endNode = newTarget;
    }
}