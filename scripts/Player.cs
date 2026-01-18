using Godot;

namespace Scripts.Gameplay;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float Sensitivity = 0.003f;

    private Node3D _neck;
    private Camera3D _camera;

    public override void _Ready()
    {
        _neck = GetNode<Node3D>("Neck");
        _camera = GetNode<Camera3D>("Neck/Camera3D");
        
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
            Input.MouseMode = Input.MouseModeEnum.Captured;
        else if (@event.IsActionPressed("ui_cancel"))
            Input.MouseMode = Input.MouseModeEnum.Visible;

        if (Input.MouseMode == Input.MouseModeEnum.Captured && @event is InputEventMouseMotion mouseMotion)
        {
            // Rotate neck horizontally, camera vertically
            _neck.RotateY(-mouseMotion.Relative.X * Sensitivity);
            _camera.RotateX(-mouseMotion.Relative.Y * Sensitivity);

            var cameraRot = _camera.Rotation;
            cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-60), Mathf.DegToRad(60));
            _camera.Rotation = cameraRot;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        // Add gravity
        if (!IsOnFloor())
            velocity += GetGravity() * (float)delta;

        // Get movement direction based on Neck rotation
        var inputDir = Input.GetVector(
            Inputs.MoveLeft, Inputs.MoveRight,
             Inputs.MoveUp, Inputs.ModeDown
        );
        var direction = (_neck.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}