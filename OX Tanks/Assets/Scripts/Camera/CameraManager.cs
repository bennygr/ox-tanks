using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Camera manager - Allows to switch between camera setups.
/// </summary>
public class CameraManager : MonoBehaviour {

	// The different available camera states
	private enum CameraState { Chase3D, Chase2D, TopDown };

	// The default camera state
	[SerializeField]
	private CameraState cameraState = CameraState.Chase2D;

	[SerializeField]
	private GameObject chase3DCamera;

	[SerializeField]
	private GameObject chase2DCamera;

	[SerializeField]
	private GameObject topDownCamera;

	private int amountOfCameraStates;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		amountOfCameraStates = Enum.GetValues(typeof(CameraState)).Length;
		SetActiveCamera();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.V)) {
			CycleCameraView();
		}
	}

	/// <summary>
	/// Cycles the camera states/views.
	/// </summary>
	private void CycleCameraView() {
		int currentCameraState = (int)cameraState;
		currentCameraState++;
		if (currentCameraState >= amountOfCameraStates) {
			currentCameraState = 0;
		}
		cameraState = (CameraState)currentCameraState;
		SetActiveCamera();
	}

	/// <summary>
	/// Sets the active camera.
	/// </summary>
	private void SetActiveCamera() {
		switch (cameraState) {
			case CameraState.Chase3D:
				chase2DCamera.SetActive(false);
				topDownCamera.SetActive(false);
				chase3DCamera.SetActive(true);
				break;
			case CameraState.Chase2D:
				topDownCamera.SetActive(false);
				chase3DCamera.SetActive(false);
				chase2DCamera.SetActive(true);
				break;
			case CameraState.TopDown:
				chase2DCamera.SetActive(false);
				chase3DCamera.SetActive(false);
				topDownCamera.SetActive(true);
				break;
		}
	}
}
