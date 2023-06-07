using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core.Elements;
using Tower_Defense.Core.Level;
using Tower_Defense.Menu;

namespace Tower_Defense.Core;

public abstract class Level<T> : Component where T : new ()
{
    private static T _object;

    public static T GetObject
    {
        get
        {
            if (_object != null) 
                return _object;
            
            _object = new T();
            
            (_object as Level<T>)?.LoadContent();
            (_object as Level<T>)?.Rendering();
            
            (_object as Level<T>)!._waves = (_object as Level<T>)?.InitializationWaves();
            
            return _object;
        }
    }
    
    protected abstract double HealthDefault { get; init; }

    private double _health;
    private double _speed = 1;

    #region MenuElements

    private Image _healthImage;
    
    private Button _speedButton;
    private Button _pauseButton;
    
    private readonly List<Button> _platforms = new ();
    private readonly HashSet<Image> _ways = new ();
    private readonly List<Opponent> _opponents = new ();
    
    #endregion
    
    private readonly Pause _pause = new ();
    private readonly EndGame _endGame = new ();
    private readonly AddTower _addTower = new ();
    
    private List<List<Opponent>> _waves = new ();
    private int _indexOpponent;
    private int _indexWave;
    
    protected readonly List<List<Vector2>> Tracks = new();
    
    private DateTime? _startLevelTime;
    
    private Background _background;

    private void LoadContent()
    {
        _health = HealthDefault;
                
        _background = new Background("Background/Level");
        
        _healthImage = new Image("Controls/Health", new Vector2(710, 10))
        {
            Texts = new Dictionary<string, Text>
            {
                {"text", new Text("Fonts/Font1", new Vector2(700, 28), $"{_health}")}
            }
        };
        
        _speedButton = new Button(
            "Controls/Pause", 
            new Vector2(10, 300), 
            ()=> _speed = _speed switch {1 => 3, 3 => 5, 5 => 10, 10 => 0.5, 0.5 => 1, _ => _speed}
        );
        
        _pauseButton = new Button("Controls/Pause", new Vector2(10, 10), () => _pause.IsPause = true);
    }
    
    private void Rendering()
    {
        var trackCodes = InitializationTrackCodes();

        for (var a = 0; a < trackCodes.Count; a++)
        {
            var current = trackCodes[a].start;
            var code =trackCodes[a].code + "e";

            _ways.Add(new Image("Controls/Path", new Vector2(current.X - 32, current.Y - 32)));
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

                _ways.Add(new Image("Controls/Path", new Vector2(current.X - 32, current.Y - 32)));
            }
        }
        
        foreach (var platform in InitializationPlatforms())
            _platforms.Add(new Button("Controls/Path", platform, () => _addTower.IsAddTower = true));
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
        
        if (_addTower.IsAddTower)
        {
            _addTower.Update();
            return;
        }
        
        _pauseButton.Update();
        _speedButton.Update();
        
        if (_opponents.Count == 0 && _indexOpponent == _waves[_indexWave].Count)
        {
            _indexWave++;
            _indexOpponent = 0;
        }
        
        _startLevelTime ??= DateTime.Now;
        
        if (_indexOpponent != _waves[_indexWave].Count && DateTime.Now.Subtract((DateTime) _startLevelTime).TotalSeconds >= _waves[_indexWave][_indexOpponent].TimeOut / _speed)
        {
            _opponents.Add(_waves[_indexWave][_indexOpponent]);

            _indexOpponent += 1;
            _startLevelTime = DateTime.Now;
        }
        
        foreach (var opponent in new List<Opponent>(_opponents).Where(opponent => opponent.TrackIndex >= opponent.Track.Count))
        {
            _health -= 1;
            _healthImage.Texts["text"].Caption = $"{_health}";
            
            _opponents.Remove(opponent);

            if (_health == 0) 
                break;
        }

        if (_opponents.Count == 0 && _indexWave + 1 == _waves.Count && _indexOpponent == _waves[_indexWave].Count || _health == 0)
        {
            var starCount = (_health / HealthDefault) switch { >= 0.9 => 3, >= 0.8 => 2, >= 0.4 => 1, _ => 0};
            
            Game.StartCount += int.Max(starCount - _endGame.StartCount, 0);
            _endGame.StartCount = int.Max(starCount, _endGame.StartCount);
            
            _endGame.IsEndGame = true;
            return;
        }
        
        foreach (var opponent in _opponents)
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
        
        foreach (var platform in _platforms) 
            platform.Draw();
        
        foreach (var opponent in _opponents) 
            opponent.Draw();
        
        _healthImage.Draw();
        
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
        
        if (_addTower.IsAddTower)
        {
            _addTower.Draw();
            return;
        }
        
        _pauseButton.Draw();
        _speedButton.Draw();
    }
}