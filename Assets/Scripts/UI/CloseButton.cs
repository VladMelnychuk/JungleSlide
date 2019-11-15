using System;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button selfButton;
    [SerializeField] private RectTransform selfPanel;

    private void Start()
    {
        selfButton.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        // TODO add animations
        selfPanel.gameObject.SetActive(false);
    }
}
