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
        if (isDead) return;

        FindClosestPlayer();

        if (player == null) 
        {
            animator.SetBool("isWalking", false);
            return;
        }

        // --- Standard Combat & Movement Logic ---
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

            // --- NEW BLEND TREE ANIMATION LOGIC ---
            // Calculate the exact X and Y direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Send that math directly to the Animator's Float parameters
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            // --------------------------------------
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

    // --- Combat Methods ---
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

    // --- Editor Visualization ---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}