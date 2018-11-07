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
        int rand = Random.Range(0, 2);
        AudioManager.instance.Play("DeathGrunt" + rand);
        Invoke("attackFinished", attackTime);
        if (collider.gameObject.GetComponent<VIPBehavior>() != null) {
            GameManager.instance.IncrementPoints();
            Destroy(collider.gameObject, 5);
        }
    }

    void attackFinished() {
        hop.state = cacState.IDLE;
    }

	// Update is called once per frame
	void Update () {
        cacState  curState = hop.state;
        anim.SetBool("charging", curState == cacState.CHARGING);
        anim.SetBool("jumping", curState == cacState.JUMPING);
        //anim.SetBool("attacking", curState == hopScript.cacState);
    }
}
