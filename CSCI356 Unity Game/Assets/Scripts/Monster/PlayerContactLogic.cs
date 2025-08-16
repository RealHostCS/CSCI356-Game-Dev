using UnityEngine;

public class PlayerContactLogic : MonoBehaviour
{
    public bool playerColision = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerDeath(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerColision = true;
            HandlePlayerDeath(other.gameObject);
        }
    }

    private void HandlePlayerDeath(GameObject playerObj)
    {
        // Get the PlayerAttributes script instead
        PlayerAttributes playerAttributes = playerObj.GetComponent<PlayerAttributes>();
        if (playerAttributes != null)
        {

            playerAttributes.health = 0; // Set health to zero
            playerAttributes.Die();      // Trigger death logic
        }
        else
        {
            Debug.LogWarning("PlayerContactLogic: PlayerAttributes script not found on player object!");
        }
    }
}
