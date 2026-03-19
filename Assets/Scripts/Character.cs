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
 
    public virtual void Move(Vector2 direction)
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
 
    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
 
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); //subclasses can override this
        }
    }
 
    public virtual void Attack()
    {
        if (currentWeapon != null)
            currentWeapon.UseWeapon(this);
    }
 
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}