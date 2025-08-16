using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Player Stats")]
    public int health = 100; // Example starting health

    public bool isDead = false; // Prevent multiple death triggers

    private PlayerMovement movement;
    void Awake()
    {
        health = 100;
        isDead = false;
    }
    void Start()
    {
        health = 100;
        isDead = false;

        // Cache PlayerMovement
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogWarning("PlayerAttributes: PlayerMovement component not found!");
        }
    }

    void Update()
    {
        // Check for death condition
        if (!isDead && health <= 0)
        {
            Die();
        }

        // Enable or disable movement based on isDead
        if (movement != null)
            movement.enabled = !isDead;
    }

    public void Die()
    {
        isDead = true; 
    }

}
