using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera; // Assign your camera here
    public float xRotationLimit = 80f; // Prevents over-rotation up/down
    
    [Header("Sprint Settings")]
    public bool isSprinting = false;
    public float maxStamina = 100f;
    public float staminaDrainRate = 25f; // Stamina per second while sprinting
    public float staminaRechargeRate = 10f; // Stamina per second while not sprinting
    public float minStaminaToSprint = 10f; // Minimum stamina needed to start sprinting
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private float currentStamina;
    private float xRotation = 0f; // For camera up/down rotation
    
    public Transform groundCheck; // Assign this in the Inspector
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("No CharacterController attached to the player!");
        }
        
        // Find camera if not assigned - prioritize child camera
        if (playerCamera == null)
        {
            // First try to find camera as a child
            playerCamera = GetComponentInChildren<Camera>()?.transform;
            
            // If no child camera, try main camera
            if (playerCamera == null)
            {
                playerCamera = Camera.main?.transform;
                if (playerCamera != null)
                {
                    Debug.LogWarning("Using Camera.main - consider making camera a child of player for better FPS control");
                }
            }
            
            if (playerCamera == null)
            {
                Debug.LogError("No camera found! Please assign a camera or make sure you have a Camera component.");
                return;
            }
        }
        
        // Ensure camera starts with no rotation relative to player
        if (playerCamera.parent == transform)
        {
            playerCamera.localRotation = Quaternion.identity;
            xRotation = 0f;
        }
        
        currentSpeed = moveSpeed;
        currentStamina = maxStamina; // Start with full stamina
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // --- Cursor Control (check first) ---
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
        
        // --- Mouse Look (only when cursor is locked) ---
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
        }
        
        // --- Sprint Input ---
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            ToggleSprint();
        }
        
        // --- Stamina Management ---
        HandleStamina();
        
        // --- Ground Check ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep player grounded
        }
        
        // --- Movement ---
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        
        // Use current speed (either normal or sprint speed)
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void HandleMouseLook()
    {
        if (playerCamera == null || Cursor.lockState != CursorLockMode.Locked) return;
        
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Rotate player body left/right (Y-axis)
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate camera up/down (X-axis) - since camera is child, only rotate locally
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Cursor Unlocked - Press ESC again to lock");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Cursor Locked");
        }
    }
    
    void ToggleSprint()
    {
        if (!isSprinting && currentStamina >= minStaminaToSprint)
        {
            // Start sprinting if we have enough stamina
            isSprinting = true;
            currentSpeed = sprintSpeed;
            Debug.Log("Sprint ON");
        }
        else if (isSprinting)
        {
            // Stop sprinting
            isSprinting = false;
            currentSpeed = moveSpeed;
            Debug.Log("Sprint OFF");
        }
        else
        {
            Debug.Log("Not enough stamina to sprint!");
        }
    }
    
    void HandleStamina()
    {
        if (isSprinting)
        {
            // Drain stamina while sprinting
            currentStamina -= staminaDrainRate * Time.deltaTime;
            // Stop sprinting if we run out of stamina
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isSprinting = false;
                currentSpeed = moveSpeed;
                Debug.Log("Sprint OFF - Out of stamina!");
            }
        }
        else
        {
            // Recharge stamina when not sprinting
            currentStamina += staminaRechargeRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
        Debug.Log("Current Stamina: " + currentStamina + " / " + maxStamina + " (" + GetStaminaPercentage() + "%)");
    }
    
    // Optional: Method to get stamina percentage for UI
    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }
}