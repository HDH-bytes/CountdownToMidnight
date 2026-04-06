using UnityEngine;

public class EskimoFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            animator.SetBool("isWalking", direction.magnitude > 0);
        }
    }
}