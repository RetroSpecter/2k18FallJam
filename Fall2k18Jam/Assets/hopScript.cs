﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hopScript : MonoBehaviour {

    private Vector3 center;
    Vector3 mouseInit;
    Camera cameron;
    
    public cacState state;
    public int MAX_POWER;
    public int JUMP_FRAMES;
    

	// Use this for initialization
	void Start () {
        mouseInit = Vector3.zero;
        state = cacState.IDLE;
        cameron = Camera.main;
        center = new Vector3(Screen.width / 2, Screen.height / 2);
        Debug.Log(center);
        this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        
        
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) { 
            mouseInit = Input.mousePosition;
            StartCoroutine("Charge");
            Debug.Log(Input.mousePosition);
        }
        
        
        //cameron.transform.LookAt(this.gameObject.transform.position);
        Debug.DrawRay(transform.position, transform.forward * 8, Color.red);
	}

  

    IEnumerator Charge()
    {
        Debug.Log("started charge");
        float startTime = Time.time;

        state = cacState.CHARGING;

        while (true)
        {
            Vector3 line = Vector3.Normalize(Input.mousePosition - center);
            line.z = line.y;
            line.y = 0;

            //transform.LookAt(transform.position - line);

            Quaternion toRotation = Quaternion.LookRotation(-1 * line, transform.up);//Quaternion.FromToRotation(transform.forward, -1 * line);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.time);
            
            if (Input.GetKeyUp(KeyCode.Mouse0)) { 
                Debug.Log("stop stop STOP!");
                break;
            }
            yield return 0;
        }
        float outTime = Time.time - startTime;
        if (outTime > MAX_POWER)
            outTime = MAX_POWER;
        IEnumerator jumpCoroutine = Jump(outTime * 12);
        StartCoroutine(jumpCoroutine);
        
    }

    //dont touch
    IEnumerator Jump(float distance)
    {
        if (state != cacState.JUMPING)
        {
            state = cacState.JUMPING;
            float initYPos = transform.position.y;
            float xpos = 0;
            while (xpos < distance)
            {
                xpos += distance / JUMP_FRAMES;
                transform.position += transform.forward * distance / JUMP_FRAMES;
                float sinSol = 3f * Mathf.Sin(Mathf.PI / distance * xpos);
                //float quadSol = (-1f * Mathf.Pow(xpos, 2)) + (distance * xpos);
                transform.position = new Vector3(transform.position.x, initYPos + sinSol, transform.position.z);

                yield return 0;
            }
            transform.position = new Vector3(transform.position.x, initYPos, transform.position.z);

            state = cacState.IDLE;
        }
    }

    public enum cacState
    {
        IDLE = 0,
        CHARGING = 1,
        JUMPING = 2,
    };
}
