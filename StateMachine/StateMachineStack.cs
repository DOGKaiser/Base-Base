﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class StateMachineStack : StateMachine {
	[OdinSerialize] List<IState> nextStates = new List<IState>();
	bool mLog = false;

	public void AddState(IState nextState) {
		nextStates.Add(nextState);

		currentState = nextState;

		if (nextState != null) {
			if (mLog) {
				Debug.Log("Enter State: " + currentState.ToString() + " " + StackTraceUtility.ExtractStackTrace());
			}
			currentState.Enter();
		}
	}

	public void PopState(IState nextState = null) {
		int stateCount = nextStates.Count;

		if (nextStates.Count > 0) {
			nextStates.RemoveAt(nextStates.Count - 1);

			stateCount = nextStates.Count;
			currentState.Exit();
		}

		if (nextState != null) {
			AddState(nextState);
		}
		else if (nextStates.Count > 0) {
			currentState = nextStates[nextStates.Count - 1];

			if (stateCount == nextStates.Count)
				currentState.OneTimeUpdate();
		}
		else {
			currentState = null;
		}
	}

	public int StateCount() {
		return nextStates.Count;
	}

	public string StatesDebugString() {
		string temp = "States: ";
        for (int i = 0; i < nextStates.Count; i++) {
			temp += i +":" + nextStates[i] + ",";
		}
		return temp;
	}

	public void SetLog(bool log) { mLog = log; }
}
