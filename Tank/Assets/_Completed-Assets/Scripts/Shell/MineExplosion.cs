using UnityEngine;
using Complete;

public class MineExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 100f; //Max damage on perfect hit
    public float m_ExplosionForce = 1000f; //
    //public float m_MaxLifeTime = 2f; //Lifetime of flying fists in seconds
    public float m_ExplosionRadius = 5f; //
    private bool active = false;
    private float activeAfter = 1f; //Activate the mine after n seconds
    private float timeExists; //The time in seconds the mine exists


    private void Start()
    {
        //Destroy(gameObject, m_MaxLifeTime);
    }

    private void Update()
    {
        timeExists += Time.deltaTime;
        active = timeExists > activeAfter;
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            // Find all the tanks in an area around the fist and damage them.

            //Get all coliders in the radius of the exploding fist
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            foreach (var collider in colliders)
            {
                Rigidbody targetRigidBody = collider.GetComponent<Rigidbody>();
                if (targetRigidBody)
                {
                    targetRigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                    TankHealth targetHealth = collider.GetComponent<TankHealth>();
                    if (targetHealth)
                    {
                        float damage = CalculateDamage(targetRigidBody.position);
                        targetHealth.TakeDamage(damage);
                    }
                }
            }

            m_ExplosionParticles.transform.parent = null;
            m_ExplosionParticles.Play();
            if (m_ExplosionAudio != null)
            {
                m_ExplosionAudio.Play();
            }
            Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
            Destroy(gameObject);
        }
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        return Mathf.Max(0f, relativeDistance * m_MaxDamage);
    }
}