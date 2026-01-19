using Godot;
using Godot.Collections;

namespace Scripts.Gameplay;

public partial class DayNightCycle : Node
{
    [Export] private DirectionalLight3D _sun;
    [Export] private WorldEnvironment _environment;
    [Export] private Array<OmniLight3D> _lights;
    [Export] private float _dayDurationSeconds = 60.0f;
    [Export] private Gradient _skyColors;

    private float _currentTime;

    public override void _Process(double delta)
    {
        _currentTime += (float)delta / _dayDurationSeconds;
        if (_currentTime > 1.0f)
            _currentTime = 0.0f;

        UpdateEnvironment();
    }

    private void UpdateEnvironment()
    {
        var sunAngle = _currentTime * 360.0f - 90.0f;
        var intensity = Mathf.Clamp(Mathf.Sin(_currentTime * Mathf.Pi * 2), 0, 1);
        var color = _skyColors.Sample(_currentTime);
        
        _sun.RotationDegrees = new Vector3(sunAngle, 0, 0);
        _sun.LightEnergy = intensity;
        _sun.LightColor = color;
        
        foreach (var light in _lights)
        {
            light.LightEnergy = intensity;
            light.LightColor = color;
        }

        var env = _environment.Environment;
        env.AmbientLightColor = color;
        env.AmbientLightEnergy = intensity;
    }
}