using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core.Elements;
using Tower_Defense.Menu;

namespace Tower_Defense.Core.Level;

public class EndGame: Component
{
    public bool IsReset;
    
    private List<Vector2> _starPositions;
    private Button _button;
    
    public int StartCount;
    public bool IsEndGame;
    
    public EndGame() { LoadContent(); }
    
    private void LoadContent()
    {
        _starPositions = new List<Vector2> { new (342, 120), new (384, 120), new (426, 120)};
        
        _button = new Button(
            "Controls/ExitMenu", new Vector2(299, 214), () => GameView.ChangeMenu(GameMenu.GetObject)
        );
    }
    
    public override void Update()
    {
        _button.Update();
    }

    public override void Draw()
    {
        Sprite.Draw(Content.Load<Texture2D>("Controls/EndGame"), new Vector2(163, 88), Color.White);
        _button.Draw();
            
        for (var i = 0; i < StartCount; i++)
            new Image("Controls/Star+", _starPositions[i]).Draw();

        for (var i = StartCount; i < 3; i++)
            new Image("Controls/Star-", _starPositions[i]).Draw();
    }
}