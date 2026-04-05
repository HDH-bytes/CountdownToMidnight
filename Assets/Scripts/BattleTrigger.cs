using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public BossAI bossManager; // Connects to the Boss

    [Header("The Door that traps the player")]
    [SerializeField] private GameObject entranceBlocker; // Look! We are using SerializeField!

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the player walks through the invisible trigger...
        if (collision.CompareTag("Player"))
        {
            // 1. Wake up the Boss!
            if (bossManager != null) bossManager.StartBattle();
            
            // 2. Slam the door shut!
            if (entranceBlocker != null)
            {
                entranceBlocker.SetActive(true); // This makes the wall appear instantly
            }

            // 3. Destroy this invisible trigger so it doesn't trigger a second time
            Destroy(gameObject); 
        }
    }
}
