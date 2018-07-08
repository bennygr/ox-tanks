﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour {

	public Text nameLabel;

	// Update is called once per frame
	void Update() {
		nameLabel.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
}
