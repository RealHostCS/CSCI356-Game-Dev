using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [Header("UI References")]
    public Image staminaBarFill;
    public Image staminaBarBackground;
    
    [Header("Settings")]
    public PlayerMovement playerMovement; // Reference to your player script
    
    [Header("Visual Settings")]
    public Color staminaBarColor = Color.white;
    
    void Start()
    {
        // Find PlayerMovement if not assigned
        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<PlayerMovement>();
        }
        
        if (playerMovement == null)
        {
            Debug.LogError("StaminaUI: Could not find PlayerMovement script!");
        }
        
        // Set initial colors
        staminaBarFill.color = staminaBarColor;
    }
    
    void Update()
    {
        if (playerMovement != null)
        {
            UpdateStaminaBar();
        }
    }
    
    void UpdateStaminaBar()
    {
        // Get stamina percentage from player
        float staminaPercentage = playerMovement.GetStaminaPercentage();
        
        // Update fill amount (bar drains from right to left)
        staminaBarFill.fillAmount = staminaPercentage;
    }
}