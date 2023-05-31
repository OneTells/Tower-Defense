using System.Collections.Generic;

namespace Tower_Defense.Core;

public abstract class Menu<T> : Component where T : new ()
{
    private static T _object;

    public static T GetObject
    {
        get
        {
            if (_object != null) 
                return _object;
            
            _object = new T();
            (_object as Menu<T>)?.LoadContent();

            return _object;
        }
    }

    protected Background Background;
    protected Dictionary<string, Component> Components;
    
    protected abstract void LoadContent();
    
    public abstract override void Update();
    
    public abstract override void Draw();
}