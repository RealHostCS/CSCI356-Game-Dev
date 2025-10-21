using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player
    public Vector2 threshold = new Vector2(2f, 2f); // Distance from camera center before it moves
    public float cameraSpeed = 5f;   // Smooth camera movement speed

    private Vector3 velocity = Vector3.zero;
    private float zOffset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player transform not assigned.");
            return;
        }

        // Store initial Z offset between camera and player
        zOffset = transform.position.z - player.position.z;
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

        // Start with Z matching player's Z with offset
        Vector3 newPosition = new Vector3(cameraPosition.x, cameraPosition.y, playerPosition.z + zOffset);

        // Move horizontally if beyond threshold
        if (difference.x >= threshold.x)
        {
            newPosition.x = playerPosition.x - Mathf.Sign(playerPosition.x - cameraPosition.x) * threshold.x;
        }

        // Move vertically if beyond threshold
        if (difference.y >= threshold.y)
        {
            newPosition.y = playerPosition.y - Mathf.Sign(playerPosition.y - cameraPosition.y) * threshold.y;
        }

        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.SmoothDamp(cameraPosition, newPosition, ref velocity, 1f / cameraSpeed);

        // Add optional camera shake offset
        transform.position = smoothedPosition + (CameraShake.Instance?.ShakeOffset ?? Vector3.zero);
    }

    // Optional: draw the threshold area in the editor
    void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(threshold.x * 2, threshold.y * 2, 0));
    }
}
