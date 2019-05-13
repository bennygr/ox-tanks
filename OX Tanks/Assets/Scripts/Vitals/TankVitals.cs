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
    private float damageMultiplierTime = 10;
    private DateTime damageChanged;

    private float speedMultiplierTime = 10;
    private DateTime speedChanged;

    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider armorSlider;
    [SerializeField]
    private FloatingTextControl floatingTextControl;
    [SerializeField]
    private Image speedBuff;
    [SerializeField]
    private Image damageBuff;
    [SerializeField]
    private Image skillCooldown;
    [SerializeField]
    private GameObject tankExplosionPrefab;
    private int playerNumber;
    [SerializeField]
    private string playerName;
    private PoolManager poolManager;
    private TankMovement tankMovement;
    private GameObject model;
    private GameObject playerInformation;
    private GameObject explosionGameObject;
    private ParticleSystem explosion;

    /// <summary>
    /// The number of the player
    /// </summary>
    /// <value>The player number.</value>
    public int PlayerNumber {
        get {
            return playerNumber;
        }
        set {
            playerNumber = value;
        }
    }

    /// <summary>
    ///	    The player's name
    /// </summary>
    public String PlayerName {
        get {
            return playerName;
        }
        set {
            playerName = value;
        }
    }

    /// <summary>
    /// Gets or sets the skill cooldown.
    /// </summary>
    /// <value>The skill cooldown.</value>
    public Image SkillCooldown {
        get {
            return skillCooldown;
        }
    }


    /// <summary>
    /// Sets the speed multiplier.
    /// </summary>
    /// <param name="speedMultiplier">Speed multiplier.</param>
    /// <param name="speedMultiplierTime">Speed multiplier time.</param>
    public void setSpeedMultiplier(int speedMultiplier, int speedMultiplierTime) {
        floatingTextControl.spawnSpeedMultiplierFloatingText("Speed x2");
        this.speedChanged = DateTime.Now;
        this.speedMultiplierTime = speedMultiplierTime;
        tankMovement.setSpeedMultiplier(speedMultiplier);
        speedBuff.enabled = true;
    }

    /// <summary>
    /// Sets the damage multiplier for the specified amount of time (in seconds).
    /// </summary>
    /// <param name="damageMultiplier">Damage multiplier.</param>
    /// <param name="damageMultiplierTime">Damage multiplier seconds.</param>
    public void setDamageMultiplier(int damageMultiplier, int damageMultiplierTime) {
        floatingTextControl.spawnDamageMultiplierFloatingText("Damage x2");
        this.damageChanged = DateTime.Now;
        this.damageMultiplierTime = damageMultiplierTime;
        this.damageMultiplier = damageMultiplier;
        damageBuff.enabled = true;
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
        poolManager = PoolManager.instance;
        tankMovement = GetComponent<TankMovement>();
        model = transform.Find("Model").gameObject;
        playerInformation = transform.Find("PlayerInformation").gameObject;
    }

    public void Update() {
        // Check if damage multiplier is expired
        if ((DateTime.Now - damageChanged).Seconds > damageMultiplierTime && damageChanged != DateTime.MinValue) {
            damageMultiplier = 1;
            damageChanged = DateTime.MinValue;
            damageBuff.enabled = false;
        }
        // Check if speed multiplier is expired
        if ((DateTime.Now - speedChanged).Seconds > speedMultiplierTime && speedChanged != DateTime.MinValue) {
            tankMovement.setSpeedMultiplier(1);
            speedChanged = DateTime.MinValue;
            speedBuff.enabled = false;
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

        //Dead?
        if (health <= 0) {
            Explode();
            RoundManager.instance.RemovePlayer(playerNumber);
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
        GetComponent<BoxCollider>().enabled = false;
        tankMovement.AudioSource.clip = null;
        model.SetActive(false);
        playerInformation.SetActive(false);
        tankMovement.enabled = false;
        explosionGameObject = poolObject.getGameObject();
        explosion = explosionGameObject.GetComponent<ParticleSystem>();
        explosion.Play();
        ParticleSystem.MainModule mainModule = explosion.main;
        StartCoroutine(Deactivate(mainModule.duration)); //TODO: Should be part of the pool management framework
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
