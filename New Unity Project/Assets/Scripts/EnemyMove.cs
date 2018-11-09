using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    public float MoveSpeed = 20.0f;
    private float accumTimeAterUpdate;
    private float updateTime;
    private Vector3 myposition;
    Vector3 lookDirection;

    public Animator animator;           //애니메이터

    public GameObject target;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>(); //애니메이터 연결

        myposition = transform.position;
        target = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        accumTimeAterUpdate += Time.deltaTime;
        if (accumTimeAterUpdate >= updateTime)
        {
            accumTimeAterUpdate = 0;
            this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        }
        if(gameObject.transform.position.x < -30 || gameObject.transform.position.x > 30 || gameObject.transform.position.z < -36 || gameObject.transform.position.x > 33) Destroy(gameObject);
    
    }
    public void Slow()
    {
        MoveSpeed = 8.0f;
        animator.SetBool("Run", false);
        animator.SetBool("Walk", true);
        
    }
    
    public void Stun()
    {
        MoveSpeed = 0f;
        animator.SetBool("Run", false);
        animator.SetBool("Stunned", true);
    }
    public void ReleaseSkill()
    {
        MoveSpeed = 20.0f;
        animator.SetBool("Run", true);
        animator.SetBool("Slow", false);
        animator.SetBool("Stunned", false);

    }
}