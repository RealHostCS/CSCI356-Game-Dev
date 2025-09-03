using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ice : MonoBehaviour
{
    [Header("Sliding Settings")]
    [Tooltip("How much this ice surface contributes to sliding.")]
    public float slideStrength = 5f;

    private void OnTriggerStay(Collider other)
    {
        // Make sure the player has the PlayerMovementWithIce script
        PlayerMovementWithIce player = other.GetComponent<PlayerMovementWithIce>();
        if (player != null)
        {
            // Tell the player to add sliding momentum in the direction they are moving
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Vector3 slideDir = other.transform.TransformDirection(inputDirection);
            player.AddSlide(slideDir, slideStrength);
        }
    }
}
