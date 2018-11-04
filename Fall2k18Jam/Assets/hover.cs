using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour {
    private float initY;
	// Use this for initialization
	void Start () {
        initY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 1.5f + initY + 0.2f * Mathf.Sin(Time.time), transform.position.z);
        Debug.Log(transform.position);
	}
}
