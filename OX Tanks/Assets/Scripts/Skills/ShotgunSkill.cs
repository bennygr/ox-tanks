using System;
using UnityEngine;

public class ShotgunSkill : AbstractSkill {

    private int maxDamage = 30;

    [SerializeField]
    private float maxRange = 20f;

    [SerializeField]
    protected LayerMask playerMask;

    [SerializeField]
    private float bulletSpeed = 60f;

    private PoolManager poolManager;

    private void Awake() {
        poolManager = PoolManager.instance;

        // TODO: Dynamically get the mine prefab.
        // Current work-around: Get it statically from position 5
        // !!! Ensure that the Mine prefab is at position 5 of the PoolInitialiser !!!
        prefab = PoolInitialiser.instance.getManagedPrefab(4);
        cooldown = DEFAULT_COOLDOWN;

        playerMask = LayerMask.GetMask("Player");
    }

    protected override void postStart() {
        // nothing
    }

    void Update() {
        if (Input.GetButtonDown(fireButton) && CanTrigger()) {
            fire();
            Triggered();
        }
    }

    private void fire() {
        foreach (Transform t in skillTransforms) {
            PoolObject poolObject = poolManager.reuseObject(prefab.GetInstanceID(), t.position, t.rotation);
            if (poolObject == null) {
                Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
                return;
            }
            GameObject poolGameObject = poolObject.getGameObject();
            poolGameObject.transform.SetPositionAndRotation(t.position, t.rotation);
            Rigidbody body = poolGameObject.GetComponent<Rigidbody>();
            body.velocity = bulletSpeed * t.up;

            RaycastHit[] hits = Physics.RaycastAll(t.position, t.up, maxRange, playerMask);
            foreach (RaycastHit hit in hits) {
                Collider c = hit.collider;
                Debug.Log(c.name);
                if (c == null) {
                    continue;
                }
                Debug.Log("Distance for hit " + hit.distance + " for collider " + c.name);

                Rigidbody targetRigidbody = c.GetComponent<Rigidbody>();

                if (!targetRigidbody) {
                    continue;
                }
                TankVitals vitals = targetRigidbody.GetComponent<TankVitals>();
                if (!vitals) {
                    continue;
                }
                int damageDealt = CalculateDamage(targetRigidbody.position, hit.distance);
                Debug.LogFormat("Dealt {0} damage to {1}", damageDealt, targetRigidbody.name);
                vitals.takeDamage(damageDealt);
            }
        }
    }

    private int CalculateDamage(Vector3 targetPosition, float distance) {
        return Mathf.RoundToInt(Mathf.Max(0f, (maxDamage - maxDamage * distance / maxRange)));
    }
}
