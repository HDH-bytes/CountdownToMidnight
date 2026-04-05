#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;

/// <summary>
/// Unity Editor tool: Tools > Setup Level4 Timer UI
/// Open Level4.unity first, then run this. It builds the full Canvas hierarchy
/// and wires all references to Level4Timer automatically.
/// </summary>
public static class Level4TimerSetup
{
    // [MenuItem("Tools/Setup Level4 Timer UI")]
    public static void SetupLevel4TimerUI()
    {
        // ── Guard: must be in Level4 ──────────────────────────────────────────
        var activeScene = EditorSceneManager.GetActiveScene();
        if (!activeScene.name.Equals("Level4", System.StringComparison.OrdinalIgnoreCase))
        {
            EditorUtility.DisplayDialog("Wrong Scene",
                "Please open Level4.unity first, then run Tools > Setup Level4 Timer UI.", "OK");
            return;
        }

        // ── Remove old setup if re-running ────────────────────────────────────
        var oldController = GameObject.Find("Level4TimerController");
        if (oldController != null) Undo.DestroyObjectImmediate(oldController);

        var oldCanvas = GameObject.Find("Level4TimerCanvas");
        if (oldCanvas != null) Undo.DestroyObjectImmediate(oldCanvas);

        // ─────────────────────────────────────────────────────────────────────
        // 1. CANVAS
        // ─────────────────────────────────────────────────────────────────────
        var canvasGO = new GameObject("Level4TimerCanvas");
        Undo.RegisterCreatedObjectUndo(canvasGO, "Create Level4 Timer Canvas");

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        // Find the main camera (CinemachineBrain or tagged MainCamera)
        Camera mainCam = Camera.main;
        if (mainCam != null) canvas.worldCamera = mainCam;
        else Debug.LogWarning("[Level4TimerSetup] Main Camera not found — assign Render Camera manually.");

        canvas.planeDistance = 1f;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1f;

        canvasGO.AddComponent<GraphicRaycaster>();

        // ─────────────────────────────────────────────────────────────────────
        // 2. TIMER HUD (top-right, semi-transparent)
        // ─────────────────────────────────────────────────────────────────────
        var hudGO = CreateUIPanel(canvasGO.transform, "TimerHUD",
            new Vector2(300, 75),
            new Vector2(1, 1), new Vector2(1, 1),  // anchor top-right
            new Vector2(-155, -45),                 // position offset
            new Color(0f, 0f, 0f, 0.45f));

        // Timer TMP_Text inside HUD
        var timerTextGO = new GameObject("TimerText");
        timerTextGO.transform.SetParent(hudGO.transform, false);
        var timerTMP = timerTextGO.AddComponent<TextMeshProUGUI>();
        timerTMP.text = "24.000";
        timerTMP.fontSize = 42;
        timerTMP.fontStyle = FontStyles.Bold;
        timerTMP.color = Color.white;
        timerTMP.alignment = TextAlignmentOptions.Center;
        var timerRect = timerTextGO.GetComponent<RectTransform>();
        timerRect.anchorMin = Vector2.zero;
        timerRect.anchorMax = Vector2.one;
        timerRect.sizeDelta = Vector2.zero;
        timerRect.offsetMin = new Vector2(4, 4);
        timerRect.offsetMax = new Vector2(-4, -4);

        // ─────────────────────────────────────────────────────────────────────
        // 3. TIME-OUT PANEL (centered, hidden by default)
        // ─────────────────────────────────────────────────────────────────────
        var panelGO = CreateUIPanel(canvasGO.transform, "TimeOutPanel",
            new Vector2(640, 360),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero,
            new Color(0.08f, 0f, 0f, 0.92f));
        panelGO.SetActive(false); // hidden at start

        // "TIME IS OUT!!!" label
        var labelGO = new GameObject("TimeOutLabel");
        labelGO.transform.SetParent(panelGO.transform, false);
        var labelTMP = labelGO.AddComponent<TextMeshProUGUI>();
        labelTMP.text = "TIME IS OUT!!!";
        labelTMP.fontSize = 72;
        labelTMP.fontStyle = FontStyles.Bold;
        labelTMP.color = new Color(1f, 0.15f, 0.15f);
        labelTMP.alignment = TextAlignmentOptions.Center;
        var labelRect = labelGO.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0.55f);
        labelRect.anchorMax = new Vector2(1, 1f);
        labelRect.sizeDelta = Vector2.zero;
        labelRect.offsetMin = new Vector2(10, 0);
        labelRect.offsetMax = new Vector2(-10, -10);

        // Retry Button
        var retryBtnGO = new GameObject("RetryButton");
        retryBtnGO.transform.SetParent(panelGO.transform, false);
        var retryImg = retryBtnGO.AddComponent<Image>();
        retryImg.color = new Color(0.85f, 0.1f, 0.1f);
        var retryBtn = retryBtnGO.AddComponent<Button>();
        var retryRect = retryBtnGO.GetComponent<RectTransform>();
        retryRect.anchorMin = new Vector2(0.25f, 0.1f);
        retryRect.anchorMax = new Vector2(0.75f, 0.45f);
        retryRect.sizeDelta = Vector2.zero;

        // Button label
        var retryLabelGO = new GameObject("RetryLabel");
        retryLabelGO.transform.SetParent(retryBtnGO.transform, false);
        var retryLabelTMP = retryLabelGO.AddComponent<TextMeshProUGUI>();
        retryLabelTMP.text = "Retry";
        retryLabelTMP.fontSize = 48;
        retryLabelTMP.fontStyle = FontStyles.Bold;
        retryLabelTMP.color = Color.white;
        retryLabelTMP.alignment = TextAlignmentOptions.Center;
        var retryLabelRect = retryLabelGO.GetComponent<RectTransform>();
        retryLabelRect.anchorMin = Vector2.zero;
        retryLabelRect.anchorMax = Vector2.one;
        retryLabelRect.sizeDelta = Vector2.zero;

        // ─────────────────────────────────────────────────────────────────────
        // 4. LEVEL4TIMERCONTROLLER — attach Level4Timer and wire refs
        // ─────────────────────────────────────────────────────────────────────
        var controllerGO = new GameObject("Level4TimerController");
        Undo.RegisterCreatedObjectUndo(controllerGO, "Create Level4TimerController");

        var timer = controllerGO.AddComponent<Level4Timer>();
        timer.timerText    = timerTMP;
        timer.timeOutPanel = panelGO;
        timer.startTime    = 24f;

        // Wire Retry button OnClick → Level4Timer.OnRetry()
        var retryEntry = new UnityEngine.Events.UnityAction(timer.OnRetry);
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(retryBtn.onClick, retryEntry);

        // ─────────────────────────────────────────────────────────────────────
        // 5. Mark scene dirty and save
        // ─────────────────────────────────────────────────────────────────────
        EditorSceneManager.MarkSceneDirty(activeScene);
        EditorSceneManager.SaveScene(activeScene);

        Debug.Log("[Level4TimerSetup] ✅ Done! Canvas, TimerHUD, TimeOutPanel, and Level4Timer all created and wired.");
        EditorUtility.DisplayDialog("Done!",
            "Level4 Timer UI is fully set up!\n\n" +
            "• TimerHUD is anchored top-right\n" +
            "• TimeOutPanel is hidden by default\n" +
            "• Retry button wired to Level4Timer.OnRetry()\n\n" +
            "Press Play to test.", "OK");
    }

    // ── Helper: create a UI panel ─────────────────────────────────────────────
    static GameObject CreateUIPanel(Transform parent, string name,
        Vector2 size, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 anchoredPos, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.sizeDelta = size;
        rt.anchoredPosition = anchoredPos;
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        return go;
    }
}
#endif
