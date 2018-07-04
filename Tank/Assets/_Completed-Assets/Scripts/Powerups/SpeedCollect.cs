using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

namespace Complete
{
    public class SpeedCollect : AbstractPowerUp
    {
        public float speedFactor = 2f;
        public float time = 0f;
        public float maxSpeed = 24f;

        private void OnTriggerEnter(Collider other)
        {
            var movement = other.GetComponent<TankMovement>();
            if (movement != null)
            {
                movement.IncreaseSpeed(speedFactor, time, maxSpeed);
            }

            PowerUpManager.CleanSpawningPoint(gameObject);
            Destroy(gameObject);
        }
    }
}