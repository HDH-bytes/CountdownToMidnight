using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Self-injecting hearts HUD.
/// RuntimeInitializeOnLoadMethod automatically spawns this into every level
/// scene (any scene that is NOT named "StartScene" or "ChooseCharacter").
/// No GameObject, no Canvas, no Inspector wiring needed in any scene.
/// </summary>
public class HeartsUI : MonoBehaviour
{
    // Scenes where hearts must NOT appear
    private static readonly HashSet<string> ExcludedScenes = new HashSet<string>
    {
        "StartScene",
        "ChooseCharacter"
    };

    [Header("Layout (optional tweaks in Inspector)")]
    [SerializeField] private float heartSize    = 32f;
    [SerializeField] private float heartSpacing =  6f;
    [SerializeField] private Vector2 padding    = new Vector2(12f, 12f);

    // ---------------------------------------------------------------
    // AUTO-INJECT — fires on EVERY scene load including transitions
    // ---------------------------------------------------------------

    // BeforeSceneLoad runs at game boot so the event is subscribed
    // before the first scene (StartScene) even finishes loading.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Subscribe once; Unity keeps this alive for the whole session
        SceneManager.sceneLoaded -= OnSceneLoaded; // guard against domain reload
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ExcludedScenes.Contains(scene.name)) return;

        // Don't create a second one if one already exists (scene reload etc.)
        if (FindAnyObjectByType<HeartsUI>() != null) return;

        GameObject go = new GameObject("HeartsUI_AutoInjected");
        go.AddComponent<HeartsUI>();
    }

    private readonly List<Image> _hearts = new List<Image>();
    private Texture2D _heartTex; // instance ref prevents GC collection

    // ------------------------------------------------------------------

    void Awake()
    {
        EnsureLifeManager();
        BuildHUD();
    }

    void Start()
    {
        Refresh();
    }

    void OnDestroy()
    {
        if (_heartTex != null) Destroy(_heartTex);
    }

    /// <summary>Show/hide hearts to match LifeManager.RemainingLives.</summary>
    public void Refresh()
    {
        int lives = LifeManager.Instance != null
            ? LifeManager.Instance.RemainingLives
            : LifeManager.MaxLives;

        for (int i = 0; i < _hearts.Count; i++)
            if (_hearts[i] != null)
                _hearts[i].gameObject.SetActive(i < lives);
    }

    // ------------------------------------------------------------------
    // BUILD
    // ------------------------------------------------------------------

    private void BuildHUD()
    {
        // 1 — Canvas (Screen Space Overlay keeps hearts fixed in the corner
        //     regardless of where the Cinemachine camera is looking)
        GameObject canvasGO = new GameObject("HeartsCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode  = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // 2 — Panel anchored to top-left
        GameObject panelGO = new GameObject("HeartsPanel");
        panelGO.transform.SetParent(canvasGO.transform, false);

        RectTransform panelRT = panelGO.AddComponent<RectTransform>();
        panelRT.anchorMin        = new Vector2(0f, 1f);
        panelRT.anchorMax        = new Vector2(0f, 1f);
        panelRT.pivot            = new Vector2(0f, 1f);
        panelRT.anchoredPosition = new Vector2(padding.x, -padding.y);

        float totalW = LifeManager.MaxLives * heartSize
                     + (LifeManager.MaxLives - 1) * heartSpacing;
        panelRT.sizeDelta = new Vector2(totalW, heartSize);

        HorizontalLayoutGroup layout = panelGO.AddComponent<HorizontalLayoutGroup>();
        layout.spacing              = heartSpacing;
        layout.childForceExpandWidth  = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment       = TextAnchor.UpperLeft;
        layout.padding              = new RectOffset(0, 0, 0, 0);

        // 3 — Heart sprite (procedural red heart, built once per instance)
        Sprite heartSprite = BuildHeartSprite();

        // 4 — 10 heart Image children
        for (int i = 0; i < LifeManager.MaxLives; i++)
        {
            GameObject hGO = new GameObject("Heart" + (i + 1));
            hGO.transform.SetParent(panelGO.transform, false);

            Image img = hGO.AddComponent<Image>();
            img.sprite = heartSprite;
            img.preserveAspect = true;

            LayoutElement le = hGO.AddComponent<LayoutElement>();
            le.minWidth        = heartSize;
            le.minHeight       = heartSize;
            le.preferredWidth  = heartSize;
            le.preferredHeight = heartSize;

            _hearts.Add(img);
        }
    }

    // ------------------------------------------------------------------
    // PROCEDURAL HEART TEXTURE
    // ------------------------------------------------------------------

    private Sprite BuildHeartSprite()
    {
        const int S = 64;
        _heartTex = new Texture2D(S, S, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode   = TextureWrapMode.Clamp,
            name       = "ProceduralHeart"
        };

        Color heartRed = new Color(0.93f, 0.10f, 0.13f, 1f);
        Color empty    = Color.clear;

        for (int py = 0; py < S; py++)
        {
            for (int px = 0; px < S; px++)
            {
                // Unity SetPixel: py=0 is bottom of the texture on screen.
                // Heart SDF: positive y = top (bumps), negative y = bottom (point).
                // So map py directly — no flip needed.
                float x = (px / (float)(S - 1)) * 2.6f - 1.3f;
                float y = (py / (float)(S - 1)) * 2.6f - 1.3f;

                _heartTex.SetPixel(px, py, HeartSDF(x, y) <= 0f ? heartRed : empty);
            }
        }
        _heartTex.Apply();

        return Sprite.Create(_heartTex,
            new Rect(0, 0, S, S),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit: 100f);
    }

    /// <summary>Returns ≤ 0 inside the classic heart curve.</summary>
    private static float HeartSDF(float x, float y)
    {
        float a = x * x + y * y - 1f;
        return a * a * a - x * x * (y * y * y);
    }

    // ------------------------------------------------------------------

    private static void EnsureLifeManager()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();
    }
}
