using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;

    [SerializeField] private GameObject panel; //assign the Game Over UI panel in Inspector

    void Awake()
    {
        instance = this;
        if (panel != null) panel.SetActive(false);
    }

    public static void Show()
    {
        if (instance == null || instance.panel == null) return;
        instance.panel.SetActive(true);
        Time.timeScale = 0f; //freeze the game
    }

    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); //change 0 to your main menu scene index if needed
    }
}
