using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core;

namespace Tower_Defense.Menu;

public class StoreMenu : Core.Menu
{
    private static StoreMenu _object;
    
    public static StoreMenu GetObject => _object ??= new StoreMenu();
    
    private  List<Button> _buttons;
    private  Texture2D _background;
    private  SpriteFont _font;
    
    private static int _power;
    private static int _length;
    private static int _speed;

    public static int PointCount = 1000;
    
    private StoreMenu()
    {
        LoadContent();
    }
    
    private void PowerButton_Click()
    {
        if (_power + 1 > PointCount) 
            return;
        
        _power += 1;
        PointCount -= _power + 1;
        _buttons[0].Texts = new List<string> { $"+{10 * _power}%", $"{_power + 1}" };
        _buttons[0].TextsVector = new List<Vector2> { new (410, 114), new (520 - _font.MeasureString($"{_power + 1}").X, 145) };
    }
    
    private void LengthButton_Click()
    {
        if (_length + 1 > PointCount) 
            return;
        
        _length += 1;
        PointCount -= _length + 1;
        _buttons[2].Texts = new List<string> { $"+{10 * _length}%", $"{_length + 1}" };
        _buttons[2].TextsVector = new List<Vector2> { new (486, 216), new (520 - _font.MeasureString($"{_length + 1}").X, 250) };
    }
    
    private void SpeedButton_Click()
    {
        if (_speed + 1 > PointCount) 
            return;
        
        _speed += 1;
        PointCount -= _speed + 1;
        _buttons[1].Texts = new List<string> { $"+{10 * _speed}%", $"{_speed + 1}" };
        _buttons[1].TextsVector = new List<Vector2> { new (466, 321), new (520 - _font.MeasureString($"{_speed + 1}").X,  353) };
    }
    
    private void CloseButton_Click()
    {
        Tower_Defense.GameView.ChangeMenu(MainMenu.GetObject);
    }
    
    


    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("Background/Store");
        _font = Content.Load<SpriteFont>("Fonts/Font1");
        
        var powerButton = new Button("Controls/Power", _font)
        {
            Position = new Vector2(235, 101),
            Click = PowerButton_Click
        };

        powerButton.Texts.Add($"+{10*_power}%");
        powerButton.TextsVector.Add(new Vector2(410, 114));
        
        powerButton.Texts.Add($"{_power + 1}");
        powerButton.TextsVector.Add(new Vector2(520 - _font.MeasureString($"{_power + 1}").X, 145));
        
        var lengthGameButton = new Button("Controls/Length", _font)
        {
            Position = new Vector2(235, 204),
            Click = LengthButton_Click
        };

        lengthGameButton.Texts.Add($"+{10*_length}%");
        lengthGameButton.TextsVector.Add(new Vector2(486, 216));
        
        lengthGameButton.Texts.Add($"{_length + 1}");
        lengthGameButton.TextsVector.Add(new Vector2(520 - _font.MeasureString($"{_length + 1}").X, 250));
        
        var speedButton = new Button("Controls/Speed", _font)
        {
            Position = new Vector2(235, 308),
            Click = SpeedButton_Click
        };

        speedButton.Texts.Add($"+{10*_speed}%");
        speedButton.TextsVector.Add(new Vector2(466, 321));
        
        speedButton.Texts.Add($"{_speed + 1}");
        speedButton.TextsVector.Add(new Vector2(520 - _font.MeasureString($"{_speed + 1}").X,  353));
        
        var closeButton = new Button("Controls/Close")
        {
            Position = new Vector2(604, 1)
        };

        closeButton.Click += CloseButton_Click;
        
        _buttons = new List<Button>
        {
            powerButton,
            speedButton,
            lengthGameButton,
            closeButton
        };
    }

    public override void Update()
    {
        foreach (var button in _buttons)
            button.Update();
    }
    public override void Draw()
    {
        Sprite.Begin();
        
        Sprite.Draw(_background, new Vector2(0, 0), Color.White);

        foreach (var button in _buttons)
            button.Draw();
        
        Sprite.End();
    }
}