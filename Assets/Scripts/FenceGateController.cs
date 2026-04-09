using UnityEngine;

public class FenceGateController : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject fenceToOpen;
    
    [Tooltip("Make sure your Player GameObject has this tag!")]
    public string playerTag = "Player";

    // Keeps track of whether the player is currently allowed through
    private bool isGateOpen = false;

    /// <summary>
    /// Call this method from your NPC's dialogue script or interaction event
    /// </summary>
    public void OpenFenceFromNPC()
    {
        if (fenceToOpen != null)
        {
            fenceToOpen.SetActive(false); // Hide the fence
            isGateOpen = true;            // Update our internal tracker
        }
    }

    /// <summary>
    /// This automatically runs when something touches the Trigger Collider on this object
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Did the Player touch it?
        // 2. Is the gate currently open?
        if (other.CompareTag(playerTag) && isGateOpen)
        {
            if (fenceToOpen != null)
            {
                fenceToOpen.SetActive(true); // Bring the fence back!
                isGateOpen = false;          // Reset the state so it doesn't trigger repeatedly
            }
        }
    }
}