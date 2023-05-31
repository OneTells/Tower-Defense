using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;

namespace Tower_Defense.Menu;

public sealed class MainMenu : Core.Menu
{
    private static MainMenu _object;
    
    public static MainMenu GetObject => _object ??= new MainMenu();
    
    private Background _background;
    private List<Button> _buttons;

    private MainMenu() { LoadContent(); }
    
    protected override void LoadContent()
    {
        _background = new Background("Background/Menu");
        
        _buttons = new List<Button>
        {
           new ("Controls/New Game")
           {
               Position = new Vector2(236, 206),
               Click = () => GameView.ChangeMenu(GameMenu.GetObject)
           },
           new ("Controls/Store")
           {
               Position = new Vector2(236, 275),
               Click = () => GameView.ChangeMenu(StoreMenu.GetObject)
           },
           new ("Controls/Exit")
           {
               Position = new Vector2(401, 275),
               Click = () => Game.Exit()
           }
        };
    }

    public override void Update()
    {
        foreach (var button in _buttons)
            button.Update();
    }
    
    public override void Draw()
    {
        Sprite.Begin();
        
        _background.Draw();

        foreach (var button in _buttons)
            button.Draw();
        
        Sprite.End();
    }
}