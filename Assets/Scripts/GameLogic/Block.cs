using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
//    public BlockTypes blockType;
    public List<Vector2Int> blockPositions = new List<Vector2Int>();
    public int size = 1;
    
    Vector3 oldPos = Vector3.zero;

    public bool debugger = false;
    
    private void Update()
    {
        if (!debugger) return;
        if (transform.position == oldPos) return;
            
        oldPos = transform.position;
        Debug.LogError(oldPos);
    }
}
