using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BTNodeState {
	RUNNING,
	SUCCESS,
	FAILURE
}

public class BTNode {
	protected BTNodeState state;

    public BTNode parent;
	protected List<BTNode> children = new List<BTNode>();

	private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
    private BTTree _tree;

    public BTNode(BTTree tree) {
		parent = null;
		_tree = tree;
	}

	public BTNode(List<BTNode> children) {
		foreach (BTNode child in children)
			_Attach(child);
	}

	private void _Attach(BTNode node) {
		node.parent = this;
		children.Add(node);
	}

	public virtual BTNodeState Evaluate() => BTNodeState.FAILURE;

	public void SetData(string key, object value) {
		_dataContext[key] = value;
	}

	public object GetData(string key) {
		object value = null;
		if (_dataContext.TryGetValue(key, out value))
			return value;

		BTNode node = parent;
		while (node != null) {
			value = node.GetData(key);
			if (value != null)
				return value;
			node = node.parent;
		}
		return null;
	}

	public bool ClearData(string key) {
		if (_dataContext.ContainsKey(key)) {
			_dataContext.Remove(key);
			return true;
		}

		BTNode node = parent;
		while (node != null) {
			bool cleared = node.ClearData(key);
			if (cleared)
				return true;
			node = node.parent;
		}
		return false;
	}
}