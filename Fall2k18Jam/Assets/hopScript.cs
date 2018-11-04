using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hopScript : MonoBehaviour {

    private Vector3 center;
    Vector3 mouseInit;
    Camera cameron;
    private GameObject marker;

    public GameObject landMarker;
    public cacState state;
    public int MAX_POWER;
    public int JUMP_FRAMES;
    

	// Use this for initialization
	void Start () {
        mouseInit = Vector3.zero;
        state = cacState.IDLE;
        cameron = Camera.main;
        center = new Vector3(Screen.width / 2, Screen.height / 2);
        
        this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        marker = GameObject.Instantiate(landMarker);
        marker.transform.position = new Vector3(0, 800, 0);

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) { 
            mouseInit = Input.mousePosition;
            StartCoroutine("Charge");
            
        }
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        //cameron.transform.LookAt(this.gameObject.transform.position);
        Debug.DrawRay(transform.position, transform.forward * 8, Color.red);
	}

  

    IEnumerator Charge()
    {
        Debug.Log("started charge");
        float initTime = Time.time;
        float outTime = 0;
        
        marker.transform.position = transform.position;
        state = cacState.CHARGING;
        
        while (true)
        {
            Vector3 line = Vector3.Normalize(Input.mousePosition - center);
            line.z = line.y;
            line.y = 0;
            
            //transform.LookAt(transform.position - line);

            Quaternion toRotation = Quaternion.LookRotation(-1 * line, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);
            
            outTime = Time.time - initTime;
            if (outTime > MAX_POWER)
                outTime = MAX_POWER;
            outTime *= 12;
            marker.transform.position = transform.position + transform.forward * outTime;

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                break;
            }

            yield return 0;
        }
        marker.transform.position += transform.up * 800;
        IEnumerator jumpCoroutine = Jump(outTime);
        StartCoroutine(jumpCoroutine);
        
    }

    //dont touch
    IEnumerator Jump(float distance)
    {
        Debug.Log(distance);
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
