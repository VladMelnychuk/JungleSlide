using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoader
{
    public static string CurrentLevelName = "def";

    public static void LoadLevel()
    {
        CurrentLevelName = EventSystem.current.currentSelectedGameObject.name;
        Game.GameState = GameState.Playing;
        SceneManager.LoadScene(1);
    }
}