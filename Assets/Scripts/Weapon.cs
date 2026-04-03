using System.Collections;
using UnityEngine;

//base class, extend to other weapon subclasses
public abstract class Weapon : MonoBehaviour
{
    [Header("WeaponInfo")]
    public string weaponName;
    public float damage;
    public float range;

    [Header("FireControl")]
    public float fireRate; //shots per second
    public float reloadTime;

    [Header("Ammo")]
    public int maxAmmo;
    public int currentAmmo;

    //reload + cooldown
    protected bool isReloading = false;
    protected float nextFireTime = 0f;

    //called by Character.Attack(), every weapon must implement this
    public abstract void UseWeapon();

    //if ready to fire then true
    public virtual bool CanUse()
    {
        return !isReloading && Time.time >= nextFireTime && currentAmmo > 0;
    }

    //auto reload
    public virtual void Reload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
            StartCoroutine(ReloadCoroutine());
    }

    //handles the reload timer and restores ammo when done
    protected virtual IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        //suggestion: trigger reload animation or sound here
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        //suggestion: fire a C# event here so UI can react to reload finishing
    }
}