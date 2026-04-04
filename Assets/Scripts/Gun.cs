using UnityEngine;

public class Gun : Weapon
{   
    public GameObject bulletPrefab;
    public Transform firePoint;

    public override void Attack()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
