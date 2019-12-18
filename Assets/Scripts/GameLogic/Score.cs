using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
	private TMP_Text scoreBox;
	private float scoreNum;

    void Start()
    {	
    	scoreNum = 0;
        scoreBox = GetComponent<TMP_Text>();
    }

    void Update()
    {
        scoreBox.text = "" + Mathf.Round(scoreNum).ToString();
    }

    public void UpdScore(int score){
    	scoreNum += score;
//    	print("Score: " + scoreNum);
    }
}
