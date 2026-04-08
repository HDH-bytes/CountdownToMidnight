using UnityEngine;

public class EnemyPatrolMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 lastDir = Vector2.down;

    void Start()
    {
        // drives the animater itself so the player and cone move together 
        if (GetComponent<EnemyPatrol>() != null)
        {
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = rb.linearVelocity;
        float h = vel.x;
        float v = vel.y;

        // Horizontal takes priority over vertical
        if (Mathf.Abs(h) > 0.01f) v = 0f;

        bool isMoving = Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            lastDir = new Vector2(h, v).normalized;
        }

        // Always apply last known direction so idle faces the right way
        animator.SetFloat("MoveX", lastDir.x);
        animator.SetFloat("MoveY", lastDir.y);
    }
}