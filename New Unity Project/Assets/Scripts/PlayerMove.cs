using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text HPLabel;        //체력 표시 UI
    public Image skill_01_image, skill_02_image, skill_03_image, skill_04_image = null; //스킬 이미지
    public GameObject[] enemy;      //적 배열
    public GameObject skill_01_button, skill_02_button, skill_03_button, skill_04_button;
    public GameObject player;

    public int MaxHP, HP;           //최대 체력, 현재 체력

    public float S1_duration, S2_duration, S3_duration, S4_duration = 0.0f;         //지속시간 카운터
    public float S1_coolTime, S2_coolTime, S3_coolTime, S4_coolTime = 0.0f;         //쿨타임 카운터
    public float S1_leftTime, S2_leftTime, S3_leftTime, S4_leftTime;                //남은 시간 카운터
    public int S1_durationWaiting, S2_durationWaiting, S3_durationWaiting, S4_durationWaiting;  //지속시간
    public int S1_coolTimeWaiting, S2_coolTimeWaiting, S3_coolTimeWaiting, S4_coolTimeWaiting;  //쿨타임

    public float ShowDamage;            //데미지 이미지 카운터
    public float ShowDamageWaiting;     //데미지 이미지 지속시간
    public float ShowGuard;             //방어 이미지
    public float ShowGuardWaiting;      //방어 이미지 지속시간

    public static float skillUseCounter = 0;      //스킬 버튼 변수 비활성화용 counter
    public static float timeCount = 0;

    private bool skill_01_Available = true; //스킬 사용 가능 표시
    private bool skill_02_Available = true;
    private bool skill_03_Available = true;
    private bool skill_04_Available = true;

    private bool skill_01_On, skill_02_On, skill_03_On, skill_04_On = false;    //스킬 사용 중

    public static bool skillUse = false;    //스킬 버튼 bool 

    private bool ShowDamage_Available = true;
    private bool ShowGuard_Available = true;

    public static int item01Cnt, item02Cnt, item03Cnt = 0;

    public Animator animator;           //애니메이터

    public float MoveSpeed = 20;        //기본 이동속도
    Vector3 lookDirection;              //바라보는 방향

    public FixedJoystick fixedJoystick; // 조이스틱 스크립트

    private void Start()
    {
        if (!fixedJoystick) fixedJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
        animator.SetBool("Dash", false);
        animator.SetBool("Defend", false);

        S1_durationWaiting = 2;     //지속시간 
        S1_coolTimeWaiting = 5;     //쿨타임
        S1_leftTime = S1_coolTimeWaiting;

        S2_durationWaiting = 3;     //지속시간 
        S2_coolTimeWaiting = 15;     //쿨타임
        S2_leftTime = S2_coolTimeWaiting;

        S3_durationWaiting = 4;     //지속시간 
        S3_coolTimeWaiting = 15;     //쿨타임
        S3_leftTime = S3_coolTimeWaiting;

        S4_durationWaiting = 2;     //지속시간 
        S4_coolTimeWaiting = 5;     //쿨타임
        S4_leftTime = S4_coolTimeWaiting;

        ShowDamage = 0.0f;         //데미지 이미지 카운터
        ShowDamageWaiting = 0.5f;     //데미지 이미지 지속시간 

        ShowGuard = 0.0f;         //방어 이미지 카운터
        ShowGuardWaiting = 0.5f;     //방어 이미지 지속시간 


        //스킬 버튼 연결, HP설정
        if (TurnOnTheStage.characterNum == 0)
        {
            skill_01_button = GameObject.Find("Skill_Button_Dash");
            skill_01_image = GameObject.Find("Skill_Dash").GetComponent<Image>();
            skill_01_image.fillAmount = 1;
            MaxHP = 3;
        }
        else if (TurnOnTheStage.characterNum == 1)
        {
            skill_02_button = GameObject.Find("Skill_Button_Slow");
            skill_02_image = GameObject.Find("Skill_Slow").GetComponent<Image>();
            skill_02_image.fillAmount = 1;
            MaxHP = 3;
        }
        else if (TurnOnTheStage.characterNum == 2)
        {
            skill_03_button = GameObject.Find("Skill_Button_Defend");
            skill_03_image = GameObject.Find("Skill_Defend").GetComponent<Image>();
            skill_03_image.fillAmount = 1;
            MaxHP = 3;
        }
        else if (TurnOnTheStage.characterNum == 3)
        {
            skill_04_button = GameObject.Find("Skill_Button_Stun");
            skill_04_image = GameObject.Find("Skill_Stun").GetComponent<Image>();
            skill_04_image.fillAmount = 1;
            MaxHP = 2;
        }
        HP = MaxHP;
        HPLabel = GameObject.Find("HPLabel").GetComponent<Text>();          //HPLabel 연결
    }

    private void Update()
    {
        SkillButton_KeyBoard();
        SkillUse();

        HPControl();
        if (skillUse == true)
        {
            SkillButton_Click();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimationSet();
        ShowCondition();
        //방향키 조작
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MoveControl();
        }
        if (fixedJoystick.GetHorizontalValue() != 0 || fixedJoystick.GetVerticalValue() != 0)
            MoveControl_Joystick();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //데미지 표시

            if (skill_03_On == true)
            {
                Debug.Log(S3_duration);
                GameObject.Find("Image_Damage").transform.Find("Guard").gameObject.SetActive(true);
                ShowGuard_Available = true;
                SoundManager.instance.SkillDefendActive();  //디펜스 활성화 소리
                gameObject.transform.Find("barrier").gameObject.SetActive(true);  //배리어 이펙트
                Debug.Log("방어함");
            }
            else
            {
                GameObject.Find("Image_Damage").transform.Find("Damaged").gameObject.SetActive(true);
                ShowDamage_Available = true;
                SoundManager.instance.Damage();
                Debug.Log("충돌!");
                HP -= 1;
            }
            Debug.Log(HP);
        }
        if (other.gameObject.tag == "item03")
        {
            if (HP < MaxHP) HP += 1;
            Destroy(other.gameObject);
            SoundManager.instance.Eat();
            Debug.Log("바나나!");
            item01Cnt += 1;
        }
        if (other.gameObject.tag == "item28")
        {
            HP = MaxHP;
            Destroy(other.gameObject);
            SoundManager.instance.DrinkPotion();
            Debug.Log("물약!");
            item02Cnt += 1;
        }
        if (other.gameObject.tag == "item25")
        {
            HP -= 1;
            Destroy(other.gameObject);
            SoundManager.instance.RottenMeat();
            Debug.Log("썩은고기!");
            item03Cnt += 1;
        }
        if (other.gameObject.tag == "Cube") Debug.Log("CUBE!");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (TurnOnTheStage.characterNum == 2)
            {
                gameObject.transform.Find("barrier").gameObject.SetActive(false);  //배리어 이펙트
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {

    }
    private void OnCollisionExit(Collision collision)
    {

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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Debug.Log(horizontal);
        lookDirection = horizontal * Vector3.right + vertical * Vector3.forward;

        this.transform.rotation = Quaternion.LookRotation(lookDirection);
        this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }

    void MoveControl_Joystick() //플레이어 움직임
    {
        float horizontal = fixedJoystick.GetHorizontalValue();
        float vertical = fixedJoystick.GetVerticalValue();
        Debug.Log("Horizontal : " + horizontal);
        Debug.Log("vertical : " + vertical);
        lookDirection = horizontal * Vector3.forward + vertical * Vector3.right;

        this.transform.rotation = Quaternion.LookRotation(lookDirection);
        this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }

    void SkillUse()
    {
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
    }
    void SkillButton_KeyBoard()     //스킬 버튼(Key)을 키보드로 누를 시
    {
        if (Input.GetButtonDown("Fire1")) skillUse = true;
        else if (Input.GetButtonUp("Fire1")) skillUse = false;
    }
    public static void SkillButton_Click()
    {
        skillUse = true;
        timeCount += Time.deltaTime;
        if (timeCount > 1)
        {
            timeCount = 0;
            skillUse = false;
        }
    }   //스킬 버튼(GUI)을 누를 시
    void AnimationSet()
    {
        //"Run" 애니메이션
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            animator.SetBool("Run", true);
        }
        else if (fixedJoystick.GetHorizontalValue() != 0 || fixedJoystick.GetVerticalValue() != 0)
        {
            animator.SetBool("Run", true);
        }
        else animator.SetBool("Run", false);
    }               //애니메이션 세팅
    void ShowCondition()                //상태 표시창
    {
        // ShowDamage
        if (ShowDamage_Available == true) //스킬 활성화 일 때
        {
            ShowDamage += Time.deltaTime;

            if (ShowDamage > ShowDamageWaiting) //지속시간이 끝나면
            {
                GameObject.Find("Image_Damage").transform.Find("Damaged").gameObject.SetActive(false);
                ShowDamage_Available = false;
                ShowDamage = 0;
            }
        }
        if (TurnOnTheStage.characterNum == 2 && skill_03_Available == false) //방어스킬 활성화 일 때
        {
            ShowGuard += Time.deltaTime;
            if (ShowGuard > ShowGuardWaiting) //방어함 이미지를 띄우는 지속시간이 끝나면
            {
                GameObject.Find("Image_Damage").transform.Find("Guard").gameObject.SetActive(false);
                ShowGuard_Available = false;
                ShowGuard = 0;
            }
        }
    }

    void Skill_Dash()         // Dash Skill
    {
        if (skill_01_Available == true) //스킬 활성화 일 때
        {
            if (skillUse == true)
            {
                SoundManager.instance.SkillDashUse();  //스킬 사운드
                player.transform.Find("Trail").gameObject.SetActive(true);  //대시 이펙트 추가
                Debug.Log("대시 스킬 활성화");
                MoveSpeed = 30;
                animator.SetBool("Dash", true);
                skill_01_Available = false;     //스킬 활성화
                S1_duration = 0;
                S1_coolTime = 0;
                skill_01_On = true;
            }
        }
        else
        {
            if (skill_01_On == true)
            {
                S1_duration += Time.deltaTime;
            }
            S1_coolTime += Time.deltaTime;
            S1_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S1_leftTime / S1_coolTimeWaiting);
            if (skill_01_image) skill_01_image.fillAmount = ratio;

            if (S1_duration > S1_durationWaiting)       //지속시간이 끝나면
            {
                player.transform.Find("Trail").gameObject.SetActive(false); //대시 이펙트 끄기
                MoveSpeed = 20;
                animator.SetBool("Dash", false);
                S1_duration = 0;
                skill_01_On = false;
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
            if (skillUse == true)
            {
                SoundManager.instance.SkillSlowUse();  //스킬 사운드
                player.transform.Find("ShockWave").gameObject.SetActive(true);  //슬로우 이펙트
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    if(oneEnemy.GetComponent<EnemyMove>())
                        oneEnemy.GetComponent<EnemyMove>().Slow();
                    else if(oneEnemy.GetComponent<EnemyMoveLevel>())
                        oneEnemy.GetComponent<EnemyMoveLevel>().Slow();
                }
                Debug.Log("슬로우 스킬 활성화");
                //animator.SetBool("Dash", true);
                skill_02_Available = false;     //스킬 활성화
                S2_duration = 0;
                S2_coolTime = 0;
                skill_02_On = true;
            }
        }
        else
        {
            if (skill_02_On == true)
            {
                S2_duration += Time.deltaTime;
            }
            S2_coolTime += Time.deltaTime;
            S2_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S2_leftTime / S2_coolTimeWaiting);
            if (skill_02_image) skill_02_image.fillAmount = ratio;

            if (S2_duration > S2_durationWaiting)       //지속시간이 끝나면
            {
                //animator.SetBool("Dash", false);
                player.transform.Find("ShockWave").gameObject.SetActive(false);  //슬로우 이펙트
                S2_duration = 0;
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().ReleaseSkill();
                }
                skill_02_On = false;
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
        if (skill_03_Available == true) //스킬 사용 가능일 때
        {
            if (skillUse == true)
            {
                Debug.Log("방어 스킬 활성화");
                player.transform.Find("ShockWave").gameObject.SetActive(true);  //슬로우 이펙트
                animator.SetBool("Cast Spell", true);
                skill_03_Available = false;     //스킬 사용 불가능
                SoundManager.instance.SkillDefendUse();  //스킬 사운드
                S3_duration = 0;
                S3_coolTime = 0;
                skill_03_On = true;
            }
        }
        else
        {
            if (skill_03_On == true)
            {
                S3_duration += Time.deltaTime;
            }
            S3_coolTime += Time.deltaTime;
            S3_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S3_leftTime / S3_coolTimeWaiting);
            if (skill_03_image) skill_03_image.fillAmount = ratio;

            if (S3_duration > S3_durationWaiting)       //지속시간이 끝나면
            {
                //animator.SetBool("Dash", false);
                player.transform.Find("ShockWave").gameObject.SetActive(false);  //슬로우 이펙트
                S3_duration = 0;
                skill_03_On = false;
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
    void Skill_Stun()         // Stun Skill
    {
        if (skill_04_Available == true) //스킬 활성화 일 때
        {
            if (skillUse == true)
            {
                SoundManager.instance.SkillStunUse();  //스킬 사운드
                player.transform.Find("ShockWave").gameObject.SetActive(true);  //슬로우 이펙트
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().Stun();
                }
                Debug.Log("스턴 스킬 활성화");
                //animator.SetBool("Dash", true);

                skill_04_Available = false;     //스킬 활성화
                S4_duration = 0;
                S4_coolTime = 0;
                skill_04_On = true;
            }
        }
        else
        {
            if (skill_04_On == true)
            {
                S4_duration += Time.deltaTime;
            }
            S4_coolTime += Time.deltaTime;
            S4_leftTime -= Time.deltaTime;

            float ratio = 1.0f - (S4_leftTime / S4_coolTimeWaiting);
            if (skill_04_image) skill_04_image.fillAmount = ratio;

            if (S4_duration > S4_durationWaiting)       //지속시간이 끝나면
            {
                //animator.SetBool("Dash", false);
                player.transform.Find("ShockWave").gameObject.SetActive(false);  //슬로우 이펙트
                enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject oneEnemy in enemy)
                {
                    oneEnemy.GetComponent<EnemyMove>().ReleaseSkill();
                }
                S4_duration = 0;
                skill_04_On = false;
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



}
