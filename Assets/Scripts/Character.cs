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
 
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    //should we have move here, so it can be extended?
 
    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
        Debug.Log(gameObject.name + " took "  + amount + " damage" );
 
        if (currentHealth <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
            currentHealth = 0;
            Die(); //subclasses can override this
        }
    }
 
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}