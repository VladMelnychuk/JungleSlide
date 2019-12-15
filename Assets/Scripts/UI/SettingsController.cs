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

    private bool _isOpen = false;

    private void Start()
    {
        settingButton.onClick.AddListener(Settings);
        DontDestroyOnLoad(this);
    }

    private void Settings()
    {
        print("Settings()");
        if (_isOpen)
        {
            // close
            
            settingButtonIcon.transform.DORotate(-buttonRotation, .5f);
            settingsPanel.gameObject.SetActive(false);
//            settingsPanel.DOMove(settingsPanel.transform.position + Vector3.up * 300, .5f);
//            settingsPanel.
        }
        else
        {
            // open
            settingButtonIcon.transform.DORotate(buttonRotation, .5f);
            settingsPanel.gameObject.SetActive(true);
//            settingsPanel.DOMove(settingsPanel.transform.position + Vector3.up * -300, .5f);
        }

        _isOpen ^= true;
    }
    
}
