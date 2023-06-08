using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core.Elements;
using Tower_Defense.Menu;

namespace Tower_Defense.Core.Level;

public class Pause<T> : Component where T : new()
{
    private List<Button> _buttons;
    private Image _background;
    
    public bool IsPause;
    
    public Pause() { LoadContent(); }

    private void LoadContent()
    {
        _buttons = new List<Button>
        {
            new ("Controls/ExitMenu", new Vector2(299, 185), () => GameView.ChangeMenu(GameMenu.GetObject)),
            new ("Controls/Close", new Vector2(597, 81), ()=> IsPause = false),
            new ("Level/Pause/Restart", new Vector2(299, 250), () => (GameView.CurrentMenu as Level<T>)!.Reset())
        };
        
        _background = new Image("Controls/PauseMenu", new Vector2(155, 88));
    }
    
    public override void Update()
    {
        foreach (var pauseButton in _buttons) 
            pauseButton.Update();
    }
    
    public override void Draw()
    {
        _background.Draw();
            
        foreach (var pauseButton in _buttons)
            pauseButton.Draw();
    }
}