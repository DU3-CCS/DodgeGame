using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.SqliteClient;

public class ShopBuyButtonEvent : MonoBehaviour {
    public int characterNum;
    public int cost;
    public Text Text_ResourceAmount;

    // sql 인스턴스 변수
    IDbConnection dbc;
    IDbCommand dbcm;        //SQL문 작동 개체
    IDataReader dbr;        //반환된 값 읽어주는 객체

    int money = 0;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Buy()
    {
        string constr = "URI=file:character.db";

        dbc = new SqliteConnection(constr);
        dbc.Open();
        dbcm = dbc.CreateCommand();

        dbcm.CommandText = "SELECT * FROM Character";

        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            money = dbr.GetInt16(1);
        }

        if(money >= cost)
        {
            if (characterNum >= 0 && characterNum < 4)
            {
                // DB에 저장
                dbcm.CommandText = "INSERT INTO Job(characterID, job) VALUES(1, " + characterNum + ")";
                Debug.Log("INSERT INTO Job(characterID, job) VALUES(1, " + characterNum + ")");
                //dbcm.ExecuteNonQuery();
                TurnOnTheStage.charactor_unlock[characterNum] = true;

                // 비용 감소
                money -= cost;
                dbcm.CommandText = "UPDATE Character SET money = " + money;
                Debug.Log("UPDATE Character SET money = " + money);
                //dbcm.ExecuteNonQuery();
                
                // 돈 컴포넌트 값 변경
                Text_ResourceAmount = GameObject.Find("Text_ResourceAmount").GetComponent<Text>();
                Text_ResourceAmount.text = money.ToString(); 

                //텍스트 변경 스크립트 문 추가 Buy->Purchased

            }
        }

        dbr.Close();
        dbr = null;
        dbcm.Dispose();
        dbcm = null;
        dbc.Close();
        dbc = null;
    }
}
