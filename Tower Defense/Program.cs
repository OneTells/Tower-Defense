namespace Tower_Defense;

public static class Program
{
    public static void Main()
    {
       using var game = GameView.GetObject;
       game.Run(); 
    }
}
