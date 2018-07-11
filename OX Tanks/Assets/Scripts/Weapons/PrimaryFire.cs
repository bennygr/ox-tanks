using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryFire : MonoBehaviour {

	[SerializeField]
	private GameObject shellPrefab;

	private Transform fireTransform;
	private float force;
	private bool fired;

	[SerializeField]
	private float forceMultiplier = 1f;

	public float maxCastTime = 0.75f;
	public float minLaunchForce = 8f;
	public float maxLaunchForce = 16f;
	public float chargeSpeed;
	private float currentLaunchForce;

	private const string PRIMARY_FIRE_BUTTON = "Primary Fire";

	private void Start() {
		chargeSpeed = (maxLaunchForce - minLaunchForce) / maxCastTime;
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	private void Update() {
		if (currentLaunchForce >= maxLaunchForce && !fired) {
			currentLaunchForce = maxLaunchForce;
			Fire();
		} else if (Input.GetButtonDown(PRIMARY_FIRE_BUTTON)) {
			fired = false;
			currentLaunchForce = minLaunchForce;
		} else if (Input.GetButton(PRIMARY_FIRE_BUTTON) && !fired) {
			currentLaunchForce += chargeSpeed * Time.deltaTime;
		} else if (Input.GetButtonUp(PRIMARY_FIRE_BUTTON) && !fired) {
			Fire();
		}
	}

	private void Fire() {
		fired = true;
		GameObject shellGameObject = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);
		shellGameObject.GetComponent<Rigidbody>().velocity = shellGameObject.transform.up * currentLaunchForce * forceMultiplier;
		Destroy(shellGameObject, 2f);
	}

	public void setFireTransform(Transform fireTransform) {
		this.fireTransform = fireTransform;
	}
}
