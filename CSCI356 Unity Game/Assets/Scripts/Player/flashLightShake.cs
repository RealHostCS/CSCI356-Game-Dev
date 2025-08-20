using UnityEngine;

public class FlashlightBob : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera; // Add camera reference for vertical look
    
    [Header("Bobbing Settings")]
    [SerializeField] private float verticalBobAmount = 7.5f;    // How much up/down rotation (degrees)
    [SerializeField] private float horizontalBobAmount = 2.5f;  // How much left/right rotation (degrees)
    [SerializeField] private float bobSpeed = 5f;              // Base bobbing speed
    [SerializeField] private float speedMultiplier = 2f;       // How much player speed affects bobbing
    
    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.1f;          // How smooth the transitions are
    
    private Vector3 lastPlayerPosition;
    private float currentPlayerSpeed;
    private float bobTimer;
    private Vector3 rotationVelocity;
    private Vector3 bobOffset; // Store the bobbing offset separately
    
    void Start()
    {
        if (player != null)
        {
            lastPlayerPosition = player.position;
        }
        else
        {
            Debug.LogWarning("FlashlightBob: Player reference not assigned!");
        }
        
        // Auto-find camera if not assigned
        if (playerCamera == null && player != null)
        {
            // Try to find camera in common locations
            playerCamera = player.Find("Camera") ?? 
                          player.Find("Main Camera") ?? 
                          player.Find("FirstPersonCamera") ?? 
                          Camera.main?.transform;
                          
            if (playerCamera == null)
            {
                Debug.LogWarning("FlashlightBob: Could not find player camera. Please assign it manually for vertical look.");
            }
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        // Calculate player speed
        CalculatePlayerSpeed();
        
        // Update bobbing based on speed
        UpdateBobbing();
    }
    
    void CalculatePlayerSpeed()
    {
        // Get the horizontal movement speed (ignore Y axis for flying/jumping)
        Vector3 horizontalMovement = new Vector3(
            player.position.x - lastPlayerPosition.x,
            0,
            player.position.z - lastPlayerPosition.z
        );
        
        currentPlayerSpeed = horizontalMovement.magnitude / Time.deltaTime;
        lastPlayerPosition = player.position;
        
        // Smooth the speed to avoid jittery movements
        currentPlayerSpeed = Mathf.Lerp(currentPlayerSpeed, currentPlayerSpeed, Time.deltaTime * 5f);
    }
    
    void UpdateBobbing()
    {
        // Follow the player's look direction (camera rotation if available, otherwise player rotation)
        Quaternion targetBaseRotation;
        if (playerCamera != null)
        {
            // Use camera rotation for full look direction (horizontal + vertical)
            targetBaseRotation = playerCamera.rotation;
        }
        else
        {
            // Fallback to player rotation (usually just horizontal)
            targetBaseRotation = player.rotation;
        }
        
        // Only add bobbing if the player is moving
        if (currentPlayerSpeed > 0.1f)
        {
            // Update timer based on speed
            float adjustedBobSpeed = bobSpeed + (currentPlayerSpeed * speedMultiplier);
            bobTimer += Time.deltaTime * adjustedBobSpeed;
            
            // Calculate bobbing rotations using sine waves
            float verticalBob = Mathf.Sin(bobTimer) * verticalBobAmount * (currentPlayerSpeed / 10f);
            float horizontalBob = Mathf.Sin(bobTimer * 0.7f) * horizontalBobAmount * (currentPlayerSpeed / 10f);
            
            // Store the bobbing offset
            bobOffset = new Vector3(verticalBob, 0, horizontalBob);
        }
        else
        {
            // Gradually reduce bobbing when not moving
            bobOffset = Vector3.Lerp(bobOffset, Vector3.zero, Time.deltaTime * (1f / smoothTime));
            bobTimer = 0f; // Reset timer when stopped
        }
        
        // Apply player rotation + bobbing offset
        Vector3 finalEulerAngles = targetBaseRotation.eulerAngles + bobOffset;
        Quaternion finalRotation = Quaternion.Euler(finalEulerAngles);
        
        // Smoothly rotate to final rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * (1f / smoothTime));
    }
    
    void OnValidate()
    {
        // Clamp values in inspector to reasonable ranges
        verticalBobAmount = Mathf.Clamp(verticalBobAmount, 0f, 22.5f);
        horizontalBobAmount = Mathf.Clamp(horizontalBobAmount, 0f, 10f);
        bobSpeed = Mathf.Max(0f, bobSpeed);
        speedMultiplier = Mathf.Max(0f, speedMultiplier);
        smoothTime = Mathf.Clamp(smoothTime, 0.01f, 1f);
    }
}