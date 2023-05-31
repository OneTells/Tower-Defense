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
    private static Core.Component _currentMenu;
    
    public int StartCount = 1000;
    
    private GameView()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "C:/Users/egork/Desktop/Tower Defense/Tower Defense/Tower Defense/Content/bin/Windows";
      IsMouseVisible = true;
    }
    
    public static void ChangeMenu(Core.Component menu)
    {
      _currentMenu = menu;
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
      _currentMenu = MainMenu.GetObject;
    }
    
    protected override void Update(GameTime gameTime)
    {
      _currentMenu.Update();
      base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Gray);
      
      Sprite.Begin();
      _currentMenu.Draw();
      Sprite.End();
      
      base.Draw(gameTime);
    }
}