using System;
using UnityEngine;

public class BulletExplosion : AbstractExplosion {

    protected override void postAwake() {
        explosionRadius = 0.01f;
    }

    protected new void OnTriggerEnter(Collider other) {
        Debug.Log("Collider hit: " + other.name);
        Explode();
    }
}
