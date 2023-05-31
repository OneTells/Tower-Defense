using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;
using Tower_Defense.Levels;

namespace Tower_Defense.Menu;

public sealed class GameMenu : Core.Menu
{
    private static GameMenu _object;
    
    public static GameMenu GetObject => _object ??= new GameMenu();
    
    private List<Component> _buttons;

    private GameMenu()
    {
        LoadContent();
    }
    
    private static void LevelOneButton_Click()
    {
        GameView.ChangeMenu(FirstLevel.GetObject);
    }
    
    private static void BackButton_Click()
    {
        GameView.ChangeMenu(MainMenu.GetObject);
    }
    
    protected override void LoadContent()
    {
        var newGameButton = new Button("Controls/New Game")
        {
            Position = new Vector2(244, 200)
        };
        
        newGameButton.Click += LevelOneButton_Click;
        
        var backButton = new Button("Controls/Back")
        {
            Position = new Vector2(400, 300)
        };
        
        backButton.Click += BackButton_Click;
        
        _buttons = new List<Component>
        {
            newGameButton,
            backButton
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
        
        foreach (var button in _buttons)
            button.Draw();

        Sprite.End();
    }
}