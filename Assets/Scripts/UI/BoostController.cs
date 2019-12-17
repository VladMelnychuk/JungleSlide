using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoostController : MonoBehaviour
{
	public Text boost1Title;
	public Text boost2Title;
	[SerializeField] private Button boost1Button; 
    [SerializeField] private Button boost2Button; 

    [SerializeField] private RectTransform boostShop;

    public static bool IsShop = false;

    private string boost1num;
    private string boost2num;

    private void Start()
    {
        boost1Button.onClick.AddListener(CheckBoost1);
        boost2Button.onClick.AddListener(CheckBoost2);
    }

    private void CheckBoost1(){
    	boost1Title=GameObject.Find("Boost1").GetComponent<Text>();
    	boost1num = boost1Title.text;
    	if(boost1num == "0"){
    		print("NEED TO OPEN SHOP!");
    		shopBoost();
    	}
    }

    private void CheckBoost2(){
    	boost2Title=GameObject.Find("Boost2").GetComponent<Text>();
    	boost2num = boost2Title.text;
    	if(boost2num=="0"){
    		print("NEED TO OPEN SHOP!");
    	}
    }

    private void shopBoost(){
    	IsShop = true;
    }
}
