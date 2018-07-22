using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour {

	[SerializeField]
	private ParticleSystem explosion;

	[SerializeField]
	private float explosionRadius = 5f;

	[SerializeField]
	private float explosionForce = 10f;
	[SerializeField]
	private float maxDamage = 25f;

	[SerializeField]
	private LayerMask playerMask;

	[SerializeField]
	private GameObject debugRadius;

	[SerializeField]
	private bool showDebugRadius = true;
	private PoolManager poolManager;
	/// The collider of this object to disable upon impact
	/// in order to avoid multiple collisions after explosion.
	private Collider shellCollider;

	private void Awake () {
		poolManager = PoolManager.instance;
		shellCollider = gameObject.GetComponent<Collider>();
	}

	private void OnTriggerEnter (Collider other) {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);
		foreach (Collider c in colliders) {
			Rigidbody targetRigidbody = c.GetComponent<Rigidbody> ();

			if (!targetRigidbody) {
				continue;
			}
			TankVitals vitals = targetRigidbody.GetComponent<TankVitals> ();
			if (!vitals) {
				continue;
			}
			float damageDealt = CalculateDamage (targetRigidbody.position);
			Debug.LogFormat ("Dealt {0} damage to {1}", damageDealt, targetRigidbody.name);
			vitals.takeDamage (damageDealt);
		}
		Explode ();
	}

	private void Explode () {
		if (showDebugRadius) {
			GameObject d = Instantiate (debugRadius);
			d.transform.position = new Vector3 (transform.position.x, 0.01f, transform.position.z);
			d.transform.localScale = new Vector3 (explosionRadius, explosionRadius, 1);
			Destroy (d, 2f);
		}
		explosion.transform.parent = null;
		explosion.Play ();
		shellCollider.enabled = false;
		ParticleSystem.MainModule mainModule = explosion.main;
		StartCoroutine ("Deactivate", mainModule.duration); //TODO: Should be part of the pool management framework
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
		return Mathf.Max (0f, Mathf.Round (damage));
	}

	/// <summary>
	/// 
	/// </summary>
	IEnumerator Deactivate (float delay) {
		yield return new WaitForSeconds (delay);
		explosion.Stop ();
		explosion.Clear ();
		explosion.transform.position = gameObject.transform.position;
		explosion.transform.parent = gameObject.transform;
		shellCollider.enabled = true;
		gameObject.SetActive(false);
	}
}