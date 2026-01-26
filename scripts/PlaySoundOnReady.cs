using Godot;

namespace Scripts.Gameplay;

public partial class PlaySoundOnReady: AudioStreamPlayer3D
{
    [Export] private AudioStreamPlayer3D _stream;
    
    public override void _Ready()
    {
        _stream.Play();
    }
}