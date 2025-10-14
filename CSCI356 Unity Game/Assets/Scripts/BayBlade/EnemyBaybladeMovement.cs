using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class EnemyBaybladeMovement : MonoBehaviour, IBayblade
{
    private Vector3 recoilVelocity;

    public float MoveSpeed => moveSpeed;
    public Transform Transform => transform;

    [Header("Target")]
    public Transform target; // assign player or another Beyblade

    [Header("Movement Settings")]
    public float baseMoveSpeed = 250f;
    public float moveSpeed;
    public float gravity = -200f;
    public float groundStickForce = -200f;
    public float rotationSpeed = 100f; // how fast it turns toward player

    [Header("Spin Settings")]
    public Transform spinObject;
    public float spinSpeed = 300f;

    [Header("Tilt Settings")]
    public Transform tiltObject;

    [Header("Objects")]
    public EnemyHealth healthBar;

    private CharacterController controller;
    private Vector3 velocity;
    private Quaternion baseRotation;
    private float maxSpinSpeed = 300f;

    private float currentMoveSpeed;
    private float lastYPosition;

    [Header("Speed Dynamics")]
    public float speedChangeRate = 400f;
    public float speedReturnRate = 10f;
    public float maxSpeedBoost = 1.5f;
    public float minSpeedPenalty = 0.6f;

    [Header("AI Movement Behaviour")]
    public float stopDistance = 2f;
    public float orbitIntensity = 0.5f; // how much it circles around the player

     public void AddRecoil(Vector3 recoil)
    {
        recoilVelocity += recoil*800;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (spinObject != null) baseRotation = spinObject.rotation;
        StartCoroutine(RemoveSpinOverTime());

        currentMoveSpeed = baseMoveSpeed;
        lastYPosition = transform.position.y;
    }

    void Update()
    {
        UpdateDynamicSpeed();  // 🧠 New function
        HandleMovement();
        HandleSpin();
    }

    IEnumerator RemoveSpinOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // wait half a second
            RemoveSpin(1f); // remove spin
        }
    }

    public void RemoveSpin(float amount)
    {
        if (spinSpeed <= 0f)
        {
            spinSpeed = 0f;
            Debug.Log("Enemy Spin has reached 0!");
            Object.FindFirstObjectByType<SceneTransitionManager>().ReturnFromMinigame();
        }
        else
        {
            spinSpeed -= amount;
            if (spinSpeed < 0f) spinSpeed = 0f; // clamp
            healthBar.SetHealth((spinSpeed/maxSpinSpeed)*100); // call TakeDamage from HealthBar
        }
    }

    void onColide()
    {
        RemoveSpin(10f);
    }

    private Vector3 smoothedNormal = Vector3.up; // keeps previous normal


    void HandleMovement()
    {
        Vector3 move = GetMovementInput();
        UpdateSlopeNormal(move);
        ApplyTilt(move);
        MoveCharacter(move);
    }

    Vector3 GetTargetDirection()
    {
        Vector3 dir = (target.position - transform.position);
        dir.y = 0f; // keep on same plane
        float distance = dir.magnitude;

        if (distance < stopDistance)
            dir = Quaternion.Euler(0, Mathf.Sin(Time.time * 2f) * orbitIntensity * 30f, 0) * transform.forward;
        else
        {
            dir.Normalize();

            // Orbiting effect (adds unpredictability)
            dir = Quaternion.Euler(0, Mathf.Sin(Time.time * 2f) * orbitIntensity * 30f, 0) * dir;
        }

        // Smoothly face the target
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        return dir;
    }

    void UpdateDynamicSpeed()
{
    float deltaY = transform.position.y - lastYPosition;

    // If moving up → lose speed; if moving down → gain speed
    if (Mathf.Abs(deltaY) > 0.001f)
    {
        float adjustment = -deltaY * speedChangeRate; // negative when going up
        currentMoveSpeed += adjustment * Time.deltaTime;
    }

    // Smoothly return toward base speed
    currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, baseMoveSpeed, Time.deltaTime * speedReturnRate);

    // Clamp to min/max
    float minSpeed = baseMoveSpeed * minSpeedPenalty;
    float maxSpeed = baseMoveSpeed * maxSpeedBoost;
    currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, minSpeed, maxSpeed);

    // Apply to actual move speed
    moveSpeed = currentMoveSpeed;

    lastYPosition = transform.position.y;
}


    Vector3 GetMovementInput()
    {
        Vector3 move = GetTargetDirection();
        return move;
    }

    void UpdateSlopeNormal(Vector3 move)
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        RaycastHit hit;
        Vector3 targetNormal = Vector3.up;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2f))
            targetNormal = hit.normal;

        float tiltSpeed = 10f;
        if (Vector3.Dot(move, transform.forward) < 0f) tiltSpeed = 20f; // faster when moving backward
        smoothedNormal = Vector3.Lerp(smoothedNormal, targetNormal, Time.deltaTime * tiltSpeed);
    }

    void ApplyTilt(Vector3 move)
    {
        Quaternion slopeTilt = Quaternion.FromToRotation(Vector3.up, smoothedNormal) * baseRotation;

        if (move.sqrMagnitude > 0.001f)
        {
            Vector3 slopeMove = Vector3.ProjectOnPlane(move, smoothedNormal).normalized;
            Vector3 tiltAxis = Vector3.Cross(Vector3.up, slopeMove);
            float tiltAngle = 15f; // adjust for tilt intensity
            Quaternion movementTilt = Quaternion.AngleAxis(tiltAngle, tiltAxis);

            Quaternion targetRotation = slopeTilt * movementTilt;
            tiltObject.rotation = Quaternion.Slerp(tiltObject.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            tiltObject.rotation = Quaternion.Slerp(tiltObject.rotation, slopeTilt, Time.deltaTime * 10f);
        }
    }

    void MoveCharacter(Vector3 move)
    {
        Vector3 slopeMoveVector = Vector3.ProjectOnPlane(move, smoothedNormal);
        Vector3 totalMove = slopeMoveVector * moveSpeed * Time.deltaTime;

        if (controller.isGrounded)
            velocity.y = groundStickForce;
        else
            velocity.y += gravity * Time.deltaTime;

        totalMove += velocity * Time.deltaTime;
        controller.Move(totalMove);
    }

    void HandleSpin()
    {
        if (spinObject != null && Mathf.Abs(spinSpeed) > 0.01f)
        {
            spinObject.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}
