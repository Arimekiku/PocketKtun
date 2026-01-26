using Godot;
using System.Threading.Tasks;

namespace Scripts.Systems.DispatchSystem;

[GlobalClass]
public partial class EmergencyResolution : Control
{
    [Export] public ColorRect SuccessBar;
    [Export] private Label _winrateLabel;
    [Export] public Control Pointer;
    [Export] public Label ResultLabel;
    
    private float _baseVelocity = 4000f; // 5x faster than 800
    private float _currentVelocity = 0f;
    private int _direction = 1; 
    private bool _isAnimating = false;
    private bool _isSlowingDown = false;

    public void UpdateWinrate(float winChance)
    {
        float successWidth = Size.X * winChance;
        SuccessBar.Size = new Vector2(successWidth, Size.Y);
        SuccessBar.Position = new Vector2((Size.X - successWidth) / 2, 0);
        
        _winrateLabel.Text = (winChance * 100).ToString("F0") + "%";
    }

    public async Task<bool> PlayResolution(float winChance)
    {
        float successWidth = Size.X * winChance;
        
        ResultLabel.Hide();

        Pointer.Position = new Vector2(0, Pointer.Position.Y);
        _currentVelocity = _baseVelocity;
        _isAnimating = true;
        _isSlowingDown = false;

        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
        _isSlowingDown = true;

        var vTween = CreateTween();
        vTween.TweenProperty(this, nameof(_currentVelocity), 0.0f, 2.0f)
              .SetTrans(Tween.TransitionType.Expo)
              .SetEase(Tween.EaseType.Out);

        await ToSignal(vTween, "finished");
        _isAnimating = false;

        float lowerBound = (Size.X - successWidth) / 2;
        float upperBound = lowerBound + successWidth;
        float pointerMid = Pointer.Position.X + (Pointer.Size.X / 2);
        bool didWin = pointerMid >= lowerBound && pointerMid <= upperBound;

        ShowResult(didWin);
        return didWin;
    }

    public override void _Process(double delta)
    {
        if (!_isAnimating) return;

        float moveAmount = _currentVelocity * (float)delta * _direction;
        Vector2 newPos = Pointer.Position;
        newPos.X += moveAmount;

        // Wall Bouncing (remains active during slowdown)
        if (newPos.X >= Size.X - Pointer.Size.X)
        {
            newPos.X = Size.X - Pointer.Size.X;
            _direction = -1;
            if (_currentVelocity > 100) PlayBounceSound();
        }
        else if (newPos.X <= 0)
        {
            newPos.X = 0;
            _direction = 1;
            if (_currentVelocity > 100) PlayBounceSound();
        }

        Pointer.Position = newPos;
    }

    private void ShowResult(bool success)
    {
        ResultLabel.Show();
        ResultLabel.Text = success ? "SUCCESS" : "FAILURE";
        ResultLabel.Modulate = success ? Colors.SpringGreen : Colors.OrangeRed;
        
        var juice = CreateTween();
        ResultLabel.Scale = new Vector2(1.5f, 1.5f);
        juice.TweenProperty(ResultLabel, "scale", Vector2.One, 0.5f)
             .SetTrans(Tween.TransitionType.Elastic);
    }

    private void PlayBounceSound() 
    {
        // For extra juice, pitch the sound based on velocity!
    }
}