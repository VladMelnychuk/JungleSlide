using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool IsPaused = false;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    [SerializeField] private RectTransform pauseMenu;

    private void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

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
}