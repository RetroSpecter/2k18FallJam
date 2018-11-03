using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBT : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<TreeNode> nodes = new List<TreeNode>();
		nodes.Add(new TreeNode(delegate() {return BTEvaluationResult.Success;}, 
			delegate() {Debug.Log("Success");}, 
			delegate() {Debug.Log("Failure");}));
		BT bt = new BT(nodes);
		StartCoroutine(bt.Tick());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
