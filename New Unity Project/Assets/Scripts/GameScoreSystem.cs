using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.SqliteClient;

public class GameScoreSystem : MonoBehaviour {

    public Text Stage;        //체력 표시 UI
    public Text Best_Score;
    public Text Score;
    public Text Gold;

    public int stage = 1;
    public float timeCount = 0;
    public int bestScore = 0;

    // sql 인스턴스 변수
    IDbConnection dbc;
    IDbCommand dbcm;        //SQL문 작동 개체
    IDataReader dbr;        //반환된 값 읽어주는 객체

    // Use this for initialization
    void Start()
    {
        Stage = GameObject.Find("Stage").GetComponent<Text>();
        Best_Score = GameObject.Find("Best_Score_Text").GetComponent<Text>();
        Score = GameObject.Find("Score_Text").GetComponent<Text>();
        Gold = GameObject.Find("Gold").GetComponent<Text>();

        string constr = "URI=file:character.db";

        dbc = new SqliteConnection(constr);
        dbc.Open();
        dbcm = dbc.CreateCommand();
    }

    // Update is called once per frame
    void Update()
    {
        Stage.text = "Stage " + stage;
        Score.text = GameManager.gameScore.ToString();
        Gold.text = GameManager.gold.ToString() + "G";

        bestScore = selectBestScore();
        Best_Score.text = bestScore.ToString();

        
    }

    int selectBestScore()
    {
        int dbScore;

        dbcm.CommandText = "SELECT Score FROM StageScore WHERE Stage = " + stage;

        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            dbScore = dbr.GetInt16(0);
        }
        else
        {
            dbScore = 0;
        }

        return dbScore;
    }

    // 현재 골드, 수익 골드, 쌓은 점수 
    void updateCharacter(int gold)
    {
        int money;
        int revenueMoney;

        dbcm.CommandText = "SELECT * FROM Character";
        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            money = dbr.GetInt16(1);
            revenueMoney = dbr.GetInt16(2);
        }
        else
        {
            money = 0;
            revenueMoney = 0;
        }
        money += gold;
        revenueMoney += gold;

        dbcm.CommandText = "UPDATE Character SET money = " + money + " and revenueMoney = " + revenueMoney;
        dbcm.ExecuteNonQuery();

        // update Reward
        dbcm.CommandText = "SELECT condition, state FROM record WHERE ID BETWEEN 200 and 299";
        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if(dbr.GetInt16(1) == 0 && revenueMoney >= dbr.GetInt16(0))
            {

            }
        }
    }

    void updateRecord(int score)
    {

    }

    void OnDisable()
    {
        dbr.Close();
        dbr = null;
        dbcm.Dispose();
        dbcm = null;
        dbc.Dispose();
        dbc = null;
    }
}