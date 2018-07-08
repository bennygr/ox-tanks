using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple chase camera script.
/// </summary>
public class ChaseCamera : MonoBehaviour {

	// The mount point of the camera, i.e. the chase target
	public Transform mountPoint;
	// The target to look at
	public Transform lookAtTarget;

	// The camera following speed
	public float followSpeed = 3f;

	// Update is called once per frame
	void Update() {
		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, mountPoint.position, followSpeed * Time.deltaTime);
		Camera.main.transform.LookAt(lookAtTarget);
	}
}
