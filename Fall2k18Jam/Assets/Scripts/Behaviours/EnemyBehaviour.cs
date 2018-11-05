using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour {

	public float viewDistance = 8;
	[Range(0, 90)]
	private float viewAngle = 70;
	public LayerMask viewMask;
	public Light spotlight;
	private Color originalColor;

	private NavMeshAgent nav;
	private int pointIndex = -1;

	public List<Vector3> points;
	private List<TreeNode> nodes;

	private bool playerSpotted = false;
	private Vector3 lastPlayerPos = Vector3.zero;

	private Coroutine lookAt;

	private BT tree;
	TreeNode idle, yellow, red;

	private hopScript player;

	// Use this for initialization
	void Start () {
		if (spotlight != null) {
			originalColor = spotlight.color;
			viewAngle = spotlight.spotAngle;
		}
		nav = GetComponent<NavMeshAgent>();
		player = FindObjectOfType<hopScript>();
		nav.updateRotation = false;
		idle = new TreeNode(isPlayerSpotted, OnPlayerSpotted, OnFailure, null);
		yellow = new TreeNode(isPlayerSpottedYellow, OnPlayerSpottedYellow, OnFailure, null);
		red = new TreeNode(isPlayerStillInSight, OnPlayerDeath, OnPlayerEscaped, null);
		nodes = new List<TreeNode>() {
			idle, yellow, red
		};
		tree = new BT(nodes);
		StartCoroutine(tree.Tick());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator isPlayerSpotted(Action<BTEvaluationResult> callback) {
		while (true) {
			playerSpotted = CanSeePlayer();
			if (playerSpotted) {
				spotlight.color = Color.red;
			} else {
				spotlight.color = originalColor;
			}
			if (playerSpotted) {
				playerSpotted = false;
				tree.treeNodes.Remove(idle);
				callback(BTEvaluationResult.Success);
				print("anemone spotted");
				yield break;
			}
			int pointToLookAt = (pointIndex + 1 >= points.Count)?0:pointIndex + 1;
			if (!nav.pathPending) {
				if (nav.remainingDistance <= nav.stoppingDistance) {
					if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f) {
						yield return StartCoroutine(MoveToPoint(pointToLookAt));
					}
				}
			}
			yield return null;
		}
	}

	IEnumerator isPlayerSpottedYellow(Action<BTEvaluationResult> callback) {
		while (true) {
			playerSpotted = CanSeePlayer();
			if (playerSpotted) {
				spotlight.color = Color.red;
			} else {
				spotlight.color = originalColor;
			}
			if (playerSpotted) {
				playerSpotted = false;
				tree.treeNodes.Remove(idle);
				callback(BTEvaluationResult.Success);
				yield break;
			}
			int pointToLookAt = (pointIndex + 1 >= points.Count)?0:pointIndex + 1;
			if (!nav.pathPending) {
				if (nav.remainingDistance <= nav.stoppingDistance) {
					if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f) {
						yield return StartCoroutine(Sweep(pointToLookAt));
					}
				}
			}
			yield return null;
		}
	}

	IEnumerator isPlayerStillInSight(Action<BTEvaluationResult> callback) {
		while (true) {
			/* if player is dead, return success */
			if (playerSpotted) {
				// shoot at player
				callback(BTEvaluationResult.Continue);
			} else {
				// if we lose sight of the player, go to lastKnownPos
				// sweep, and then go back to path. This will be handled in Failure.
				callback(BTEvaluationResult.Failure);
				yield break;
			}
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
		playerSpotted = false;
		yield return new WaitForSeconds(2);
		// called when the player is spotted. If we want ot do a metal gear alert 
		// noise or something this is where we do it
	}

	IEnumerator OnPlayerSpottedYellow() {
		Debug.Log("Player spotted, shooting");
		yield return new WaitForSeconds(2);
	}

	IEnumerator OnFailure() {
		Debug.Log("Failure");
		yield return null;
	}

	IEnumerator MoveToPoint(int pointToLookAt) {
		pointIndex++;
		if (pointIndex >= points.Count) {
			pointIndex = 0;
		}
		yield return StartCoroutine(LookAtPoint(pointToLookAt));
		nav.SetDestination(points[pointIndex]);
	}

	IEnumerator Sweep(int index) {
		float startTime = Time.time;
		Vector3 forward = transform.forward;
		Vector3 right = transform.right;
		Vector3 left = -transform.right;
		Vector3 lookDir = points[index] - transform.position;
		lookDir.y = 0;
		Quaternion toRot = Quaternion.LookRotation(lookDir, Vector3.up);
		Quaternion nextRot = Quaternion.LookRotation(right, Vector3.up);
		while (Time.time - startTime <= 1) {
			transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, Time.time - startTime);
			yield return null;
		}
		startTime = Time.time;
		nextRot = Quaternion.LookRotation(forward, Vector3.up);
		while (Time.time - startTime <= 1) {
			transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, Time.time - startTime);
			yield return null;
		}
		startTime = Time.time;
		nextRot = Quaternion.LookRotation(left, Vector3.up);
		while (Time.time - startTime <= 1) {
			transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, Time.time - startTime);
			yield return null;
		}
		yield return MoveToPoint(index);
	}

	IEnumerator LookAtPoint(int index) {
		Vector3 lookDir = points[index] - transform.position;
		lookDir.y = 0;
		Quaternion toRot = Quaternion.LookRotation(lookDir, Vector3.up);
			float startTime = Time.time;
			while (Time.time - startTime <= 1) {
				print("rotating");
				transform.rotation = Quaternion.Slerp(transform.rotation, toRot, Time.time - startTime);
				yield return null;
			}
			
	}

	bool CanSeePlayer() {
		if (player.state != cacState.JUMPING) {
			return false;
		}
		if (Vector3.Distance(transform.position, player.transform.position) < viewDistance) {
			Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
			float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
			if (angleToPlayer < viewAngle / 2f) {
				if (!Physics.Linecast(transform.position, player.transform.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}
}
