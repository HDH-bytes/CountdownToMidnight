using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;

    [SerializeField] private GameObject panel; //assign the Game Over UI panel in Inspector

    [Tooltip("Optional: the Retry/Try Again button. If left null it is found automatically.")]
    [SerializeField] private Button retryButton;

    void Awake()
    {
        instance = this;
        if (panel != null) panel.SetActive(false);
    }

    /// <summary>
    /// Shows the caught/game-over panel and deducts a heart.
    /// If no hearts remain the panel still shows but the retry button is disabled
    /// (the player must use OnMainMenu / OnNewGame to return to StartScene).
    /// </summary>
    public static void Show()
    {
        if (instance == null || instance.panel == null) return;

        // Deduct heart first so HeartsUI reflects the new count immediately
        EnsureLifeManager();
        LifeManager.Instance.LoseHeart();

        // If hearts hit 0, LoseHeart() already redirected to StartScene – bail out
        if (LifeManager.Instance.RemainingLives <= 0) return;

        instance.panel.SetActive(true);
        Time.timeScale = 0f; //freeze the game

        // Wire retry button dynamically in case Inspector reference is missing
        if (instance.retryButton == null)
            instance.retryButton = instance.panel.GetComponentInChildren<Button>(true);
    }

    /// <summary>Try Again – reload the current level with one fewer heart.</summary>
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Return to the main menu / start scene.</summary>
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    /// <summary>
    /// Start a brand-new game: reset hearts to 10 and go to StartScene.
    /// Wire this to a "New Game" button if you add one to the game-over screen.
    /// </summary>
    public void OnNewGame()
    {
        EnsureLifeManager();
        LifeManager.Instance.ResetLives();
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    // -------------------------------------------------------------------------

    private static void EnsureLifeManager()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();
    }
}
