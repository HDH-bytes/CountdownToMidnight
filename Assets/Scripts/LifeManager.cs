using UnityEngine;

/// <summary>
/// Persistent singleton that tracks the player's remaining hearts across all scenes.
/// Max 10 hearts. Hearts are spent when the player is caught/fails a level.
/// When hearts reach 0, the player is forced to the StartScene for a full reset.
/// Call LifeManager.Instance.LoseHeart() from any game-over trigger.
/// Call LifeManager.Instance.ResetLives() when starting a completely new game.
/// </summary>
public class LifeManager : MonoBehaviour
{
    public const int MaxLives = 10;

    // Name used in PlayerPrefs to persist heart count across sessions
    private const string PrefsKey = "RemainingLives";

    public static LifeManager Instance { get; private set; }

    /// <summary>Current remaining hearts (0–10).</summary>
    public int RemainingLives { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load persisted value; default to max on first run
        RemainingLives = PlayerPrefs.GetInt(PrefsKey, MaxLives);
        RemainingLives = Mathf.Clamp(RemainingLives, 0, MaxLives);
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
