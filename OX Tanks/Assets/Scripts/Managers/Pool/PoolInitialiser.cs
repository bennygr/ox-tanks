using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolInitialiser : MonoBehaviour {

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
}