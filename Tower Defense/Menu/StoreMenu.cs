using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tower_Defense.Core;

namespace Tower_Defense.Menu;

public class StoreMenu : Menu<StoreMenu>
{
    private static int _power;
    private static int _length;
    private static int _speed;
    
    private void ShopButton_Click(ref int parameter, string parameterName)
    {
        if (parameter + 1 > Game.StartCount) 
            return;
        
        parameter += 1;
        Game.StartCount -= parameter;
        
        var texts = ((Button) Components[parameterName]).Texts;
        
        texts["percent"].Caption = $"+{10 * parameter}%";
        texts["star"].Caption = $"{parameter + 1}";
    }
    
    protected override void LoadContent()
    {
        Background = new Background("Background/Store");
        
        Components = new Dictionary<string, Component>
        {
            { "powerButton", new Button("Controls/Power", new Vector2(235, 101), () => ShopButton_Click(ref _power, "powerButton"))
            {
                Texts = new Dictionary<string, Text>
                {
                    { "percent", new Text("Fonts/Font1", new Vector2(410, 114), $"+{10*_power}%") },
                    { "star", new Text("Fonts/Font1", new Vector2(520, 145), $"{_power + 1}") }
                }
            }},
            { "speedButton", new Button("Controls/Speed", new Vector2(235, 308), () => ShopButton_Click(ref _speed, "speedButton"))
            {
                Texts = new Dictionary<string, Text>
                {
                    {"percent", new Text("Fonts/Font1", new Vector2(466, 321), $"+{10*_speed}%")},
                    {"star", new Text("Fonts/Font1", new Vector2(520, 353), $"{_speed + 1}")}
                }
            }},
            { "lengthButton", new Button("Controls/Length", new Vector2(235, 204), () => ShopButton_Click(ref _length, "lengthButton"))
            {
                Texts = new Dictionary<string, Text>
                {
                    { "percent", new Text("Fonts/Font1", new Vector2(486, 216), $"+{10*_length}%") },
                    { "star", new Text("Fonts/Font1", new Vector2(520, 250), $"{_length + 1}") }
                }
            }},
            { "closeButton", new Button("Controls/Close",  new Vector2(604, 1), () => GameView.ChangeMenu(MainMenu.GetObject))}
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