using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;
using Tower_Defense.Core.Elements;
using Tower_Defense.Levels;

namespace Tower_Defense.Menu;

public sealed class GameMenu : Menu<GameMenu>
{
    protected override void LoadContent()
    {
        Components = new Dictionary<string, Component>
        {
            { "New Game1", new Button("Controls/Level+", new Vector2(90, 105), () => GameView.ChangeMenu(FirstLevel.GetObject))},
            { "New Game2", new Button("Controls/Level+", new Vector2(250, 105), () => GameView.ChangeMenu(SecondLevel.GetObject))},
            { "New Game3", new Button("Controls/Level+", new Vector2(90, 300), () => GameView.ChangeMenu(ThirdLevel.GetObject))},
            { "New Game4", new Button("Controls/Level+", new Vector2(250, 300), () => GameView.ChangeMenu(ThirdLevel.GetObject))},
            { "New Game5", new Button("Controls/Level+", new Vector2(410, 300), () => GameView.ChangeMenu(ThirdLevel.GetObject))},
            { "Back", new Button("Controls/Back", new Vector2(600, 370), () => GameView.ChangeMenu(MainMenu.GetObject))},
            { "star", new Image("Controls/Star+", new Vector2(650, 10))
            {
                Texts = new Dictionary<string, Text>
                {
                    { "star", new Text("Fonts/Font1", new Vector2(700, 15), $"{Game.StartCount}", false) }
                }
            }}
        };
        
        Background = new Background("Controls/Menu1");
    }

    public override void Update()
    {
        ((Image)Components["star"]).Texts["star"].Caption = $"{Game.StartCount}";
        
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