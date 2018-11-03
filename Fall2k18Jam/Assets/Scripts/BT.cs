using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BT {

	public List<TreeNode> treeNodes;

	public BT(List<TreeNode> treeNodes) {
		this.treeNodes = treeNodes;
	}

	public IEnumerator Tick() {
		foreach (TreeNode node in treeNodes) {
			while (true) {
				BTEvaluationResult result = node.Test();
				switch(result) {
					case BTEvaluationResult.Success:
						yield return GameManager.instance.StartCoroutine(node.success());
						break;
					case BTEvaluationResult.Failure:
						yield return GameManager.instance.StartCoroutine(node.failure());
						break;
					case BTEvaluationResult.Continue:
						break;
					default:
						break;
				}
				if (result != BTEvaluationResult.Continue) {
					break;
				}
				yield return null;
			}
		}
	}
}

public class TreeNode {
	public TreeNode child;
	private Func<BTEvaluationResult> test;

	public Func<IEnumerator> success, failure;

	public TreeNode(Func<BTEvaluationResult> test, Func<IEnumerator> success, Func<IEnumerator> failure, TreeNode child = null) {
		this.test = test;
		this.success = success;
		this.failure = failure;
		this.child = child;
	}

	public BTEvaluationResult Test() {
		if (child != null) {
			return child.Test();
		}
		return test();
	}
}

public enum BTEvaluationResult {
	Success,
	Failure,
	Continue
}
