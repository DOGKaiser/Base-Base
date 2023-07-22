using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayWithFreeIndex<T> where T : new() {
	T[] array;
	Queue<int> freeIndexs;
	
	public ArrayWithFreeIndex(int size) {
		array = new T[size];
		freeIndexs = new Queue<int>();

		for (int i = 0; i < size; i++) {
			freeIndexs.Enqueue(i);
		}
	}

	public void FreeIndex(int index) {
		array[index] = default(T);
		freeIndexs.Enqueue(index);
	}

	public int GetNextFreeIndex() {
		return freeIndexs.Dequeue();
	}
	
	public T this[int index] {
		get {
			return array[index];
		}
		set {
			array[index] = value;
		}
	}
}
