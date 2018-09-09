using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
    static public Quaternion GetRotFromVectors(Vector2 posStart, Vector2 posEnd)
    {
        return Quaternion.Euler(0, 0, -Mathf.Atan2(posEnd.x - posStart.x, posEnd.y - posStart.y) * Mathf.Rad2Deg);
    }
    public float MoveSpeed = 1.0f;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, 0, MoveSpeed * 20 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, 0, -MoveSpeed * 20 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(MoveSpeed * 20 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-MoveSpeed * 20 * Time.deltaTime, 0, 0);
        }
        this.transform.rotation = GetRotFromVectors(this.transform.position, this.transform.position);
    }


}
