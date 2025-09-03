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

    [Header("Sliding Settings")]
    [Tooltip("Layer assigned to slippery surfaces")]
    public LayerMask iceLayer;
    [Tooltip("How much momentum is gained from ice")]
    public float iceSlideFactor = 5f;

    [Header("Sprint & Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 25f;
    public float staminaRechargeRate = 10f;
    public float minStaminaToSprint = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 slideVelocity;
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
        slideVelocity = Vector3.zero;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleSprintInput();
        HandleStamina();
        HandleMovement();
    }

    #region Movement

    public void AddSlide(Vector3 direction, float strength)
    {
        // Add momentum in a frame-independent way
        slideVelocity += direction.normalized * strength * Time.deltaTime;
    }

    private void HandleMovement()
{
    // Input
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    Vector3 inputDir = transform.right * horizontal + transform.forward * vertical;
    inputDir = inputDir.normalized;

    float speed = isSprinting ? sprintSpeed : moveSpeed;

    // Check if on ice
    bool onIce = Physics.CheckSphere(groundCheck.position, groundCheckDistance, iceLayer);

    if (onIce)
    {
        // Instead of instantly applying input, blend it with current slide velocity
        Vector3 targetVelocity = inputDir * speed;
        slideVelocity = Vector3.Lerp(slideVelocity, targetVelocity, Time.deltaTime * 2f); // 2f = turning smoothness
    }
    else
    {
        // Normal movement off ice
        slideVelocity = Vector3.Lerp(slideVelocity, inputDir * speed, Time.deltaTime * 10f);
    }

    // Gravity
    if (controller.isGrounded && velocity.y < 0f)
        velocity.y = -2f;

    velocity.y += gravity * Time.deltaTime;

    // Combine sliding and gravity
    Vector3 totalMove = slideVelocity + new Vector3(0, velocity.y, 0);

    controller.Move(totalMove * Time.deltaTime);
}


    #endregion

    #region Sprint & Stamina

    private void HandleSprintInput()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentStamina >= minStaminaToSprint)
            isSprinting = true;
        else
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

    public float GetStaminaPercentage() => currentStamina / maxStamina;

    #endregion

    #region Reset Player

    public void ResetPlayer()
    {
        velocity = Vector3.zero;
        slideVelocity = Vector3.zero;
        isSprinting = false;
        currentStamina = maxStamina;

        if (spawnPoint)
            transform.position = spawnPoint.position;

        controller.enabled = false;
        controller.enabled = true;
    }

    #endregion
}
