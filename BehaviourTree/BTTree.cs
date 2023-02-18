using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTTree : MonoBehaviour {

	private BTNode _root = null;
    private Animator _animator;

    protected void Start() {
		_root = SetupTree();
        _animator = transform.GetComponent<Animator>();
    }

	void Update() {
		if (_root != null) {
			_root.Evaluate();
		}
	}

	protected abstract BTNode SetupTree();


    // Animator
    private void SetAnimatorBool(string name, bool value) {
        if (_animator) {
            _animator.SetBool(name, value);
        }
    }
}
