using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button startGameButton; 
    [SerializeField] private Button settingsButton; 
    [SerializeField] private Button shopButton;

    #region Menus

    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform levelMenu;
    [SerializeField] private RectTransform settingsMenu;
    [SerializeField] private RectTransform shopMenu;
    
    #endregion

    private void Start()
    {
//        DontDestroyOnLoad(this);
        
        var sa = Screen.safeArea;

        AddListeners();
    }


    private void AddListeners()
    {
        startGameButton.onClick.AddListener(() => OpenPanel(levelMenu));
        settingsButton.onClick.AddListener(() => OpenPanel(settingsMenu));
//        shopButton.onClick.AddListener(() => OpenPanel(shopMenu));
    }

    public void LoadLevel()
    {
        LevelLoader.LoadLevel();
    }

    private void OpenPanel(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
    }
}
