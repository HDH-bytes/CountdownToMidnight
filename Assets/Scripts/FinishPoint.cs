using UnityEngine;
using UnityEngine.SceneManagement;
public class FinishPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" || other.name == "Player2")
        {
            SceneManager.LoadScene("Level2");
        }
    }
}
