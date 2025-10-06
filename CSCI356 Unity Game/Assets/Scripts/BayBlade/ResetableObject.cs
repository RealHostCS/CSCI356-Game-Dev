using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    [Header("Reset Points (leave second empty if unused)")]
    public Transform alternateResetPoint; // Optional second reset location

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool initialized = false;

    void Awake()
    {
        // Cache the original transform as the default reset point
        startPosition = transform.position;
        startRotation = transform.rotation;
        initialized = true;
    }

    /// <summary>
    /// Resets the object to either the default or alternate position.
    /// </summary>
    /// <param name="useAlternate">If true, reset to the alternate location.</param>
    public void ResetToStart(bool useAlternate = false)
    {
        if (!initialized) return;

        if (useAlternate && alternateResetPoint != null)
        {
            // Move to alternate reset point
            transform.position = alternateResetPoint.position;
            transform.rotation = alternateResetPoint.rotation;
        }
        else
        {
            // Move to original start point
            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
