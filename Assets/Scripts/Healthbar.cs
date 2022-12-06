using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider _healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI _healthText;
    private string healthText = "N";
    private string maxHealthText = "A";


    public void setMaxHealth(int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
        maxHealthText = ((int)maxHealth).ToString();
        _healthText.text = healthText + " / " + maxHealthText;
    }

    public void setHealth(int health)
    {
        _healthSlider.value = health;
        healthText = ((int)health).ToString();
        _healthText.text = healthText + " / " + maxHealthText;
    }
}
