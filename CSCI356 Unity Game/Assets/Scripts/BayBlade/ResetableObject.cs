using UnityEngine;
using System.Collections;

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
    StartCoroutine(ResetAfterLegsCleared(useAlternate));
}

private IEnumerator ResetAfterLegsCleared(bool useAlternate)
{
    if (TryGetComponent<MimicSpace.Mimic>(out var mimic))
        mimic.SendMessage("ResetMimic", SendMessageOptions.DontRequireReceiver);

    // Disable collider for safety
    BoxCollider safeCollider = GetComponent<BoxCollider>();
    if (safeCollider != null)
        safeCollider.enabled = false;

    // Wait one physics frame to clear legs
    yield return new WaitForFixedUpdate();

    if (TryGetComponent<Rigidbody>(out var rb))
        rb.isKinematic = true;

    Transform target = (useAlternate && alternateResetPoint != null)
        ? alternateResetPoint
        : normalStartingPoint;

    transform.SetPositionAndRotation(target.position, target.rotation);

    if (TryGetComponent<Rigidbody>(out var rb2))
    {
        rb2.isKinematic = false;
        rb2.linearVelocity = Vector3.zero;
        rb2.angularVelocity = Vector3.zero;
    }

    // Wait 3 seconds before turning the collider back on
    yield return new WaitForSeconds(3f);

    if (safeCollider != null)
        safeCollider.enabled = true;

    Debug.Log($"Reset complete at {transform.position}");
}


}
