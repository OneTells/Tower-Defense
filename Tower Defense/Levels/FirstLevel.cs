﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;
using Tower_Defense.Core.Level;

namespace Tower_Defense.Levels;

public sealed class FirstLevel : Level<FirstLevel>
{
    protected override double HealthDefault { get; init; } = 500;
    
    protected override List<(string code, Vector2 start)> InitializationTrackCodes()
    {
        return new List<(string code, Vector2 start)>
        {
            ("rrrddrrr", new Vector2(100, 120))
        };
    }

    protected override List<List<Opponent>> InitializationWaves()
    {
        return new List<List<Opponent>>
        {
            new () { 
                new Opponent(Tracks[0], OpponentVariants.Normal, 2), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 2), 
                new Opponent(Tracks[0], OpponentVariants.Hulk, 2),
                new Opponent(Tracks[0], OpponentVariants.Normal, 2),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1),
                new Opponent(Tracks[0], OpponentVariants.Normal, 1)
            },
            new () { 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1), 
                new Opponent(Tracks[0], OpponentVariants.Flash, 1),
                new Opponent(Tracks[0], OpponentVariants.Hulk, 1),
                new Opponent(Tracks[0], OpponentVariants.Hulk, 1),
                new Opponent(Tracks[0], OpponentVariants.Hulk, 1),
                new Opponent(Tracks[0], OpponentVariants.Hulk, 1)
            }
        };
    }
    
    protected override List<Vector2> InitializationPlatforms()
    {
        return new List<Vector2>
        {
            new (132, 152), new (196, 152), new (324, 152), new (388, 152)
        };
    }
}