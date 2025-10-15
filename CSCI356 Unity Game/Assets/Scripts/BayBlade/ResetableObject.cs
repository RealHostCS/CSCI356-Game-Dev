using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    [Header("Reset Points (leave second empty if unused)")]
    public Transform alternateResetPoint; // Optional second reset location
    public Transform normalStartingPoint;

    private Vector3 startPosition;
    private Quaternion startRotation;

    /// <summary>
    /// Resets the object to either the default or alternate position.
    /// </summary>
    /// <param name="useAlternate">If true, reset to the alternate location.</param>
    public void ResetToStart(bool useAlternate = false)
    {
        if (useAlternate && alternateResetPoint != null)
        {
            // Move to alternate reset point
            Debug.LogWarning("Player alternate.");
            transform.position = alternateResetPoint.position;
            transform.rotation = alternateResetPoint.rotation;
        }
        else
        {
            Debug.LogWarning("normal");
            // Move to original start point
            transform.position = normalStartingPoint.position;
            transform.rotation = normalStartingPoint.rotation;
        }

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
