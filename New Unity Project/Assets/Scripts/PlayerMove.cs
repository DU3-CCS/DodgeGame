using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text HPLabel;
    public Image skill_01_image, skill_02_image, skill_03_image, skill_04_image = null;
    public GameObject[] enemy;

    public int MaxHP;
    public int HP;

    public float S1_duration, S2_duration, S3_duration, S4_duration = 0.0f;         //지속시간 카운터
    public float S1_coolTime, S2_coolTime, S3_coolTime, S4_coolTime = 0.0f;         //쿨타임 카운터
    public float S1_leftTime, S2_leftTime, S3_leftTime, S4_leftTime;                //남은 시간 카운터
    public int S1_durationWaiting, S2_durationWaiting, S3_durationWaiting, S4_durationWaiting;  //지속시간
    public int S1_coolTimeWaiting, S2_coolTimeWaiting, S3_coolTimeWaiting, S4_coolTimeWaiting;  //쿨타임

    public float ShowDamage;            //데미지 카운터
    public float ShowDamageWaiting;     //데미지 지속시간

    public Animator animator;           //애니메이터

    public float MoveSpeed = 20;        //기본 이동속도
    Vector3 lookDirection;              //바라보는 방향

    private bool skill_01_Available = true;
    private bool skill_02_Available = true;
    private bool skill_03_Available = true;
    private bool skill_04_Available = true;

    private bool ShowDamage_Available = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Dash", false);
        animator.SetBool("Defend", false);
        
        S1_durationWaiting = 2;     //지속시간 
        S1_coolTimeWaiting = 5;     //쿨타임
        S1_leftTime = S1_coolTimeWaiting;

        S2_durationWaiting = 2;     //지속시간 
        S2_coolTimeWaiting = 5;     //쿨타임
        S2_leftTime = S2_coolTimeWaiting;

        S3_durationWaiting = 2;     //지속시간 
        S3_coolTimeWaiting = 5;     //쿨타임
        S3_leftTime = S3_coolTimeWaiting;

        S4_durationWaiting = 2;     //지속시간 
        S4_coolTimeWaiting = 5;     //쿨타임
        S4_leftTime = S4_coolTimeWaiting;

        ShowDamage = 0.0f;         //데미지 이미지 카운터
        ShowDamageWaiting = 0.5f;     //데미지 이미지 지속시간 

        //HP 설정
        if (TurnOnTheStage.characterNum == 0)
        {
            MaxHP = 3;
        }
        else if (TurnOnTheStage.characterNum == 1)
        {
            MaxHP = 4;
        }
        else if (TurnOnTheStage.characterNum == 2)
        {
            MaxHP = 1;
        }
        else if (TurnOnTheStage.characterNum == 3)
        {
            MaxHP = 2;
        }
        HP = MaxHP;

        HPLabel = GameObject.Find("HPLabel").GetComponent<Text>();          //HPLabel 연결

        //스킬 버튼 연결
        if (TurnOnTheStage.characterNum == 0)
        {
            skill_01_image = GameObject.Find("Skill_Dash").GetComponent<Image>();
            skill_01_image.fillAmount = 1;
        }
        else if (TurnOnTheStage.characterNum == 1)
        {
            Debug.Log("슬로우 스킬 이미지");
            skill_02_image = GameObject.Find("Skill_Slow").GetComponent<Image>();
            skill_02_image.fillAmount = 1;
        }
        else if (TurnOnTheStage.characterNum == 2)
        {
            Debug.Log("디펜드 스킬 이미지");
            skill_03_image = GameObject.Find("Skill_Defend").GetComponent<Image>();
            skill_03_image.fillAmount = 1;
        }
        else if (TurnOnTheStage.characterNum == 3)
        {
            Debug.Log("스턴 스킬 이미지");
            skill_04_image = GameObject.Find("Skill_Stun").GetComponent<Image>();
            skill_04_image.fillAmount = 1;
        }
    }
  
    private void Update()
    {
        

        //방향키 조작
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MoveControl();          
        }
        
        //직업별 스킬
        if (TurnOnTheStage.characterNum == 0)
        {
            Skill_Dash();
        }
        else if (TurnOnTheStage.characterNum == 1)
        {
            Skill_Slow();
        }
        else if (TurnOnTheStage.characterNum == 2)
        {
            Skill_Defend();
        }
        else if (TurnOnTheStage.characterNum == 3)
        {
            Skill_Stun();
        }


        HPControl();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimationSet();

        // ShowDamage
        if (ShowDamage_Available == true) //스킬 활성화 일 때
        {
            ShowDamage += Time.deltaTime;
            if (ShowDamage > ShowDamageWaiting) //지속시간이 끝나면
            {
                GameObject.Find("Image_Damage").transform.Find("GameObject").gameObject.SetActive(false);
                ShowDamage_Available = false;
                ShowDamage = 0;
            }

        }
    }

    void HPControl() //체력 관련
    {
        
        if (MaxHP == 4)
        {
            if (HP == 4) HPLabel.text = "♥♥♥♥";
            else if (HP == 3) HPLabel.text = "♥♥♥♡";
            else if (HP == 2) HPLabel.text = "♥♥♡♡";
            else if (HP == 1) HPLabel.text = "♥♡♡♡";
        }
        else if (MaxHP == 3)
        {
            if (HP == 3) HPLabel.text = "♥♥♥";
            else if (HP == 2) HPLabel.text = "♥♥♡";
            else if (HP == 1) HPLabel.text = "♥♡♡";
        }
        else if (MaxHP == 2)
        {
            if (HP == 2) HPLabel.text = "♥♥";
            else if (HP == 1) HPLabel.text = "♥♡";
        }
        else if (MaxHP == 1)
        {
            if (HP == 1) HPLabel.text = "♥";
        }

        //체력 0이하일시 팝업 씬으로 이동
        if (HP <= 0) { SceneManager.LoadScene("PopUp"); }
    }

    void MoveControl() //플레이어 움직임
    {
        float horizontal = Input.GetAxisRaw("Vertical");
        float vertical = Input.GetAxisRaw("Horizontal");
        lookDirection = horizontal * Vector3.forward + vertical * Vector3.right;

        this.transform.rotation = Quaternion.LookRotation(lookDirection);
        this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }

    void Skill_Dash()         // Dash Skill
    {
        if (skill_01_Available == true) //스킬 활성화 일 때
        {
            if (Input.GetButtonDown("Fire1"))  // 만약 Fire1(왼쪽 컨트롤)버튼이 눌리면 아래 내용을 실행.
            {
                Debug.Log("대시 스킬 활성화");
                MoveSpeed = 30;
                animator.SetBool("Dash", true);
                skill_01_Available = false;     //스킬 활성화
                S1_duration = 0;
                S1_coolTime = 0;
            }
        }
        else
        {
            S1_duration += Time.deltaTime;
            S1_coolTime += Time.deltaTime;
            S1_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S1_leftTime / S1_coolTimeWaiting);
            if (skill_01_image) skill_01_image.fillAmount = ratio;

            if (S1_duration > S1_durationWaiting)       //지속시간이 끝나면
            {
                MoveSpeed = 20;
                animator.SetBool("Dash", false);
                S1_duration = 0;
            }
            if (S1_coolTime > S1_coolTimeWaiting)       //쿨타임이 끝나면
            {
                Debug.Log("쿨타임 끝");
                skill_01_Available = true;
                S1_coolTime = 0;
                S1_leftTime = S1_coolTimeWaiting;
            }            
        }
    }

    void Skill_Slow()         // Slow Skill
    {
        if (skill_02_Available == true) //스킬 활성화 일 때
        {
            if (Input.GetButtonDown("Fire1"))  // 만약 Fire1(왼쪽 컨트롤)버튼이 눌리면 아래 내용을 실행.
            {
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().Slow();
                }
                Debug.Log("슬로우 스킬 활성화");
                //animator.SetBool("Dash", true);
                skill_02_Available = false;     //스킬 활성화
                S2_duration = 0;
                S2_coolTime = 0;
            }
        }
        else
        {
            S2_duration += Time.deltaTime;
            S2_coolTime += Time.deltaTime;
            S2_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S2_leftTime / S2_coolTimeWaiting);
            if (skill_02_image) skill_02_image.fillAmount = ratio;

            if (S2_duration > S2_durationWaiting)       //지속시간이 끝나면
            {
                //animator.SetBool("Dash", false);
                S2_duration = 0;
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().ReleaseSkill();
                }
            }
            if (S2_coolTime > S2_coolTimeWaiting)       //쿨타임이 끝나면
            {
                Debug.Log("쿨타임 끝");
                skill_02_Available = true;
                S2_coolTime = 0;
                S2_leftTime = S2_coolTimeWaiting;
            }
        }
    }

    void Skill_Defend()       // Defend Skill
    {
        if (skill_03_Available == true) //스킬 활성화 일 때
        {
            if (Input.GetButtonDown("Fire1"))  // 만약 Fire1(왼쪽 컨트롤)버튼이 눌리면 아래 내용을 실행.
            {
                Debug.Log("방어 스킬 활성화");
                //animator.SetBool("Dash", true);
                skill_03_Available = false;     //스킬 활성화
                S3_duration = 0;
                S3_coolTime = 0;
            }
        }
        else
        {
            S3_duration += Time.deltaTime;
            S3_coolTime += Time.deltaTime;
            S3_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S3_leftTime / S3_coolTimeWaiting);
            if (skill_03_image) skill_03_image.fillAmount = ratio;

            if (S3_duration > S3_durationWaiting)       //지속시간이 끝나면
            {
                MoveSpeed = 20;
                //animator.SetBool("Dash", false);
                S3_duration = 0;
            }
            if (S3_coolTime > S3_coolTimeWaiting)       //쿨타임이 끝나면
            {
                Debug.Log("쿨타임 끝");
                skill_03_Available = true;
                S3_coolTime = 0;
                S3_leftTime = S3_coolTimeWaiting;
            }


        }
    }

    void Skill_Stun()         // Dash Skill
    {
        if (skill_04_Available == true) //스킬 활성화 일 때
        {
            if (Input.GetButtonDown("Fire1"))  // 만약 Fire1(왼쪽 컨트롤)버튼이 눌리면 아래 내용을 실행.
            {
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().Stun();
                }
                Debug.Log("스턴 스킬 활성화");
                //animator.SetBool("Dash", true);

                skill_04_Available = false;     //스킬 활성화
                S4_duration = 0;
                S4_coolTime = 0;
            }
        }
        else
        {
            S4_duration += Time.deltaTime;
            S4_coolTime += Time.deltaTime;
            S4_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S4_leftTime / S4_coolTimeWaiting);
            if (skill_04_image) skill_04_image.fillAmount = ratio;

            if (S4_duration > S4_durationWaiting)       //지속시간이 끝나면
            {
                //animator.SetBool("Dash", false);
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {          
                    oneEnemy.GetComponent<EnemyMove>().ReleaseSkill();
                }
                S4_duration = 0;
            }
            if (S4_coolTime > S4_coolTimeWaiting)       //쿨타임이 끝나면
            {
                Debug.Log("쿨타임 끝");
                skill_04_Available = true;
                S4_coolTime = 0;
                S4_leftTime = S4_coolTimeWaiting;
            }
        }
    }

    void AnimationSet()
    {
        //"Run" 애니메이션
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            animator.SetBool("Run", true);
        }
        else animator.SetBool("Run", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("충돌!");
            HP -= 1;
            
            //데미지 표시
            GameObject.Find("Image_Damage").transform.Find("GameObject").gameObject.SetActive(true);
            ShowDamage_Available = true;

            Debug.Log(HP);
        }
        if (other.gameObject.tag == "item03")
        {
            if(HP < MaxHP) HP += 1;
            Destroy(other.gameObject);
            Debug.Log("바나나!");

        }
        if (other.gameObject.tag == "item28")
        {
            HP = MaxHP;         
            Destroy(other.gameObject);
            Debug.Log("물약!");
        }
        if (other.gameObject.tag == "item25")
        {
            HP -= 1;
            Destroy(other.gameObject);
            Debug.Log("썩은고기!");
        }
        if (other.gameObject.tag == "Cube") Debug.Log("CUBE!");
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }
}
 