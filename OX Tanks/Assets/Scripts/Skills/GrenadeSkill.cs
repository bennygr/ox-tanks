using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Grenade skill.
/// </summary>
public class GrenadeSkill : AbstractSkill {

    [SerializeField]
    private GameObject prefab;
    private PoolManager poolManager;

    private void Awake() {
        poolManager = PoolManager.instance;

        // TODO: Dynamically get the rocket prefab.
        // Current work-around: Get it statically from position 6
        // !!! Ensure that the Grenade prefab is at position 6 of the PoolInitialiser !!!
        prefab = PoolInitialiser.instance.getManagedPrefab(5);
        cooldown = DEFAULT_COOLDOWN;
    }

    protected override void postStart() {
        // nothing
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update() {
        if (Input.GetButtonDown(fireButton) && CanTrigger()) {
            fire();
            Triggered();
        }
    }

    private void fire() {
        Triggered();
        foreach (Transform t in skillTransforms) {
            PoolObject poolObject = poolManager.reuseObject(prefab.GetInstanceID(), t.position, t.rotation);
            if (poolObject == null) {
                Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
                return;
            }
            GameObject poolGameObject = poolObject.getGameObject();
            GrenadeExplosion explosion = poolGameObject.GetComponent<GrenadeExplosion>();
            explosion.setMaxDamage(10); //TODO: adjust accordingly
        }
    }
}
