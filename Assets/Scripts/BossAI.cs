using UnityEngine;

public class BossAI : MonoBehaviour
{
    private Transform player; 

    public int maxHealth = 3;
    private int currentHealth;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float detectionRange = 7f; 

    private Animator animator;
    private Collider2D bossCollider;
    
    // State Tracking
    private bool isDead = false;
    private bool isAttacking = false;

    // --- NEW: WAKE UP SYSTEM ---
    public bool isAwake = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        bossCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    // --- NEW: THE START BATTLE COMMAND ---
    // This is exactly what the BattleTrigger script is looking for!
    public void StartBattle()
    {
        isAwake = true;
        Debug.Log("The Boss has woken up!");
    }

    void Update()
    {
        if (isDead) return;

        // --- NEW: SLEEP SHIELD ---
        // If the trigger hasn't woken the boss up yet, freeze its brain!
        if (!isAwake) return; 

        FindClosestPlayer();

        if (player == null) 
        {
            animator.SetBool("isWalking", false);
            return;
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isWalking", false);
            
            if (!isAttacking)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking) 
        {
            animator.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1); 
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            animator.SetBool("isWalking", false);
        }
    }

    void FindClosestPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        float closestDistance = Mathf.Infinity; 
        Transform closestTarget = null;

        foreach (GameObject p in allPlayers)
        {
            float distanceToP = Vector2.Distance(transform.position, p.transform.position);

            if (distanceToP < closestDistance)
            {
                closestDistance = distanceToP;
                closestTarget = p.transform;
            }
        }

        player = closestTarget;
    }

    void AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        
        Debug.Log("Boss swings at the closest player!");

        Invoke("ResetAttack", 1f); 
    }

    void ResetAttack()
    {
        if (!isDead)
        {
            isAttacking = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Boss took damage! Health remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Death");

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        Debug.Log("Boss Defeated!");
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow circle for the Detection Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw a red circle for the Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}