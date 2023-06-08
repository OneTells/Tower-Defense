using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core.Elements;
using Tower_Defense.Core.Level;

namespace Tower_Defense.Core;

public abstract class Level<T> : Component where T : new ()
{
    private static T _object;

    public static T GetObject
    {
        get
        {
            var star = 0;
            
            if (_object != null)
            {
                if ((_object as Level<T>)!._isReset ||  (_object as Level<T>)!._endGame.IsEndGame)
                    star = (_object as Level<T>)!._endGame.StartCount;
                else
                    return _object;
            } 
            
            _object = new T();
            
            (_object as Level<T>)!._endGame.StartCount = star;
            
            (_object as Level<T>)?.LoadContent();
            (_object as Level<T>)?.Rendering();
            
            (_object as Level<T>)!._waves = (_object as Level<T>)?.InitializationWaves();
            
            (_object as Level<T>)?.Initialization();
            
            return _object;
        }
    }
    
    public void Reset()
    {
        _isReset = true;
        GameView.ChangeMenu(GetObject as Component);
    }

    private bool _isReset;
    
    protected abstract double HealthDefault { get; init; }

    private double _health;
    private double _speed = 1;

    public int Coin = 500;
    
    #region MenuElements

    private Image _healthImage;
    private Image _coinImage;
    
    private Button _speedButton;
    private Button _pauseButton;
    
    private readonly List<Tower<T>> _platforms = new ();
    private readonly HashSet<Image> _ways = new ();
    public readonly List<Opponent> Opponents = new ();
    
    #endregion
    
    private readonly Pause<T> _pause = new ();
    private readonly EndGame<T> _endGame = new ();
    
    private List<List<Opponent>> _waves = new ();
    private int _indexOpponent;
    private int _indexWave;
    
    protected readonly List<List<Vector2>> Tracks = new();
    
    private DateTime _startLevelTime;
    
    private Background _background;

    private readonly HashSet<Image> _portals = new ();
    private Image _stage;
    
    private void ChangeSpeed()
    {
        _speed = _speed switch {1 => 3, 3 => 5, 5 => 10, 10 => 0.5, 0.5 => 1, _ => _speed};
        
        _speedButton.Texture = Content.Load<Texture2D>(
            "Level/Time/" + _speed switch {1 => "1", 3 => "3", 5 => "5", 10 => "10", 0.5 => "0", _ => "1"}
        );
    }
    
    private void LoadContent()
    {
        _startLevelTime = DateTime.Now;
        
        _health = HealthDefault;
                
        _background = new Background("Background/Level");
        
        _healthImage = new Image("Level/Health", new Vector2(600, 10))
        {
            Texts = new Dictionary<string, Text>
            {
                {"text", new Text("Fonts/Font1", new Vector2(630, 10), $"{_health}", false)}
            }
        };
        
        _coinImage = new Image("Level/Coin", new Vector2(700, 10))
        {
            Texts = new Dictionary<string, Text>
            {
                {"text", new Text("Fonts/Font1", new Vector2(730, 10), $"{Coin}", false)}
            }
        };
        
        _speedButton = new Button( "Level/Time/1",  new Vector2(10, 370), ChangeSpeed);
        
        _pauseButton = new Button("Controls/Pause", new Vector2(10, 10), () => _pause.IsPause = true);
    }
    
    private void Rendering()
    {
        var trackCodes = InitializationTrackCodes();

        for (var a = 0; a < trackCodes.Count; a++)
        {
            var current = trackCodes[a].start;
            var code =trackCodes[a].code + "e";

            _ways.Add(new Image("Level/Map/Path", new Vector2(current.X - 32, current.Y - 32)));
            
            _portals.Add(new Image("Level/Map/Start", new Vector2(current.X - 32, current.Y - 32)));
            
            Tracks.Add(new List<Vector2> {current});
                        
            for (var i = 0; i < code.Length - 1; i++)
            {
                switch (code[i])
                {
                    case 'r':
                        current.X += 64;
                        break;
                    case 'l':
                        current.X -= 64;
                        break;
                    case 'u':
                        current.Y -= 64;
                        break;
                    case 'd':
                        current.Y += 64;
                        break;
                }

                if (code[i] != code[i + 1])
                    Tracks[a].Add(current);

                _ways.Add(new Image("Level/Map/Path", new Vector2(current.X - 32, current.Y - 32)));
            }
            
            _portals.Add(new Image("Level/Map/Finish", new Vector2(current.X - 32, current.Y - 32)));
        }
        
        foreach (var platform in InitializationPlatforms())
            _platforms.Add(new Tower<T>(platform, ref _speed));
    }
    
    private void Initialization()
    {
        _stage = new Image("Level/Stage", new Vector2(500, 10))
        {
            Texts = new Dictionary<string, Text> { {"text", new Text("Fonts/Font1", new Vector2(530, 10), $"{_indexWave + 1}/{_waves.Count}", false)} }
        };
    }
    
    protected abstract List<(string code, Vector2 start)> InitializationTrackCodes();
    
    protected abstract List<List<Opponent>> InitializationWaves();
    
    protected abstract List<Vector2> InitializationPlatforms();
    
    public override void Update()
    {
        if (_endGame.IsEndGame)
        {
            _endGame.Update();
            return;
        }
        
        if (_pause.IsPause)
        {
            _pause.Update();
            return;
        }
        
        
        _pauseButton.Update();
        _speedButton.Update();
        
        _stage.Texts["text"].Caption = $"{_indexWave + 1}/{_waves.Count}";
        _coinImage.Texts["text"].Caption = $"{Coin}";
        
        if (Opponents.Count == 0 && _indexOpponent == _waves[_indexWave].Count)
        {
            _indexWave++;
            _indexOpponent = 0;
        }
        
        if (_indexOpponent != _waves[_indexWave].Count && DateTime.Now.Subtract(_startLevelTime).TotalSeconds >= _waves[_indexWave][_indexOpponent].TimeOut / _speed)
        {
            Opponents.Add(_waves[_indexWave][_indexOpponent]);

            _indexOpponent += 1;
            _startLevelTime = DateTime.Now;
        }
        
        foreach (var opponent in new List<Opponent>(Opponents).Where(opponent => opponent.Health <= 0))
        {
            Opponents.Remove(opponent);
        }
        
        foreach (var opponent in new List<Opponent>(Opponents).Where(opponent => opponent.TrackIndex >= opponent.Track.Count))
        {
            _health -= 1;
            _healthImage.Texts["text"].Caption = $"{_health}";
            
            Opponents.Remove(opponent);

            if (_health == 0) 
                break;
        }

        if (Opponents.Count == 0 && _indexWave + 1 == _waves.Count && _indexOpponent == _waves[_indexWave].Count || _health == 0)
        {
            var starCount = (_health / HealthDefault) switch { >= 0.9 => 3, >= 0.8 => 2, >= 0.4 => 1, _ => 0};
            
            Game.StartCount += int.Max(starCount - _endGame.StartCount, 0);
            _endGame.StartCount = int.Max(starCount, _endGame.StartCount);
            
            _endGame.IsEndGame = true;
            return;
        }
        
        foreach (var opponent in Opponents)
        {
            opponent.Speed = opponent.SpeedDefault * _speed;
            opponent.Update();
        }
        
        foreach (var platform in _platforms)
            platform.Update();
    }

    public override void Draw()
    {
        _background.Draw();
        
        foreach (var way in _ways) 
            way.Draw();
        
        foreach (var portal in _portals) 
            portal.Draw();
        
        foreach (var opponent in Opponents) 
            opponent.Draw();
        
        foreach (var platform in _platforms) 
            platform.Draw();
        
        _stage.Draw();
        _healthImage.Draw();
        _coinImage.Draw();
        
        if (_pause.IsPause)
        {
            _pause.Draw();
            return;
        }
        
        if (_endGame.IsEndGame)
        {
            _endGame.Draw();
            return;
        }
        
        _pauseButton.Draw();
        _speedButton.Draw();
    }
}