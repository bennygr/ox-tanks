using UnityEngine;

public class PoolObject {

    private GameObject gameObject;

    public PoolObject (GameObject gameObject) {
        this.gameObject = gameObject;
        this.gameObject.SetActive (false);
    }

    public void reuse (Vector3 position, Quaternion rotation) {
        gameObject.SetActive (true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }

    public void setParent (Transform parent) {
        this.gameObject.transform.parent = parent;
    }

    public void destroy () {
        this.gameObject.SetActive (false);
    }

    public GameObject getGameObject () {
        return this.gameObject;
    }
}