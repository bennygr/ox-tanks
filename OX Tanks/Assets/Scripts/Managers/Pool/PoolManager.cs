using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    static PoolManager _instance;

    public static PoolManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    private Dictionary<int, Queue<PoolObject>> pools = new Dictionary<int, Queue<PoolObject>>();

    /// <summary>
    /// Creates a new pool with the specified size for the specified prefab.
    /// </summary>
    public void addPool(GameObject prefab, int size) {
        int poolId = prefab.GetInstanceID();
        if (pools.ContainsKey(poolId)) {
            Debug.LogWarningFormat("There is already a pool with id {0}.", poolId);
            return;
        }

        GameObject poolGameObject = new GameObject(prefab.name + "Pool");
        poolGameObject.transform.parent = transform;

        Queue<PoolObject> queue = new Queue<PoolObject>(size);
        for (int i = 0; i < size; i++) {
            GameObject instance = Instantiate(prefab);
            instance.transform.parent = poolGameObject.transform;
            queue.Enqueue(new PoolObject(instance));
        }
        pools.Add(poolId, queue);
    }

    public PoolObject reuseObject(int poolId, Transform parent, Vector3 position, Quaternion rotation) {
        return reuseObjectFromPool(poolId, parent, position, rotation);
    }

    public PoolObject reuseObject(int poolId, Vector3 position, Quaternion rotation) {
        return reuseObjectFromPool(poolId, null, position, rotation);
    }

    private PoolObject reuseObjectFromPool(int poolId, Transform parent, Vector3 position, Quaternion rotation) {
        if (!pools.ContainsKey(poolId)) {
            Debug.LogFormat("No pool found with id {0}.", poolId);
            return null;
        }

        PoolObject pooledObject = pools[poolId].Dequeue();
        pooledObject.reuse(position, rotation);
        if (parent != null) {
            pooledObject.setParent(parent);
        }
        pools[poolId].Enqueue(pooledObject);
        return pooledObject;
    }
}