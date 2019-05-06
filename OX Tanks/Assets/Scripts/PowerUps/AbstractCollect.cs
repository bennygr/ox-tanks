using UnityEngine;

public abstract class AbstractCollect : MonoBehaviour, PowerUpCollectable {

    protected int layer;
    private const float rotationSpeed = 3f;

    private void Awake() {
        layer = LayerMask.NameToLayer("Player");
    }

    private void Update() {
        gameObject.transform.Rotate(Vector3.up * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if (layer != other.gameObject.layer) {
            return;
        }
        TankVitals tankVitals = other.GetComponent<TankVitals>();
        if (tankVitals == null) {
            return;
        }

        applyPowerUp(tankVitals);
        Destroy(gameObject);
    }

    public abstract void applyPowerUp(TankVitals tankVitals);
}