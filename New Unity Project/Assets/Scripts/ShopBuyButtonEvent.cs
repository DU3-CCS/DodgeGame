using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class ShopBuyButtonEvent : MonoBehaviour {
    public int characterNum;
    public int cost;
    private Text Text_ResourceAmount;
    public Text Text_Cost;
    
    // sql 인스턴스 변수
    IDbConnection dbc;
    IDbCommand dbcm;        //SQL문 작동 개체
    IDataReader dbr;        //반환된 값 읽어주는 객체

    DatabaseManager dbm = DatabaseManager.dbm;

    int money = 0;

    // Use this for initialization
    void Start () {
        Text_ResourceAmount = GameObject.Find("Text_ResourceAmount").GetComponent<Text>();
        Text_Cost = transform.Find("Text_Cost").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		if(TurnOnTheStage.charactor_unlock[characterNum] == false)
        {
            Text_Cost.text = "Purchased";
            gameObject.GetComponent<Button>().interactable = false;
        }
	}

    public void Buy()
    {
        dbr = dbm.SelectData("SELECT * FROM Character");

        if (dbr.Read())
        {
            money = dbr.GetInt16(1);
        }
        dbm.Disconnect();

        if(money >= cost)
        {
            if (characterNum >= 0 && characterNum < 4)
            {
                /* 사운드 */
                SoundManager.instance.BuyShopItem();

                /* DB에 저장 */
                dbm.UpdateData("INSERT INTO Job(characterID, job) VALUES(1, " + characterNum + ")");
                TurnOnTheStage.charactor_unlock[characterNum] = false;

                /* 구입 업적 추가 */
                dbr = dbm.SelectData("SELECT ID, condition, state FROM record WHERE ID = " + characterNum);

                if(dbr.Read())
                {
                    int ID = dbr.GetInt16(0);
                    int state = dbr.GetInt16(2);
                    if (state == 0)
                    {
                        dbm.UpdateData("UPDATE Record SET state = 1 WHERE ID = " + ID);
                    }
                }

                /* 비용 감소 */
                money -= cost;
                dbm.UpdateData("UPDATE Character SET money = " + money);
                
                /* 돈 컴포넌트 값 변경*/
                Text_ResourceAmount.text = money.ToString();

                /* 텍스트 변경 스크립트 문 추가 Buy->Purchased */
                Text_Cost.text = "Purchased";
                // Buy 버튼 클릭 불가능하게 수정
            }
        }
    }
}
