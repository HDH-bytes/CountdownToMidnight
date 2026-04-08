using UnityEngine;

public class BossAI : Enemy
{
    [Header("Targeting")]
    private Transform player;

    [Header("Boss Combat")]
    public float attackRange = 1.5f;
    public float detectionRange = 7f;

    [Header("Components")]
    private Animator animator;
    private Collider2D bossCollider;

    private bool isDead = false;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();

        if (maxHealth == 0) maxHealth = 3;
        if (speed == 0f) speed = 3f;

        currentHealth = maxHealth;

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
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            animator.SetBool("isWalking", true);

            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
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
        LifeManager.Instance.LoseHeart();

        Invoke(nameof(ResetAttack), 1f);
    }

    void ResetAttack()
    {
        if (!isDead)
        {
            isAttacking = false;
        }
    }

    // public override void TakeDamage(int damage)
    // {
    //     if (isDead) return;
    //
    //     base.TakeDamage(damage);
    //     Debug.Log("Boss took damage! Health remaining: " + currentHealth);
    // }

    protected override void Die()
    {
        // if (isDead) return;

        // isDead = true;

        animator.SetBool("isWalking", false);
        animator.SetTrigger("Death");

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }
        
        base.Die();

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