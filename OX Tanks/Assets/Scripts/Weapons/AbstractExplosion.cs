using System;
using System.Collections;
using UnityEngine;

public abstract class AbstractExplosion : MonoBehaviour {
    [SerializeField]
    protected ParticleSystem explosion;

    [SerializeField]
    protected float explosionRadius = 5f;

    [SerializeField]
    protected float explosionForce = 10f;
    [SerializeField]
    protected int maxDamage = DefaultVitals.MAX_DAMAGE;

    [SerializeField]
    protected LayerMask playerMask;

    [SerializeField]
    private GameObject debugRadius;

    [SerializeField]
    private bool showDebugRadius = true;
    private PoolManager poolManager;
    /// The collider of this object to disable upon impact
    /// in order to avoid multiple collisions after explosion.
    protected Collider weaponCollider;
    private GameObject geometry;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip fireClip;

    public AudioClip FireClip {
        get {
            return fireClip;
        }
    }

    [SerializeField]
    private AudioClip explosionClip;

    private void Awake() {
        poolManager = PoolManager.instance;
        weaponCollider = gameObject.GetComponent<Collider>();
        geometry = gameObject.transform.Find("Model").gameObject;
        postAwake();
    }

    protected abstract void postAwake();

    protected void OnTriggerEnter(Collider other) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerMask);
        foreach (Collider c in colliders) {
            Rigidbody targetRigidbody = c.GetComponent<Rigidbody>();

            if (!targetRigidbody) {
                continue;
            }
            TankVitals vitals = targetRigidbody.GetComponent<TankVitals>();
            if (!vitals) {
                continue;
            }
            int damageDealt = CalculateDamage(targetRigidbody.position);
            Debug.LogFormat("Dealt {0} damage to {1}", damageDealt, targetRigidbody.name);
            vitals.takeDamage(damageDealt);
        }
        Explode();
    }

    protected int CalculateDamage(Vector3 targetPosition) {
        // Create a vector from the shell to the target.
        Vector3 explosionToTarget = targetPosition - transform.position;
        // Calculate the distance from the shell to the target.
        float explosionDistance = explosionToTarget.magnitude;
        // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
        Debug.Log("Target's distance from explosion " + explosionDistance + ", relative distance : " + relativeDistance);
        // Calculate damage as this proportion of the maximum possible damage.
        float damage = relativeDistance * maxDamage;
        return Mathf.RoundToInt(Mathf.Max(0f, Mathf.Round(damage)));
    }

    protected void Explode() {
        if (showDebugRadius) {
            GameObject d = Instantiate(debugRadius);
            d.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
            d.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1);
            Destroy(d, 2f);
        }
        explosion.transform.parent = null;
        explosion.Play();
        audioSource.Play();
        weaponCollider.enabled = false;
        ParticleSystem.MainModule mainModule = explosion.main;
        StartCoroutine("Deactivate", mainModule.duration); //TODO: Should be part of the pool management framework
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator Deactivate(float delay) {
        geometry.SetActive(false);
        yield return new WaitForSeconds(delay);
        explosion.Stop();
        explosion.Clear();
        explosion.transform.position = gameObject.transform.position;
        explosion.transform.parent = gameObject.transform;
        weaponCollider.enabled = true;
        geometry.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the max damage that the explosion can deal to the opponent.
    /// </summary>
    /// <param name="maxDamage">Max damage.</param>
    public void setMaxDamage(int maxDamage) {
        this.maxDamage = maxDamage;
    }
}
