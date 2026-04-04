using UnityEngine;
 
public interface IDamageable
{
    void TakeDamage(int amount);
}
 
// base class for player, enemies, and boss
public abstract class Character : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public float speed;
    public Weapon currentWeapon;
 
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    //should we have move here, so it can be extended?
 
    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
 
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); //subclasses can override this
        }
    }
 
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}