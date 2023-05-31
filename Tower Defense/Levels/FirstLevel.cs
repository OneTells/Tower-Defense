using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core;
using Tower_Defense.Menu;

namespace Tower_Defense.Levels;

public sealed class FirstLevel : Core.Menu
{
    private static FirstLevel _object;
    
    public static FirstLevel GetObject => _object ??= new FirstLevel();
    
    private const double HealthDefault = 20;
    
    public double Health = HealthDefault;
    
    public static int StartCount;
    
    private  Button _pause;
    private  Button _speedButton;
    
    private  Button _pauseMenu;
    private  Button _pauseExitMenu;
    private bool _isPause;

    private  List<int> _wave;
    private  List<List<Vector2>> _tracks;
    private  DateTime _time;
    private Texture2D _texture;

    private int _number;

    private List<Opponent> _opponents = new ();
    
    private  Texture2D _background;
    private  Texture2D _path;
    private  Texture2D _health;
    private  SpriteFont _font;
    
    private  List<Vector2> _field;

    private double _speed = 1;
    
    private FirstLevel() { LoadContent(); }
    
    private void PauseButton_Click()
    {
        _isPause = true;
    }
    
    private void SpeedButton_Click()
    {
        _speed = _speed switch
        {
            1 => 3,
            3 => 5,
            5 => 10,
            10 => 0.5,
            0.5 => 1,
            _ => _speed
        };
    }
    
    private void PauseMenuButton_Click()
    {
        GameView.ChangeMenu(Menu.GameMenu.GetObject);
    }
    
    private void PauseExitMenuButton_Click()
    {
        _isPause = false;
    }

    protected override void LoadContent()
    {
        _background = Content.Load<Texture2D>("Background/Level");
        _path = Content.Load<Texture2D>("Controls/Path");
        _health = Content.Load<Texture2D>("Controls/Health");
        
        _font = Content.Load<SpriteFont>("Fonts/Font1");
        
        _wave = new List<int> { 1, 3, 5, 7, 8, 9, 11, 13, 15, 17, 21, 25, 25, 27, 29, 29, 29, 29, 29, 29, 30, 31};
        
        _tracks = new List<List<Vector2>>
        {
            new () { new Vector2(100, 136), new Vector2(315, 136), new Vector2(315, 264), new Vector2(484, 264)}
        };
        
        _pause = new Button("Controls/Pause")
        {
            Position = new Vector2(10, 10),
            Click = PauseButton_Click
        };

        _pauseMenu = new Button("Controls/ExitMenu")
        {
            Position = new Vector2(299, 214),
            Click = PauseMenuButton_Click
        };

        _speedButton = new Button("Controls/Pause")
        {
            Position = new Vector2(10, 300),
            Click = SpeedButton_Click
        };

        _pauseExitMenu = new Button("Controls/Close")
        {
            Position = new Vector2(597, 81),
            Click = PauseExitMenuButton_Click
        };

        _time = DateTime.Now;
        _texture = Content.Load<Texture2D>("Controls/O");

        _field = new List<Vector2>
        {
            new (100, 120),new (164, 120),new (228, 120),new (292, 120),new (292, 184),new (292, 248),new (356, 248),new (420, 248),new (484, 248)
        };
    }

    public override void Update()
    {
        if (!_isPause)
        {   
            if (_opponents.Count == 0 && _wave.Count == 0 || Health == 0)
            {
                _pauseMenu.Update();
            }
            else
            {
                _pause.Update();
                _speedButton.Update();
            
                foreach (var t in new List<int>(_wave).Where(t => DateTime.Now.Subtract(_time).TotalSeconds >= t/_speed))
                {
                    _wave.Remove(t);
                    _opponents.Add(new Opponent(_tracks[0], _texture));
                } 
            
                foreach (var o in new List<Opponent>(_opponents).Where(o => o.TrackIndex >= _tracks[0].Count))
                {
                    _opponents.Remove(o);

                    Health -= 1;
                    
                    if (Health == 0)
                        return;
                } 
            
                foreach (var o in _opponents)
                {
                    o.Speed = Opponent.SpeedDefault * _speed;
                    o.Update();
                }
            }
            
        }
        else
        {
            _pauseMenu.Update();
            _pauseExitMenu.Update();
        }

    }
    
    public override void Draw()
    {
        Sprite.Begin();
        
        Sprite.Draw(_background, new Vector2(0, 0), Color.White);
        Sprite.Draw(_health, new Vector2(710, 10), Color.White);
        
        Sprite.DrawString(_font, $"{Health}", new Vector2(700-_font.MeasureString($"{Health}").X, 28), Color.White);
        
        foreach (var f in _field)
        {
            Sprite.Draw(_path, f, Color.White);
        }
        
        foreach (var o in _opponents)
        {
            o.Draw();
        }
        
        if (!_isPause)
        {
            if (_opponents.Count == 0 && _wave.Count == 0 || Health == 0)
            {
                Sprite.Draw(Content.Load<Texture2D>("Controls/EndGame"), new Vector2(163, 88), Color.White);
                _pauseMenu.Draw();

                var starCount = 0;

                if (Health / HealthDefault >= 0.4)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(342, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(342, 120), Color.White);

                if (Health / HealthDefault >= 0.8)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(384, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(384, 120), Color.White);

                if (Health / HealthDefault >= 1)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(426, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(426, 120), Color.White);
                
                StoreMenu.PointCount += StartCount - StartCount;
                StartCount = starCount;
            }
            else
            {
                _pause.Draw();
                _speedButton.Draw();
            }
                
        }
        else
        {
            Sprite.Draw(Content.Load<Texture2D>("Controls/PauseMenu"), new Vector2(155, 88), Color.White);
            _pauseMenu.Draw();
            _pauseExitMenu.Draw();
        }
        Sprite.End();
    }

    
}