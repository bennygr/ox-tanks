using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Complete {
	public class NetworkShellExplosion : NetworkBehaviour {
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
        public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
        public float m_DamageMultiplier = 1f;               // Increases the default damage by a factor        

		private int m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".

		private void Start() {
			if (isServer) {
				// If it isn't destroyed by then, destroy the shell after it's lifetime.
				Destroy(gameObject, m_MaxLifeTime);
				GetComponent<Collider>().enabled = false;
				StartCoroutine(EnableCollision());
			}

			// Set the value of the layer mask based solely on the Players layer.
			m_TankMask = LayerMask.GetMask("Players");
		}

        /// <summary>
		/// Delays collision detection a bit to avoid any accidental collisions on start-up
        /// </summary>
        /// <returns>The collision.</returns>
		IEnumerator EnableCollision() {
			yield return new WaitForSeconds(0.1f);
			GetComponent<Collider>().enabled = true;
		}

		/// <summary>
		/// Handle collision triggers only by the server
		/// </summary>
		/// <param name="other">Other.</param>
		[ServerCallback]
		private void OnTriggerEnter(Collider other) {
            // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++) {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody) {
                    continue;
                }

                // Find the TankHealth script associated with the rigidbody.
                NetworkTankHealth targetHealth = targetRigidbody.GetComponent<NetworkTankHealth>();

                // If there is no TankHealth script attached to the gameobject, go on to the next collider.
                if (!targetHealth) {
                    continue;
                }

                // Calculate the amount of damage the target should take based on it's distance from the shell.
                float damage = CalculateDamage(targetRigidbody.position);

                // Deal this damage to the tank.
                targetHealth.TakeDamage(damage);
            }

            if (!NetworkClient.active) {//if we are ALSO client (so hosting), this will be done by the Destroy so Skip
                AddExplosionForce();
            }

            // Destroy the shell on clients as well.
            NetworkServer.Destroy(gameObject);
        }

        /// <summary>
        /// Notifies the clients (called on client side) that the shell was destroyed on the server
        /// </summary>
        public override void OnNetworkDestroy() {
			KaBoom();
			// Once the particles have finished, destroy the gameobject they are on.
			ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
			Destroy(m_ExplosionParticles.gameObject, mainModule.duration);
			base.OnNetworkDestroy();
		}

		/// <summary>
		/// !!!!KA-BOOM!!!!
		/// </summary>
		void KaBoom() {
			// Unparent the particles from the shell.
			m_ExplosionParticles.transform.parent = null;

			// Play the particle system.
			m_ExplosionParticles.Play();

			// Play the explosion sound effect.
			if (m_ExplosionAudio) {
				m_ExplosionAudio.Play();
			}

			AddExplosionForce();
		}

		/// <summary>
		/// Adds the explosion force to all the colliders
		/// </summary>
		void AddExplosionForce() {
			Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
			// Go through all the colliders...
			for (int i = 0; i < colliders.Length; i++) {
				// ... and find their rigidbody.
				Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

				// If they don't have a rigidbody or we don't own that object, go on to the next collider.
                if (!targetRigidbody || (targetRigidbody.GetComponent<NetworkIdentity>() != null && !targetRigidbody.GetComponent<NetworkIdentity>().hasAuthority)) {
					continue;
				}

				// Add the explosion force.
				targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
			}
		}

		/// <summary>
		/// Calculates the damage
		/// </summary>
		/// <returns>The damage.</returns>
		/// <param name="targetPosition">Target position.</param>
		private float CalculateDamage(Vector3 targetPosition) {
			// Create a vector from the shell to the target.
			Vector3 explosionToTarget = targetPosition - transform.position;

			// Calculate the distance from the shell to the target.
			float explosionDistance = explosionToTarget.magnitude;

			// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
			float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

			// Calculate damage as this proportion of the maximum possible damage.
			float damage = relativeDistance * m_MaxDamage;

			// Make sure that the minimum damage is always 0.
			return Mathf.Max(0f, damage);
		}
	}
}