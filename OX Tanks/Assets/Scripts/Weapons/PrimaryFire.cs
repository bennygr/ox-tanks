using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimaryFire : MonoBehaviour {

    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private float forceMultiplier = 1f;

    [SerializeField]
    private Slider castSlider;

    private Transform fireTransform;
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
    private TankVitals tankVitals;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip charge;
    [SerializeField]
    private AudioClip fire;

    private const string PRIMARY_FIRE_BUTTON = "Primary Fire Player {0}";

    private void Awake() {
        castSlider.minValue = minLaunchForce;
        castSlider.maxValue = maxLaunchForce;

        poolManager = PoolManager.instance;
        tankVitals = GetComponent<TankVitals>();
    }

    private void Start() {
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxCastTime;
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update() {
        string primaryFireButtonName = string.Format(PRIMARY_FIRE_BUTTON, tankVitals.PlayerNumber);
        castSlider.value = minLaunchForce;
        if (currentLaunchForce >= maxLaunchForce && !fired) {
            currentLaunchForce = maxLaunchForce;
            Fire();
        } else if (Input.GetButtonDown(primaryFireButtonName)) {
            fired = false;
            currentLaunchForce = minLaunchForce;

            audioSource.clip = charge;
            audioSource.Play();
        } else if (Input.GetButton(primaryFireButtonName) && !fired) {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            castSlider.value = currentLaunchForce;
        } else if (Input.GetButtonUp(primaryFireButtonName) && !fired) {
            Fire();
        }
    }

    private void Fire() {
        fired = true;
        PoolObject poolObject = poolManager.reuseObject(shellPrefab.GetInstanceID(), fireTransform.position, fireTransform.rotation);
        if (poolObject == null) {
            Debug.LogWarning("Cannot fire! Pool manager does not contain any objects");
            return;
        }

        audioSource.clip = fire;
        audioSource.Play();

        GameObject poolGameObject = poolObject.getGameObject();
        ShellExplosion shellExplosion = poolGameObject.GetComponent<ShellExplosion>();
        shellExplosion.setMaxDamage(tankVitals.getMaxDamage());
        poolGameObject.GetComponent<Rigidbody>().velocity = poolGameObject.transform.up * currentLaunchForce * forceMultiplier;
    }

    public void setFireTransform(Transform fireTransform) {
        this.fireTransform = fireTransform;
    }
}
