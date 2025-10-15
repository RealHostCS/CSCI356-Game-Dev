using UnityEngine;

public class PlayerProximityChecker : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // The player to check
    public Transform targetLocation;  // The point to check distance to

    [Header("Settings")]
    public float radius = 5f;         // The detection radius

    /// <summary>
    /// Returns true if the player is within the radius of the targetLocation.
    /// </summary>
    public bool IsPlayerInRange()
    {
        if (player == null || targetLocation == null)
        {
            Debug.LogWarning("Player or TargetLocation is not assigned.");
            return false;
        }

        float distance = Vector3.Distance(player.position, targetLocation.position);
        return distance <= radius;
    }

    // Optional: draw the detection radius in the Scene view
    void OnDrawGizmosSelected()
    {
        if (targetLocation != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetLocation.position, radius);
        }
    }
}