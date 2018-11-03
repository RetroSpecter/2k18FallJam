using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hopScript : MonoBehaviour {
    Vector3 mouseInit;
    bool dragging;
    Camera cameron;
    public GameObject food;

    public int MAX_POWER;
    public int JUMP_FRAMES;
    

	// Use this for initialization
	void Start () {
        mouseInit = Vector3.zero;
        dragging = false;
        cameron = Camera.main;
        
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) { 
            mouseInit = Input.mousePosition;
            StartCoroutine("Charge");
        }
        
        
        cameron.transform.LookAt(this.gameObject.transform.position);
        Debug.DrawRay(transform.position, transform.forward * 8);
	}

  

    IEnumerator Charge()
    {
        Debug.Log("started charge");
        int timer = 0;
        while (true)
        {
            Vector3 line = Vector3.Normalize(Input.mousePosition - mouseInit);
            line.z = line.y;
            line.y = 0;
            transform.LookAt(transform.position - line);
            if (Input.GetKeyUp(KeyCode.Mouse0)) { 
                Debug.Log("stop stop STOP!");
                break;
            }
            if(timer < MAX_POWER)
            {
                timer++;
            }
            yield return 0;
        }
        IEnumerator jumpCoroutine = Jump(timer * 1f * Time.deltaTime);
        StartCoroutine(jumpCoroutine);
        
    }

    //dont touch
    IEnumerator Jump(float distance)
    {
        float initYPos = transform.position.y;
        float xpos = 0;
        while (xpos < distance)
        {
            xpos += distance / JUMP_FRAMES;
            transform.position += transform.forward * distance / JUMP_FRAMES;
            float quadSol = (-1 * Mathf.Pow(xpos, 2)) + (distance * xpos);
            transform.position = new Vector3(transform.position.x, initYPos + quadSol, transform.position.z);
            
            yield return 0;
        }
        transform.position = new Vector3(transform.position.x, initYPos, transform.position.z);

        /*
        Debug.Log("vector: " + line);
        Vector3 goTo = transform.position - transform.right * line.x;
        goTo -= transform.forward * line.y;
        
        transform.position = goTo;

        */
        
    }
}
