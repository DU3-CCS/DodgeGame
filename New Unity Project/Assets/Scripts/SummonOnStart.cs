using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOnStart : MonoBehaviour {
    int charactors;
    public GameObject []sHero = new GameObject[4];
    GameObject respawn = null;

	// Use this for initialization
	void Start () {
        charactors = TurnOnTheStage.charactorNum;
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        // 위치를 위한 오브젝트
        for(int i=0; i<4; i++)
            if (charactors == i) Instantiate(sHero[i],respawn.transform.position , Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
