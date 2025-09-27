using UnityEngine;

public class SprayCan : MonoBehaviour
{
    [Header("Spray Settings")]
    public Transform nozzle;             // Where the spray comes from
    public GameObject sprayDecalPrefab;  // Prefab of spray blob
    public float sprayRate = 0.01f;      // Time between blobs
    public float sprayDistance = 7f;     // Max spray distance

    private float sprayTimer;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse held
        {
            sprayTimer -= Time.deltaTime;

            if (sprayTimer <= 0f)
            {
                TrySpray();
                sprayTimer = sprayRate;
            }
        }
    }

    void TrySpray()
    {
        // Cast ray from the nozzle forward
        Ray ray = new Ray(nozzle.position, nozzle.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, sprayDistance))
        {
            Debug.Log("Sprayed on: " + hit.collider.gameObject.name);

            // Align decal with surface + random rotation for variation
            Quaternion randomRot = Quaternion.FromToRotation(Vector3.forward, hit.normal) *
                                   Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            // Slight offset along surface normal to prevent z-fighting
            Vector3 spawnPos = hit.point + hit.normal * 0.01f;

            GameObject decal = Instantiate(sprayDecalPrefab, spawnPos, randomRot);
            decal.transform.localScale *= Random.Range(0.9f, 1.1f);
        }
    }
}
