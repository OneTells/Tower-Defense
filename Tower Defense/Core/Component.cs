using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core;

public abstract class Component
{
    protected static readonly GameView Game = GameView.GetObject;
    protected static readonly ContentManager Content = Game.Content;
    protected static readonly SpriteBatch Sprite = Game.Sprite;
    
    public abstract void Update();
    
    public abstract void Draw();
}
