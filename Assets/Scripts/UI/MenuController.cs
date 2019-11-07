using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button startGameButton; 
    [SerializeField] private Button settingsButton; 
    [SerializeField] private Button shopButton;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;

    private void Start()
    {
        DontDestroyOnLoad(this);
        
        AddListeners();
    }


    private void AddListeners()
    {
        startGameButton.onClick.AddListener(ChooseLevel);
    }

    private void ChooseLevel()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void LoadLevel()
    {
        LevelLoader.LoadLevel();
    }
    
}
