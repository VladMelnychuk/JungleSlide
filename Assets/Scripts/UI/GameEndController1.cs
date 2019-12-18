using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndController1: MonoBehaviour
{
    // Start is called before the first frame 
    public static bool IsEnd = false;

    [SerializeField] private Button gameEnd;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private RectTransform GameEndMenu;

    private void Start()
    {
        gameEnd.onClick.AddListener(GameEnd);
        replayButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(Home);
    }

    private void Home(){}

    private void GameEnd(){
    	GameEndMenu.gameObject.SetActive(true);
    	IsEnd = true;
    }

    private void RestartGame()
    {
        GameEndMenu.gameObject.SetActive(false);
        IsEnd = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
