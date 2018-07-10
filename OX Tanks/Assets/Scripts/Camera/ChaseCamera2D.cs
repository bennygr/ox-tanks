using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera2D : MonoBehaviour {

	[SerializeField]
	private Transform fallbackTargetToFollow;

	// The camera following speed
	[SerializeField]
	private float followSpeed = 3f;

	// The target to follow
	private Transform targetToFollow;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake() {
		if (targetToFollow == null) {
			targetToFollow = fallbackTargetToFollow;
		}
	}

	// LateUpdate is called once per frame after the Update()
	void LateUpdate() {
		if (isActiveAndEnabled) {
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetToFollow.position, followSpeed * Time.deltaTime);
		}
	}

	/// <summary>
	/// Sets the target to follow.
	/// </summary>
	/// <param name="targetToFollow">Target to follow.</param>
	public void setTargetToFollow(Transform targetToFollow) {
		this.targetToFollow = targetToFollow;
	}
}
