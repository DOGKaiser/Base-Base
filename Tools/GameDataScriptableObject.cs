using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataScriptableObject<T> where T : ScriptableObject {

	Dictionary<string, T> _array;

	public void Init(string debug) {
		_array = MakeConfigDictionary<T>(debug);
	}
    
	public T GetConfig(string id) {
		return _array[id];
	}
	
	public List<T> GetList() {
		return _array.Values.ToList();
	}

	public string GetRandomKey() {
		return _array.Keys.ElementAt(Random.Range(0, _array.Keys.Count));
	}
	
	public T GetRandomScriptableObject() {
		return _array[GetRandomKey()];
	}
	
	protected Dictionary<string, T> MakeConfigDictionary<T>(string debugLog, string loc = "") where T : ScriptableObject {
		Dictionary<string, T> dic = new Dictionary<string, T>();
		T[] array = Resources.LoadAll<T>(loc);
		UnityTools.DataLogs<T>(array.ToArray(), debugLog);
		foreach (T t in array) {
			dic.Add(t.name, t);
		}
		return dic;
	}
}
