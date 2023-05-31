using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;

namespace Tower_Defense.Menu;

public sealed class MainMenu : Menu<MainMenu>
{
    protected override void LoadContent()
    {
        Background = new Background("Background/Menu");
        
        Components = new Dictionary<string, Component>
        {
            { "New Game", new Button("Controls/New Game", new Vector2(236, 206), () => GameView.ChangeMenu(GameMenu.GetObject))},
            { "Store", new Button("Controls/Store", new Vector2(236, 275), () => GameView.ChangeMenu(StoreMenu.GetObject))},
            { "Exit", new Button("Controls/Exit", new Vector2(401, 275), () => Game.Exit())}
        };
    }

    public override void Update()
    {
        foreach (var (_, component) in Components)
            component.Update();
    }
    
    public override void Draw()
    {
        Background.Draw();

        foreach (var (_, component) in Components)
            component.Draw();
    }
}