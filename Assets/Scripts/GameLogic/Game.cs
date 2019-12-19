using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Game : MonoBehaviour
{
    public static GameState GameState = GameState.Menu;

    public static JObject Levels;

    private void Start()
    {
        StartCoroutine(GetLevelsFromFile());
    }

    private static IEnumerator GetLevelsFromFile()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Levels", "levels.json");

        var req = UnityWebRequest.Get(path);

        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            var jObject = JObject.Parse(req.downloadHandler.text);
            
            Levels = jObject;
        }
    }
}

public enum GameState
{
    Menu,
    Playing,
    Paused
}