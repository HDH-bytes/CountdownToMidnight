using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePoint; 
        public float fireRate = 1f;
        private float nextTimeToFire = 0f;
    
        void Update()
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    
        void Shoot()
        {
            Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        }
}