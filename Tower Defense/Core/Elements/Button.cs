using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defense.Core.Elements;

public class Button : Component
{
    private MouseState _currentMouse;
        
    private bool _isSelect;
    
    public Texture2D Texture;
    private readonly Vector2 _position;
    private readonly Action _click;

    private Rectangle Rectangle => new ((int) _position.X, (int) _position.Y, Texture.Width, Texture.Height);

    public Dictionary<string, Text> Texts = new ();

    public Button(string textureName, Vector2 position, Action click)
    {
        Texture = Content.Load<Texture2D>(textureName);
        _position = position;
        _click = click;
    }
    
    public override void Update()
    {
        var previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
        
        _isSelect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1).Intersects(Rectangle);
        
        if (_isSelect && _currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            _click?.Invoke();
    }
    
    public override void Draw()
    {
        Sprite.Draw(Texture, Rectangle, _isSelect ? Color.Gray : Color.White);
        
        foreach (var (_, text) in Texts)
        {
            text.IsSelect = _isSelect;
            text.Draw();
        }
    }
}