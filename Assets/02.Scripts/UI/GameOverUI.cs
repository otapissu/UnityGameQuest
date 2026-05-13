using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private PlayerInput playerInput;

    public bool IsGameOver { get; private set; }

    public void Show()
    {
        IsGameOver = true;
        playerInput.enabled = false;

        foreach (Transform child in uiCanvas.transform)
        {
            if (child.gameObject == gameOverText)
            {
                continue;
            }

            child.gameObject.SetActive(false);
        }

        gameOverText.SetActive(true);
    }

    public void Hide()
    {
        IsGameOver = false;
        playerInput.enabled = true;

        foreach (Transform child in uiCanvas.transform)
        {
            if (child.gameObject == gameOverText)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            child.gameObject.SetActive(true);
        }
    }
}
