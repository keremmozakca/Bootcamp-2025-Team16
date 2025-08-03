using UnityEngine;

public class Obstacle_Explosion : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 100;
    public int overhead = 0;

    [Header("Explosion  Effect")]
    [SerializeField] private GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            // Instantiate explosion particle at the object's position and rotation
            GameObject explosion = Instantiate(
            explosionPrefab, new Vector3(transform.position.x, transform.position.y + overhead, transform.position.z), transform.rotation);

            // Destroy the particle system after it finishes playing
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosion, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // Fallback in case the prefab doesn't have a ParticleSystem directly on the root
                Destroy(explosion, 2f);
            }
        }

        // Destroy the obstacle
        Destroy(gameObject);
    }
}