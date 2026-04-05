using UnityEngine;

// Inherits from Character base class
public class BossAI : Character
{
    [Header("Targeting")]
    private Transform player; 

    [Header("Boss Specific Stats")]
    public float attackRange = 1.5f;
    public float detectionRange = 7f; 

    [Header("Components")]
    private Animator animator;
    private Collider2D bossCollider;
    
    // State Tracking
    private bool isDead = false;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start(); 

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

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isWalking", false);
            
            if (!isAttacking)
            {
                // Send direction to the Attack Blend Tree
                Vector2 direction = (player.position - transform.position).normalized;
                animator.SetFloat("AttackX", direction.x);
                animator.SetFloat("AttackY", direction.y);

                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking) 
        {
            animator.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Send direction to the Walk Blend Tree
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
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
        
        // Notice we deleted the Invoke("ResetAttack") timer here!
    }

    // This is the new method that your Animation Event will trigger!
    public void FinishAttack()
    {
        if (!isDead)
        {
            isAttacking = false;
            Debug.Log("Animation Event Fired: Attack Finished");
        }
    }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;
        base.TakeDamage(amount); 
    }

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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}