using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject character1Prefab;
    [SerializeField] private GameObject character2Prefab;
    [SerializeField] private GameObject character3Prefab; 

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (selectedCharacter == 0)
        {
            Instantiate(character1Prefab, transform.position, transform.rotation);
        }
        else if (selectedCharacter == 1)
        {
            Instantiate(character2Prefab, transform.position, transform.rotation);
        }
        else if (selectedCharacter == 2)
        {
            Instantiate(character3Prefab, transform.position, transform.rotation);
        }
    }
}