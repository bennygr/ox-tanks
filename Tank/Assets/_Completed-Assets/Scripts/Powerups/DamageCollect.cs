using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

namespace Complete
{

    public class DamageCollect : AbstractPowerUp
    {
        public Rigidbody m_DamageSign;
        public float damageFactor = 2f;
        public float time = 10f;

        private void OnTriggerEnter(Collider other)
        {
            var shooting = other.GetComponent<TankShooting>();
            if (shooting != null)
            {
                shooting.IncreaseDamage(damageFactor, time, m_DamageSign);
            }

            PowerUpManager.CleanSpawningPoint(gameObject);
            Destroy(gameObject);
        }
    }
}