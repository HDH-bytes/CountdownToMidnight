using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform attackPoint; // Tip of the knife
    public float attackRange = 0.5f;
    public int damage = 40;

    public override void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D col in hitColliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        Debug.Log("Stabbing");
    }
}
