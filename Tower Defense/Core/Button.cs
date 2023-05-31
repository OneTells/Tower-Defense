using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defense.Core;

public class Button : Component
{
    private MouseState _currentMouse;
    
    private readonly SpriteFont _font;
    
    private bool _isHovering;

    private MouseState _previousMouse;

    private readonly Texture2D _texture;
    private readonly Vector2 _position;
    
    public Action Click;
    
    public Vector2 Position
    {
        get => _position;
        init => _position = value;
    }

    public Rectangle Rectangle => new ((int) Position.X, (int) Position.Y, _texture.Width, _texture.Height);

    public List<string> Texts { get; set; } = new();

    public List<Vector2> TextsVector { get; set; } = new();
    
    public Button(string assetName)
    {
        _texture = Content.Load<Texture2D>(assetName);
        _font = null;
    }
    
    public Button(string assetName, SpriteFont font)
    {
        _texture = Content.Load<Texture2D>(assetName);
        _font = font;
    }
    
    public override void Update()
    {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
        
        if (!mouseRectangle.Intersects(Rectangle))
        {
            _isHovering = false;
            return;
        }
        
        _isHovering = true;

        if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
        {
            Click?.Invoke();
        }
    }
    
    public override void Draw()
    {
        var colour = Color.White;

        if (_isHovering)
            colour = Color.Gray;

        Sprite.Draw(_texture, Rectangle, colour);

        if (_font == null) 
            return;
        
        for (var index = 0; index < Texts.Count; index++)
            Sprite.DrawString(_font, Texts[index], new Vector2(TextsVector[index].X, TextsVector[index].Y), colour);
    }
}