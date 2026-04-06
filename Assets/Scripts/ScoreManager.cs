using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI References")]
    public Slider xpBar;
    public TMP_Text rankLabel;
    public TMP_Text xpLabel;

    // Scenes where the XP bar must be hidden
    private static readonly System.Collections.Generic.HashSet<string> HiddenScenes
        = new System.Collections.Generic.HashSet<string> { "StartScene", "ChooseCharacter" };

    //name = rank name, xpRequired = total XP needed
    static readonly (string name, int xpRequired)[] Ranks =
    {
        ("Rookie",       0),
        ("Soldier",     50),
        ("Experienced", 125),
        ("Pro",         200),
        ("Legend",      300),
    };

    int _totalXP;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateUI();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hide the XP bar canvas on non-level scenes.
        // Use canvas.enabled (not gameObject.SetActive) so this MonoBehaviour
        // stays active and OnSceneLoaded keeps firing on future scene loads.
        Canvas c = GetXPCanvas();
        if (c != null)
            c.enabled = !HiddenScenes.Contains(scene.name);
    }

    /// <summary>Finds the Canvas that contains the XP bar.</summary>
    private Canvas GetXPCanvas()
    {
        if (xpBar != null)
            return xpBar.GetComponentInParent<Canvas>();
        return null;
    }

    public void AddXP(int amount)
    {
        _totalXP += amount;
        UpdateUI();
    }

    public void ResetXP()
    {
        _totalXP = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        int rankIndex = 0;
        for (int i = Ranks.Length - 1; i >= 0; i--)
        {
            if (_totalXP >= Ranks[i].xpRequired) { rankIndex = i; break; }
        }

        rankLabel.text = Ranks[rankIndex].name;

        if (rankIndex == Ranks.Length - 1)
        {
            xpBar.value = 1f;
            xpLabel.text = "MAX";
        }
        else
        {
            int from     = Ranks[rankIndex].xpRequired;
            int to       = Ranks[rankIndex + 1].xpRequired;
            int intoRank = _totalXP - from;
            xpBar.value  = (float)intoRank / (to - from);
            xpLabel.text = $"{_totalXP} / {to} XP";
        }
    }
}