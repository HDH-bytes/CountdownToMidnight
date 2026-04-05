using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform attackPoint; // Tip of the knife
    public float attackRange = 0.5f;
    public int damage = 40;
    
    public float lungeDistance = 1f;
    public float lungeDuration = 0.2f;
    
    private bool _isAttacking = false;
    private Vector3 _originalLocalPosition;
    
    void Start()
    {
        // Remember the resting position of the knife when the game starts
        _originalLocalPosition = transform.localPosition;
    }
    
    public override void Attack()
    {
        // Only attack if player is not already attacking
        if (!_isAttacking)
        {
            // This is how you start a Coroutine
            StartCoroutine(LungeRoutine());
        }
    }
    
    private IEnumerator LungeRoutine()
    {
        _isAttacking = true;

        // Calculate where the peak of the stab should be
        Vector3 targetPosition = _originalLocalPosition + new Vector3(lungeDistance, 0, 0);

        float halfDuration = lungeDuration / 2f;
        float elapsedTime = 0f;

        // Stab Forward
        // This loop runs every single frame until half the duration is over
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Lerp (Linear Interpolation) smoothly slides a value from Point A to Point B
            transform.localPosition = Vector3.Lerp(_originalLocalPosition, targetPosition, elapsedTime / halfDuration);
            
            // "yield return null" tells Unity: "Pause here, render the game frame, and come back to this line next frame"
            yield return null; 
        }

        // Snap exactly to the target to ensure the math is perfect
        transform.localPosition = targetPosition;

        // Deal damage at peak of the lunge
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Pull the knife back
        elapsedTime = 0f; // Reset the timer
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            // Notice we swap the target and original positions to pull it backwards
            transform.localPosition = Vector3.Lerp(targetPosition, _originalLocalPosition, elapsedTime / halfDuration);
            yield return null;
        }

        // Snap back to the starting position
        transform.localPosition = _originalLocalPosition;
        
        // Let the player attack again
        _isAttacking = false;
    }
}
