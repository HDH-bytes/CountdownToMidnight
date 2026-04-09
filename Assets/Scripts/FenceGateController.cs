using UnityEngine;

public class FenceGateController : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject fence;
    public string playerTag = "Player";

    [Header("Trigger Behavior")]
    [Tooltip("Check this if this tripwire opens the fence. Uncheck if it closes the fence.")]
    public bool isOpener = true;
    
    [Tooltip("If true, this tripwire permanently turns off after one use.")]
    public bool triggerOnlyOnce = true; 

    // Internal tracker to see if we've already used this tripwire
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player AND if this tripwire is still active
        if (other.CompareTag(playerTag) && !hasTriggered)
        {
            if (fence != null)
            {
                if (isOpener)
                {
                    fence.SetActive(false); // Hide the fence (Open)
                }
                else
                {
                    fence.SetActive(true);  // Show the fence (Close)
                }
            }

            // Lock this tripwire so it can never be used again
            if (triggerOnlyOnce)
            {
                hasTriggered = true;
            }
        }
    }
}