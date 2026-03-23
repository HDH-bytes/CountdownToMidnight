using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    private Vector2 moveInput;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;

    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isRunning", true);

        if (context.canceled) // happens when input stops
        {
            animator.SetBool("isRunning", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        
        // The idle logic is before the input direction is read so last input values will not be zero
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX",  moveInput.x);
        animator.SetFloat("InputY",  moveInput.y);
    }
        
}
