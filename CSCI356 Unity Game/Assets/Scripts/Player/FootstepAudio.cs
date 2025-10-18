using UnityEngine;

public class SimpleFootsteps : MonoBehaviour
{
    public AudioSource footstepSource;
    public float speedThreshold = 0.1f;

    CharacterController controller;
    Rigidbody rb;
    Vector3 lastPos;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
    }

    void Update()
    {
        if (!footstepSource) return;

        float speed = 0f;

        if (controller)            speed = controller.velocity.magnitude;
        else if (rb)               speed = rb.linearVelocity.magnitude;
        else                       speed = ((transform.position - lastPos) / Mathf.Max(Time.deltaTime, 1e-6f)).magnitude;

        lastPos = transform.position;

        bool moving = speed > speedThreshold;

        if (moving && !footstepSource.isPlaying && controller.isGrounded)
            footstepSource.Play();
        else if (!moving && footstepSource.isPlaying)
            footstepSource.Stop();
    }

    void OnDisable()
    {
        if (footstepSource) footstepSource.Stop();
    }
}