using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public List<Vector3> points;
	private List<TreeNode> nodes;

	private bool playerSpotted = false;
	private Vector3 lastPlayerPos = Vector3.zero;

	private BT tree;
	TreeNode idle, yellow, red;

	// Use this for initialization
	void Start () {
		idle = new TreeNode(isPlayerSpotted, OnPlayerSpotted, OnFailure, null);
		yellow = new TreeNode(isPlayerSpottedYellow, OnPlayerSpottedYellow, OnFailure, null);
		red = new TreeNode(isPlayerStillInSight, OnPlayerDeath, OnPlayerEscaped, null);
		nodes = new List<TreeNode>() {
			idle, yellow, red
		};
		tree = new BT(nodes);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	BTEvaluationResult isPlayerSpotted() {
		if (playerSpotted) {
			playerSpotted = false;
			tree.treeNodes.Remove(idle);
			return BTEvaluationResult.Success;
		} else {
			// DO enemy movement here
			return BTEvaluationResult.Continue;
		}
	}

	BTEvaluationResult isPlayerSpottedYellow() {
		if (playerSpotted) {
			playerSpotted = false;
			return BTEvaluationResult.Success;
		} else {
			// DO enemy movement + sweeping here
			return BTEvaluationResult.Continue;
		}
	}

	BTEvaluationResult isPlayerStillInSight() {
		/* if player is dead, return success */
		if (playerSpotted) {
			// shoot at player
			return BTEvaluationResult.Continue;
		} else {
			// if we lose sight of the player, go to lastKnownPos
			// sweep, and then go back to path. This will be handled in Failure.
			return BTEvaluationResult.Failure;
		}
	}

	IEnumerator OnPlayerDeath() {
		yield return null;
	}

	IEnumerator OnPlayerEscaped() {
		// go to last known pos
		// sweep
		// go back to yellow path
		yield return null;
	}

	IEnumerator OnPlayerSpotted() {
		Debug.Log("PLayer Sptted");
		yield return null;
		// called when the player is spotted. If we want ot do a metal gear alert 
		// noise or something this is where we do it
	}

	IEnumerator OnPlayerSpottedYellow() {
		Debug.Log("Player spotted, shooting");
		yield return null;
	}

	IEnumerator OnFailure() {
		Debug.Log("Failure");
		yield return null;
	}
}
