using UnityEngine;
using UnityEngine.InputSystem;

public class GameRestart : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameOverUI gameOverUI;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;
    private Enemy[] enemies;

    private void Start()
    {
        playerStartPosition = playerTransform.position;
        playerStartRotation = playerTransform.rotation;
        enemies = FindObjectsOfType<Enemy>(true);
    }

    private void Update()
    {
        if (!gameOverUI.IsGameOver)
        {
            return;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    private void Restart()
    {
        playerTransform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
        playerHealth.currentHp = playerHealth.maxHp;

        foreach (Enemy enemy in enemies)
        {
            enemy.ResetEnemy();
        }

        gameOverUI.Hide();
    }
}
