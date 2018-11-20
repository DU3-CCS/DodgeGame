using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour {
    public int stageNum;
    public Text Text_Cost;
    private connectScene scene;

    // Use this for initialization
    void Start () {
        if (!scene) scene = new connectScene();
	}
	
	// Update is called once per frame
	void Update () {
        if (StageManager.myLevel[stageNum - 1] == false)
        {
            Text_Cost.text = "Lock";
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void Stage()
    {
        if (StageManager.myLevel[stageNum - 1] == true)
        {
            GameScoreSystem.stage = stageNum;
            connectScene.stage = stageNum;

            if (stageNum == 1)
                scene.loadSceneLv1();
            else if (stageNum == 2)
                scene.loadSceneLv2();
            else if (stageNum == 3)
                scene.loadSceneLv3();
            else if (stageNum == 4)
                scene.loadSceneLv4();
        }
    }
}
