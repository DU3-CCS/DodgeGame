using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyMoveLevel : MonoBehaviour
{
    public float MoveSpeed = 20.0f;
    private float accumTimeAterUpdate;
    private float updateTime;
    private Vector3 myposition;
    private ThreadStart td;
    private Thread t;
    private int count = 0;
    private int turnCount = 50;
    public int maxTurnCount;

    public Animator animator;           //애니메이터
    public GameObject star_B;       //파티클

    Vector3 lookDirection;

    public GameObject target;
    // Use this for initialization
    void Start()
    {
        myposition = transform.position;
        target = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        accumTimeAterUpdate += Time.deltaTime;
        count++;

        if(count % turnCount == 0 && count / turnCount <= maxTurnCount)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            transform.LookAt(target.transform);
        }

        if (accumTimeAterUpdate >= updateTime)
        {
            accumTimeAterUpdate = 0;
            this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        }
        if (gameObject.transform.position.x < -30 || gameObject.transform.position.x > 30 || gameObject.transform.position.z < -36 || gameObject.transform.position.x > 33)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject items = Instantiate(star_B, transform.position, Quaternion.identity);
            Destroy(items, 3);
        }
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