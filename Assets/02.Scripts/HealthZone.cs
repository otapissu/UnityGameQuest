using UnityEngine;

public class HealthZone : MonoBehaviour
{
    [Tooltip("초당 회복량 (양수 = 힐, 음수 = 독 데미지)")]
    public float hpPerSecond = 5f;

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            return;
        }

        if (hpPerSecond >= 0f)
        {
            playerHealth.Heal(hpPerSecond * Time.deltaTime);
        }
        else
        {
            playerHealth.TakeDamage(-hpPerSecond * Time.deltaTime);
        }
    }
}
