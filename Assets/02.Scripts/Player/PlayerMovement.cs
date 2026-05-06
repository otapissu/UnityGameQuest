using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 12f;

    [Header("Air Control")]
    public float airControlForce = 5f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public int maxJumpCount = 2;

    [Header("Ground Check")]
    public float rayDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Transform modelTransform;
    private Rigidbody rb;
    private PlayerStamina stamina;

    private Vector2 moveInput;
    private bool isRunning;
    private bool jumpPressed;

    private bool isGrounded;
    private bool wasGrounded;
    private int currentJumpCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stamina = GetComponent<PlayerStamina>();

        Animator foundAnimator = GetComponentInChildren<Animator>();

        if (foundAnimator != null)
        {
            modelTransform = foundAnimator.transform;
        }
        else
        {
            Debug.LogError("Animator 못 찾음");
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        CheckGround();
        HandleMove();
        UpdateAnimation();
        HandleJump();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpPressed = true;
        }
    }

    private void HandleMove()
    {
        if (cameraTransform == null)
        {
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        float speed = isRunning ? runSpeed : walkSpeed;

        if (isRunning == true && stamina != null)
        {
            if (stamina.UseStamina(20f * Time.fixedDeltaTime) == false)
            {
                isRunning = false;
            }
        }

        if (moveDir.sqrMagnitude <= 0.001f)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
            return;
        }

        moveDir.Normalize();

        Vector3 currentVelocity = rb.linearVelocity;

        if (isGrounded == true)
        {
            rb.linearVelocity = new Vector3
            (
                moveDir.x * speed,
                currentVelocity.y,
                moveDir.z * speed
            );
        }
        else
        {
            Vector3 airForce = moveDir * speed * 2f;

            rb.AddForce(airForce, ForceMode.Acceleration);

            Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (horizontal.magnitude > speed)
            {
                horizontal = horizontal.normalized * speed;

                rb.linearVelocity = new Vector3(
                    horizontal.x,
                    rb.linearVelocity.y,
                    horizontal.z
                );
            }
        }

        RotateTowards(moveDir);
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(direction);

        modelTransform.rotation = Quaternion.Slerp
        (
            modelTransform.rotation,
            targetRot,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void HandleJump()
    {
        if (jumpPressed == false)
        {
            return;
        }

        jumpPressed = false;

        if (isGrounded == true)
        {
            currentJumpCount = 0;
            DoJump();
            return;
        }

        if (currentJumpCount >= maxJumpCount)
        {
            return;
        }

        if (stamina != null)
        {
            bool canJump = stamina.UseStamina(10f);

            if (canJump == false)
            {
                return;
            }
        }

        DoJump();
        currentJumpCount++;
    }

    private void DoJump()
    {
        rb.linearVelocity = new Vector3
        (
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void CheckGround()
    {
        Vector3 rayStartPos = transform.position + Vector3.up * 0.1f;

        bool hit = Physics.Raycast
        (
            rayStartPos,
            Vector3.down,
            rayDistance,
            groundLayer
        );

        if (hit == true && wasGrounded == false && rb.linearVelocity.y <= 0f)
        {
            currentJumpCount = 0;
        }

        isGrounded = hit;
        wasGrounded = hit;
    }

    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        float speed = new Vector3
        (
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        ).magnitude;

        animator.SetFloat("Speed", speed, 0.1f, Time.fixedDeltaTime);
        animator.SetBool("isGrounded", isGrounded);
    }
}