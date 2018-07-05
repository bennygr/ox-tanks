using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour {

	private Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.

	Vector3 offset = new Vector3(-60,60,10); // The initial offset from the target.

	void Update() {
		if (target == null) {
			return;
		}

        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position;// - offset;
		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

	}

	public void SetTarget(Transform target) {
		this.target = target;
	}
}
