using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip damage;        //재생할 소리를 변수로 담습니다.
    public AudioClip skillDashUse;
    public AudioClip skillSlowUse;
    public AudioClip skillDefendUse;
    public AudioClip skillDefendActive;
    public AudioClip skillStunUse;
    public AudioClip eat;
    public AudioClip drinkPotion;
    public AudioClip rottenMeat;

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