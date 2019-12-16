using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoader
{
    public static string CurrentLevelName;

    public static void LoadLevel()
    {
        CurrentLevelName = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(1);
    }
}