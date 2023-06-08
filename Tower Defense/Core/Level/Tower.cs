using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Core.Elements;

namespace Tower_Defense.Core.Level;

public static class TowerVariants
{
    public static readonly (double damage, double speed, double range, string textureName) One = (1, 1, 1, "One");
    public static readonly (double damage, double speed, double range, string textureName) Two = (3, 1, 1, "Two");
    public static readonly (double damage, double speed, double range, string textureName) Three = (1, 5, 1, "Three");
    public static readonly (double damage, double speed, double range, string textureName) Four = (2, 2, 5, "Four");
}

public class Tower<T>: Component where T : new()
{
    public bool IsAddTower;
    public bool IsInit;
    
    private List<Button> _buttonsAdd;

    private Image _backgroundAdd;
    private Button _emptyAdd;

    public double Damage;
    public double Speed;
    public double Range;
    private string _texture;
    
    private Button _platform;
    private readonly Vector2 _position;

    private void Click()
    {
        if (!IsInit)
        {
            IsAddTower = true;
            return;
        }
            
    }
    
    private void AddClick((double damage, double speed, double range, string textureName) tower)
    {
        var coin = (GameView.CurrentMenu as Level<T>)!.Coin;
        
        if (coin < 20)
            return;

        (GameView.CurrentMenu as Level<T>)!.Coin -= 20;
            
        Damage = tower.damage;
        Speed = tower.speed;
        Range = tower.range;
        Range = tower.range;
        _texture = tower.textureName;
        
        IsInit = true;
        IsAddTower = false;
        
        _platform = new Button("Level/Tower/" + tower.textureName, _position, Click);
    }
    
    public Tower(Vector2 position)
    {
        _position = position;
        LoadContent();
    }
    
    private void LoadContent()
    {
        _platform = new Button("Level/Map/Platform", _position, Click);
        
        _buttonsAdd = new List<Button>
        {
            new ("Level/Tower/Add/TowerOne", new Vector2(500, 45), () => AddClick(TowerVariants.One)),
            new ("Level/Tower/Add/TowerTwo", new Vector2(500, 140), () => AddClick(TowerVariants.Two)),
            new ("Level/Tower/Add/TowerThree", new Vector2(500, 235), () => AddClick(TowerVariants.Three)),
            new ("Level/Tower/Add/TowerFour", new Vector2(500, 330), () => AddClick(TowerVariants.Four))
        };
        
        _backgroundAdd = new Image("Level/Tower/Add/Menu", new Vector2(472, 0));
        _emptyAdd = new Button("Level/Tower/Add/Empty", new Vector2(0, 0), () => IsAddTower=false);
    }
    
    public override void Update()
    {
        if (IsAddTower)
        {
            foreach (var button in _buttonsAdd)
                button.Update();
        
            _emptyAdd.Update();
        }
        
        _platform.Update();
    }

    public override void Draw()
    {
        _platform.Draw();

        if (IsInit)
        {
            var texture = Content.Load<Texture2D>($"Level/Tower/{_texture}Head");
            Sprite.Draw(Content.Load<Texture2D>($"Level/Tower/{_texture}Head"), new Rectangle((int) _position.X, (int) _position.Y, texture.Width, texture.Height), Color.White);
        }
        
        if (IsAddTower)
        {
            _backgroundAdd.Draw();
            _emptyAdd.Draw();
        
            foreach (var button in _buttonsAdd)
                button.Draw();
        }

    }
}