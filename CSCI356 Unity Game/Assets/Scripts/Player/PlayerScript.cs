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

    private Vector3 slideVelocity;


    [Header("Sprint & Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 25f; // Stamina per second while sprinting
    public float staminaRechargeRate = 10f; // Stamina per second when not sprinting
    public float minStaminaToSprint = 10f;

    [Header("Sliding Settings")]
    [Tooltip("Layer assigned to slippery surfaces")]
    public LayerMask iceLayer;
    [Tooltip("How much momentum is gained from ice")]
    public float iceSlideFactor = 5f;

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
        Time.timeScale = 1f;
        velocity = Vector3.zero;
        slideVelocity = Vector3.zero;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMovement();
        HandleSprintInput();
        HandleStamina();
    }

    #region Movement

    // Allows external scripts (like Ice) to add sliding momentum
    public void AddSlide(Vector3 direction, float strength)
    {
        slideVelocity += direction * strength * Time.deltaTime;
        }


    private void HandleMovement()
    {
        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        float speed = isSprinting ? sprintSpeed : moveSpeed;

        // Check if on ice
        bool onIce = Physics.CheckSphere(groundCheck.position, groundCheckDistance, iceLayer);

        // Apply ice sliding
        if (onIce)
        {
            slideVelocity += move * iceSlideFactor * Time.deltaTime;
        }
        else
        {
            // Decay sliding when not on ice
            slideVelocity = Vector3.Lerp(slideVelocity, Vector3.zero, Time.deltaTime * 5f);
        }

        // Combine normal movement and sliding
        Vector3 totalMove = move * speed + slideVelocity;

        // Gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        totalMove += new Vector3(0, velocity.y, 0);

        // Move the CharacterController
        controller.Move(totalMove * Time.deltaTime);
    }

    #endregion

    #region Sprint & Stamina

    private void HandleSprintInput()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && currentStamina >= minStaminaToSprint)
            isSprinting = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || currentStamina <= 0f)
            isSprinting = false;
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
        slideVelocity = Vector3.zero;
        isSprinting = false;
        currentStamina = maxStamina;

        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        controller.enabled = false;
        controller.enabled = true;
    }

    #endregion
}
