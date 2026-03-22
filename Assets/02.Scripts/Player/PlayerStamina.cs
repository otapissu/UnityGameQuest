using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;

    public float regenRate = 15f;
    public float regenDelay = 1.5f;

    public float jumpCost = 10f;
    public float runCostPerSecond = 20f;

    private float lastUseTime;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (Time.time - lastUseTime < regenDelay)
        {
            return;
        }

        if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina < amount)
        {
            return false;
        }

        currentStamina -= amount;
        lastUseTime = Time.time;

        return true;
    }
}