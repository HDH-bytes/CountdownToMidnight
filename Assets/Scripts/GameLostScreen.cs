using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shown when the player's hearts reach zero.
/// Builds itself entirely in code — no scene setup needed.
/// Call GameLostScreen.Show() from LifeManager when lives hit 0.
/// </summary>
public class GameLostScreen : MonoBehaviour
{
    private static GameLostScreen _instance;

    // ---------------------------------------------------------------
    // STATIC ENTRY POINT
    // ---------------------------------------------------------------
    public static void Show()
    {
        // Spawn if not already in scene
        if (_instance == null)
        {
            GameObject go = new GameObject("GameLostScreen");
            _instance = go.AddComponent<GameLostScreen>();
        }
        _instance.gameObject.SetActive(true);
        _instance.Build();
        Time.timeScale = 0f;
    }

    // ---------------------------------------------------------------
    // BUILD UI
    // ---------------------------------------------------------------
    private void Build()
    {
        // Full-screen overlay canvas
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 300; // on top of everything
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        // Dark semi-transparent background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(transform, false);
        Image bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0f, 0f, 0f, 0.88f);
        RectTransform bgRT = bg.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        // "YOU LOST!!!" label
        GameObject titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(bg.transform, false);
        TextMeshProUGUI title = titleGO.AddComponent<TextMeshProUGUI>();
        title.text      = "YOU LOST!!!";
        title.fontSize  = 72;
        title.fontStyle = FontStyles.Bold;
        title.color     = new Color(0.93f, 0.10f, 0.13f, 1f); // red
        title.alignment = TextAlignmentOptions.Center;

        RectTransform titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchorMin        = new Vector2(0.5f, 0.5f);
        titleRT.anchorMax        = new Vector2(0.5f, 0.5f);
        titleRT.pivot            = new Vector2(0.5f, 0.5f);
        titleRT.anchoredPosition = new Vector2(0f, 80f);
        titleRT.sizeDelta        = new Vector2(700f, 120f);

        // "Play Again" button
        GameObject btnGO = new GameObject("PlayAgainButton");
        btnGO.transform.SetParent(bg.transform, false);

        Image btnImg = btnGO.AddComponent<Image>();
        btnImg.color = Color.white;

        Button btn = btnGO.AddComponent<Button>();
        btn.targetGraphic = btnImg;

        ColorBlock cb = btn.colors;
        cb.normalColor      = Color.white;
        cb.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        cb.pressedColor     = new Color(0.65f, 0.65f, 0.65f, 1f);
        btn.colors          = cb;

        btn.onClick.AddListener(OnPlayAgain);

        RectTransform btnRT = btnGO.GetComponent<RectTransform>();
        btnRT.anchorMin        = new Vector2(0.5f, 0.5f);
        btnRT.anchorMax        = new Vector2(0.5f, 0.5f);
        btnRT.pivot            = new Vector2(0.5f, 0.5f);
        btnRT.anchoredPosition = new Vector2(0f, -30f);
        btnRT.sizeDelta        = new Vector2(260f, 65f);

        // Button label
        GameObject btnLabelGO = new GameObject("ButtonLabel");
        btnLabelGO.transform.SetParent(btnGO.transform, false);
        TextMeshProUGUI btnLabel = btnLabelGO.AddComponent<TextMeshProUGUI>();
        btnLabel.text      = "Play Again";
        btnLabel.fontSize  = 36;
        btnLabel.fontStyle = FontStyles.Bold;
        btnLabel.color     = Color.black;
        btnLabel.alignment = TextAlignmentOptions.Center;

        RectTransform btnLabelRT = btnLabelGO.GetComponent<RectTransform>();
        btnLabelRT.anchorMin = Vector2.zero;
        btnLabelRT.anchorMax = Vector2.one;
        btnLabelRT.offsetMin = Vector2.zero;
        btnLabelRT.offsetMax = Vector2.zero;

        // "Quit" button — same size, placed directly below Play Again
        GameObject quitGO = new GameObject("QuitButton");
        quitGO.transform.SetParent(bg.transform, false);

        Image quitImg = quitGO.AddComponent<Image>();
        quitImg.color = Color.white;

        Button quitBtn = quitGO.AddComponent<Button>();
        quitBtn.targetGraphic = quitImg;

        ColorBlock qcb = quitBtn.colors;
        qcb.normalColor      = Color.white;
        qcb.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        qcb.pressedColor     = new Color(0.65f, 0.65f, 0.65f, 1f);
        quitBtn.colors       = qcb;

        quitBtn.onClick.AddListener(OnQuit);

        RectTransform quitRT = quitGO.GetComponent<RectTransform>();
        quitRT.anchorMin        = new Vector2(0.5f, 0.5f);
        quitRT.anchorMax        = new Vector2(0.5f, 0.5f);
        quitRT.pivot            = new Vector2(0.5f, 0.5f);
        quitRT.anchoredPosition = new Vector2(0f, -115f); // 65 height + 20 gap below Play Again
        quitRT.sizeDelta        = new Vector2(260f, 65f);

        GameObject quitLabelGO = new GameObject("QuitLabel");
        quitLabelGO.transform.SetParent(quitGO.transform, false);
        TextMeshProUGUI quitLabel = quitLabelGO.AddComponent<TextMeshProUGUI>();
        quitLabel.text      = "Quit";
        quitLabel.fontSize  = 36;
        quitLabel.fontStyle = FontStyles.Bold;
        quitLabel.color     = Color.black;
        quitLabel.alignment = TextAlignmentOptions.Center;

        RectTransform quitLabelRT = quitLabelGO.GetComponent<RectTransform>();
        quitLabelRT.anchorMin = Vector2.zero;
        quitLabelRT.anchorMax = Vector2.one;
        quitLabelRT.offsetMin = Vector2.zero;
        quitLabelRT.offsetMax = Vector2.zero;
    }

    // ---------------------------------------------------------------
    // BUTTON HANDLER
    // ---------------------------------------------------------------
    private void OnPlayAgain()
    {
        // Reset lives to 10 and XP to 0 for a fresh game
        if (LifeManager.Instance != null)
            LifeManager.Instance.ResetLives();

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetXP();

        Time.timeScale = 1f;
        _instance = null;
        SceneManager.LoadScene("StartScene");
    }

    private void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}
