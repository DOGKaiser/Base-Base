using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TaskJump : BTNode {

	private Transform _transform;
	private Transform _target;
	private float _duration;

	private float _waitTime = 1f; // in seconds
	private float _jumpHeight = 5f; // in seconds
	private float _waitCounter = 0f;
	private bool _waiting = false;
	private bool _targeting = false;
	private Animator _animator;
	private Vector3 _targetPos;

	public TaskJump(BTTree tree, Transform transform, Transform target, float duration) : base(tree) {
		_transform = transform;
		_target = target;
		_duration = duration;
		
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
				SetAnimatorBool("Jumping", true);
			}
		}
		else
		{
			if (!_targeting) {
				_targetPos = _target.position;
				
				_transform.DOJump(_targetPos, _jumpHeight, 1, _duration);
				_transform.LookAt(_targetPos);
				
				_targeting = true;
			}
			
			if (Vector3.Distance(_transform.position, _targetPos) < 0.01f)
			{
				_transform.position = _targetPos;
				_waitCounter = 0f;
				_waiting = true;
				_targeting = false;
				
				SetAnimatorBool("Jumping", false);
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
