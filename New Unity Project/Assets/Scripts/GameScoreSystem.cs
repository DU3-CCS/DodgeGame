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

    public static int stage;
    public float timeCount = 0;
    public int bestScore = 0;
    private int clearScore = 15000;

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
        Debug.Log(stage);
        dbc = new SqliteConnection(constr);
        dbc.Open();
        dbcm = dbc.CreateCommand();

        int score = GameManager.gameScore;
        int gold = GameManager.gold;
        int revenueMoney;
        int clearCount;

        Stage.text = "Stage " + stage;
        Score.text = score.ToString();
        Gold.text = gold + "G";

        bestScore = SelectBestScore(score);         // DB에서 해당 스테이지 최고점수 반환
        Best_Score.text = bestScore.ToString();     // 최고점수 세팅

        clearCount = CheckClearCount(score);        // 클리어 여부 판별 후 DB에 클리어 횟수 업데이트 후 반환
        UpdateClearRecord(clearCount);              // 클리어 횟수에 관한 업적 업데이트

        revenueMoney = UpdateCharacter(gold);       // 소지금과 총 수익 골드를 업데이트 후 총 수익 골드를 반환
        UpdateMoneyRecord(revenueMoney);            // 총 수익 관련 업적 업데이트

        UpdatePlayCountRecord();                    // 총 플레이 횟수에 관한 업적 업데이트
        //UpdateScoreRecord(bestScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// DB로부터 해당 스테이지의 최고 점수 찾아 반환합니다.
    /// </summary>
    /// <param name="score">이번 판에서 획득한 점수</param>
    /// <returns>int 최고 점수</returns>
    int SelectBestScore(int score)
    {
        int dbScore;

        dbcm.CommandText = "SELECT score, playCount, clearCount FROM Stage WHERE Stage = " + stage;
        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            dbcm.CommandText = "UPDATE Stage SET playCount = " + (dbr.GetInt16(1) + 1);
            dbcm.ExecuteNonQuery();
            Debug.Log("stage = " + stage + ", score = " + score);
            // DB에 저장된 최고 점수와 현재 점수 비교
            if (dbr.GetInt16(0) > score)
                dbScore = dbr.GetInt16(0);
            else
            {
                dbcm.CommandText = "UPDATE Stage SET score = " + score + " WHERE stage = " + stage;
                dbcm.ExecuteNonQuery();
                dbScore = score;
            }
        }
        else
        {
            dbcm.CommandText = "INSERT INTO Stage(stage, score, playCount, clearCount) VALUES(" + stage + ", " + score + ", 0, 0)";
            dbcm.ExecuteNonQuery();
            dbScore = score;
        }

        return dbScore;
    }

    /// <summary>
    /// 점수를 체크하여 DB의 클리어 횟수를 업데이트하고 클리어 횟수를 반환합니다.
    /// </summary>
    /// <param name="score">현재 판의 점수</param>
    /// <returns>클리어 횟수</returns>
    int CheckClearCount(int score)
    {
        int clearCount = 0;
        
        dbcm.CommandText = "SELECT score, playCount, clearCount FROM Stage WHERE Stage = " + stage;
        dbr = dbcm.ExecuteReader();

        if (score >= clearScore)
        {
            if (dbr.Read())
            {
                clearCount = dbr.GetInt16(2) + 1;
                dbcm.CommandText = "UPDATE Stage SET clearCount = " + clearCount;
                dbcm.ExecuteNonQuery();
            }
        }

        return clearCount;
    }

    /// <summary>
    /// 현재 캐릭터의 획득 골드와 수익 골드를 업데이트 합니다.
    /// </summary>
    /// <param name="gold">이번 판에서 획득한 골드</param>
    /// <returns>int 수익 골드</returns>
    int UpdateCharacter(int gold)
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

        dbcm.CommandText = "UPDATE Character SET money = " + money + ", revenueMoney = " + revenueMoney;
        dbcm.ExecuteNonQuery();

        return revenueMoney;
    }

    /// <summary>
    /// 수익 업적에 관한 정보를 업데이트 합니다.
    /// </summary>
    /// <param name="revenueMoney">수익 골드</param>
    void UpdateMoneyRecord(int revenueMoney)
    {
        dbcm.CommandText = "SELECT ID, condition, state FROM record WHERE ID BETWEEN 200 and 299";
        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if (dbr.GetInt16(2) == 0 && revenueMoney >= dbr.GetInt16(1))
            {
                dbcm.CommandText = "UPDATE Record SET state = 1 WHERE ID = " + dbr.GetInt16(0);
                dbcm.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// 해당 스테이지 최고 점수 업적 정보를 업데이트 합니다.
    /// </summary>
    /// <param name="score">이번 판에서 획득한 점수</param>
    void UpdateScoreRecord(int score)
    {
        string strStage;

        if (stage < 10)
            strStage = "0" + stage;
        else
            strStage = stage.ToString();

        // ex) 1 Stage = BETWEEN 1010 and 1019, 2 Stage = BETWEEN 1020 and 1029, 10 Stage = BETWEEN 1100 and 1109
        dbcm.CommandText = "SELECT ID, condition, state FROM Record WHERE ID BETWEEN 1" + strStage + "0 and 10" + strStage + "9";
        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if (dbr.GetInt16(2) == 0 && score >= dbr.GetInt16(1))
            {
                dbcm.CommandText = "UPDATE Record SET state = 1 WHERE ID = " + dbr.GetInt16(0);
                dbcm.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// 해당 스테이지의 클리어 업적 정보를 업데이트 합니다.
    /// </summary>
    /// <param name="clearCount">클리어한 횟수</param>
    void UpdateClearRecord(int clearCount)
    {
        string strStage;

        if (stage < 10)
            strStage = "0" + stage;
        else
            strStage = stage.ToString();

        // ex) 1 Stage = BETWEEN 2010 and 2019, 2 Stage = BETWEEN 2020 and 2029, 10 Stage = BETWEEN 2100 and 2109
        dbcm.CommandText = "SELECT ID, condition, state FROM record WHERE ID BETWEEN 2" + strStage + "0 and 2" + strStage + "9";
        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if (dbr.GetInt16(2) == 0 && clearCount >= dbr.GetInt16(1))
            {
                dbcm.CommandText = "UPDATE Record SET state = 1 WHERE ID = " + dbr.GetInt16(0);
                dbcm.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// 총 플레이 횟수 업적 업데이트
    /// </summary>
    void UpdatePlayCountRecord()
    {
        int playCount = 0;

        dbcm.CommandText = "SELECT sum(playCount) FROM Stage";
        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            playCount = dbr.GetInt16(0);
        }

        dbcm.CommandText = "SELECT ID, condition, state FROM record WHERE ID BETWEEN 100 and 199";
        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if (dbr.GetInt16(2) == 0 && playCount >= dbr.GetInt16(1))
            {
                dbcm.CommandText = "UPDATE Record SET state = 1 WHERE ID = " + dbr.GetInt16(0);
                dbcm.ExecuteNonQuery();
            }
        }
    }

    void OnDisable()
    {
        /* DB 연결 정보들을 초기화 합니다. */
        dbr.Close();
        dbr = null;
        dbcm.Dispose();
        dbcm = null;
        dbc.Close();
        dbc = null;
    }
}