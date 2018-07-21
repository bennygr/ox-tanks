﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankVitals : MonoBehaviour {

	[SerializeField]
	private float health = 100f;
	[SerializeField]
	private float maxHealth = 100f;
	[SerializeField]
	private float armor = 0f;
	[SerializeField]
	private float maxArmor = 100f;
	[SerializeField]
	private Slider healthSlider;
	[SerializeField]
	private Slider armorSlider;
	[SerializeField]
	private FloatingTextControl floatingTextControl;

	private string playerName;

	private void Awake () {
		updateSliders ();
		playerName = gameObject.name;
	}

	public void takeDamage (float damageAmount) {
		if (damageAmount <= 0) {
			return;
		}

		if (armor >= damageAmount) {
			armor -= damageAmount;
			Debug.LogFormat ("{0} took {1} armor damage", playerName, damageAmount);
			floatingTextControl.spawnDamageShieldFloatingText ("-" + damageAmount);
		} else {
			damageAmount -= armor;
			Debug.LogFormat ("{0} took {1} armor damage", playerName, armor);
			if (armor > 0) {
				floatingTextControl.spawnDamageShieldFloatingText ("-" + damageAmount);
			}
			armor = 0;
			health -= damageAmount;
			Debug.LogFormat ("{0} took {1} health damage", playerName, damageAmount);
			floatingTextControl.spawnDamageHPFloatingText ("-" + damageAmount);
		}
		updateSliders ();
	}

	public void healHP (float healAmount) {
		floatingTextControl.spawnHealHPFloatingText ("+" + healAmount);
		if (health == maxHealth) {
			return;
		}
		health += healAmount;
		if (health > maxHealth) {
			health = maxHealth;
		}
		updateSliders ();
	}

	public void healArmor (float healAmount) {
		floatingTextControl.spawnHealShieldFloatingText ("+" + healAmount);
		if (armor == maxArmor) {
			return;
		}
		armor += healAmount;
		if (armor > maxArmor) {
			armor = maxArmor;
		}
		updateSliders ();
	}

	private void updateSliders () {
		healthSlider.value = (health / maxHealth) * 100;
		armorSlider.value = (armor / maxArmor) * 100;
	}
}