using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth health;
    public Slider slider;

    private void Update()
    {
        if (health == null || slider == null)
        {
            return;
        }

        slider.value = health.currentHp / health.maxHp;
    }
}
