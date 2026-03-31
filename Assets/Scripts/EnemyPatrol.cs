using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolAxis { Horizontal, Vertical }

    [SerializeField] private PatrolAxis axis = PatrolAxis.Horizontal;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void FixedUpdate()
    {
        Vector2 moveDir = axis == PatrolAxis.Horizontal ? Vector2.right : Vector2.up;

        float currentOffset = axis == PatrolAxis.Horizontal
            ? rb.position.x - startPosition.x
            : rb.position.y - startPosition.y;

        if (currentOffset >= patrolDistance)
            direction = -1;
        else if (currentOffset <= -patrolDistance)
            direction = 1;

        rb.linearVelocity = moveDir * direction * speed;
    }

    //draw patrol range 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = Application.isPlaying ? startPosition : (Vector2)transform.position;

        if (axis == PatrolAxis.Horizontal)
        {
            Gizmos.DrawLine(origin + Vector2.left * patrolDistance, origin + Vector2.right * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.left * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.right * patrolDistance, 0.15f);
        }
        else
        {
            Gizmos.DrawLine(origin + Vector2.down * patrolDistance, origin + Vector2.up * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.down * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.up * patrolDistance, 0.15f);
        }
    }
}
