using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Persistent singleton that tracks the player's remaining hearts across all scenes.
/// Max 10 hearts. Hearts reset to 10 automatically whenever StartScene or ChooseCharacter loads.
/// </summary>
public class LifeManager : MonoBehaviour
{
    public const int MaxLives = 10;

    private const string PrefsKey = "RemainingLives";

    public static LifeManager Instance { get; private set; }

    public int RemainingLives { get; private set; }

    // ---------------------------------------------------------------
    // AUTO-BOOT — subscribes to sceneLoaded before the first scene runs
    // ---------------------------------------------------------------
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Guarantee LifeManager exists in every scene
        if (Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();

        // Reset to full hearts whenever the player visits the start/character screens
        if (scene.name == "StartScene" || scene.name == "ChooseCharacter")
            Instance.ResetLives();
    }

    // ---------------------------------------------------------------

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Start at full lives — stale PlayerPrefs are overwritten by OnSceneLoaded anyway
        RemainingLives = MaxLives;
        Save();
    }

    /// <summary>
    /// Call when the player fails a level (caught, time out, etc.).
    /// Decrements hearts and either reloads the current level (if hearts remain)
    /// or forces a return to StartScene (if hearts hit 0).
    /// </summary>
    public void LoseHeart()
    {
        RemainingLives = Mathf.Max(0, RemainingLives - 1);
        Save();

        // Notify the HeartsUI in the current scene, if any
        HeartsUI ui = FindAnyObjectByType<HeartsUI>();
        if (ui != null) ui.Refresh();

        if (RemainingLives <= 0)
        {
            // No hearts left – show YOU LOST screen; player must press Play Again
            Time.timeScale = 1f; // unfreeze so the screen can render
            GameLostScreen.Show();
        }
        // If hearts remain, the caller's retry/try-again flow handles scene reload
    }

    /// <summary>
    /// Resets hearts to 10 and saves. Call this when starting a brand-new game
    /// (e.g. the player presses "New Game" or chooses a character).
    /// </summary>
    public void ResetLives()
    {
        RemainingLives = MaxLives;
        Save();
    }

    /// <summary>
    /// Adds one heart (up to MaxLives). Call from the shopkeeper health upgrade.
    /// </summary>
    public void GainHeart()
    {
        RemainingLives = Mathf.Min(MaxLives, RemainingLives + 1);
        Save();
    }

    /// <summary>
    /// Returns true when the player is allowed to retry the current level
    /// (i.e. they still have hearts remaining after losing one this run).
    /// </summary>
    public bool CanRetry => RemainingLives > 0;

    // -------------------------------------------------------------------------

    private void Save()
    {
        PlayerPrefs.SetInt(PrefsKey, RemainingLives);
        PlayerPrefs.Save();
    }
}
