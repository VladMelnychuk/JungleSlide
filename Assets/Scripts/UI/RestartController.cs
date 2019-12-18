using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartController : MonoBehaviour
{
    private Button _selfButton;

    private void Start()
    {
        _selfButton = GetComponent<Button>();
        _selfButton.onClick.AddListener(RestartGame);
    }

    private static void RestartGame()
    {
        // TODO manually restart game?
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
