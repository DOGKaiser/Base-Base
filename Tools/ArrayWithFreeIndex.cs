using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayWithFreeIndex<T> {
	List<T> array = new List<T>();
	List<int> freeIndexs = new List<int>();

	public void FreeIndex(int index, T value) {
		array[index] = value;
		freeIndexs.Add(index);
	}

	public int SetNextFreeIndex(T value) {
		int index;
		if (freeIndexs.Count == 0) {
			index = array.Count;
			array.Add(value);
		} 
		else {
			index = freeIndexs[0];
			freeIndexs.RemoveAt(0);
			array[index] = value;
		}
		
		return index;
	}
	
	public void SetIndex(T value, int index) {
		if (freeIndexs.Contains(index)) {
			freeIndexs.Remove(index);
		}

		if (array.Count <= index) {
			while (array.Count < index) {
				freeIndexs.Add(array.Count);
				array.Add(default(T));
			}
			array.Add(value);
		} 
		else {
			array[index] = value;
		}
	}
	
	public T this[int index] {
		get {
			return array[index];
		}
	}
	
	public int Count {
		get {
			return array.Count; 
		}
	}
}
