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

	private const string PRIMARY_FIRE_BUTTON = "Primary Fire";

	private void Awake() {
		castSlider.minValue = minLaunchForce;
		castSlider.maxValue = maxLaunchForce;

		poolManager = PoolManager.instance;
	}

	private void Start() {
		chargeSpeed = (maxLaunchForce - minLaunchForce) / maxCastTime;
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	private void Update() {
		castSlider.value = minLaunchForce;
		if (currentLaunchForce >= maxLaunchForce && !fired) {
			currentLaunchForce = maxLaunchForce;
			Fire();
		} else if (Input.GetButtonDown(PRIMARY_FIRE_BUTTON)) {
			fired = false;
			currentLaunchForce = minLaunchForce;
		} else if (Input.GetButton(PRIMARY_FIRE_BUTTON) && !fired) {
			currentLaunchForce += chargeSpeed * Time.deltaTime;
			castSlider.value = currentLaunchForce;
		} else if (Input.GetButtonUp(PRIMARY_FIRE_BUTTON) && !fired) {
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
		GameObject gameObject = poolObject.getGameObject();
		gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.up * currentLaunchForce * forceMultiplier;
	}

	public void setFireTransform(Transform fireTransform) {
		this.fireTransform = fireTransform;
	}
}
