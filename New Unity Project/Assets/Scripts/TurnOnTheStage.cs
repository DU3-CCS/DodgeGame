using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// UI : UI 기능 사용을 위한 것
using UnityEngine.SceneManagement;
// SceneManagement : 씬 전환을 위한 것
using System.Data;
using Mono.Data.SqliteClient;
// SQL : SQLite 연동하는 것

public class TurnOnTheStage : MonoBehaviour {
    bool bTurnLeft = false;
    bool bTurnRight = false;
    private Quaternion turn = Quaternion.identity;
    string strMoney = "";
    public Text Text_ResourceAmount;
    // 정의
    public static int characterNum = 0; //캐릭터 바꾸는 변수

    // 직업 언락 변수
    public static bool[] charactor_unlock = { true, true, true, true };

    // sql 인스턴스 변수
    IDbConnection dbc;
    IDbCommand dbcm;        //SQL문 작동 개체
    IDataReader dbr;        //반환된 값 읽어주는 객체

    int value = 0;
	// Use this for initialization
	void Start () {
        turn.eulerAngles = new Vector3(0, value, 0);
        // 각을 초기화합니다.

        string constr = "URI=file:character.db";

        dbc = new SqliteConnection(constr);
        dbc.Open();
        dbcm = dbc.CreateCommand();

        dbcm.CommandText = "SELECT * FROM Job";

        dbr = dbcm.ExecuteReader();

        while (dbr.Read())
        {
            if (dbr.GetInt16(2) >= 0 && dbr.GetInt16(2) < 4)
            {
                charactor_unlock[dbr.GetInt16(2)] = false;
                
            }
            else
                Debug.Log("job 테이블에 잘못된 값이 들어갔습니다. 잘못된 값 = " + dbr.GetInt16(2));
        }

        Text_ResourceAmount = GameObject.Find("Text_ResourceAmount").GetComponent<Text>();

        dbcm.CommandText = "SELECT * FROM Character";

        dbr = dbcm.ExecuteReader();

        if (dbr.Read())
        {
            strMoney = dbr.GetString(1);
        }

        Text_ResourceAmount.text = strMoney;
    }
	
	// Update is called once per frame
	void Update () {
		if(bTurnLeft)
        {
            Debug.Log("Left");
            characterNum--;
            if (characterNum == -1)
                characterNum = 3;
            value -= 90;
            // 각도를 90도 뺍니다.
            bTurnLeft = false;
            // 부울 변수를 취소합니다.
            Debug.Log(characterNum);
        }
        if(bTurnRight)
        {
            Debug.Log("Right");
            characterNum++;
            if (characterNum == 4)
                characterNum = 0;
            value += 90;
            // 각도를 90도 더합니다.
            bTurnRight = false;
            // 부울 변수를 취소합니다.
            Debug.Log(characterNum);
        }
        turn.eulerAngles = new Vector3(0, value, 0);
        // 각도를 잽니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 5.0f);
        // 돌립니다.

        //선택한 캐릭터 번호가 언락 상태인지 체크
        if(charactor_unlock[characterNum])
        {
            GameObject.Find("Lock_Message").transform.Find("GameObject").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Lock_Message").transform.Find("GameObject").gameObject.SetActive(false);
            //[characterNum];
            // 상점에서 해당 번호의 Panel_Card를 Purchased로 텍스트 수정과 버튼 이벤트 제거 코드 삽입 요망
        }
    }

    public void turnLeft()
    {
        bTurnLeft = true;
        bTurnRight = false;
        // 다른 버튼을 누를때의 컨트롤
    }

    public void turnRight()
    {
        bTurnRight = true;
        bTurnLeft = false;
        // 다른 버튼을 누를때의 컨트롤
    }

    public void turnStage()
    {
        /* db 닫기 */
        dbr.Close();
        dbr = null;
        dbcm.Dispose();
        dbcm = null;
        dbc.Close();
        dbc = null;

        // 스테이지 전환을 위한 함수
        SceneManager.LoadScene("OnTheStage");
    }
}
