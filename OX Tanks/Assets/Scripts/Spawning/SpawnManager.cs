using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Keypad1)) {
			Debug.Log("Spawn MW tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad2)) {
			Debug.Log("Spawn QA tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad3)) {
			Debug.Log("Spawn UI tank");
		} else if (Input.GetKeyDown(KeyCode.Keypad4)) {
			Debug.Log("Spawn Finance tank");
		}
	}
}
