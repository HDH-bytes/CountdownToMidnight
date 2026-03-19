using UnityEngine;
 
public abstract class Weapon : MonoBehaviour
{
    public float damage;
 
    public abstract void UseWeapon(Character user);
}
 