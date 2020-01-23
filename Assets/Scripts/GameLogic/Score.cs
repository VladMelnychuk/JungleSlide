using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
//	private TMP_Text scoreBox;
    [SerializeField] private Text scoreBox;
    private float scoreNum;
    private int level_score = 200;

    void Start()
    {
        
        scoreNum = 0;
//        scoreBox = GetComponent<TMP_Text>();
    }

    void Update()
    {
        scoreBox.text = "" + Mathf.Round(scoreNum).ToString();
    }

    public void UpdScore(int score)
    {
        scoreNum += score;
       if(score == level_score)
        {
            print("GAME END");
        }
    }
}