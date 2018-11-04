using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hopScript : MonoBehaviour {

    private Vector3 center;
    private Vector3 mouseInit;
    private GameObject marker;
    private float previousJumpTime;
    private bool midair;
    private IEnumerator jumpRoutine;
    private float initYPos;

    public GameObject landMarker;
    [System.NonSerialized]
    public cacState state;
    public bool MOUSE_CONTROLS;
    public int MAX_POWER;
    public int JUMP_FRAMES;
    public float JUMP_REFRESH;

    public delegate void kill(Collision victim);
    public kill attack;

	// Use this for initialization
	void Start () {
        initYPos = transform.position.y;
        previousJumpTime = 0;
        mouseInit = Vector3.zero;
        state = cacState.IDLE;
        center = new Vector3(Screen.width / 2, Screen.height / 2);
        midair = false;

        this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        marker = GameObject.Instantiate(landMarker);
        marker.transform.position = new Vector3(0, 800, 0);

        if (MOUSE_CONTROLS)
            StartCoroutine("mouseControls");
        else
            StartCoroutine("keyboardControls");
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("enemy") && (Time.time - previousJumpTime) < JUMP_REFRESH * 2)
        {
            state = cacState.ATTACKING;
            StopCoroutine(jumpRoutine);
            jumpRoutine = null;
            transform.position = new Vector3(transform.position.x, initYPos, transform.position.z);
            attack(collision);
        }
    }

    public void setIdle()
    {
        state = cacState.IDLE;
    }

    // Update is called once per frame
    void Update() {
        
        
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        //Debug.DrawRay(transform.position, transform.forward * 8, Color.red);
	}

    IEnumerator mouseControls()
    {
        while (true)
        {
            Vector3 line = Vector3.Normalize(Input.mousePosition - center);
            line.z = line.y;
            line.y = 0;

            //transform.LookAt(transform.position - line);

            Quaternion toRotation = Quaternion.LookRotation(-1 * line, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);

            if ((Time.time - previousJumpTime) > JUMP_REFRESH && Input.GetKeyDown(KeyCode.Mouse0))
            {
                mouseInit = Input.mousePosition;
                StartCoroutine("Charge");

            }
            yield return 0;
        }
    }

    IEnumerator keyboardControls()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0, 150 * Time.deltaTime, 0));
            } else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0, -150 * Time.deltaTime, 0));
            }
            if (Input.GetKeyDown(KeyCode.Space) && (Time.time - previousJumpTime) > JUMP_REFRESH )
            {
                StartCoroutine("Charge");
            }

            yield return 0;
        }
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
            
            outTime = Time.time - initTime;
            if (outTime > MAX_POWER)
                outTime = MAX_POWER;
            outTime *= 12;

            RaycastHit hit = new RaycastHit();
            Physics.Raycast(transform.position, transform.forward, out hit);
            if (hit.collider != null && hit.distance < outTime)
            {
                Debug.Log("we are jumping less now");
                outTime = hit.distance - 0.8f; //minus some offset
            }
            marker.transform.position = transform.position + transform.forward * outTime;

            if (MOUSE_CONTROLS && Input.GetKeyUp(KeyCode.Mouse0))
            {
                break;
            } else if (!MOUSE_CONTROLS && Input.GetKeyUp(KeyCode.Space))
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

        if (jumpRoutine == null)
        {
            previousJumpTime = Time.time;
            state = cacState.JUMPING;
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
            
        } else
        {
            Debug.Log("jump called while midair");
        }
        jumpRoutine = null;
    }

    public enum cacState
    {
        IDLE = 0,
        CHARGING = 1,
        JUMPING = 2,
        ATTACKING = 3,
    };
}
