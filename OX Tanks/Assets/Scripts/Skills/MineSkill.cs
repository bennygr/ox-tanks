using System;
using UnityEngine;

public class MineSkill : AbstractSkill {

    [SerializeField]
    private GameObject minePrefab;

    private PoolManager poolManager;

    private void Awake() {
        poolManager = PoolManager.instance;

        // TODO: Dynamically get the mine prefab.
        // Current work-around: Get it statically from position 4
        // !!! Ensure that the Mine prefab is at position 4 of the PoolInitialiser !!!
        minePrefab = PoolInitialiser.instance.getManagedPrefab(3);
        cooldown = DEFAULT_COOLDOWN;
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
        PoolObject poolObject = poolManager.reuseObject(minePrefab.GetInstanceID(), skillTransform.position, skillTransform.rotation);
        if (poolObject == null) {
            Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
            return;
        }
        GameObject poolGameObject = poolObject.getGameObject();
        poolGameObject.transform.SetPositionAndRotation(skillTransform.position, skillTransform.rotation);
    }
}
