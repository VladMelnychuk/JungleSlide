using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    [SerializeField] private RawImage settingButtonIcon;
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private Vector3 buttonRotation;

    [SerializeField] private Button blur;

    private bool _isPaused = false;

    #region SettingsButtons

    [Header("Settings Menu Buttons")]
//    [SerializeField] private Button soundsButton;
//    [SerializeField] private Button musicButton;
    [SerializeField]
    private Button facebookButton;

    [SerializeField] private Button restartButton;

    #endregion

    private bool _isOpen = false;

    private void Start()
    {
        settingButton.onClick.AddListener(Settings);
        blur.onClick.AddListener(ResumeGame);
        DontDestroyOnLoad(this);
    }

    private void Settings()
    {
        var tween = settingButtonIcon.transform.DORotate(_isOpen ? -buttonRotation : buttonRotation,
            AnimationsInfo.UIAnimationDuration);

        // TODO only if game state changes?
        Populate();

        settingsPanel.gameObject.SetActive(!_isOpen);

        switch (Game.GameState)
        {
            case GameState.Playing:
                PauseGame();
                break;
            case GameState.Paused:
                ResumeGame();
                break;
        }

        _isOpen ^= true;
    }

    private void Populate()
    {
        switch (Game.GameState)
        {
            case GameState.Menu:
                facebookButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                break;
            case GameState.Paused:
                // TODO change icon?
                facebookButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                break;
        }
    }

    #region Pause

    private void PauseGame()
    {
        blur.gameObject.SetActive(true);
        blur.GetComponent<CanvasGroup>().DOFade(1, AnimationsInfo.UIAnimationDuration).onComplete += () =>
        {
            Game.GameState = GameState.Paused;
            blur.enabled = true;
        };
    }

    private void ResumeGame()
    {
        blur.enabled = false;
        Game.GameState = GameState.Playing;
        blur.GetComponent<CanvasGroup>().DOFade(0, AnimationsInfo.UIAnimationDuration).onComplete +=
            () => blur.gameObject.SetActive(false);
    }

    #endregion
}