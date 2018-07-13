using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour {

	[SerializeField]
	private ParticleSystem explosion;

	[SerializeField]
	private float explosionRadius = 5f;

	[SerializeField]
	private LayerMask playerMask;

	private void OnTriggerEnter(Collider other) {
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerMask);
		foreach (Collider c in colliders) {
			Rigidbody targetRigidbody = c.GetComponent<Rigidbody>();

			if (!targetRigidbody) {
				continue;
			}

			Debug.Log(targetRigidbody.name);
		}
		Explode();
		Destroy(gameObject);
	}

	private void Explode() {
		explosion.transform.parent = null;
		explosion.Play();
		ParticleSystem.MainModule mainModule = explosion.main;
		Destroy(explosion.gameObject, mainModule.duration);
	}
}
