using UnityEngine;

public class Rotator : MonoBehaviour
{
    protected void LookAt(Vector3 target)
    {
        // Calculate the base angle
        float lookAngle = AngleBetweenTwoPoints(transform.position, target);
        
        // Apply the rotation. 
        // Note: If your sprite art faces UP by default, add "- 90f" to lookAngle here.
        transform.eulerAngles = new Vector3(0, 0, lookAngle);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        // FIX: Destination (b) minus Origin (a). 
        float directionY = b.y - a.y;
        float directionX = b.x - a.x;

        return Mathf.Atan2(directionY, directionX) * Mathf.Rad2Deg;
    }
}