using System;
using UnityEngine;

public class BulletExplosion : AbstractExplosion {

    protected override void postAwake() {
        explosionRadius = 0.01f;
    }

    protected new void OnTriggerEnter(Collider other) {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.up, 5f, playerMask);
        foreach (RaycastHit hit in hits) {
            Collider c = hit.collider;
            if (c == null) {
                continue;
            }

            Rigidbody targetRigidbody = c.GetComponent<Rigidbody>();

            if (!targetRigidbody) {
                continue;
            }
            TankVitals vitals = targetRigidbody.GetComponent<TankVitals>();
            if (!vitals) {
                continue;
            }
            int damageDealt = CalculateDamage(targetRigidbody.position, hit.distance);
            Debug.LogFormat("Dealt {0} damage to {1}", damageDealt, targetRigidbody.name);
            vitals.takeDamage(damageDealt);
        }
        Explode();
    }

    protected int CalculateDamage(Vector3 targetPosition, float distance) {
        float damage = (maxDamage - maxDamage * distance / 2f);
        Debug.Log("Damage " + damage);
        return Mathf.RoundToInt(Mathf.Max(0f, damage));
    }
}
