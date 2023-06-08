using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tower_Defense.Menu;


namespace Tower_Defense;

public class GameView : Game
{
    private static GameView _object;
    public static GameView GetObject => _object ??= new GameView();
    
    public SpriteBatch Sprite;
    
    private static GraphicsDeviceManager _graphics;
    public static Core.Component CurrentMenu { get; private set; }
    
    public int StartCount = 1000;
    
    private GameView()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "C:/Users/egork/Desktop/Tower Defense/Tower Defense/Tower Defense/Content/bin/Windows";
      IsMouseVisible = true;
    }
    
    public static void ChangeMenu(Core.Component menu)
    {
      CurrentMenu = menu;
    }
    
    protected override void Initialize()
    {
      _graphics.IsFullScreen = false;
      _graphics.PreferredBackBufferWidth = 800;
      _graphics.PreferredBackBufferHeight = 450;
      _graphics.ApplyChanges();

      base.Initialize();
    }
    
    protected override void LoadContent()
    {
      Sprite = new SpriteBatch(GraphicsDevice);
      CurrentMenu = MainMenu.GetObject;
    }
    
    protected override void Update(GameTime gameTime)
    {
      CurrentMenu.Update();
      base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Gray);
      
      Sprite.Begin();
      CurrentMenu.Draw();
      Sprite.End();
      
      base.Draw(gameTime);
    }
}