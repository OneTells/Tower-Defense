using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core;

public class Background : Component
{
    private readonly Texture2D _texture;
    
    public Background(string assetName)
    {
        _texture = Content.Load<Texture2D>(assetName);
    }
    
    public override void Draw()
    {
        Sprite.Draw(_texture, new Vector2(0, 0), Color.White);
    }

    public override void Update() {}
}