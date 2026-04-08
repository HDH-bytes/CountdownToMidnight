using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private float nextFireTime;
    private Transform player;

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }
        
        Vector2 direction = player.position - firePoint.position;
        firePoint.right = direction; 
        
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}