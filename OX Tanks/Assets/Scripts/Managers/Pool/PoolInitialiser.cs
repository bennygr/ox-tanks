using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolInitialiser : MonoBehaviour {

    static PoolInitialiser _instance;

    public static PoolInitialiser instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<PoolInitialiser>();
            }
            return _instance;
        }
    }

    [SerializeField]
	private List<GameObject> managedPrefabs;

	/// Testing the pool manager initialisation
	private void Awake () {
		PoolManager poolManager = PoolManager.instance;
		foreach (GameObject prefab in managedPrefabs) {
			//FIXME: instantiate dynamically on startup the needed pools
			poolManager.addPool (prefab, 50);
		}
	}

    public GameObject getManagedPrefab(int prefabNum) {
        if (managedPrefabs.Count <= prefabNum) {
            Debug.Log("Unknown managed prefab with number " + prefabNum);
            return null;
        }
        return managedPrefabs[prefabNum];
    }
}