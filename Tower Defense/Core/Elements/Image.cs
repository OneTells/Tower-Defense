﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core.Elements;

public class Image : Component
{
    private readonly Texture2D _texture;
    public Vector2 Position;

    private Rectangle Rectangle => new ((int) Position.X, (int) Position.Y, _texture.Width, _texture.Height);

    public Dictionary<string, Text> Texts = new ();

    public Image(string textureName, Vector2 position)
    {
        _texture = Content.Load<Texture2D>(textureName);
        Position = position;
    }
    
    public override void Update() {}
    
    public override void Draw()
    {
        Sprite.Draw(_texture, Rectangle, Color.White);
        
        foreach (var (_, text) in Texts)
        {
            text.Draw();
        }
    }
}