using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

namespace Complete
{
    public class ShieldCollect : AbstractPowerUp
    {
        public float increaseShield = 25f;

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<TankHealth>();
            if (health != null)
            {
                health.AddShield(increaseShield);
            }
            PowerUpManager.CleanSpawningPoint(gameObject);
            Destroy(gameObject);
        }
    }
}