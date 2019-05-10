using UnityEngine;
using System.Collections;

public abstract class AbstractCollect : MonoBehaviour, PowerUpCollectable {

    protected int layer;
    private const float rotationSpeed = 3f;
    [SerializeField]
    private AudioSource audioSource;

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

        audioSource.Play();
        gameObject.transform.Find("Model").gameObject.SetActive(false);
        StartCoroutine("PlayEffect", 2);
        applyPowerUp(tankVitals);
    }

    public abstract void applyPowerUp(TankVitals tankVitals);

    IEnumerator PlayEffect(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        // FIXME: Dirty hack. Introduce an event manager to distribute events to registered listeners.
        gameObject.transform.parent.GetComponent<PowerUpSpawnPoint>().HasPowerUp = false;
    }
}