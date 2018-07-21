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
	[SerializeField]
	private FloatingTextControl floatingTextControl;
	[SerializeField]
	private GameObject tankExplosionPrefab;
	private GameObject tankExplosion;
	private ParticleSystem tankExplosionParticleSystem;

	private string playerName;

	private void Awake () {
		updateSliders ();
		playerName = gameObject.name;
		tankExplosion = Instantiate(tankExplosionPrefab);
		tankExplosion.SetActive(false);
		tankExplosionParticleSystem = tankExplosion.GetComponent<ParticleSystem>();
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

		if (health <= 0) {
			Explode ();
		}
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

	private void Explode () {
		gameObject.SetActive(false);
		tankExplosion.SetActive(true);
		tankExplosion.transform.position = gameObject.transform.position;
		tankExplosionParticleSystem.Play();
		ParticleSystem.MainModule mainModule = tankExplosionParticleSystem.main;
		StartCoroutine ("Deactivate", mainModule.duration); //TODO: Should be part of the pool management framework
	}

	IEnumerator Deactivate (float delay) {
		yield return new WaitForSeconds (delay);
		tankExplosionParticleSystem.Stop ();
		tankExplosionParticleSystem.Clear ();
		tankExplosion.SetActive(false);
	}
}