using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

	[SerializeField]
	private List<GameObject> tankPrefabs = new List<GameObject>(4);

	[SerializeField]
	private GameObject playerRigPrefab;

	private ChaseCamera playerCamera;
	private ChaseCamera2D playerCamera2D;
	private Camera deathCamera;

	private void Awake() {
		GameObject mainCam = GameObject.Find("Main Camera").gameObject;
		playerCamera = mainCam.GetComponent<ChaseCamera>();

		GameObject playerCam = GameObject.Find("CameraRig2D").gameObject;
		playerCamera2D = playerCam.GetComponent<ChaseCamera2D>();

		deathCamera = GameObject.Find("TopDownCamera").gameObject.GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Keypad1)) {
			Debug.Log("Spawn MW tank");
			initialiseTankPrefab(Instantiate(tankPrefabs[0]), "MW Tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad2)) {
			Debug.Log("Spawn QA tank");
			initialiseTankPrefab(Instantiate(tankPrefabs[1]), "QA Tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad3)) {
			Debug.Log("Spawn UI tank");
			initialiseTankPrefab(Instantiate(tankPrefabs[2]), "UI Tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad4)) {
			Debug.Log("Spawn Finance tank");
			initialiseTankPrefab(Instantiate(tankPrefabs[3]), "Finance Tank");
		}
	}

	/// <summary>
	/// Initialises the tank prefab and makes all necessary connections.
	/// </summary>
	/// <param name="tankPrefab">Tank prefab.</param>
	/// <param name="playerName">Player name.</param>
	private void initialiseTankPrefab(GameObject tankPrefab, string playerName) {
		GameObject playerRig = Instantiate(playerRigPrefab);
		playerRig.name = playerName;

		GameObject modelNode = playerRig.transform.Find("Model").gameObject;
		tankPrefab.transform.parent = modelNode.transform;

		GameObject nameTagNode = playerRig.transform.Find("NameTag").gameObject;
		NameTag nameTag = nameTagNode.GetComponent<NameTag>();
		nameTag.setNameLabel(playerName);

		GameObject cameraRig = playerRig.transform.Find("CameraRig").gameObject;
		playerCamera.setMountPoint(cameraRig.transform.Find("CameraMount").gameObject.transform);
		playerCamera.setLookAtTarget(cameraRig.transform.Find("CameraLookAtTarget").gameObject.transform);

		playerCamera2D.setTargetToFollow(playerRig.transform);
	}
}
