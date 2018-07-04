using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour {

	public Transform lookAt;
	public Transform camTransform;

	private Camera cam;

	private float distance = 10.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensitivityX = 4.0f;
	private float sensitivityY = 1.0f;

    /// <summary>
    /// Initialise the camera's transform and pick up the main camera
    /// </summary>
	private void Start() {
		camTransform = transform;
		cam = Camera.main;
	}

    /// <summary>
    /// Positions the camera behind the player.
    /// </summary>
	private void LateUpdate() {
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		camTransform.position = lookAt.position + rotation * direction;
	}

}
