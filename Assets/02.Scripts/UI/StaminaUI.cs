using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public PlayerStamina stamina;
    public Slider slider;

    private void Update()
    {
        if (stamina == null || slider == null)
        {
            return;
        }

        slider.value = stamina.currentStamina / stamina.maxStamina;
    }
}