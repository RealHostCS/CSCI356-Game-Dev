using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource footstepSource;

    [Header("Movement Detection")]
    private float moveThreshold = 14f; // min movement before footsteps play

    [Header("Pitch Settings")]
    public float minPitch = 0.9f;      // standing still
    public float maxPitch = 1.3f;      // max movement speed
    public float maxSpeed = 18f;        // top speed of player

    private Vector3 lastPosition;
    private Rigidbody rb;

    void Start()
    {
        lastPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (footstepSource == null) return;

        // Prefer Rigidbody.velocity (accurate, in units/sec) when available.
        float speed;
        if (rb != null)
        {
            speed = rb.linearVelocity.magnitude;
        }
        else
        {
            // Fallback: estimate speed from transform delta. Guard against tiny dt.
            float dt = Mathf.Max(Time.deltaTime, 1e-6f);
            Vector3 delta = transform.position - lastPosition;
            speed = delta.magnitude / dt;
            lastPosition = transform.position;
        }

        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        if (speed > moveThreshold)
        {
            // Start footsteps if not already playing
            if (!footstepSource.isPlaying)
                footstepSource.Play();
                Debug.Log("Playing footstep sound at speed: " + speed);

            // Adjust pitch based on speed
            float t = Mathf.Clamp01(speed / maxSpeed);
            footstepSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
                Debug.Log("Stopping footstep sound due to low speed: " + speed);
        }
    }
}
