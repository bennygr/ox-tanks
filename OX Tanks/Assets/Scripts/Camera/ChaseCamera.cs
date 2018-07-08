using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple chase camera script.
/// </summary>
public class ChaseCamera : MonoBehaviour {

	// The mount point of the camera, i.e. the chase target
	[HideInInspector]
	public Transform mountPoint;
	// The target to look at
	[HideInInspector]
	public Transform lookAtTarget;

	public Transform fallbackTarget;

	// The camera following speed
	public float followSpeed = 3f;

	private void Awake() {
		if (mountPoint == null) {
			//TODO: Get fallback mount point from the level
			mountPoint = fallbackTarget;
		}
		if (lookAtTarget == null) {
			//TODO: Get fallback look at target from the level
			lookAtTarget = fallbackTarget;
		}
	}

	// LateUpdate is called once per frame after the Update()
	void LateUpdate() {
		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, mountPoint.position, followSpeed * Time.deltaTime);
		Camera.main.transform.LookAt(lookAtTarget);
	}
}
