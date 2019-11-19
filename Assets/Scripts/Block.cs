using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector2Int gridNewPosition;

    public int size = 1;

    private void OnEnable()
    {
        gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }
}