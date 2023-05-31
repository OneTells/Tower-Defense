using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defense.Core;

public class Image : Component
{
    private bool _isSelect;
    
    private readonly Texture2D _texture;
    private readonly Vector2 _position;

    private Rectangle Rectangle => new ((int) _position.X, (int) _position.Y, _texture.Width, _texture.Height);

    public Dictionary<string, Text> Texts = new ();

    public Image(string textureName, Vector2 position)
    {
        _texture = Content.Load<Texture2D>(textureName);
        _position = position;
    }
    
    public override void Update() {}
    
    public override void Draw()
    {
        Sprite.Draw(_texture, Rectangle, _isSelect ? Color.Gray : Color.White);
        
        foreach (var (_, text) in Texts)
        {
            text.IsSelect = _isSelect;
            text.Draw();
        }
    }
}