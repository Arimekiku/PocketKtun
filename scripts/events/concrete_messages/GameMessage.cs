using Scripts.MessageManager;

namespace Scripts.Gameplay.Messages;

public class GameMessage : BaseMessage<GameMessages>
{
    public int Int;
    public float Float;
    public bool Bool;

    public GameMessage(GameMessages message) : base(message)
    { }
}