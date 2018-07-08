using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera2D : MonoBehaviour {

	// The target to follow
	public Transform targetToFollow;

	// The camera following speed
	public float followSpeed = 3f;

	private Camera cam;

	private void Awake() {
		cam = GetComponent<Camera>();
	}

	// LateUpdate is called once per frame after the Update()
	void LateUpdate() {
		if (isActiveAndEnabled) {
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetToFollow.position, followSpeed * Time.deltaTime);
			//Camera.main.transform.LookAt(targetToFollow);
		}
	}

	public void SetActive(bool active) {
		cam.enabled = active;
	}
}
