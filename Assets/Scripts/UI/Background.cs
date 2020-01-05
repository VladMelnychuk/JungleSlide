using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Background : MonoBehaviour
{
    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        var sprite = sr.sprite;

        var width = sprite.bounds.size.x;
        var height = sprite.bounds.size.y;
     
        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        var newScale = new Vector3 {x = (float) (worldScreenWidth / width), y = (float) (worldScreenHeight / height)};
        transform.localScale = newScale;
        
    }
}
