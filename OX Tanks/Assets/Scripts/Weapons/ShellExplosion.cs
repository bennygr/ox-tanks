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
	private float maxDamage = 25f;

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
			targetRigidbody.AddExplosionForce (explosionForce, transform.position, explosionRadius);
			TankVitals vitals = targetRigidbody.GetComponent<TankVitals> ();
			float damageDealt = CalculateDamage (targetRigidbody.position);
			Debug.LogFormat("Dealt {0} damage to {1}", damageDealt, targetRigidbody.name);
			vitals.takeDamage (damageDealt);
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

	private float CalculateDamage (Vector3 targetPosition) {
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;
		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;
		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * maxDamage;
		return Mathf.Max (0f, damage);
	}
}