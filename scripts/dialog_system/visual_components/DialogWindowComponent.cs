using Godot;

namespace Scripts.DialogSystem.Visual;

[GlobalClass]
public partial class DialogWindowComponent : Node
{
    [Export] private DialogChoicesVisual _dialogChoicesVisual;
    [Export] private NpcLineVisual _npcLineVisual;
}