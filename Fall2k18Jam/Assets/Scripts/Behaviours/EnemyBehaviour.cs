using System.Collections;
using System.Collections.Generic;
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
	private bool sweeping = false;
	private Vector3 lastPlayerPos = Vector3.zero;

	private Coroutine lookAt;

	private BT tree;
	TreeNode idle, yellow, red;

	private Transform player;

	// Use this for initialization
	void Start () {
		if (spotlight != null) {
			originalColor = spotlight.color;
			viewAngle = spotlight.spotAngle;
		}
		nav = GetComponent<NavMeshAgent>();
		player = FindObjectOfType<hopScript>().transform;
		nav.updateRotation = false;
		idle = new TreeNode(isPlayerSpotted, OnPlayerSpotted, OnFailure, null);
		yellow = new TreeNode(isPlayerSpottedYellow, OnPlayerSpottedYellow, OnFailure, null);
		red = new TreeNode(isPlayerStillInSight, OnPlayerDeath, OnPlayerEscaped, null);
		nodes = new List<TreeNode>() {
			idle, yellow, red
		};
		tree = new BT(nodes);
		StartCoroutine(tree.Tick());
		nav.SetDestination(points[0]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	BTEvaluationResult isPlayerSpotted() {
		playerSpotted = CanSeePlayer();
		if (playerSpotted) {
			spotlight.color = Color.red;
		} else {
			spotlight.color = originalColor;
		}
		if (playerSpotted) {
			playerSpotted = false;
			tree.treeNodes.Remove(idle);
			return BTEvaluationResult.Success;
		}
		int pointToLookAt = (pointIndex + 1 >= points.Count)?0:pointIndex + 1;
		if (Vector3.Distance(transform.position, nav.destination) < 2) {
			MoveToPoint(pointToLookAt);
		}
		return BTEvaluationResult.Continue;
	}

	BTEvaluationResult isPlayerSpottedYellow() {
		playerSpotted = CanSeePlayer();
		if (playerSpotted) {
			spotlight.color = Color.red;
		} else {
			spotlight.color = originalColor;
		}
		if (playerSpotted) {
			playerSpotted = false;
			tree.treeNodes.Remove(idle);
			return BTEvaluationResult.Success;
		}
		if (!sweeping) {
			int pointToLookAt = (pointIndex + 1 >= points.Count)?0:pointIndex + 1;
			if (Vector3.Distance(transform.position, nav.destination) < 2) {
				MoveToPoint(pointToLookAt);
				Sweep();
			}
		}
		return BTEvaluationResult.Continue;
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

	void MoveToPoint(int pointToLookAt) {
		if (lookAt != null) {
			StopCoroutine(lookAt);
		}
		pointIndex++;
		if (pointIndex >= points.Count) {
			pointIndex = 0;
		}
		lookAt = StartCoroutine(LookAtPoint(pointToLookAt));
		nav.SetDestination(points[pointIndex]);
	}

	IEnumerator LookAtPoint(int index) {
		Vector3 lookDir = points[index] - transform.position;
		lookDir.y = 0;
		Quaternion toRot = Quaternion.LookRotation(lookDir, Vector3.up);
		float startTime = Time.time;
		while (transform.rotation != toRot) {
			transform.rotation = Quaternion.Slerp(transform.rotation, toRot, Time.time - startTime);
			yield return null;
		}
	}

	bool CanSeePlayer() {
		if (Vector3.Distance(transform.position, player.position) < viewDistance) {
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
			if (angleToPlayer < viewAngle / 2f) {
				if (!Physics.Linecast(transform.position, player.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}

	void Sweep() {
		sweeping = true;
		StartCoroutine(Sweeping());
	}

	IEnumerator Sweeping() {
		Vector3 forward = transform.forward;
		Vector3 right = transform.right;
		Vector3 left = -transform.right;
		transform.rotation = Quaternion.LookRotation(right, Vector3.up);
		yield return new WaitForSeconds(1);
		transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
		yield return new WaitForSeconds(1);
		transform.rotation = Quaternion.LookRotation(left, Vector3.up);
		yield return new WaitForSeconds(1);
		transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
		yield return new WaitForSeconds(1);
		sweeping = false;
	}
}
