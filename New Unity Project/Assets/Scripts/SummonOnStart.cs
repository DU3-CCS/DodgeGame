using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonOnStart : MonoBehaviour {
    int charactors;
    public GameObject []sHero = new GameObject[4];

    public Image skill_01_image, skill_02_image, skill_03_image, skill_04_image = null;

    GameObject respawn = null;
    Quaternion qRotation = Quaternion.Euler(0f, 180f, 0f);

    // Use this for initialization
    void Start () {
        charactors = TurnOnTheStage.characterNum;
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        // 위치를 위한 오브젝트
        for(int i=0; i<4; i++)
            if (charactors == i) Instantiate(sHero[i],respawn.transform.position , qRotation);

        if (TurnOnTheStage.characterNum == 0)
        {
            GameObject.Find("Skill_Button").transform.Find("Skill_Button_Dash").gameObject.SetActive(true);
        }
        else if (TurnOnTheStage.characterNum == 1)
        {
            GameObject.Find("Skill_Button").transform.Find("Skill_Button_Slow").gameObject.SetActive(true);
        }
        else if (TurnOnTheStage.characterNum == 2)
        {
            GameObject.Find("Skill_Button").transform.Find("Skill_Button_Defend").gameObject.SetActive(true);
        }
        else if (TurnOnTheStage.characterNum == 3)
        {
            GameObject.Find("Skill_Button").transform.Find("Skill_Button_Stun").gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
