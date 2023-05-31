using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;
using Tower_Defense.Levels;

namespace Tower_Defense.Menu;

public sealed class GameMenu : Menu<GameMenu>
{
    protected override void LoadContent()
    {
        Components = new Dictionary<string, Component>
        {
            { "New Game", new Button("Controls/New Game", new Vector2(244, 200), () => GameView.ChangeMenu(FirstLevel.GetObject))},
            { "Back", new Button("Controls/Back", new Vector2(400, 300), () => GameView.ChangeMenu(MainMenu.GetObject))}
        };
    }

    public override void Update()
    {
        foreach (var (_, component) in Components)
            component.Update();
    }
    
    public override void Draw()
    {
        foreach (var (_, component) in Components)
            component.Draw();
    }
}