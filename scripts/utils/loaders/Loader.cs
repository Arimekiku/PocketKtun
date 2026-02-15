using System;

namespace Scripts.Utils.Loaders;

public abstract class Loader<TTarget> where TTarget : class
{
    private WeakReference<TTarget> _target;
    
    public TTarget Target
    {
        get
        {
            _target ??= new WeakReference<TTarget>(LoadTarget());
            
            if (_target.TryGetTarget(out var target))
                return target;
            
            // Maybe need add thread save code??? 
            
            target = LoadTarget();
            _target.SetTarget(target);
            
            return target;
        }
    }
    
    protected abstract TTarget LoadTarget();
}