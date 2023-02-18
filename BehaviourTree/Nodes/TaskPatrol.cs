using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPatrol : BTNode {

	private Transform _transform;
	private Transform[] _waypoints;
	private float _speed;
	
	private int _currentWaypointIndex = 0;

	private float _waitTime = 1f; // in seconds
	private float _waitCounter = 0f;
	private bool _waiting = false;
	private Animator _animator;

	public TaskPatrol(BTTree tree, Transform transform, Transform[] waypoints, float speed) : base(tree) {
		_transform = transform;
		_waypoints = waypoints;
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
		else
		{
			Transform wp = _waypoints[_currentWaypointIndex];
			if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
			{
				_transform.position = wp.position;
				_waitCounter = 0f;
				_waiting = true;

				_currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
				SetAnimatorBool("Walking", false);
			}
			else
			{
				_transform.position = Vector3.MoveTowards(_transform.position,wp.position, _speed * Time.deltaTime);
				_transform.LookAt(wp.position);
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
