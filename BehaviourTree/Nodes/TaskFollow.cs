using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFollow : BTNode {

	private Transform _transform;
	private Transform _target;
	private float _speed;
	
	private int _currentWaypointIndex = 0;

	private float _waitTime = 1f; // in seconds
	private float _waitCounter = 0f;
	private bool _waiting = false;
	private Animator _animator;

	public TaskFollow(BTTree tree, Transform transform, Transform target, float speed) : base(tree) {
		_transform = transform;
		_target = target;
		_speed = speed;
		
		_animator = transform.GetComponent<Animator>();
	}

	public override BTNodeState Evaluate()
	{
		if (_waiting)
		{
			_waitCounter += Time.deltaTime;
			if (_waitCounter >= _waitTime)
			{
				_waiting = false;
				SetAnimatorBool("Walking", true);
			}
		}
		else {
			Vector3 pos = _target.position;
			if (Vector3.Distance(_transform.position, pos) < 0.01f)
			{
				_transform.position = pos;
				_waitCounter = 0f;
				_waiting = true;

				SetAnimatorBool("Walking", false);
			}
			else
			{
				_transform.position = Vector3.MoveTowards(_transform.position,pos, _speed * Time.deltaTime);
				_transform.LookAt(pos);
			}
		}
    
		state = BTNodeState.RUNNING;
		return state;
	}

	private void SetAnimatorBool(string name, bool value) {
		if (_animator) {
			_animator.SetBool(name, value);
		}
	}
}
