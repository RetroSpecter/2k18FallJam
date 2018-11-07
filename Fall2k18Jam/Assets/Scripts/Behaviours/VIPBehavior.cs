using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class VIPBehavior : MonoBehaviour {

    private NavMeshAgent nav;
    private int pointIndex = 0;

    private List<TreeNode> nodes;

    private bool playerSpotted = false;
    private Vector3 lastPlayerPos = Vector3.zero;

    private BT tree;
    TreeNode idle, yellow, red;

    public float viewDistance = 8;
	[Range(0, 90)]
	private float viewAngle = 70;
	public LayerMask viewMask;
    public Light spotlight;
	private Color originalColor;
    public List<Vector3> points;

    private hopScript player;
	private Animator anim;
    
    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<hopScript>();
        anim = GetComponent<Animator>();
        idle = new TreeNode(IsPlayerSpotted, OnPlayerSpotted, OnFailure, null);
        red = new TreeNode(IsPlayerStillInSight, OnEscape, OnDeath, null);
        nodes = new List<TreeNode>()
        {
            idle, red
        };
        tree = new BT(nodes);
        if (spotlight != null) {
            originalColor = spotlight.color;
        }
        StartCoroutine(tree.Tick());
	}

    IEnumerator IsPlayerSpotted(Action<BTEvaluationResult> callback)
    {
        while (true) {
			anim.SetInteger("enemyState", 0);
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
    
    
    IEnumerator IsPlayerStillInSight(Action<BTEvaluationResult> callback)
    {
        while (true) {
            if (playerSpotted)
            {
                //keep running
                callback(BTEvaluationResult.Continue);
                yield return null;
            }
            callback(BTEvaluationResult.Success);
            yield break;
        }
    }

    IEnumerator OnPlayerSpotted()
    {
        playerSpotted = true;
        yield return null;
    }

    IEnumerator OnFailure()
    {
        playerSpotted = false;
        yield return null;
    }

    IEnumerator OnEscape()
    {
        yield return null;
    }

    

    IEnumerator OnDeath()
    {
        yield return null;
        //give the player some points, spawn a new vip somewhere else
    }

    IEnumerator MoveToPoint(int pointToLookAt) {
		pointIndex++;
		if (pointIndex >= points.Count) {
			pointIndex = 0;
		}
		yield return StartCoroutine(LookAtPoint(points[pointToLookAt]));
		nav.SetDestination(points[pointIndex]);
	}

	IEnumerator MoveToPoint(Vector3 point) {
		yield return StartCoroutine(LookAtPoint(point));
		nav.SetDestination(point);
	}

    IEnumerator LookAtPoint(Vector3 point) {
		Vector3 lookDir = point - transform.position;
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
