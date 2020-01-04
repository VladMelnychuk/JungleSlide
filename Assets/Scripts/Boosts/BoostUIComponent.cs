using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoostUIComponent : MonoBehaviour
{
    [SerializeField] private Boost currentBoost;

    public static bool IsBoostActive;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var pos = Input.mousePosition;
            _rectTransform.position = pos;
            currentBoost.Interact(pos);
        }
    }

    private IEnumerable FollowBoost()
    {
        while (IsBoostActive)
        {
            var uiPos = Camera.main.WorldToScreenPoint(currentBoost.transform.position);
            transform.position = uiPos;
            yield return null;
        }
    }
}