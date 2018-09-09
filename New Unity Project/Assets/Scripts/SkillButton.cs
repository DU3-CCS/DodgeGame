using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillButton : MonoBehaviour
{
    public Image skillFilter;
    public float coolTime;

    void start()
    {
        skillFilter.fillAmount = 0;
    }
    
    public void UseSkill()
    {
        Debug.Log("Use Skill");
    }
}