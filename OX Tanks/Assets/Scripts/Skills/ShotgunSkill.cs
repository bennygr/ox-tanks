using System;
using UnityEngine;

public class ShotgunSkill : AbstractSkill {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed = 20f;

    private PoolManager poolManager;

    private void Awake() {
        poolManager = PoolManager.instance;

        // TODO: Dynamically get the mine prefab.
        // Current work-around: Get it statically from position 5
        // !!! Ensure that the Mine prefab is at position 5 of the PoolInitialiser !!!
        bulletPrefab = PoolInitialiser.instance.getManagedPrefab(4);
        cooldown = DEFAULT_COOLDOWN;

        //TMP
        cooldown = 0f;
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
            PoolObject poolObject = poolManager.reuseObject(bulletPrefab.GetInstanceID(), t.position, t.rotation);
            if (poolObject == null) {
                Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
                return;
            }
            GameObject poolGameObject = poolObject.getGameObject();
            BulletExplosion explosion = poolGameObject.GetComponent<BulletExplosion>();
            explosion.setMaxDamage(30); //TODO: adjust accordingly
            poolGameObject.transform.SetPositionAndRotation(t.position, t.rotation);
            Rigidbody body = poolGameObject.GetComponent<Rigidbody>();
            body.velocity = bulletSpeed * t.up;
        }
    }
}
