using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
namespace Complete
{
    public class HealthCollect : MonoBehaviour
    {
        public float increaseHealth = 25f;
        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<TankHealth>();
            if (health != null)
            {
                health.Heal(increaseHealth);
            }
            Destroy(gameObject);
        }
    }
}