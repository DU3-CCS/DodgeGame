using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class RecordManager : MonoBehaviour
{
    public GameObject pfClone;
    public GameObject goEmpty;

    private Text Text_AchievementDescription;       // 조건        1번
    private Text Text_AchievementName;              // 타이틀      2번
    private Text Text_RewardAmount;                 // 보상        4번
    private Image Fill_Green;                       // 진행률      7-0번
    private Text Text_FillAmount;                   // 진행수치    7-1번
    private Button Button_Claim;                    // 수령        8번
    private Text Text_Completed;

    // sql 인스턴스 변수
    IDataReader dbr;        //반환된 값 읽어주는 객체

    DatabaseManager dbm = DatabaseManager.dbm;

    // Use this for initialization
    void Start()
    {
        dbr = dbm.SelectData("SELECT * FROM Record");
        
        while (dbr.Read())
        {
            int id = dbr.GetInt16(0);
            // string name = dbr.GetString(5);
            string description = dbr.GetString(1);
            int condition = dbr.GetInt16(2);
            int reward = dbr.GetInt16(3);
            int state = dbr.GetInt16(4);

            resetPrefab();

            Text_AchievementName = pfClone.transform.GetChild(1).GetComponent<Text>();
            Text_AchievementName.text = description;
            
            Text_RewardAmount = pfClone.transform.GetChild(4).GetComponent<Text>();
            Text_RewardAmount.text = "x " + reward;

            if (state == 0)            // 퀘스트 클리어 X 상태
            {
                float progress = 0;
                int info = 0;
                IDataReader idr;

                if(id >= 0 && id < 100)
                {
                    idr = dbm.SelectData("SELECT job FROM Job");

                    if(idr.Read())
                    {
                        info = idr.GetInt16(0);
                        progress = info / (float)condition;
                    }
                }
                else if(id >= 100 && id < 200)
                {
                    idr = dbm.SelectData("SELECT sum(playCount) FROM Stage");

                    if (idr.Read())
                    {
                        info = idr.GetInt16(0);
                        progress = info / (float)condition;
                    }
                }
                else if(id >= 200 && id < 300)
                {
                    idr = dbm.SelectData("SELECT revenueMoney FROM Character");

                    if (idr.Read())
                    {
                        info = idr.GetInt16(0);
                        progress = idr.GetInt16(0) / (float)condition;
                    }
                }
                else if(id >= 2000 && id < 3000)
                {
                    int stage = (id % 1000) / 10;
                    
                    idr = dbm.SelectData("SELECT clearCount FROM Stage WHERE stage = " + stage);

                    if (idr.Read())
                    {
                        info = idr.GetInt16(0);
                        progress = info / (float)condition;
                    }
                }

                Fill_Green = pfClone.transform.GetChild(7).GetChild(0).GetComponent<Image>();
                Fill_Green.fillAmount = progress;

                Text_FillAmount = pfClone.transform.GetChild(7).GetChild(1).GetComponent<Text>();
                Text_FillAmount.text = info + " / " + condition;

                Debug.Log("ID = " + id + ", progress = " + progress);
            }
            else if(state == 1)       // 퀘스트 클리어, 보상 미수령
            {
                pfClone.transform.GetChild(7).gameObject.SetActive(false);
                pfClone.transform.GetChild(8).gameObject.SetActive(true);

                ClaimReward cr = pfClone.transform.GetChild(8).GetComponent<ClaimReward>();
                cr.rewardID = id;
                cr.rewardGold = reward;
                cr.Button = pfClone.transform.GetChild(8).gameObject;
                cr.Text_Completed = pfClone.transform.GetChild(9).gameObject;
            }
            else if(state == 2)       // 퀘스트 클리어, 보상 수령
            {
                pfClone.transform.GetChild(7).gameObject.SetActive(false);
                pfClone.transform.GetChild(9).gameObject.SetActive(true);
            }

            GameObject goTemp = Instantiate(pfClone, Vector3.zero, Quaternion.identity) as GameObject;
            goTemp.transform.SetParent(goEmpty.transform);
            goTemp.transform.localScale = new Vector3(1, 1, 1);
        }

        dbm.Disconnect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void resetPrefab()
    {
        pfClone.transform.GetChild(1).GetComponent<Text>().text = "";
        pfClone.transform.GetChild(4).GetComponent<Text>().text = "x 0";
        pfClone.transform.GetChild(7).gameObject.SetActive(true);
        pfClone.transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = 0;
        pfClone.transform.GetChild(7).GetChild(1).GetComponent<Text>().text = "";
        pfClone.transform.GetChild(8).gameObject.SetActive(false);
        pfClone.transform.GetChild(9).gameObject.SetActive(false);
    }
}

