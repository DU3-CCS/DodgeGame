using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Text HPLabel;
    public int HP = 3;
    public float S1_duration;
    public float S1_coolTime;
    public int S1_durationWaiting;
    public int S1_coolTimewaiting;
    public float ShowDamage;
    public float ShowDamageWaiting;

    public Animator animator;

    public float MoveSpeed = 20;
    Vector3 lookDirection;

    private bool skill_01_Available = true;
    private bool skill_02_Available = true;

    private bool ShowDamage_Available = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Dash", false);
        animator.SetBool("Defend", false);

        S1_duration = 0.0f;         //지속시간 카운터
        S1_coolTime = 0.0f;         //쿨타임 카운터
        S1_durationWaiting = 5;     //지속시간 
        S1_coolTimewaiting = 20;     //쿨타임

        ShowDamage = 0.0f;         //데미지 이미지 카운터
        ShowDamageWaiting = 0.5f;     //데미지 이미지 지속시간 

        HPLabel = GameObject.Find("HPLabel").GetComponent<Text>();  //HPLabel 연결
    }


    private void Update()
    {
        //방향키 조작
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) { moveControl(); }

        // Dash Skill
        if (skill_01_Available == true) //스킬 활성화 일 때
        {
            if (Input.GetButtonDown("Fire1"))  // 만약 Fire1(왼쪽 컨트롤)버튼이 눌리면 아래 내용을 실행.
            {
                Debug.Log("활성화");
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
            if (S1_duration > S1_durationWaiting)       //지속시간이 끝나면
            {
                MoveSpeed = 20;
                animator.SetBool("Dash", false);
                S1_duration = 0;
            }
            if (S1_coolTime > S1_coolTimewaiting)       //쿨타임이 끝나면
            {
                Debug.Log("쿨타임 끝");
                skill_01_Available = true;
                S1_coolTime = 0;
            }
        }

        //체력 관련

        if (HP == 3) HPLabel.text = "♥♥♥";
        else if (HP == 2) HPLabel.text = "♥♥♡";
        else if (HP == 1) HPLabel.text = "♥♡♡";

        //체력 0이하일시 팝업 씬으로 이동
        if (HP <= 0) { SceneManager.LoadScene("PopUp"); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {     

        //"Run" 애니메이션
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("Run", true);
        }
        else animator.SetBool("Run", false);

        

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

  
    void moveControl()
    {
        float horizontal = Input.GetAxisRaw("Vertical");
        float vertical = Input.GetAxisRaw("Horizontal");
        lookDirection = horizontal * Vector3.forward + vertical * Vector3.right;

        this.transform.rotation = Quaternion.LookRotation(lookDirection);
        this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
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
            if(HP < 3) HP += 1;
            Destroy(other.gameObject);
            Debug.Log("바나나!");

        }
        if (other.gameObject.tag == "item28")
        {
            HP = 3;         
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
 