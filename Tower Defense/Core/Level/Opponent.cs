using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defense.Core.Level;

public static class OpponentVariants
{
    public static readonly (int health, double speed, string textureName) Normal = (4, 1, "Level/Opponent/Circle");
    public static readonly (int health, double speed, string textureName) Flash = (2, 2, "Level/Opponent/Square");
    public static readonly (int health, double speed, string textureName) Hulk = (6, 0.5, "Level/Opponent/Triangle");
}

public class Opponent: Component
{
    public readonly List<Vector2> Track;
    public int TrackIndex = 1;

    public Vector2 Position;

    public readonly Texture2D Texture;

    public double Health;
    private readonly double _healthDefault;
    public readonly double SpeedDefault;
    public double Speed;
    public readonly double TimeOut;
    
    public Opponent(IEnumerable<Vector2> track, (int health, double speed, string textureName) opponent, double timeOut)
    {
        TimeOut = timeOut;
        
        Health = opponent.health;
        _healthDefault = opponent.health;
        SpeedDefault = opponent.speed;
        Speed = SpeedDefault;
        Texture = Content.Load<Texture2D>(opponent.textureName);
        Track = track.Select(pos=> new Vector2(pos.X - Texture.Width / (float) 2, pos.Y - Texture.Height / (float) 2)).ToList();
        Position = Track[0];
    }
    
    public override void Draw()
    {
        Sprite.Draw(Texture, Position, Color.White);

        var healthTexture = Content.Load<Texture2D>("Level/Opponent/Health/" + (Health / _healthDefault) switch
        {
            >= 1 => "100", >= 0.9 => "90", >= 0.8 => "80", >= 0.7 => "70", >= 0.6 => "60", >= 0.5 => "50", >= 0.4 => "40",
            >= 0.3 => "30", >= 0.2 => "20", >= 0.1 => "10", _ => "10"
        });
        
        Sprite.Draw(
            healthTexture, 
            new Vector2(Position.X + Texture.Width / (float) 2 - healthTexture.Width / (float) 2, Position.Y - 5 - healthTexture.Height), Color.White
      );
    }

    public override void Update()
    {
        var currentPoint = Track[TrackIndex];
        var lastPoint = Track[TrackIndex - 1];
        
        if (lastPoint.X < currentPoint.X)
        {
            if (Position.X + Speed >= currentPoint.X)
            {
                Position = currentPoint;
                TrackIndex += 1;
            }
            else
                Position.X += (float) Speed;
            
            return;
        }
        
        if (lastPoint.X > currentPoint.X)
        {
            if (Position.X + Speed <= currentPoint.X)
            {
                Position = currentPoint;
                TrackIndex += 1;
            }
            else
                Position.X -= (float) Speed;
            
            return;
        }
        
        if (lastPoint.Y < currentPoint.Y)
        {
            if (Position.Y + Speed >= currentPoint.Y)
            {
                Position = currentPoint;
                TrackIndex += 1;
            }
            else
                Position.Y += (float) Speed;
            
            return;
        }
        
        if (lastPoint.Y > currentPoint.Y)
        {
            if (Position.Y + Speed <= currentPoint.Y)
            {
                Position = currentPoint;
                TrackIndex += 1;
                return;
            }
            
            Position.Y -= (float) Speed;
        }
    }
}