using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Level 4 only — 48-second countdown timer with millisecond display.
/// Shows a "TIME IS OUT!!!" panel with a Retry button when time expires.
/// Deducts one heart on time-out. If hearts reach 0 the player is sent to StartScene.
/// Attach this script to any GameObject in the Level4 scene.
/// </summary>
public class Level4Timer : MonoBehaviour
{
    [Header("Timer HUD")]
    [Tooltip("The TMP_Text that shows the countdown (e.g. '46.503')")]
    public TMP_Text timerText;

    [Header("Time Out Panel")]
    [Tooltip("The panel that appears when time runs out — disabled by default")]
    public GameObject timeOutPanel;

    [Header("Settings")]
    public float startTime = 48f;

    /// <summary>True once the timer hits zero this session. Reset when the scene reloads.</summary>
    public static bool IsExpired { get; private set; }

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        IsExpired = false; // reset each time the scene loads fresh
        timeRemaining = startTime;

        // Safety: wire the button in code dynamically, bypassing Inspector bugs
        if (timeOutPanel != null)
        {
            Button btn = timeOutPanel.GetComponentInChildren<Button>(true);
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnRetry);
            }
            timeOutPanel.SetActive(false); // Make sure the panel starts hidden
        }
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            UpdateDisplay(0f);
            ShowTimeOut();
            return;
        }

        UpdateDisplay(timeRemaining);
    }

    void UpdateDisplay(float t)
    {
        int seconds      = Mathf.FloorToInt(t);
        int milliseconds = Mathf.FloorToInt((t - seconds) * 1000f);

        if (timerText != null)
            timerText.text = string.Format("{0:00}.{1:000}", seconds, milliseconds);
    }

    void ShowTimeOut()
    {
        IsExpired = true;

        // Close any open shopkeeper panel so the player can't slip in an upgrade
        ShopKeeper shop = FindAnyObjectByType<ShopKeeper>();
        if (shop != null) shop.ForceClose();

        // Deduct a heart
        EnsureLifeManager();
        LifeManager.Instance.LoseHeart();

        // If hearts are now 0, LoseHeart() already navigated to StartScene
        if (LifeManager.Instance.RemainingLives <= 0) return;

        Time.timeScale = 0f; // freeze the game
        if (timeOutPanel != null)
            timeOutPanel.SetActive(true);
    }

    /// <summary>
    /// Wired to the Retry button's OnClick in the Inspector.
    /// Reloads Level 4 and automatically resets the timer (fresh scene = fresh Start()).
    /// </summary>
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // -------------------------------------------------------------------------

    private static void EnsureLifeManager()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();
    }
}
