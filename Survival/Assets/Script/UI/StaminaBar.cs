using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI staminaText;
    
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
        UpdateStaminaText(stamina);
    }

    public void SetCurrentStamina(float stamina)
    {
        slider.value = stamina;
        UpdateStaminaText(stamina);
    }

    private void UpdateStaminaText(float stamina)
    {
        staminaText.text = $"{stamina}/{slider.maxValue}";
    }
}