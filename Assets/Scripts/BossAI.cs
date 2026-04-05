using UnityEngine;

// 1. Inherit from Character instead of MonoBehaviour
public class BossAI : Character
{
    [Header("Targeting")]
    private Transform player; 

    [Header("Boss Specific Stats")]
    // maxHealth, currentHealth, and speed are now inherited from Character!
    // We just need the attack and detection ranges here.
    public float attackRange = 1.5f;
    public float detectionRange = 7f; 

    [Header("Components")]
    private Animator animator;
    private Collider2D bossCollider;
    
    // State Tracking
    private bool isDead = false;
    private bool isAttacking = false;

    // 2. Use protected override to hook into Character's Start method
    protected override void Start()
    {
        // Call base.Start() to initialize currentHealth = maxHealth
        base.Start(); 

        // Since we removed moveSpeed, make sure the inherited 'speed' has a value
        if (speed == 0f) speed = 3f;
        if (maxHealth == 0) maxHealth = 3;

        animator = GetComponent<Animator>();
        bossCollider = GetComponent<Collider2D>();
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
            animator.SetBool("isWalking", false);
            
            if (!isAttacking)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking) 
        {
            animator.SetBool("isWalking", true);
            
            // 3. Replaced 'moveSpeed' with the inherited 'speed' variable
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }
        else 
        {
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

    // 4. Override TakeDamage to prevent taking damage after death, 
    // but let the base Character class handle the actual health subtraction.
    public override void TakeDamage(int amount)
    {
        if (isDead) return;
        
        // This calls the math and Debug.Logs from Character.cs
        base.TakeDamage(amount); 
    }

    // 5. Override Die so the Boss plays an animation instead of getting Destroyed instantly.
    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Death");

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        Debug.Log("Boss Defeated!");
        
        // Note: We intentionally DO NOT call base.Die() here.
        // base.Die() calls Destroy(gameObject). Since we want the death animation 
        // to play out, we skip calling it. If you want the body to disappear later, 
        // you can add: Destroy(gameObject, 3f);
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