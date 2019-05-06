using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankVitals : MonoBehaviour {

    [SerializeField]
    private int health = DefaultVitals.HEALTH;
    [SerializeField]
    private int maxHealth = DefaultVitals.MAX_HEALTH;
    [SerializeField]
    private int armor = DefaultVitals.ARMOR;
    [SerializeField]
    private int maxArmor = DefaultVitals.MAX_ARMOR;
    [SerializeField]
    private int maxDamage = DefaultVitals.MAX_DAMAGE;
    [SerializeField]
    private int damageMultiplier = 1;
    [SerializeField]
    private int damageMultiplierTime = 10;

    private DateTime damageChanged;
    private float damageTime;

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
    private GameObject explosionGameObject;
    private ParticleSystem explosion;

    /// <summary>
    /// Sets the damage multiplier for the specified amount of time (in seconds).
    /// </summary>
    /// <param name="damageMultiplier">Damage multiplier.</param>
    /// <param name="damageMultiplierTime">Damage multiplier seconds.</param>
    public void setDamageMultiplier(int damageMultiplier, int damageMultiplierTime) {
        this.damageChanged = DateTime.Now;
        this.damageTime = damageMultiplierTime;
        this.damageMultiplierTime = damageMultiplierTime;
        this.damageMultiplier = damageMultiplier;
    }

    /// <summary>
    /// Gets the max damage that the current tank can deal to the opponent.
    /// </summary>
    /// <returns>The max damage.</returns>
    public int getMaxDamage() {
        return maxDamage * damageMultiplier;
    }

    private void Awake() {
        updateSliders();
        playerName = gameObject.name;
        poolManager = PoolManager.instance;
        tankMovement = GetComponent<TankMovement>();
        model = transform.Find("Model").gameObject;
        playerInformation = transform.Find("PlayerInformation").gameObject;
    }

    public void Update() {
        // Check if damage multiplier is expired
        if ((DateTime.Now - damageChanged).Seconds > damageTime && damageChanged != DateTime.MinValue) {
            damageMultiplier = 1;
            damageChanged = DateTime.MinValue;
        }
    }

    /// <summary>
    /// Takes the damage.
    /// </summary>
    /// <param name="damageAmount">Damage amount.</param>
	public void takeDamage(int damageAmount) {
        if (damageAmount <= 0 || health <= 0) {
            return;
        }

        if (armor >= damageAmount) {
            armor -= damageAmount;
            Debug.LogFormat("{0} took {1} armor damage", playerName, damageAmount);
            floatingTextControl.spawnDamageShieldFloatingText("-" + damageAmount);
        } else {
            damageAmount -= armor;
            Debug.LogFormat("{0} took {1} armor damage", playerName, armor);
            if (armor > 0) {
                floatingTextControl.spawnDamageShieldFloatingText("-" + damageAmount);
            }
            armor = 0;
            health -= damageAmount;
            Debug.LogFormat("{0} took {1} health damage", playerName, damageAmount);
            floatingTextControl.spawnDamageHPFloatingText("-" + damageAmount);
        }
        updateSliders();

        if (health <= 0) {
            Explode();
        }
    }

    /// <summary>
    /// Heals the hp.
    /// </summary>
    /// <param name="healAmount">Heal amount.</param>
	public void healHP(int healAmount) {
        floatingTextControl.spawnHealHPFloatingText("+" + healAmount);
        if (health == maxHealth) {
            return;
        }
        health += healAmount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        updateSliders();
    }

    /// <summary>
    /// Heals the armor.
    /// </summary>
    /// <param name="healAmount">Heal amount.</param>
	public void healArmor(int healAmount) {
        floatingTextControl.spawnHealShieldFloatingText("+" + healAmount);
        if (armor == maxArmor) {
            return;
        }
        armor += healAmount;
        if (armor > maxArmor) {
            armor = maxArmor;
        }
        updateSliders();
    }

    /// <summary>
    /// Updates the sliders.
    /// </summary>
	private void updateSliders() {
        healthSlider.value = (((float)health / maxHealth)) * 100;
        armorSlider.value = (((float)armor / maxArmor)) * 100;
    }

    /// <summary>
    /// Explode this instance.
    /// </summary>
	private void Explode() {
        //gameObject.SetActive(false);
        PoolObject poolObject = poolManager.reuseObject(tankExplosionPrefab.GetInstanceID(), gameObject.transform.position, Quaternion.identity);
        if (poolObject == null) {
            Debug.LogWarning("Cannot explode! Pool manager does not contain any objects");
            return;
        }
        model.SetActive(false);
        playerInformation.SetActive(false);
        tankMovement.enabled = false;
        explosionGameObject = poolObject.getGameObject();
        explosion = explosionGameObject.GetComponent<ParticleSystem>();
        explosion.Play();
        ParticleSystem.MainModule mainModule = explosion.main;
        StartCoroutine("Deactivate", mainModule.duration); //TODO: Should be part of the pool management framework
    }

    /// <summary>
    /// Deactivates after a specified delay.
    /// </summary>
    /// <returns>The deactivate.</returns>
    /// <param name="delay">Delay.</param>
	IEnumerator Deactivate(float delay) {
        yield return new WaitForSeconds(delay);
        explosion.Stop();
        explosion.Clear();
        explosionGameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}