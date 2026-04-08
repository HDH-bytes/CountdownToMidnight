using UnityEngine;

public class EskimoAttack : Enemy
{
    
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private float nextFireTime;
    
    
    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }
        
        Vector2 direction = player.position - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
        
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
    }
}