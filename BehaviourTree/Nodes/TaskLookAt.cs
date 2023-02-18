using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskLookAt : BTNode {

	private Transform _transform;
	private Transform _lookAt;
	private float _turnSpeed;
	private bool _lookingAt;

	public TaskLookAt(BTTree tTree, TaskLookAtData data): base(tTree) {
		_transform = data.T;
        _lookAt = data.LookAt;
        _turnSpeed = data.TurnSpeed;
		
	}

	public override BTNodeState Evaluate() {
		if (_lookingAt) {
            state = BTNodeState.SUCCESS;
        }
		else {
            Vector3 relativePos = _lookAt.position - _transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
			Quaternion current = _transform.localRotation;

            _transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * _turnSpeed);
            state = BTNodeState.RUNNING;

            if (Quaternion.Angle(rotation, _transform.localRotation) < 1f) {
				// _lookingAt = true;
                state = BTNodeState.SUCCESS;
            }
		}
    
		return state;
	}

	public class TaskLookAtData {
		public Transform T;
		public Transform LookAt;
		public float TurnSpeed;
	}
}
