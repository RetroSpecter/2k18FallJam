using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VIPBehavior : MonoBehaviour {

    private NavMeshAgent nav;
    private int pointIndex = 0;

    public List<Vector3> points;
    private List<TreeNode> nodes;

    private bool playerSpotted = false;
    private Vector3 lastPlayerPos = Vector3.zero;

    private BT tree;
    TreeNode idle, yellow, red;
    /*
    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();
        idle = new TreeNode(IsPlayerSpotted, OnPlayerSpotted, OnFailure, null);
        red = new TreeNode(IsPlayerStillInSight, OnEscape, OnDeath, null);
        nodes = new List<TreeNode>()
        {
            idle, red
        };
        tree = new BT(nodes);
        StartCoroutine(tree.Tick());
	}

    BTEvaluationResult IsPlayerSpotted()
    {
        if (playerSpotted)
        {
            playerSpotted = false;
            tree.treeNodes.Remove(idle);
            return BTEvaluationResult.Success;
        }
        if ((transform.position - points[pointIndex]).sqrMagnitude < nav.stoppingDistance * nav.stoppingDistance)
        {
            Debug.Log("Setting distance");
            pointIndex++;
            if (pointIndex >= points.Count)
            {
                pointIndex = 0;
            }
            nav.SetDestination(points[pointIndex]);
        }
        return BTEvaluationResult.Continue;
    }
    
    
    BTEvaluationResult IsPlayerStillInSight()
    {
        if (playerSpotted)
        {
            //keep running
            return BTEvaluationResult.Continue;
        } else
        {
            return BTEvaluationResult.Failure;
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

    }

    

    IEnumerator OnDeath()
    {
        //give the player some points, spawn a new vip somewhere else
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
