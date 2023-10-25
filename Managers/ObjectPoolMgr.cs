using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPoolMgr {
	private static readonly ObjectPoolMgr instance = new ObjectPoolMgr();
	static ObjectPoolMgr() { }
	private ObjectPoolMgr() { }
	public static ObjectPoolMgr Instance {
		get { return instance; }
	}

	Dictionary<string, GameObject> mLoadedObject = new Dictionary<string, GameObject>();
	Dictionary<int, Queue<GameObject>> mCreatedObjects = new Dictionary<int, Queue<GameObject>>();

	public bool IsLoaded(string path) {
		GameObject loadedObject;
		
		if (!mLoadedObject.TryGetValue(path, out loadedObject)) {
			return false;
		}

		return true;
	}

	GameObject LoadObject(string path) {
		GameObject loadedObject;

		if (!mLoadedObject.TryGetValue(path, out loadedObject)) {
			loadedObject = Resources.Load<GameObject>(path);

			mLoadedObject.Add(path, loadedObject);
//			Debug.LogWarning("Loaded: " + loadedObject.name);
		}

		return loadedObject;
	}
	
	public GameObject GetObject(string path, Transform parent = null) {
		if (!mLoadedObject.TryGetValue(path, out GameObject prefab)) {
			prefab = LoadObject(path);
		}

		if (parent) {
			GetObject(prefab, parent);
		}

		return GetObject(prefab);
	}

	public GameObject GetObject(GameObject prefab) {
		return GetObject(prefab, prefab.transform.localPosition, prefab.transform.localRotation, null);
	}

	public GameObject GetObject(GameObject prefab, Transform parent) {
		return GetObject(prefab, parent.position + prefab.transform.localPosition, Quaternion.LookRotation(parent.forward) * Quaternion.LookRotation(prefab.transform.forward), parent);
	}
	
	public void ReuseObject(string path, GameObject createdObj) {
		createdObj.SetActive(false);
		GameObject prefab;

		if (!mLoadedObject.TryGetValue(path, out prefab))
			return;

		ReuseObject(prefab, createdObj);
	}

	public async Task ReuseObject(GameObject prefab, GameObject createdObj) {
		createdObj.SetActive(false);
		
		if (mCreatedObjects.TryGetValue(prefab.GetInstanceID(), out Queue<GameObject> createdObjects)) {
			await Task.Delay(100);
			createdObjects.Enqueue(createdObj);
		}
	}

	public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
		
		int id = prefab.GetInstanceID();
		
		if (mCreatedObjects.TryGetValue(id, out Queue<GameObject> createdObjects)) {
			while (createdObjects.Count > 0) {
				GameObject obj = createdObjects.Dequeue();
				if (obj != null) {
					obj.transform.SetParent(parent, false);
					obj.transform.SetPositionAndRotation(position, rotation);
					obj.transform.localScale = prefab.transform.localScale;
					obj.SetActive(true);
					return obj;
				}
			}
		}

		GameObject poolObject = GameObject.Instantiate(prefab, position, rotation, parent);
		poolObject.name = poolObject.name.Replace("(Clone)", "");
		
		if (!mCreatedObjects.ContainsKey(id)) {
			mCreatedObjects.Add(id, new Queue<GameObject>());
		}
		
		return poolObject;
	}
}
