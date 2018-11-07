using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollSwitch : MonoBehaviour {

    Rigidbody[] rigidbodies;
    Animator anim;
    public float ragdollTime = 1;
    public float deathAnimTime = 1;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rigidbodies) {
            r.isKinematic = true;
        }
	}

    public void death() {
        GetComponent<CapsuleCollider>().enabled = false;
        EnemyBehaviour enemy = GetComponent<EnemyBehaviour>();
        if (enemy != null) {
            GetComponent<EnemyBehaviour>().StopAllCoroutines();
            GetComponent<EnemyBehaviour>().enabled = false;
        }
        VIPBehavior vip = GetComponent<VIPBehavior>();
        if (vip != null) {
            GetComponent<VIPBehavior>().StopAllCoroutines();
            GetComponent<VIPBehavior>().enabled = false;
        }
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        Invoke("turnOnRagdoll", deathAnimTime);
    }


    void turnOnRagdoll() {
        if (anim != null) {
            anim.enabled = false;
        }

        foreach (Rigidbody r in rigidbodies) {
            r.isKinematic = false;
            r.AddForce(-transform.forward * 500 + transform.up * 200);
        }

        Invoke("turnOffRagdoll", ragdollTime);
    }

    void turnOffRagdoll() {
        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
        }
    }
}
