using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateHealthText(health);
    }

    public void SetCurrentHealth(float health)
    {
        slider.value = health;
        UpdateHealthText(health);
    }

    private void UpdateHealthText(float health)
    {
        healthText.text = $"{health}/{slider.maxValue}";
    }
}