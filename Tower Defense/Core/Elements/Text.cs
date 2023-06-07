using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core;

public class Text : Component
{
    private readonly SpriteFont _font;
    private readonly Vector2 _position;
        
    public bool IsSelect;
    public string Caption;
    
    public Text(string fontName, Vector2 position, string caption)
    {
        _position = position;
        _font = Content.Load<SpriteFont>(fontName);
        Caption = caption;
    }

    public override void Update() {}
    
    public override void Draw()
    {
        Sprite.DrawString(_font, Caption, new Vector2(_position.X - _font.MeasureString(Caption).X, _position.Y), IsSelect ? Color.Gray : Color.White);
    }
}