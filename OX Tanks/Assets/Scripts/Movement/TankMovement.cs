using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple tank movement script to controll the tank. It should
/// be applied to the <code>PlayerRig</code> prefab.
/// </summary>
public class TankMovement : MonoBehaviour {

	// Initial moving speed
	public float movingSpeed = 5f;
	// Initial turning speed
	public float turningSpeed = 20f;
	// Initial speed multiplier (in case a speed buff is applied)
	public float speedMultiplier = 1f;

	// The rigid body of the tank
	private Rigidbody rigidBody;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake() {
		rigidBody = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Since movement is applied at the rigid body with the physics engine,
	/// the calculates must happen in fixed update.
	/// </summary>
	void FixedUpdate() {
		Move();
		Turn();
	}


	/// <summary>
	/// Moves the tank to the new position dictated by the
	/// amount of movement registered from the vertical input axes
	/// of the controller (e.g. Keyboard: up/down)
	/// </summary>
	private void Move() {
		float vertical = Input.GetAxis("Vertical");
		Vector3 movement = transform.forward * vertical * movingSpeed * speedMultiplier * Time.deltaTime;
		rigidBody.MovePosition(rigidBody.position + movement);
	}

	/// <summary>
	/// Turns/rotates the tank to a new angle dicated by the
	/// amount of the rotation registered from the horizontal input axes
	/// of the controller. (e.g. Keyboard: left/right)
	/// </summary>
	private void Turn() {
		float horizontal = Input.GetAxis("Horizontal");
		float turn = horizontal * turningSpeed * speedMultiplier * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
		rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
	}
}
