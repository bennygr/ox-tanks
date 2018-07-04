using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Complete {
	public class NetworkTankHealth : NetworkBehaviour {
		public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
		public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
		public Image m_FillImage;                           // The image component of the slider.
		public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
		public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
		public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
		public NetworkTankManager m_TankManager;
		public GameObject m_TankRenderers;                // References to all the gameobjects that need to be disabled when the tank is dead.
		public GameObject m_HealthCanvas;
		public GameObject m_AimCanvas;
		public GameObject m_LeftDustTrail;
		public GameObject m_RightDustTrail;
		public AudioClip m_TankExplosion;                 // The clip to play when the tank explodes.
		public ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.

		[SyncVar(hook = "OnCurrentHealthChanged")]
		public float m_CurrentHealth;                      // How much health the tank currently has.
		[SyncVar]
		private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
		private BoxCollider m_Collider;                     // Used to deactivate the tank's collider

		private void Awake() {
			m_Collider = GetComponent<BoxCollider>();
		}

		/// <summary>
		/// Setup tank heatlh
		/// </summary>
		public void SetDefaults() {
			m_CurrentHealth = m_StartingHealth;
			m_Dead = false;
			SetTankActive(true);
		}

		private void OnEnable() {
			// When the tank is enabled, reset the tank's health and whether or not it's dead.
			m_CurrentHealth = m_StartingHealth;
			m_Dead = false;
		}


		public void TakeDamage(float amount) {
			// Reduce current health by the amount of damage done.
			m_CurrentHealth -= amount;

			// If the current health is at or below zero and it has not yet been registered, call OnDeath.
			if (m_CurrentHealth <= 0f && !m_Dead) {
				RpcOnDeath();
			}
		}


		private void SetHealthUI() {
			// Set the slider's value appropriately.
			m_Slider.value = m_CurrentHealth;

			// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
			m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
		}


		void OnCurrentHealthChanged(float value) {
			m_CurrentHealth = value;
			SetHealthUI();
		}

		private void OnDeath() {
			// Set the flag so that this function is only called once.
			m_Dead = true;
			RpcOnDeath();
		}


		[ClientRpc]
		void RpcOnDeath() {
			// Play the particle system of the tank exploding.
			m_ExplosionParticles.Play();

            // Create a gameobject that will play the tank explosion sound effect and then destroy itself.
            AudioSource.PlayClipAtPoint(m_TankExplosion, transform.position);

			// Turn the tank off.
			SetTankActive(false);
		}

		private void SetTankActive(bool active) {
			m_Collider.enabled = active;

			//m_TankRenderers.SetActive(active);
			m_HealthCanvas.SetActive(active);
			m_AimCanvas.SetActive(active);
			m_LeftDustTrail.SetActive(active);
			m_RightDustTrail.SetActive(active);

			if (active) {
				m_TankManager.EnableControl();
			} else {
				m_TankManager.DisableControl();
			}
		}
	}
}