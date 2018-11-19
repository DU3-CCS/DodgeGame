using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//씬 전환을 위해 필요한 SceneManagement

public class connectScene : MonoBehaviour {
    public static int stage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void loadSceneLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void LoadSceneAcheievemnet()
    {
        SceneManager.LoadScene("Achievements");
    }
    public void loadSceneSelectStage()
    {
        SceneManager.LoadScene("SelectStage");
    }
    public void loadSceneLv1()
    {
        SceneManager.LoadScene("Lv1");
        stage = 1;
    }
    public void loadSceneLv2()
    {
        SceneManager.LoadScene("Lv2");
        stage = 2;
    }
    public void loadSceneLv3()
    {
        SceneManager.LoadScene("Lv3");
        stage = 3;
    }
    public void loadSceneLv4()
    {
        SceneManager.LoadScene("Lv4");
        stage = 4;
    }
    public void loadSceneRestart()
    {
        if (stage == 1) loadSceneLv1();
        else if (stage == 2) loadSceneLv2();
        else if (stage == 3) loadSceneLv3();
        else if (stage == 4) loadSceneLv4();
    }
    public void loadSceneSettingWindow()
    {
        SceneManager.LoadScene("SettingWindow");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
