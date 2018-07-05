using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public abstract class AbstractGameHealth : MonoBehaviour
    {

        public float m_StartingHealth = 100f;
        public float m_StartingShield = 0f;
        public float m_MaxShield = 100f;
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
        private AudioSource m_ExplosionAudio;               // The audio source to play when the object explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the object is destroyed.
        private float m_CurrentHealth;                      // How much health the object currently has.

        private bool m_Dead;

        public float m_CurrentShield { get; protected set; }

        public void Init()
        {
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive(false);
        }

        public void Enable()
        {
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
        }

        public void ApplyDeath()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

            // Turn the tank off.
            gameObject.SetActive(false);
        }

        public void ApplyDamage(float amount)
        {
            float shieldOverflow = 0f;
            if (m_CurrentShield > 0)
            {
                shieldOverflow = m_CurrentShield - amount;
                //Apply shield damage
                m_CurrentShield = Mathf.Max(0, m_CurrentShield - amount);
            }

            if (shieldOverflow < 0)
            {
                //rest damage goes to health
                amount = Mathf.Abs(shieldOverflow);
            }
            else
            {
                // Reduce current health by the amount of damage done.
                m_CurrentHealth -= amount;
            }



        }

        public void CalculateDeath()
        {
            // If the current health is at or below zero and it has not yet been registered, call OnDeath.
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                ApplyDeath();
            }
        }

        public float CurrentHealth
        {
            get
            {
                return m_CurrentHealth;
            }

            set
            {
                m_CurrentHealth = value;
            }
        }

        public bool Dead
        {
            get
            {
                return m_Dead;
            }

            set
            {
                m_Dead = value;
            }
        }

    }
}
