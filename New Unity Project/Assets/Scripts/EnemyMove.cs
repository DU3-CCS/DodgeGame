using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    float MoveSpeed = 20.0f;
    private float accumTimeAterUpdate;
    private float updateTime;
    private Vector3 myposition;
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
        if (accumTimeAterUpdate >= updateTime)
        {
            accumTimeAterUpdate = 0;
            this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        }
        if(gameObject.transform.position.x < -30 || gameObject.transform.position.x > 30 || gameObject.transform.position.z < -36 || gameObject.transform.position.x > 33) Destroy(gameObject);
    
     }
}