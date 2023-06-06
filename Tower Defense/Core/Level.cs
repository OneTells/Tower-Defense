using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            
            (_object as Level<T>)?.Initialize();
            (_object as Level<T>)?.Rendering();
            
            (_object as Level<T>)!._waves = (_object as Level<T>)?.InitializationWaves();
            
            return _object;
        }
    }
    
    protected abstract double HealthDefault { get; init; }

    private double _health;
    private double _speed = 1;

    private readonly HashSet<Image> _ways = new ();
    private Image _healthImage;

    private Button _speedButton;
    
    private List<Button> _pauseButtons;
    private Button _pauseButton;
    private Image _pauseBackground;
    
    private bool _isPause;
    private bool _isEndGame;

    private Button _endGameButton;

    public int StartCount;

    private List<List<Opponent>> _waves = new ();
    private int _indexOpponent;
    private int _indexWave;
    
    protected readonly List<List<Vector2>> Tracks = new();
    
    private readonly List<Button> _platforms = new ();
    
    private DateTime? _startLevelTime;

    private readonly List<Opponent> _opponents = new ();
    
    private List<Vector2> _startPositions;
    
    private Background _background;

    private void Initialize()
    {
        _background = new Background("Background/Level");
        
        _health = HealthDefault;
        
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
        
        _pauseButton = new Button("Controls/Pause", new Vector2(10, 10), () => _isPause = true);

        _pauseButtons = new List<Button>
        {
            new ("Controls/ExitMenu", new Vector2(299, 214), () => GameView.ChangeMenu(GameMenu.GetObject)),
            new ("Controls/Close", new Vector2(597, 81), ()=> _isPause = false)
        };
        
        _endGameButton = new Button(
            "Controls/ExitMenu", new Vector2(299, 214), () => GameView.ChangeMenu(GameMenu.GetObject)
        );
        
        _pauseBackground = new Image("Controls/PauseMenu", new Vector2(155, 88));
        
        _startPositions = new List<Vector2> { new (342, 120), new (384, 120), new (426, 120)};
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
            _platforms.Add(new Button("Controls/Path", platform, () => GameView.ChangeMenu(GameMenu.GetObject)));
    }

    protected abstract List<(string code, Vector2 start)> InitializationTrackCodes();
    
    protected abstract List<List<Opponent>> InitializationWaves();
    
    protected abstract List<Vector2> InitializationPlatforms();
    
    public override void Update()
    {
        if (_isEndGame)
        {
            _endGameButton.Update();
            return;
        }
        
        if (_isPause)
        {
            foreach (var pauseButton in _pauseButtons) 
                pauseButton.Update();
            
            return;
        }
        
        if (_opponents.Count == 0 && _indexOpponent == _waves[_indexWave].Count)
        {
            _indexWave++;
            _indexOpponent = 0;
        }
        
        _startLevelTime ??= DateTime.Now;

        _pauseButton.Update();
        _speedButton.Update();
            
        if (_indexOpponent != _waves[_indexWave].Count && DateTime.Now.Subtract((DateTime) _startLevelTime).TotalSeconds >= _waves[_indexWave][_indexOpponent].TimeOut / _speed)
        {
            _opponents.Add(_waves[_indexWave][_indexOpponent]);

            _indexOpponent += 1;
            _startLevelTime = DateTime.Now;
        }
            
        foreach (var o in new List<Opponent>(_opponents).Where(o => o.TrackIndex >= o.Track.Count))
        {
            _health -= 1;
            _healthImage.Texts["text"].Caption = $"{_health}";
            
            _opponents.Remove(o);

            if (_health != 0) 
                continue;
            
            break;
        }

        if (_opponents.Count == 0 && _indexWave + 1 == _waves.Count && _indexOpponent == _waves[_indexWave].Count || _health == 0)
        {
            var starCount = (_health / HealthDefault) switch { >= 0.9 => 3, >= 0.8 => 2, >= 0.4 => 1, _ => 0};
            
            Game.StartCount += int.Max(starCount - StartCount, 0);
            StartCount = int.Max(starCount, StartCount);
            
            _isEndGame = true;
            return;
        }
        
        foreach (var o in _opponents)
        {
            o.Speed = o.SpeedDefault * _speed;
            o.Update();
        }
        
        foreach (var platform in _platforms)
            platform.Update();
    }

    public override void Draw()
    {
        _background.Draw();
        
        foreach (var way in _ways)
            way.Draw();

        foreach (var opponent in _opponents)
            opponent.Draw();
        
        _healthImage.Draw();
        
        foreach (var platform in _platforms)
            platform.Draw();
        
        if (_isPause)
        {
            _pauseBackground.Draw();
            
            foreach (var pauseButton in _pauseButtons)
                pauseButton.Draw();
            
            return;
        }
        
        if (_isEndGame)
        {
            Sprite.Draw(Content.Load<Texture2D>("Controls/EndGame"), new Vector2(163, 88), Color.White);
            _endGameButton.Draw();
            
            for (var i = 0; i < StartCount; i++)
                new Image("Controls/Star+", _startPositions[i]).Draw();

            for (var i = StartCount; i < 3; i++)
                new Image("Controls/Star-", _startPositions[i]).Draw();

            return;
        }
        
        _pauseButton.Draw();
        _speedButton.Draw();
    }
}