using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    public GameObject menuUI;

    private bool isOpen = false;

    private void Start()
    {
        OpenMenu();
    }

    public void ToggleMenu()
    {
        if (isOpen == true)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        isOpen = true;

        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        isOpen = false;

        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}