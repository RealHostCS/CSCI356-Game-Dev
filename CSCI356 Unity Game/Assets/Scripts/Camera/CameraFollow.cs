using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player
    public Vector2 threshold = new Vector2(2f, 2f); // Distance from camera center before it moves
    public float cameraSpeed = 5f;   // Smooth camera movement speed

    private Vector3 velocity = Vector3.zero;

    

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player transform not assigned.");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.position;

        Vector2 difference = new Vector2(
            Mathf.Abs(playerPosition.x - cameraPosition.x),
            Mathf.Abs(playerPosition.y - cameraPosition.y)
        );

        Vector3 newPosition = cameraPosition;

        // Check if player has moved beyond the threshold horizontally
        if (difference.x >= threshold.x)
        {
            newPosition.x = playerPosition.x - Mathf.Sign(playerPosition.x - cameraPosition.x) * threshold.x;
        }

        // Check if player has moved beyond the threshold vertically
        if (difference.y >= threshold.y)
        {
            newPosition.y = playerPosition.y - Mathf.Sign(playerPosition.y - cameraPosition.y) * threshold.y;
        }

        Vector3 smoothedPosition = Vector3.SmoothDamp(cameraPosition, newPosition, ref velocity, 1f / cameraSpeed);

        // Smoothly move camera to the new position
        transform.position = smoothedPosition + CameraShake.Instance?.ShakeOffset ?? Vector3.zero;
    }

    // Optional: draw the threshold area in the editor
    void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(threshold.x * 2, threshold.y * 2, 0));
    }
}
