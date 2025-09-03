using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ice : MonoBehaviour
{
    [Header("Sliding Settings")]
    [Tooltip("How much this ice surface contributes to sliding.")]
    public float slideStrength = 5f;

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Apply momentum in player's current input direction
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = player.transform.right * horizontal + player.transform.forward * vertical;
            if (move.magnitude > 0f)
                player.AddSlide(move.normalized, slideStrength);
        }
    }
}
