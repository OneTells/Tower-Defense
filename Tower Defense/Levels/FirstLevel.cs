using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core;
using Tower_Defense.Menu;

namespace Tower_Defense.Levels;

public sealed class FirstLevel : Menu<FirstLevel>
{
    private const double HealthDefault = 20;
    
    private double _health = HealthDefault;
    private double _speed = 1;
        
    public static int StartCount;
    
    private  Button _pause;
    private  Button _speedButton;
    
    private  Button _pauseMenu;
    private  Button _pauseExitMenu;
    private bool _isPause;

    private  List<int> _wave;
    private  List<List<Vector2>> _tracks;
    private  DateTime? _time;
    private Texture2D _texture;

    private readonly List<Opponent> _opponents = new ();
    
    private Image _healthImage;
    
    private  List<Image> _map;
    
    protected override void LoadContent()
    {
        Background = new Background("Background/Level");
        
        _healthImage = new Image("Controls/Health", new Vector2(710, 10))
        {
            Texts = new Dictionary<string, Text>
            {
                {"text", new Text("Fonts/Font1", new Vector2(700, 28), $"{_health}")}
            }
        };
        
        _map = new List<Image>
        {
            new ("Controls/Path", new Vector2(100, 120)),
            new ("Controls/Path", new Vector2(164, 120)),
            new ("Controls/Path", new Vector2(228, 120)),
            new ("Controls/Path", new Vector2(292, 120)),
            new ("Controls/Path", new Vector2(292, 184)),
            new ("Controls/Path", new Vector2(292, 248)),
            new ("Controls/Path", new Vector2(356, 248)),
            new ("Controls/Path", new Vector2(420, 248)),
            new ("Controls/Path", new Vector2(484, 248))
        };
        
        _wave = new List<int> { 1, 3, 5, 7, 8, 9, 11, 13, 15, 17, 21, 25, 25, 27, 29, 31, 33, 35, 37, 39, 41, 42};
        
        _tracks = new List<List<Vector2>>
        {
            new () { new Vector2(100, 136), new Vector2(315, 136), new Vector2(315, 264), new Vector2(484, 264)}
        };
        
        _pause = new Button("Controls/Pause", new Vector2(10, 10), () => _isPause = true);

        _pauseMenu = new Button("Controls/ExitMenu", new Vector2(299, 214), () => GameView.ChangeMenu(GameMenu.GetObject));

        _speedButton = new Button("Controls/Pause", new Vector2(10, 300), ()=> _speed = _speed switch {1 => 3, 3 => 5, 5 => 10, 10 => 0.5, 0.5 => 1, _ => _speed});

        _pauseExitMenu = new Button("Controls/Close", new Vector2(597, 81), ()=> _isPause = false);
        
        _texture = Content.Load<Texture2D>("Controls/O");
    }

    public override void Update()
    {
        _time ??= DateTime.Now;
        
        _healthImage.Texts["text"].Caption = $"{_health}";
        
        if (!_isPause)
        {   
            if (_opponents.Count == 0 && _wave.Count == 0 || _health == 0)
            {
                _pauseMenu.Update();
            }
            else
            {
                _pause.Update();
                _speedButton.Update();
            
                foreach (var t in new List<int>(_wave).Where(t => DateTime.Now.Subtract((DateTime) _time).TotalSeconds >= t/_speed))
                {
                    _wave.Remove(t);
                    _opponents.Add(new Opponent(_tracks[0], _texture));
                } 
            
                foreach (var o in new List<Opponent>(_opponents).Where(o => o.TrackIndex >= _tracks[0].Count))
                {
                    _opponents.Remove(o);

                    _health -= 1;
                    
                    if (_health == 0)
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
        Background.Draw();
        
        _healthImage.Draw();
        
        foreach (var road in _map)
            road.Draw();

        foreach (var o in _opponents)
        {
            o.Draw();
        }
        
        if (!_isPause)
        {
            if (_opponents.Count == 0 && _wave.Count == 0 || _health == 0)
            {
                Sprite.Draw(Content.Load<Texture2D>("Controls/EndGame"), new Vector2(163, 88), Color.White);
                _pauseMenu.Draw();

                var starCount = 0;

                if (_health / HealthDefault >= 0.4)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(342, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(342, 120), Color.White);

                if (_health / HealthDefault >= 0.8)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(384, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(384, 120), Color.White);

                if (_health / HealthDefault >= 1)
                {
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star+"), new Vector2(426, 120), Color.White);
                    starCount++;
                }
                else
                    Sprite.Draw(Content.Load<Texture2D>("Controls/Star-"), new Vector2(426, 120), Color.White);
                
                Game.StartCount += StartCount - StartCount;
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
    }
}