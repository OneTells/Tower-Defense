using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core;

public class Opponent: Component
{
    private readonly List<Vector2> _track;
    public int TrackIndex = 1;
    
    public Vector2 Position;

    private readonly Texture2D _texture;

    public int Health = 10;
    public const double SpeedDefault = 1;
    public double Speed = SpeedDefault;
    
    public Opponent(List<Vector2> track, Texture2D texture)
    {
        _track = track;
        Position = track[0];

        _texture = texture;
    }
    
    public override void Draw()
    {
        Sprite.Draw(_texture, Position, Color.White);
    }

    public override void Update()
    {
        var nextPoint = _track[TrackIndex];
        var currentPoint = _track[TrackIndex - 1];
        
        if (currentPoint.X != nextPoint.X)
        {
            if (Position.X >= nextPoint.X)
            {
                Position = nextPoint;
                TrackIndex += 1;
                return;
            }
                
            Position.X += (float) Speed;
            return;
        }


        if (currentPoint.Y < nextPoint.Y)
        {
            if (Position.Y >= nextPoint.Y)
            {
                Position = nextPoint;
                TrackIndex += 1;
                return;
            }
            
            Position.Y += (float) Speed;
        }
        else
        {
            if (Position.Y <= nextPoint.Y)
            {
                Position = nextPoint;
                TrackIndex += 1;
                return;
            }
            
            Position.Y -= (float) Speed;
        }
    }
}