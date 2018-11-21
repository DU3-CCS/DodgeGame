using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

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
        IDataReader dbr;        //반환된 값 읽어주는 객체
        DatabaseManager dbm = DatabaseManager.dbm;

        dbm.UpdateData("UPDATE Record SET state = 2 WHERE ID = " + rewardID);
        
        dbr = dbm.SelectData("SELECT money FROM Character");

        if(dbr.Read())
        {
            int gold = dbr.GetInt16(0) + rewardGold;

            dbm.Disconnect();
            dbm.UpdateData("UPDATE Character SET money = " + gold);
        }

        Button.SetActive(false);
        Text_Completed.SetActive(true);
    }
}
