using System;
using System.Collections.Generic;

public class TypeOfObjectsDictionary : Dictionary<Type, object> {
	
	public T GetTypeObject<T>() {
		return (T)this[typeof(T)];
	}
	
	public void AddTypeObject<T>(object obj) {
		Add(typeof(T), obj);
	}
}
