using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour {

	[SerializeField]
	private ParticleSystem explosion;

	[SerializeField]
	private float explosionRadius = 5f;

	[SerializeField]
	private float explosionForce = 100f;

	[SerializeField]
	private LayerMask playerMask;

	[SerializeField]
	private GameObject debugRadius;

	[SerializeField]
	private bool showDebugRadius = true;

	private void OnTriggerEnter (Collider other) {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);
		foreach (Collider c in colliders) {
			Rigidbody targetRigidbody = c.GetComponent<Rigidbody> ();

			if (!targetRigidbody) {
				continue;
			}
			Debug.LogFormat("Explision force {0}", explosionForce);
			targetRigidbody.AddExplosionForce (explosionForce, transform.position, explosionRadius);


			Debug.Log (targetRigidbody.name);
		}

		if (showDebugRadius) {
			GameObject d = Instantiate (debugRadius);
			d.transform.position = new Vector3 (transform.position.x, 0.01f, transform.position.z);
			d.transform.localScale = new Vector3 (explosionRadius, explosionRadius, 1);
			Destroy (d, 2f);
		}

		Explode ();
		Destroy (gameObject);
	}

	private void Explode () {
		explosion.transform.parent = null;
		explosion.Play ();
		ParticleSystem.MainModule mainModule = explosion.main;
		Destroy (explosion.gameObject, mainModule.duration);
	}
}