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
    public float staminaDrainRate = 25f;
    public float staminaRechargeRate = 10f;
    public float minStaminaToSprint = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isSprinting;
    private float currentStamina;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!controller)
            Debug.LogError("PlayerMovement requires a CharacterController!");
    }

    void Start()
    {
        velocity = Vector3.zero;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleSprintInput();
        HandleStamina();
        HandleMovement();
    }

    #region Movement

    private void HandleMovement()
    {
        // Get raw input for instant response
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Determine current speed
        float speed = isSprinting ? sprintSpeed : moveSpeed;

        // Apply movement instantly (no slipping)
        Vector3 move = inputDir * speed;

        // Gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f; // Small downward force to stay grounded

        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal movement with vertical velocity
        Vector3 finalMove = move + new Vector3(0, velocity.y, 0);

        controller.Move(finalMove * Time.deltaTime);
    }

    #endregion

    #region Sprint & Stamina

    private void HandleSprintInput()
    {
        isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentStamina >= minStaminaToSprint;
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
            currentStamina = Mathf.Clamp(currentStamina + staminaRechargeRate * Time.deltaTime, 0f, maxStamina);
        }
    }

    public float GetStaminaPercentage() => currentStamina / maxStamina;

    #endregion

    #region Reset Player

    public void ResetPlayer()
    {
        velocity = Vector3.zero;
        isSprinting = false;
        currentStamina = maxStamina;

        if (spawnPoint)
            transform.position = spawnPoint.position;

        controller.enabled = false;
        controller.enabled = true;
    }

    #endregion
}
