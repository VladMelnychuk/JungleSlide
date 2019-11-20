using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    DictionaryObjectPool _objectpool;
    [SerializeField]
    GameObject[] gos;
    [SerializeField]
    int numberstospawn = 1000;
	// Use this for initialization
	void Start ()
    {
        _objectpool = new DictionaryObjectPool();
        _objectpool.AddObjectPool("squares", gos[0], this.transform, 100);
        _objectpool.AddObjectPool("spheres", gos[1], this.transform, 100);
    }

    // Update is called once per frame
    void Update ()
    {
        if (_objectpool["spheres"].Count < numberstospawn)
        {
            _objectpool["spheres"].Spawn(this.transform.position);
        }
        if (_objectpool["squares"].Count < numberstospawn)
        {
            _objectpool["squares"].Spawn(this.transform.position);
        }
    }
}
