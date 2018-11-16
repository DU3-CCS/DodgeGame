using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_Shop : MonoBehaviour {
    /* 재생할 소리를 변수로 담습니다. */
    public AudioClip buttonClick;   //버튼 클릭음
    public AudioClip buyShopItem;   //상점 구매 시

    AudioSource myAudio;                    //AudioSorce 컴포넌트를 변수로 담습니다.

    public static SoundManager_Shop instance;  //자기자신을 변수로 담습니다.

    void Awake() //Start보다도 먼저, 객체가 생성될때 호출됩니다
    {
        if (SoundManager_Shop.instance == null) //incetance가 비어있는지 검사합니다.
        {
            SoundManager_Shop.instance = this; //자기자신을 담습니다.
        }
    }
    void Start()
    {
        myAudio = this.gameObject.GetComponent<AudioSource>(); //AudioSource 오브젝트를 변수로 담습니다.
    }

    void Update()
    {

    }

    public void ButtonClick()
    {
        myAudio.PlayOneShot(buttonClick);
    }
    public void BuyShopItem()
    {
        myAudio.PlayOneShot(buyShopItem);
    }
}