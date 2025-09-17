using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float sprintSpeed = 1.6f;
    public float gravity = -9.81f;
    public float friction = 1.0f;
    public float playerSpeed = 1.0f;

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
    private Rigidbody charRigid;
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
        charRigid = GetComponent<Rigidbody>();
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
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        var vel = transform.TransformDirection(dir);

        // Determine current speed
        float speed = isSprinting ? sprintSpeed : moveSpeed;

        // Apply movement instantly (no slipping)
        Vector3 move = dir * speed;

        // Gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f; // Small downward force to stay grounded

        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal movement with vertical velocity
        Vector3 finalMove = move + new Vector3(0, velocity.y, 0);

        //controller.Move(finalMove * playerSpeed* Time.deltaTime);
        
        // Apply the friction factor and make movement
        velocity = Vector3.Lerp(velocity, vel*speed, 15*friction*friction*Time.deltaTime);
        controller.Move(velocity * playerSpeed * Time.deltaTime);
    }

    // For slowing effects
    public void StartSlow()
    {
        playerSpeed = 0.2f;
    }
    public void ResetSpeed()
    {
        playerSpeed = 1.0f;
    }

    // For slippery effects
    public void SlipperyFriction()
    {
        friction = 0.15f;
    }
    public void ResetFriction()
    {
        friction = 1.0f;
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
