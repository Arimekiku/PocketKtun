using System;
using Godot;

namespace Scripts.UI;

[GlobalClass, Tool]
public partial class FollowProgressBar : ProgressBar
{
    [Export] private Label _label;
    [Export] private float _padding = 6f;

    private Tween _tween;

    public override void _Process(double delta)
    {
        if (!IsVisibleInTree())
            return;
        
        var ratio = (float)((Value - MinValue) / (MaxValue - MinValue));
        ratio = Mathf.Clamp(ratio, 0f, 1f);

        _label.Text = $"{Mathf.RoundToInt((float)Value)}";

        var fillWidth = Size.X * ratio;
        var x = fillWidth - _label.Size.X - _padding;
        x = Mathf.Clamp(x, _padding, Size.X - _label.Size.X - _padding);

        _label.Position = new Vector2(x, (Size.Y - _label.Size.Y) / 2);
    }

    public void MoveTo(int value, float time)
    {
        _tween?.Kill();
        _tween = CreateTween();
        
        var to = Math.Clamp(value, MinValue, MaxValue);
        _tween.TweenProperty(this, "value", to, time);
        
        _tween.Play();
    }
}
