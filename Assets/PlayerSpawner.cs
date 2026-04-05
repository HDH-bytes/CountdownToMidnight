using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject character1Prefab;
    [SerializeField] private GameObject character2Prefab;
    void Start()
    {
        // This grabs the exact choice you saved in your ChooseCharacterController
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject spawnedPlayer = null;
        if (selectedCharacter == 0)
        {
            // Spawns Character 1 at the Spawner's location
            spawnedPlayer = Instantiate(character1Prefab, transform.position, transform.rotation);
        }
        else if (selectedCharacter == 1)
        {
            // Spawns Character 2 at the Spawner's location
            spawnedPlayer = Instantiate(character2Prefab, transform.position, transform.rotation);
        }
        
    }
}