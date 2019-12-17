using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool IsPaused = false;
    public static bool IsSett = false;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private RectTransform pauseMenu;
    [SerializeField] private RectTransform settings;


    private void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        replayButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(HomeFunc);
        settingsButton.onClick.AddListener(SettingsFunc);
    }

    private void SettingsFunc(){
        settings.gameObject.SetActive(true);
        IsSett = true;
    }

    private void HomeFunc(){}

    private void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void RestartGame()
    {
        pauseMenu.gameObject.SetActive(false);
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}