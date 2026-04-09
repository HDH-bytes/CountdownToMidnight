using UnityEngine;

public class Enemy : Character
{
    public int points;
    public void Initialize(int points, int health)
    {
        this.points = points;
        this.maxHealth = health;
        this.currentHealth = health;
    }
    
    protected override void Die()
    {
        Destroy(gameObject);
        ScoreManager.Instance.AddXP(points);
    }
}
