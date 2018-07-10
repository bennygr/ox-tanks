﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple chase camera script.
/// </summary>
public class ChaseCamera : MonoBehaviour {

	// The mount point of the camera, i.e. the chase target
	private Transform mountPoint;
	// The target to look at
	private Transform lookAtTarget;

	[SerializeField]
	private Transform fallbackTarget;

	// The camera following speed
	[SerializeField]
	private float followSpeed = 10f;

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
	void FixedUpdate() {
		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, mountPoint.position, followSpeed * Time.deltaTime);
		Camera.main.transform.LookAt(lookAtTarget);
	}

	/// <summary>
	/// Sets the mount point of the camera.
	/// </summary>
	/// <param name="mountPoint">Mount point.</param>
	public void setMountPoint(Transform mountPoint) {
		this.mountPoint = mountPoint;
	}

	/// <summary>
	/// Sets the look at target.
	/// </summary>
	/// <param name="lookAtTarget">Look at target.</param>
	public void setLookAtTarget(Transform lookAtTarget) {
		this.lookAtTarget = lookAtTarget;
	}
}
