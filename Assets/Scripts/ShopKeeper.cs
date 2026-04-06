using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopKeeper : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopPanel;
    public Button speedButton;
    public Button healthButton;
    public Button closeButton;

    [Header("Speed Upgrade")]
    public float speedIncrease = 2f;

    // Optional: assign in Inspector, otherwise created automatically
    [Header("Warning Text (auto-created if left empty)")]
    public TMP_Text warningText;

    private PlayerMovement _playerMovement;
    private bool _upgradeBought;  //one pick locks both options

    private void Start()
    {
        shopPanel.SetActive(false);

        // Auto-create warning label if not assigned
        if (warningText == null)
            warningText = CreateWarningLabel();

        warningText.gameObject.SetActive(false);

        speedButton.onClick.AddListener(UpgradeSpeed);
        healthButton.onClick.AddListener(UpgradeHealth);
        closeButton.onClick.AddListener(CloseShop);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerMovement = other.GetComponent<PlayerMovement>();
        speedButton.interactable = !_upgradeBought;
        healthButton.interactable = !_upgradeBought;
        shopPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerMovement = null;
        CloseShop();
    }

    /// <summary>Called by Level4Timer when time expires to dismiss the shop immediately.</summary>
    public void ForceClose()
    {
        CloseShop();
    }

    private void UpgradeSpeed()
    {
        if (Level4Timer.IsExpired) return; // time ran out — no upgrades allowed

        if (_playerMovement != null)
            _playerMovement.IncreaseSpeed(speedIncrease);

        LockShop();
    }

    private void UpgradeHealth()
    {
        if (Level4Timer.IsExpired) return; // time ran out — no upgrades allowed

        EnsureLifeManager();

        if (LifeManager.Instance.RemainingLives >= LifeManager.MaxLives)
        {
            // Already full — show warning, do NOT lock the shop
            ShowWarning("Health is full!!!");
            return;
        }

        // Add one heart
        LifeManager.Instance.GainHeart();

        HeartsUI ui = FindAnyObjectByType<HeartsUI>();
        if (ui != null) ui.Refresh();

        HideWarning();
        LockShop();
    }

    private void LockShop()
    {
        _upgradeBought = true;
        speedButton.interactable = false;
        healthButton.interactable = false;
        CloseShop();
    }

    private void CloseShop()
    {
        HideWarning();
        shopPanel.SetActive(false);
    }

    // ---------------------------------------------------------------
    // Warning helpers
    // ---------------------------------------------------------------

    private void ShowWarning(string message)
    {
        if (warningText == null) return;
        warningText.text = message;
        warningText.gameObject.SetActive(true);
    }

    private void HideWarning()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    private TMP_Text CreateWarningLabel()
    {
        GameObject go = new GameObject("WarningText");
        go.transform.SetParent(shopPanel.transform, false);

        TMP_Text txt = go.AddComponent<TextMeshProUGUI>();
        txt.text      = "";
        txt.fontSize  = 22;
        txt.color     = new Color(0.93f, 0.10f, 0.13f, 1f); // red
        txt.alignment = TextAlignmentOptions.Center;

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0f);
        rt.anchorMax        = new Vector2(0.5f, 0f);
        rt.pivot            = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(0f, 8f);   // just above the bottom edge
        rt.sizeDelta        = new Vector2(300f, 36f);

        return txt;
    }

    // ---------------------------------------------------------------

    private static void EnsureLifeManager()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();
    }
}