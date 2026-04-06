using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Time.timeScale = 1f; // ensure Level4Timer freeze doesn't carry into the next scene

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        ScoreManager.Instance?.AddXP(levelManager.GetXPForLevel(currentIndex));

        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(0);
    }
}