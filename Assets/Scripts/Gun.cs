using UnityEngine;

public class Gun : Weapon
{   
    public GameObject bulletPrefab;
    public Transform firePoint;

    // How many seconds the player must wait between shots
    public float cooldownTime = 0.5f; 

    // An internal tracker to remember when we are allowed to shoot again
    private float nextFireTime = 0f;

    public override void Attack()
    {
        // Check if the current game time has passed our "next allowed" time
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Calculate the next time we are allowed to shoot
            nextFireTime = Time.time + cooldownTime;
        }
        else
        {
            Debug.Log("Weapon is on cooldown!");
        }
    }
}
