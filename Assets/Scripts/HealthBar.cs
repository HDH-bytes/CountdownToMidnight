using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public TextMeshProUGUI playerHealtBarValueText;

    public int maxHealth;
    public int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        playerHealtBarValueText.text = currentHealth + "/" + maxHealth.ToString();

        healthBarSlider.value = currentHealth;
        healthBarSlider.maxValue = maxHealth;
    }
}
