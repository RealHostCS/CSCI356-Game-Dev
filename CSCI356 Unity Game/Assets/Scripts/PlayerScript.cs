using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Sprint & Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 25f; // Stamina per second while sprinting
    public float staminaRechargeRate = 10f; // Stamina per second when not sprinting
    public float minStaminaToSprint = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isSprinting = false;
    private float currentStamina;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("PlayerMovement requires a CharacterController!");
    }

    void Start()
    {
        velocity = Vector3.zero;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMovement();
        HandleSprintInput();
        HandleStamina();
    }

    #region Movement

    private void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply movement
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f; // Small negative to stick to ground

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    #endregion

    #region Sprint & Stamina

    private void HandleSprintInput()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && currentStamina >= minStaminaToSprint)
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || currentStamina <= 0f)
        {
            isSprinting = false;
        }
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isSprinting = false;
            }
        }
        else
        {
            currentStamina += staminaRechargeRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }

    #endregion

    #region Reset Player

    public void ResetPlayer()
    {
        velocity = Vector3.zero;
        isSprinting = false;
        currentStamina = maxStamina;

        // Reset position
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        // Reset CharacterController collisions
        controller.enabled = false;
        controller.enabled = true;
    }

    #endregion
}
