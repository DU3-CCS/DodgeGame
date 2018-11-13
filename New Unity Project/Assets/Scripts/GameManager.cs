using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text timeLabel;
    public float timeCount = 0;
    public static int gameScore = 0;
    public static int gold = 0;
    //public int gameScore_item01Cnt = PlayerMove.item01Cnt;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeCount += Time.deltaTime;
        timeLabel.text = string.Format ("{0:N2}", timeCount);
        
    }

    private void OnDisable()
    {
        gameScore += (int)timeCount*100;
        gameScore += PlayerMove.item01Cnt * 100;
        gameScore += PlayerMove.item02Cnt * 200;
        gameScore -= PlayerMove.item03Cnt * 100;

        gold = gameScore / 100;
    }
}
