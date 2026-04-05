using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI References")]
    public Slider xpBar;
    public TMP_Text rankLabel;
    public TMP_Text xpLabel;  

    //name = rank name, xpRequired = total XP neede
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
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        _totalXP += amount;
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
            int from    = Ranks[rankIndex].xpRequired;
            int to      = Ranks[rankIndex + 1].xpRequired;
            int intoRank = _totalXP - from;
            xpBar.value  = (float)intoRank / (to - from);
            xpLabel.text = $"{_totalXP} / {to} XP";
        }
    }
}