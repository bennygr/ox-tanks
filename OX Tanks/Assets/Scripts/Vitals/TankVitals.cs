using System.Collections;
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

	private void Awake () {
		updateSliders ();
	}

	public void takeDamage (float damageAmount) {
		if (damageAmount <= 0) {
			return;
		}

		if (armor >= damageAmount) {
			armor -= damageAmount;
		} else {
			damageAmount -= armor;
			armor = 0;
			health -= damageAmount;
		}
		updateSliders ();
	}

	private void updateSliders () {
		healthSlider.value = (health / maxHealth) * 100;
		armorSlider.value = (armor / maxArmor) * 100;
	}
}