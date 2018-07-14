using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to make sure world space UI
// elements such as the health bar face the correct direction.
public class UIDirectionControl : MonoBehaviour {

	[SerializeField]
	private bool useRelativePosition = true;
	[SerializeField]
	private bool useRelativeRotation = true;

	private Vector3 relativePosition;
	private Quaternion relativeRotation;

	private void Start () {
		relativePosition = transform.localPosition;
		relativeRotation = transform.localRotation;
	}

	private void Update () {
		if (useRelativeRotation) {
			transform.rotation = relativeRotation;
		}

		if (useRelativePosition) {
			transform.position = transform.parent.position + relativePosition;
		}
	}
}