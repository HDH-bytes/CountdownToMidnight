using UnityEngine;

//melee weapon, no ammo, hits in a circle around the player
public class Knife : Weapon
{
    private void Awake()
    {
        weaponName = "Knife";
        damage = 40f;
        range = 1.2f;
        fireRate = 1.5f;
        maxAmmo = 0;
        currentAmmo = 0;
        reloadTime = 0f;
    }

    //knife has no ammo
    public override bool CanUse()
    {
        return !isReloading && Time.time >= nextFireTime;
    }

    // in melee range
    public override void UseWeapon(Character c)
    {
        if (!CanUse()) return;

        nextFireTime = Time.time + (1f / fireRate);

        Collider2D[] hits = Physics2D.OverlapCircleAll(c.transform.position, range);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == c.gameObject) continue;

            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
                target.TakeDamage((int)damage);
            //suggestion: spawn a hit vfx at hit.transform.position
        }
        //suggestion: play swing animation on user here
    }
}