using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip lobby;

    private bool pause_lobby, pause_shop;

    AudioSource myAudio;                    //AudioSorce 컴포넌트를 변수로 담습니다.

    public static BGMManager instance;  //자기자신을 변수로 담습니다.

    void Awake() //Start보다도 먼저, 객체가 생성될때 호출됩니다
    {
        if (BGMManager.instance == null) //incetance가 비어있는지 검사합니다.
        {
            BGMManager.instance = this; //자기자신을 담습니다.
        }
    }
    void Start()
    {
        pause_lobby = false;
        pause_shop = false;

        myAudio = GetComponent<AudioSource>(); //AudioSource 오브젝트를 변수로 담습니다.
        myAudio.clip = lobby;
        myAudio.loop = true;
    }

    void Update()
    {

    }

    public void BGM_Pause()
    {
        pause_lobby = true;
    }
    public void BGM_Replay()
    {
        pause_lobby = false;
    }
}

