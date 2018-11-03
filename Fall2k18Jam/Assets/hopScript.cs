using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hopScript : MonoBehaviour {
    Vector2 mouseInit;
    bool dragging;
    Camera cameron;
    

	// Use this for initialization
	void Start () {
        mouseInit = Vector2.zero;
        dragging = false;
        cameron = Camera.main;
        
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) { 
            mouseInit = Input.mousePosition;
            
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            jump(Input.mousePosition);
        }
        cameron.transform.LookAt(this.gameObject.transform.position);
        Debug.DrawRay(transform.position, transform.forward * 8);
	}

    //dont touch
    private void jump(Vector2 mouseFin)
    {
        Vector3 line = Vector3.Normalize(mouseFin - mouseInit);
        
        Debug.Log("vector: " + line);
        Vector3 goTo = transform.position - transform.right * line.x;
        goTo -= transform.forward * line.y;
        float rot = Vector3.Angle(new Vector3(0, -1, 0), line);
        transform.RotateAround(Vector3.up, Mathf.Deg2Rad * rot);
        transform.position = goTo;

        
        
    }
}
