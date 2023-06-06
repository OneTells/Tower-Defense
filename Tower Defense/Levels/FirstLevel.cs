using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;

namespace Tower_Defense.Levels;

public sealed class FirstLevel : Level<FirstLevel>
{
    protected override double HealthDefault { get; init; } = 500;
    
    protected override List<(string code, Vector2 start)> InitializationTrackCodes()
    {
        return new List<(string code, Vector2 start)>
        {
            ("rrrddrrr", new Vector2(100, 120)), ("drrddlddrruurr", new Vector2(164, 56))
        };
    }

    protected override List<List<Opponent>> InitializationWaves()
    {
        return new List<List<Opponent>>
        {
            new () { 
                new Opponent(Tracks[0], OpponentVariants.Normal, 2), 
                new Opponent(Tracks[1], OpponentVariants.Flash, 2), 
                new Opponent(Tracks[1], OpponentVariants.Hulk, 2) 
            },
            new () { 
                new Opponent(Tracks[1], OpponentVariants.Flash, 2), 
                new Opponent(Tracks[1], OpponentVariants.Flash, 2), 
                new Opponent(Tracks[1], OpponentVariants.Flash, 2) 
            }
        };
    }
    
    protected override List<Vector2> InitializationPlatforms()
    {
        return new List<Vector2>
        {
            new (420, 120)
        };
    }
}