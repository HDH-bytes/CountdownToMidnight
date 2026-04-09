using UnityEngine;
using System.Collections; 

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float startDelay = 3f;
    public float spawnInterval = 2f;  
    public float spawnDuration = 10f; 

    [Header("Progression Settings")]
    // NEW: We will drag the piece of the fence we want to disappear into this slot
    public GameObject fenceToOpen; 

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }
        
        float stopTime = Time.time + spawnDuration;

        while (Time.time < stopTime)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        // --- NEW LOGIC: OPEN THE GATE ---
        // Check if we actually assigned a fence in the Inspector to avoid errors
        if (fenceToOpen != null)
        {
            // SetActive(false) makes the object completely invisible and removes its collision, 
            // allowing the player to walk right through where it used to be!
            fenceToOpen.SetActive(false); 
        }

        // Destroy the spawner object once it finishes its job 
        Destroy(gameObject);
    }
}