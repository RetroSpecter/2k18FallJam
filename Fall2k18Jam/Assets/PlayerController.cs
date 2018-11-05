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
        hop.attack += attack;

	}

    void attack(Collision collider) {
        Animator enemyAnim = collider.gameObject.GetComponent<Animator>();
        enemyAnim.transform.position = transform.position + transform.forward * 1f;
        enemyAnim.transform.LookAt(this.transform);
        enemyAnim.Play("death");
        anim.Play("Hug");
        enemyAnim.GetComponent<RagdollSwitch>().death();
        Invoke("attackFinished", attackTime);
    }

    void attackFinished() {
        hop.state = hopScript.cacState.IDLE;
    }

	// Update is called once per frame
	void Update () {
        hopScript.cacState  curState = hop.state;
        anim.SetBool("charging", curState == hopScript.cacState.CHARGING);
        anim.SetBool("jumping", curState == hopScript.cacState.JUMPING);
        //anim.SetBool("attacking", curState == hopScript.cacState);
    }
}
