using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text timeLabel;
    public float timeCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeCount += Time.deltaTime;
        timeLabel.text = string.Format ("{0:N2}", timeCount);
	}
}
