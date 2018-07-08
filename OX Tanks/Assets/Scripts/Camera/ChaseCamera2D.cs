using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera2D : MonoBehaviour {

	// The target to follow
	[HideInInspector]
	public Transform targetToFollow;

	public Transform fallbackTargetToFollow;

	// The camera following speed
	public float followSpeed = 3f;

	private Camera cam;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake() {
		cam = GetComponent<Camera>();

		if (targetToFollow == null) {
			targetToFollow = fallbackTargetToFollow;
		}
	}

	// LateUpdate is called once per frame after the Update()
	void LateUpdate() {
		if (isActiveAndEnabled) {
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetToFollow.position, followSpeed * Time.deltaTime);
			//Camera.main.transform.LookAt(targetToFollow);
		}
	}
}
