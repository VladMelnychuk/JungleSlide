public class Game
{
    public static GameState GameState = GameState.Menu;

    public static void ChangeGameState(GameState state)
    {
        
    }
}

public enum GameState
{
    Menu, Playing, Paused
}
