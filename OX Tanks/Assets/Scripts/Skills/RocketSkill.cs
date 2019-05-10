using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rocket skill.
/// </summary>
//TODO: Consolidate with PrimaryFire, maybe abstract common logic to a generic class
public class RocketSkill : AbstractSkill {

    [SerializeField]
    private GameObject rocketPrefab;

    [SerializeField]
    private float forceMultiplier = 5f;

    //TODO: Maybe introduce a second slider for the skill?
    //[SerializeField]
    //private Slider castSlider;

    private float force;
    private bool fired;

    [SerializeField]
    private float maxCastTime = 0.75f;

    [SerializeField]
    private float minLaunchForce = 8f;
    [SerializeField]
    private float maxLaunchForce = 16f;
    private float chargeSpeed;
    private float currentLaunchForce;

    private PoolManager poolManager;
    //private TankVitals tankVitals;

    //private const string PRIMARY_FIRE_BUTTON = "Primary Fire";

    private void Awake() {
        //castSlider.minValue = minLaunchForce;
        //castSlider.maxValue = maxLaunchForce;
        poolManager = PoolManager.instance;
        //tankVitals = GetComponent<TankVitals>();

        // TODO: Dynamically get the rocket prefab.
        // Current work-around: Get it statically from position 3
        // !!! Ensure that the Rocket prefab is at position 3 of the PoolInitialiser !!!
        rocketPrefab = PoolInitialiser.instance.getManagedPrefab(2);
        cooldown = DEFAULT_COOLDOWN;
    }

    protected override void postStart() {
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxCastTime;
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update() {
        //castSlider.value = minLaunchForce;
        if (currentLaunchForce >= maxLaunchForce && !fired && CanTrigger()) {
            currentLaunchForce = maxLaunchForce;
            Fire();
        } else if (Input.GetButtonDown(fireButton) && CanTrigger()) {
            fired = false;
            currentLaunchForce = minLaunchForce;
        } else if (Input.GetButton(fireButton) && !fired && CanTrigger()) {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            //castSlider.value = currentLaunchForce;
        } else if (Input.GetButtonUp(fireButton) && !fired && CanTrigger()) {
            Fire();
        }
    }

    private void Fire() {
        Triggered();
        fired = true;
        PoolObject poolObject = poolManager.reuseObject(rocketPrefab.GetInstanceID(), skillTransform.position, skillTransform.rotation);
        if (poolObject == null) {
            Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
            return;
        }
        GameObject poolGameObject = poolObject.getGameObject();
        RocketExplosion rocketExplosion = poolGameObject.GetComponent<RocketExplosion>();
        rocketExplosion.setMaxDamage(MAX_DAMAGE);
        poolGameObject.GetComponent<Rigidbody>().velocity = poolGameObject.transform.up * currentLaunchForce * forceMultiplier;
    }
}
