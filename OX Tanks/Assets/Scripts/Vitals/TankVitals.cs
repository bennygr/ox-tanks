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
	private string playerName;
	private PoolManager poolManager;
	private TankMovement tankMovement;
	private GameObject model;
	private GameObject playerInformation;

	private void Awake () {
		updateSliders ();
		playerName = gameObject.name;
		poolManager = PoolManager.instance;
		tankMovement = GetComponent<TankMovement>();
		model = transform.Find("Model").gameObject;
		playerInformation = transform.Find("PlayerInformation").gameObject;
	}

	public void takeDamage (float damageAmount) {
		if (damageAmount <= 0 || health <= 0) {
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
	private GameObject explosionGameObject;
	private ParticleSystem explosion;

	private void Explode () {
		//gameObject.SetActive(false);
		PoolObject poolObject = poolManager.reuseObject (tankExplosionPrefab.GetInstanceID (), gameObject.transform.position, Quaternion.identity);
		if (poolObject == null) {
			Debug.LogWarning ("Cannot explode! Pool manager does not contain any objects");
			return;
		}
		model.SetActive(false);
		playerInformation.SetActive(false);
		tankMovement.enabled = false;
		explosionGameObject = poolObject.getGameObject();
		explosion = explosionGameObject.GetComponent<ParticleSystem>();
		explosion.Play();
		ParticleSystem.MainModule mainModule = explosion.main;
		StartCoroutine ("Deactivate", mainModule.duration); //TODO: Should be part of the pool management framework
	}

	IEnumerator Deactivate (float delay) {
		yield return new WaitForSeconds (delay);
		explosion.Stop ();
		explosion.Clear ();
		explosionGameObject.SetActive (false);
		gameObject.SetActive(false);
	}
}