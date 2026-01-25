using System.Threading.Tasks;
using Godot;
using Scripts.DIContainer;
using Scripts.Gameplay.Services;
using Scripts.UI;

namespace Scripts.DispatchSystem;

public partial class EmergencyReportWindow : Control
{
    [Export] private EmergencyResolution _resolution;
    [Export] private FollowProgressBar[] _emergencyProgressBars;
    [Export] private FollowProgressBar[] _unitProgressBars;

    [Inject] private IDispatcherMapStateService _dispatcherMapStateService;

    public override void _Ready()
    {
        _dispatcherMapStateService.OnReportFormRequested += ReportSequence;
    }

    public override void _ExitTree()
    {
        _dispatcherMapStateService.OnReportFormRequested -= ReportSequence;
    }

    public override void _PhysicsProcess(double delta)
    {
        var diffs = 0.0f;
        for (var i = 0; i < _emergencyProgressBars.Length; i++)
        {
            var first = (float)_emergencyProgressBars[i].Value;
            var second = (float)_unitProgressBars[i].Value;

            var result = first == 0 ? 0 : second / first;
            diffs += Mathf.Clamp(result, 0, 1);
        }
        diffs /= _emergencyProgressBars.Length;
        
        _resolution.UpdateWinrate(diffs);
    }

    private void ReportSequence(Emergency emergency, Character character)
    {
        _ = StartReportSequence(emergency, character);
    }

    private async Task StartReportSequence(Emergency emergency, Character character)
    {
        Show();
        
        foreach (var bar in _emergencyProgressBars)
            bar.Value = 0;
        
        foreach (var bar in _unitProgressBars)
            bar.Value = 0;
        
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        var counter = 0;
        foreach (var (statType, emergencyStat) in emergency.Stats)
        {
            _emergencyProgressBars[counter].MoveTo(emergencyStat, 3.0f);
            counter++;
            
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        }
        
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        counter = 0;
        foreach (var (statType, characterStat) in character.Stats)
        {
            _unitProgressBars[counter].MoveTo(characterStat, 3.0f);
            counter++;
            
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        }
        
        await ToSignal(GetTree().CreateTimer(2.5f), "timeout");

        var diffs = 0.0f;
        for (var i = 0; i < _emergencyProgressBars.Length; i++)
        {
            var first = (float)_emergencyProgressBars[i].Value;
            var second = (float)_unitProgressBars[i].Value;

            var result = first == 0 ? 0 : second / first;
            diffs += Mathf.Clamp(result, 0, 1);
        }
        diffs /= _emergencyProgressBars.Length;
        await _resolution.PlayResolution(diffs);
        
        _dispatcherMapStateService.FreeEmergency(emergency);
        GD.Print("End of sequence");
    }
}