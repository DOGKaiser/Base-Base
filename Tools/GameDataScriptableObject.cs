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
	
	protected Dictionary<string, TConfig> MakeConfigDictionary<TConfig>(string debugLog, string loc = "") where TConfig : ScriptableObject {
		Dictionary<string, TConfig> dic = new Dictionary<string, TConfig>();
		TConfig[] array = Resources.LoadAll<TConfig>(loc);
		UnityTools.DataLogs(array.ToArray(), debugLog);
		foreach (TConfig t in array) {
			dic.Add(t.name, t);
		}
		return dic;
	}
}
