using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
namespace Complete
{
    public class HealthCollect : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<TankHealth>();
            if (health != null)
            {
                health.Heal(25);
            }
            Destroy(gameObject);
        }
    }
}