using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core.Elements;
using Tower_Defense.Menu;

namespace Tower_Defense.Core.Level;

public class EndGame<T>: Component where T : new()
{
    private List<Vector2> _starPositions;
    private List<Button> _buttons;
    
    private Image _background;
    
    public int StartCount;
    public bool IsEndGame;
    
    public EndGame() { LoadContent(); }
    
    private void LoadContent()
    {
        _starPositions = new List<Vector2> { new (342, 120), new (384, 120), new (426, 120)};
        
        _buttons = new List<Button>
        {
            new ("Controls/ExitMenu", new Vector2(299, 250), () => GameView.ChangeMenu(GameMenu.GetObject)),
            new ("Level/Pause/Restart", new Vector2(299, 185), () => (GameView.CurrentMenu as Level<T>)!.Reset())
        };
        
        _background = new Image("Controls/EndGame", new Vector2(163, 88));
    }
    
    public override void Update()
    {
        foreach (var button in _buttons)
            button.Update();
    }

    public override void Draw()
    {
        _background.Draw();
        
        foreach (var button in _buttons)
            button.Draw();
            
        for (var i = 0; i < StartCount; i++)
            new Image("Controls/Star+", _starPositions[i]).Draw();

        for (var i = StartCount; i < 3; i++)
            new Image("Controls/Star-", _starPositions[i]).Draw();
    }
}