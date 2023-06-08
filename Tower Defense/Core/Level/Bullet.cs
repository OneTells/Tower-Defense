using System;
using Microsoft.Xna.Framework;
using Tower_Defense.Core.Elements;

namespace Tower_Defense.Core.Level;

public class Bullet: Component
{
    private Image _image;
    public readonly Opponent Opponent;
    public Vector2 Position;

    public Bullet(Vector2 position, Opponent opponent)
    {
        Position = new Vector2(position.X + 32, position.Y + 32);
        Opponent = opponent;
        
        LoadContent();
    }
    
    private void LoadContent()
    {
        _image = new Image("Level/Tower/Bullet", Position);
    }
    
    public override void Update()
    {
        if (Math.Abs(Opponent.Position.X - Position.X) > 1)
        {
            if (Opponent.Position.X >Position.X)
                Position.X += float.Min(5 * (float) Opponent.Speed, Math.Abs(Opponent.Position.X - Position.X));
            else
                Position.X -= float.Min(5 * (float) Opponent.Speed, Math.Abs(Opponent.Position.X - Position.X));
        }
        
        if (Math.Abs(Opponent.Position.Y - Position.Y) > 1)
        {
            if (Opponent.Position.Y >Position.Y)
                Position.Y += float.Min(5 * (float) Opponent.Speed, Math.Abs(Opponent.Position.Y - Position.Y));
            else
                Position.Y -= float.Min(5 * (float) Opponent.Speed, Math.Abs(Opponent.Position.Y - Position.Y));
        }
        
        _image.Position = Position;
    }

    public override void Draw()
    {
        _image.Draw();
    }
}