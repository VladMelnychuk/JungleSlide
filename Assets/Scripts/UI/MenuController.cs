using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button startGameButton; 
    [SerializeField] private Button settingsButton; 
    [SerializeField] private Button shopButton;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;

    [SerializeField] private RectTransform settingsMenu;
    [SerializeField] private RectTransform shopMenu;

    private void Start()
    {
        DontDestroyOnLoad(this);
        
        AddListeners();
    }


    private void AddListeners()
    {
        startGameButton.onClick.AddListener(ChooseLevel);
        settingsButton.onClick.AddListener(OpenSetings);
        shopButton.onClick.AddListener(OpenShop);
    }

    private void ChooseLevel()
    {
        levelMenu.SetActive(true);
    }

    public void LoadLevel()
    {
        LevelLoader.LoadLevel();
    }

    private void OpenSetings()
    {
        settingsMenu.gameObject.SetActive(true);
    }

    private void OpenShop()
    {
        shopMenu.gameObject.SetActive(true);
    }

    private void ShowMenu(RectTransform menu)
    {
        
    }
    
}
