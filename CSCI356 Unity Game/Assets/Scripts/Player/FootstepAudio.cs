using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource footstepSource;

    [Header("Movement Detection")]
    public float moveThreshold = 0.1f; // min movement before footsteps play

    [Header("Pitch Settings")]
    public float minPitch = 0.9f;      // standing still
    public float maxPitch = 1.3f;      // max movement speed
    public float maxSpeed = 18f;        // top speed of player

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (footstepSource == null) return;

        // Calculate speed from transform delta
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        if (speed > moveThreshold)
        {
            // Start footsteps if not already playing
            if (!footstepSource.isPlaying)
                footstepSource.Play();

            // Adjust pitch based on speed
            float t = Mathf.Clamp01(speed / maxSpeed);
            footstepSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }

        lastPosition = transform.position;

        //Debug
        Debug.Log(speed);
    }
}
