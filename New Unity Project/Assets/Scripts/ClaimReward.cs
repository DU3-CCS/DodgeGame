using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;

public class ClaimReward : MonoBehaviour {
    public int rewardID;
    public int rewardGold;
    public GameObject Button;
    public GameObject Text_Completed;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Claim()
    {
        // sql 인스턴스 변수
        IDbConnection dbc;
        IDbCommand dbcm;        //SQL문 작동 개체
        IDataReader dbr;        //반환된 값 읽어주는 객체

        string constr = "URI=file:character.db";

        dbc = new SqliteConnection(constr);
        dbc.Open();
        dbcm = dbc.CreateCommand();

        dbcm.CommandText = "UPDATE Record SET state = 2 WHERE ID = " + rewardID;
        Debug.Log("UPDATE Record SET state = 2 WHERE ID = " + rewardID);
        dbcm.ExecuteNonQuery();

        dbcm.CommandText = "SELECT money FROM Character";
        dbr = dbcm.ExecuteReader();

        if(dbr.Read())
        {
            int gold = dbr.GetInt16(0) + rewardGold;

            dbcm.CommandText = "UPDATE Character SET money = " + gold;
            Debug.Log("UPDATE Character SET money = " + gold);
            dbcm.ExecuteNonQuery();
        }

        Button.SetActive(false);
        Text_Completed.SetActive(true);

        /* db 닫기 */
        dbr.Close();
        dbr = null;
        dbcm.Dispose();
        dbcm = null;
        dbc.Close();
        dbc = null;
    }
}
