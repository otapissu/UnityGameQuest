using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform pivot;

    public float sensitivity = 10f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private Vector2 lookInput;

    [SerializeField] private Camera thirdPersonCam;
    [SerializeField] private Camera firstPersonCam;

    private bool isFirstPerson = false;

    public float minPitch = -25f;
    public float maxPitch = 70f;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnViewSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFirstPerson = !isFirstPerson;

            thirdPersonCam.gameObject.SetActive(!isFirstPerson);
            firstPersonCam.gameObject.SetActive(isFirstPerson);
        }
    }

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}