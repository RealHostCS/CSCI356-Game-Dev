using UnityEngine;
using Unity.Collections;

[RequireComponent(typeof(SphereCollider))]
public class BaybladeCollisionHandler : MonoBehaviour
{
    private IBayblade myBayblade;
    private CharacterController controller;
    private Vector3 recoilVelocity;

    [Header("Recoil Settings")]
    public float recoilScale = 0.02f;
    public float recoilDamp = 5f;

    [Header("Effects")]
    public GameObject sparkPrefab;

    void Start()
    {
        myBayblade = GetComponentInParent<IBayblade>();
        controller = GetComponentInParent<CharacterController>();

        if (myBayblade == null)
            Debug.LogError("No IBayblade implementation found on parent!");
    }

    void Update()
    {
        if (recoilVelocity.magnitude > 0.1f)
        {
            controller.Move(recoilVelocity * Time.deltaTime);
            recoilVelocity = Vector3.Lerp(recoilVelocity, Vector3.zero, Time.deltaTime * recoilDamp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherBayblade = other.GetComponentInParent<IBayblade>();
        if (otherBayblade == null || otherBayblade == myBayblade) return;

        // Only one handles it
        if (myBayblade.Transform.GetInstanceID() > otherBayblade.Transform.GetInstanceID()) return;

        // Apply recoil
        recoilVelocity = otherBayblade.Transform.forward * otherBayblade.MoveSpeed * recoilScale;
        otherBayblade.AddRecoil(myBayblade.Transform.forward * myBayblade.MoveSpeed * recoilScale);

        // Spin loss
        myBayblade.RemoveSpin(otherBayblade.MoveSpeed * 0.05f);
        otherBayblade.RemoveSpin(myBayblade.MoveSpeed * 0.05f);

        // 💥 Spawn sparks
        if (sparkPrefab != null)
        {
            Vector3 contactPoint = (transform.position + other.transform.position) / 2f;
            Quaternion rotation = Quaternion.LookRotation(other.transform.position - transform.position);
            GameObject sparks = Instantiate(sparkPrefab, contactPoint, rotation);

            ParticleSystem ps = sparks.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();  // ensures old particles don’t block new bursts
                ps.Play();   // restart the effect
                Destroy(sparks, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(sparks, 1f); // fallback
            }
        }

        Debug.Log($"{myBayblade.Transform.name} hit {otherBayblade.Transform.name}");
    }



}
