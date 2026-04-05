using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 5;
    public float bulletSpeed = 20f;
    public float lifeTime = 3f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = transform.right * bulletSpeed;
        
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            return; 
        }
        
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }

        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            // Destroys bullet upon collision
            Destroy(gameObject);
        }
    }
}