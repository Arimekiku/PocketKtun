using Scripts.MessageManager;

namespace Scripts.Gameplay.Messages;

public class GameMessage : BaseMessage<GameMessages>
{
    public int Int;
    public float Float;
    public bool Bool;

    public GameMessage(
        GameMessages message,
        int intParam = 0,
        float floatParm = 0f,
        bool boolParam = false
    ) : base(message)
    {
        Int = intParam;
        Float = floatParm;
        Bool = boolParam;
    }
}