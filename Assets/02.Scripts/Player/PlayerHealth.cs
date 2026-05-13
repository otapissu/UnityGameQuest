using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    [SerializeField] private GameOverUI gameOverUI;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + amount);
    }

    public void TakeDamage(float amount)
    {
        currentHp = Mathf.Max(0f, currentHp - amount);

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameOverUI != null)
        {
            gameOverUI.Show();
        }
    }
}
