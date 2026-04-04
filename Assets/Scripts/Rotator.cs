using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Transform pivotToRotate;
    protected void LookAt(Vector3 target)
    {
        // If a pivot is not assigned it will rotate the player instead of crashing
        Transform targetTransform = pivotToRotate != null ? pivotToRotate : transform;
        // Calculate the base angle
        float lookAngle = AngleBetweenTwoPoints(transform.position, target);
        
        // Apply the rotation to the pivot 
        // Note: If your sprite art faces UP by default, add "- 90f" to lookAngle here.
        targetTransform.eulerAngles = new Vector3(0, 0, lookAngle);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        float directionY = b.y - a.y;
        float directionX = b.x - a.x;

        return Mathf.Atan2(directionY, directionX) * Mathf.Rad2Deg;
    }
}