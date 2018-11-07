using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private int points = 0;
	public bool gameOver = false;

#region Singleton
	public static GameManager instance;
	void Awake() {
		if (instance == null) 
			instance = this;
		else
			Destroy(gameObject);
	}
#endregion

	public void IncrementPoints() {
		points++;
	}

	public int GetPoints() {
		return points;
	}

	public void TriggerGameOver() {
		if (gameOver) {
			return;
		}
		gameOver = true;
		// Show that you died and with how many points.
		// then set points = 0;
	}
}
