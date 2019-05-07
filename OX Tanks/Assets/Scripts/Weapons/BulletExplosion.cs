using System;
using UnityEngine;

public class BulletExplosion : AbstractExplosion {

    protected override void postAwake() {
        explosionRadius = 0.01f;
    }

    private new void OnTriggerEnter(Collider other) {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.up, 3f, playerMask);
        foreach (RaycastHit hit in hits) {
            Collider c = hit.collider;
            if (c == null) {
                continue;
            }
            Debug.Log("distance for hit: " + hit.distance);

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
            Explode();
        }
    }

    protected int CalculateDamage(Vector3 targetPosition, float distance) {
        float damage = (maxDamage - maxDamage * distance / 3f);
        Debug.Log("Damage " + damage);
        return Mathf.RoundToInt(Mathf.Max(0f, damage));
    }
}
