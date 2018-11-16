using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /* 재생할 소리를 변수로 담습니다. */
    public AudioClip buttonClick;   //버튼 클릭음
    public AudioClip buyShopItem;   //상점 구매 시
    public AudioClip damage;        //피격 시
    public AudioClip skillDashUse;  //대시 스킬 사용 시
    public AudioClip skillSlowUse;  //슬로우 스킬 사용시
    public AudioClip skillDefendUse;        //디펜드 스킬 사용 시
    public AudioClip skillDefendActive;     //디펜드 스킬 발동 시
    public AudioClip skillStunUse;          //스턴 스킬 사용 시
    public AudioClip eat;                   //아이템 01 획득 시
    public AudioClip drinkPotion;           //아이템 02 획득 시
    public AudioClip rottenMeat;            //아이템 03 획득 시

    AudioSource myAudio;                    //AudioSorce 컴포넌트를 변수로 담습니다.

    public static SoundManager instance;  //자기자신을 변수로 담습니다.

    void Awake() //Start보다도 먼저, 객체가 생성될때 호출됩니다
    {
        if (SoundManager.instance == null) //incetance가 비어있는지 검사합니다.
        {
            SoundManager.instance = this; //자기자신을 담습니다.
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
    public void Damage()
    {
        myAudio.PlayOneShot(damage);
    }
    public void SkillDashUse()
    {
        myAudio.PlayOneShot(skillDashUse);
    }
    public void SkillSlowUse()
    {
        myAudio.PlayOneShot(skillSlowUse);
    }
    public void SkillDefendUse()
    {
        myAudio.PlayOneShot(skillDefendUse);
    }
    public void SkillDefendActive()
    {
        myAudio.PlayOneShot(skillDefendActive);
    }
    public void SkillStunUse()
    {
        myAudio.PlayOneShot(skillStunUse);
    }
    public void Eat()
    {
        myAudio.PlayOneShot(eat);
    }
    public void DrinkPotion()
    {
        myAudio.PlayOneShot(drinkPotion);
    }
    public void RottenMeat()
    {
        myAudio.PlayOneShot(rottenMeat);
    }
}