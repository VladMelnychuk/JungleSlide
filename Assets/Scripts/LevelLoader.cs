using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoader
{
    public static string CurrentLevelName = "lvl1";

    public static void LoadLevel()
    {
        CurrentLevelName = EventSystem.current.currentSelectedGameObject.name;
        Game.GameState = GameState.Playing;
        SceneManager.LoadScene(1);
    }
}