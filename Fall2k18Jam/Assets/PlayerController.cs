using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    hopScript hop;
    Animator anim;
    public static float attackTime = 3;

	// Use this for initialization
	void Start () {
        hop = GetComponent<hopScript>();
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        hopScript.cacState  curState = hop.state;
        anim.SetBool("charging", curState == hopScript.cacState.CHARGING);
        anim.SetBool("jumping", curState == hopScript.cacState.JUMPING);
        //anim.SetBool("attacking", curState == hopScript.cacState);
    }
}
