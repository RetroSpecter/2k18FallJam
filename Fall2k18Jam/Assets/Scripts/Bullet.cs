using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player") {
			GameManager.instance.TriggerGameOver();
			Debug.Log("Game Over");
			Destroy(transform.parent.gameObject);
		}
	}
}
