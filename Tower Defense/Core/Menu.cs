namespace Tower_Defense.Core;

public abstract class Menu: Component
{
    protected abstract void LoadContent();
    
    public abstract override void Update();
    
    public abstract override void Draw();
}