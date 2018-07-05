using UnityEngine;
using UnityEngine.Networking;

namespace Complete {
    public class NetworkMineExplosion : NetworkBehaviour {
        public LayerMask m_TankMask;
        public ParticleSystem m_ExplosionParticles;
        public AudioSource m_ExplosionAudio;
        public float m_MaxDamage = 100f; //Max damage on perfect hit
        public float m_ExplosionForce = 1000f; //
                                               //public float m_MaxLifeTime = 2f; //Lifetime of flying fists in seconds
        public float m_ExplosionRadius = 5f; //
        private bool active = false;
        private float activeAfter = 1f; //Activate the mine after n seconds
        private float timeExists; //The time in seconds the mine exists


        private void Start() {
            //Destroy(gameObject, m_MaxLifeTime);
        }

        private void Update() {
            timeExists += Time.deltaTime;
            active = timeExists > activeAfter;
        }


        [ServerCallback]
        private void OnTriggerEnter(Collider other) {
            if (!active) {
                return;
            }
            // Find all the tanks in an area around the fist and damage them.

            //Get all coliders in the radius of the exploding fist
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            foreach (var c in colliders) {
                Rigidbody targetRigidBody = c.GetComponent<Rigidbody>();
                if (!targetRigidBody) {
                    continue;
                }

                // Find the TankHealth script associated with the rigidbody.
                NetworkTankHealth targetHealth = targetRigidBody.GetComponent<NetworkTankHealth>();
                if (!targetHealth) {
                    continue;
                }

                float damage = CalculateDamage(targetRigidBody.position);
                targetHealth.TakeDamage(damage);
            }

            if (!NetworkClient.active) {
                AddExplosionForce();
            }

            NetworkServer.Destroy(gameObject);
        }

        /// <summary>
        /// Notifies the clients (called on client side) that the mine was destroyed on the server
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
                if (!targetRigidbody || !targetRigidbody.GetComponent<NetworkIdentity>().hasAuthority) {
                    continue;
                }

                // Add the explosion force.
                targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
            }
        }


        private float CalculateDamage(Vector3 targetPosition) {
            // Calculate the amount of damage a target should take based on it's position.
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
            return Mathf.Max(0f, relativeDistance * m_MaxDamage);
        }
    }
}