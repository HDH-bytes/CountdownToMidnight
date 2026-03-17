using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points;
    public int health;
    public void Initialize(int points, int health)
    {
        this.points = points;
        this.health = health;
    }
    
    public void TakeDamage(int damage)
    {
        health = health - damage;

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        ScoreManager.Instance.AddPoints(points);
        Destroy(gameObject);
    }
}
