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

    private PlayerMovement _playerMovement;

    private void Start()
    {
        shopPanel.SetActive(false);

        speedButton.onClick.AddListener(UpgradeSpeed);
        healthButton.onClick.AddListener(UpgradeHealth);
        closeButton.onClick.AddListener(CloseShop);
    }

    //called when the player's trigger collider enters thes collider range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerMovement = other.GetComponent<PlayerMovement>();
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

        CloseShop();
    }

    private void UpgradeHealth()
    {
        // Teammate is implementing health
        CloseShop();
    }

    private void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}