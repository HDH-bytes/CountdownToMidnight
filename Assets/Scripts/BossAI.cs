using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Targeting")]
    private Transform player; 

    [Header("Stats")]
    public int maxHealth = 3;
    private int currentHealth;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float detectionRange = 7f; 

    [Header("Components")]
    private Animator animator;
    private Collider2D bossCollider;
    
    // State Tracking
    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        bossCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 1. If the boss is dead, stop running logic entirely
        if (isDead) return;

        // 2. Constantly scan the arena to find the closest player
        FindClosestPlayer();

        // 3. If there are NO players currently in the scene, stand still and wait
        if (player == null) 
        {
            animator.SetBool("isWalking", false);
            return;
        }

        // --- Standard Combat & Movement Logic ---
        
        // Calculate distance to whoever won the "closest player" check
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Inside Attack Range: Stop walking and swing!
            animator.SetBool("isWalking", false);
            
            if (!isAttacking)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking) 
        {
            // Inside Detection Range, but outside Attack Range: Chase them!
            animator.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Flip the boss sprite to face the player's X direction
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1); 
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            // Outside Detection Range: Player is too far away, stand still.
            animator.SetBool("isWalking", false);
        }
    }

    // --- The Target-Finding Algorithm ---
    void FindClosestPlayer()
    {
        // Find every GameObject in the scene right now with the "Player" tag
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        float closestDistance = Mathf.Infinity; 
        Transform closestTarget = null;

        // Loop through the array to find the shortest distance
        foreach (GameObject p in allPlayers)
        {
            float distanceToP = Vector2.Distance(transform.position, p.transform.position);

            if (distanceToP < closestDistance)
            {
                closestDistance = distanceToP;
                closestTarget = p.transform;
            }
        }

        // Assign the winner as the boss's current target
        player = closestTarget;
    }

    // --- Combat Methods ---
    void AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        
        Debug.Log("Boss swings at the closest player!");

        // Prevents the boss from moving or attacking again for 1 second.
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

    // --- Editor Visualization ---
    // This draws visible circles in the Unity Editor Scene view to help you balance the game ranges
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