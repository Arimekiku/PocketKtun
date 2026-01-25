using System.Collections.Generic;

namespace Scripts.DispatchSystem;

public class Emergency
{
    public float TimeToResolve { get; private set; }
    public IReadOnlyDictionary<StatType, int> Stats { get; private set; }
    
    public Emergency(float timeToResolve, Dictionary<StatType, int> stats)
    {
        TimeToResolve = timeToResolve;
        Stats = stats;
    }
}