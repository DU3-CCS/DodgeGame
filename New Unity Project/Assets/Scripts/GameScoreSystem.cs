using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreSystem : MonoBehaviour {

    public Text Stage;        //체력 표시 UI
    public Text Best_Score;
    public Text Score;

    public float timeCount = 0;
    public int bestScore = 0;

    // Use this for initialization
    void Start()
    {
        Stage = GameObject.Find("Stage").GetComponent<Text>();
        Best_Score = GameObject.Find("Best_Score_Text").GetComponent<Text>();
        Score = GameObject.Find("Score_Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Stage.text = "Stage 1";
        Best_Score.text = "5000";
        Score.text = GameManager.gameScore.ToString();
    }
}