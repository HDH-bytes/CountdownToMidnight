using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Level 4 only — 24-second countdown timer with millisecond display.
/// Shows a "TIME IS OUT!!!" panel with a Retry button when time expires.
/// Attach this script to any GameObject in the Level4 scene.
/// </summary>
public class Level4Timer : MonoBehaviour
{
    [Header("Timer HUD")]
    [Tooltip("The TMP_Text that shows the countdown (e.g. '23.415')")]
    public TMP_Text timerText;

    [Header("Time Out Panel")]
    [Tooltip("The panel that appears when time runs out — disabled by default")]
    public GameObject timeOutPanel;

    [Header("Settings")]
    public float startTime = 24f;

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = startTime;

        // Safety: make sure the panel is hidden at the start
        if (timeOutPanel != null)
            timeOutPanel.SetActive(false);
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
        Time.timeScale = 0f;                          // freeze the game
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
}
