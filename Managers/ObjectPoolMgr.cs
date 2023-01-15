using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditorInternal.ReorderableList;

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
		mLoadedObject.TryGetValue(path, out loadedObject);

		if (loadedObject == null) {
			return false;
		}

		return true;
	}

	GameObject LoadObject(string path) {
		GameObject loadedObject;
		mLoadedObject.TryGetValue(path, out loadedObject);

		if (loadedObject == null) {
			loadedObject = Resources.Load<GameObject>(path);

			mLoadedObject.Add(path, loadedObject);
//			Debug.LogWarning("Loaded: " + loadedObject.name);
		}

		return loadedObject;
	}

	public GameObject GetObject(string path, Transform parent) {
		GameObject prefab;
		if (mLoadedObject.ContainsKey(path)) {
			prefab = mLoadedObject[path];
		}
		else {
			prefab = LoadObject(path);
		}

		return GetObject(prefab, parent);
	}

	public void ReuseObject(string path, GameObject createdObj) {
		createdObj.SetActive(false);
		GameObject prefab;
		mLoadedObject.TryGetValue(path, out prefab);

		if (prefab == null)
			return;

		ReuseObject(prefab, createdObj);
	}

	public async void ReuseObject(GameObject prefab, GameObject createdObj) {
		createdObj.SetActive(false);
		Queue<GameObject> createdObjects;
		mCreatedObjects.TryGetValue(prefab.GetInstanceID(), out createdObjects);

		if (createdObjects != null) {
			Task task = Task.Run(() => {
				System.Threading.Thread.Sleep(100);
				createdObjects.Enqueue(createdObj);
			});
			await Task.WhenAll(task);
		}
	}

	public GameObject GetObject(GameObject prefab) {
		return GetObject(prefab, prefab.transform.localPosition, prefab.transform.localRotation, null);
	}

	public GameObject GetObject(GameObject prefab, Transform parent) {
		return GetObject(prefab, parent.position + prefab.transform.localPosition, Quaternion.LookRotation(parent.forward) * Quaternion.LookRotation(prefab.transform.forward), parent);
	}

		// Object already loaded
	public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
		Queue<GameObject> createdObjects;
		int id = prefab.GetInstanceID();
		mCreatedObjects.TryGetValue(id, out createdObjects);

		//		Debug.LogWarning("Loaded: " + mCreatedObjects.Count + " Created: " + objCount);
		
		if (createdObjects != null) {
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
		else {
			createdObjects = new Queue<GameObject>();
			mCreatedObjects.Add(id, createdObjects);
		}

		GameObject poolObject = GameObject.Instantiate(prefab, position, rotation, parent);
		poolObject.name = poolObject.name.Replace("(Clone)", "");
		
		return poolObject;
	}
}
