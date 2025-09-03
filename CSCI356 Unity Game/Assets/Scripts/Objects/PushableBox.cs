using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    [Header("Push Settings")]
    [Tooltip("How strongly the player pushes the box.")]
    public float pushMultiplier = 5f;

    [Tooltip("Minimum player input magnitude required to push.")]
    public float minInputMagnitude = 0.1f;

    [Header("Custom Damping (replaces Rigidbody drag)")]
    [Tooltip("How quickly the box slows down.")]
    public float damping = 2f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void FixedUpdate()
    {
        // Use the new API linearVelocity
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel.sqrMagnitude > 0.0001f)
        {
            Vector3 dampingForce = -horizontalVel * damping;
            rb.AddForce(dampingForce, ForceMode.Acceleration);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Get player input for horizontal push
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 pushDir = new Vector3(horizontal, 0f, vertical);

        if (pushDir.magnitude < minInputMagnitude) return;

        pushDir.Normalize();
        rb.AddForce(pushDir * pushMultiplier, ForceMode.Acceleration);
    }
}
