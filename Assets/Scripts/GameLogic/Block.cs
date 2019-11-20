using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, ISpawnEvent
{
    public Vector2Int gridPosition;

    public int size = 1;
    
    public ObjectPool pool;

    private void OnEnable()
    {
        var position = transform.position;
        gridPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    public void OnSpawned(GameObject targetGameObject, ObjectPool sender)
    {
        pool = sender;
    }

    public void Despawn()
    {
        pool.Despawn(this.gameObject);
    }
    
}