using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopPanel;
    public Button speedButton;
    public Button healthButton;
    public Button closeButton;

    [Header("Speed Upgrade")]
    public float speedIncrease = 2f;

    private PlayerMovement _playerMovement;
    private bool _upgradeBought;  //one pick locks both options

    private void Start()
    {
        shopPanel.SetActive(false);

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

    private void UpgradeSpeed()
    {
        if (_playerMovement != null)
            _playerMovement.IncreaseSpeed(speedIncrease);

        LockShop();
    }

    private void UpgradeHealth()
    {
        // Teammate is implementing health
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
        shopPanel.SetActive(false);
    }
}