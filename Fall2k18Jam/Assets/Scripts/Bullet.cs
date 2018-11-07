using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player") {
			Debug.Log("Game Over");
			Destroy(col.gameObject);
			GameManager.instance.TriggerGameOver();
			Destroy(transform.parent.gameObject);
		}
	}
}
