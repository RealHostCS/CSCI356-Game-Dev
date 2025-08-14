using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;


public class PlayerMovement : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

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


    public Transform groundCheck; // Assign this in the Inspector

    void Start()
    {


        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        controller.enabled = true;
        if (controller == null)
        {
            Debug.LogError("No CharacterController attached to the player!");
        }

        velocity = Vector3.zero;
        currentSpeed = moveSpeed;
        currentStamina = maxStamina;

    }

    void Update()
    {

        HandleMovement();


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
            isSprinting = false;
            currentSpeed = moveSpeed;
            Debug.Log("Not enough stamina to sprint!");
        }
    }

    void HandleMovement()
    {
        // --- Sprint Input ---
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            ToggleSprint();
        }

        // --- Stamina Management ---
        HandleStamina();

        // --- Movement ---
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Use current speed (either normal or sprint speed)
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- Gravity ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // small negative to stick to the ground
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

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
        //Debug.Log("Current Stamina: " + currentStamina + " / " + maxStamina + " (" + GetStaminaPercentage() + "%)");
    }

    // Optional: Method to get stamina percentage for UI
    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }
    
    public void ResetPlayer()
    {
        // Reset velocity and movement
        velocity = Vector3.zero;
        currentSpeed = moveSpeed;
        currentStamina = maxStamina;
        isSprinting = false;

        // Reset position to spawn point
        transform.position = spawnPoint.position;

        // Ensure CharacterController resets collision
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;
        cc.enabled = true;
    }


}