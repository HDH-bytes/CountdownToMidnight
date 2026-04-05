using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector2 lastDir = Vector2.down;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Horizontal takes priority over vertical
        if (h != 0f) v = 0f;

        bool isMoving = h != 0f || v != 0f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            lastDir = new Vector2(h, v).normalized;
            animator.SetFloat("MoveX", lastDir.x);
            animator.SetFloat("MoveY", lastDir.y);
        }
        else
        {
            // Keep last direction so idle faces the right way
            animator.SetFloat("MoveX", lastDir.x);
            animator.SetFloat("MoveY", lastDir.y);
        }
    }
}