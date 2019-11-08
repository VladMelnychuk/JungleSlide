using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Time : MonoBehaviour
{
	private TMP_Text timeBox;
	public float timeStart;
    
    void Start()
    {
        timeBox = GetComponent<TMP_Text>();
    }
    void Update()
    {
    	timeStart += UnityEngine.Time.deltaTime;
        timeBox.text = "" + Mathf.Round(timeStart).ToString();
		// print(Mathf.Round(timeStart).ToString());
    }
}
