using UnityEngine;

public class PlayerProximityChecker : MonoBehaviour
{
    [Header("References")]
    public Transform player;        
    public Transform targetLocation;  

    [Header("Settings")]
    public float radius = 5f;      


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

  
    void OnDrawGizmosSelected()
    {
        if (targetLocation != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetLocation.position, radius);
        }
    }
}