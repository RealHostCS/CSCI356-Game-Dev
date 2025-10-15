using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class BaybladeMovement : MonoBehaviour, IBayblade
{
    private Vector3 recoilVelocity;

    public float MoveSpeed => moveSpeed;
    public Transform Transform => transform;

   [Header("Movement Settings")]
    public float baseMoveSpeed = 250f;
    public float moveSpeed; // dynamically updated
    public float gravity = -200f;
    public float groundStickForce = -200f;

    [Header("Sprint Settings")]
    public float sprintMultiplier = 1.5f; // how much faster the Beyblade moves while sprinting
    public KeyCode sprintKey = KeyCode.LeftShift; // key to hold for sprint


    [Header("Camera Settings")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;   // Assign your camera here in Inspector

    [Header("Tilt Settings")]
    public Transform tiltObject; 

    [Header("Spin Settings")]
    public Transform spinObject;        // The object that visually spins (e.g., model)
    public float spinSpeed = 300f;        // Degrees per second — can be modified externally

    [Header("Objects")]
    public HealthBar healthBar; // reference to the HealthBar script

    [Header("Dash Effect")]
    public GameObject dashEffectPrefab;
    public float dashSpawnInterval = 0.1f; // time between each puff
    private float dashSpawnTimer = 0f;


    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private Quaternion baseRotation;
    private float maxSpinSpeed = 300f;

    // --- Add these near the top ---
    private float currentMoveSpeed;
    private float lastYPosition;
    public float speedChangeRate = 400f;     // how much speed changes when moving vertically
    public float speedReturnRate = 10f;     // how fast it returns to base speed
    public float maxSpeedBoost = 1.5f;      // 150% of base speed cap
    public float minSpeedPenalty = 0.6f;    // 60% of base speed cap

    public void AddRecoil(Vector3 recoil)
    {
        recoilVelocity += recoil*800;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        if (spinObject != null) baseRotation = spinObject.rotation;
        StartCoroutine(RemoveSpinOverTime());

        currentMoveSpeed = baseMoveSpeed;
        lastYPosition = transform.position.y;
    }

    void Update()
{
    UpdateDynamicSpeed();
    HandleMovement();
    HandleCameraLook();
    HandleSpin();
    HandleDashTrail(); 
}

    IEnumerator RemoveSpinOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // wait half a second
            if (Input.GetKey(sprintKey))
            {
                RemoveSpin(3f); // remove spin
            }
            else
            {
                RemoveSpin(1f); // remove spin
            }
        }
    }

    public void RemoveSpin(float amount)
    {
        if (spinSpeed <= 0f)
        {
            spinSpeed = 0f;
            Debug.Log("Player Spin has reached 0!");
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



    void HandleDashTrail()
    {
        bool isDashing = Input.GetKey(sprintKey) && controller.isGrounded;

        if (isDashing)
        {
            dashSpawnTimer -= Time.deltaTime;
            if (dashSpawnTimer <= 0f)
            {
                SpawnDashParticle();
                dashSpawnTimer = dashSpawnInterval;
            }
        }
        else
        {
            dashSpawnTimer = 0f; // reset timer
        }
    }

    void SpawnDashParticle()
    {
        if (dashEffectPrefab == null) return;

        // find ground position slightly below the Beyblade
        RaycastHit hit;
        Vector3 spawnPos = transform.position + Vector3.down * 0.1f;

        // Raycast to find actual floor contact point
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 2f))
            spawnPos = hit.point + Vector3.up * 0.05f;

        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        GameObject dashFx = Instantiate(dashEffectPrefab, spawnPos, rotation);

        // auto-destroy when done
        var ps = dashFx.GetComponent<ParticleSystem>();
        if (ps != null)
            Destroy(dashFx, ps.main.duration + ps.main.startLifetime.constantMax);
        else
            Destroy(dashFx, 1f);
    }



    void HandleMovement()
    {
        Vector3 move = GetMovementInput();
        UpdateSlopeNormal(move);
        ApplyTilt(move);
        MoveCharacter(move);
    }

    void UpdateDynamicSpeed()
    {
        float deltaY = transform.position.y - lastYPosition;

        // Adjust speed based on slope
        if (Mathf.Abs(deltaY) > 0.001f)
            currentMoveSpeed += -deltaY * speedChangeRate * Time.deltaTime;

        // Smooth return
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, baseMoveSpeed, Time.deltaTime * speedReturnRate);

        // Clamp
        float minSpeed = baseMoveSpeed * minSpeedPenalty;
        float maxSpeed = baseMoveSpeed * maxSpeedBoost;
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, minSpeed, maxSpeed);

        // Apply sprint
        moveSpeed = currentMoveSpeed;
        if (Input.GetKey(sprintKey))
            moveSpeed *= sprintMultiplier;

        lastYPosition = transform.position.y;
    }


    Vector3 GetMovementInput() 
    { 
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        return transform.right * x + transform.forward * z; 
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


   void HandleCameraLook()
{
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -80f, 80f);

    // Camera pitch (up/down)
    cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    // Player yaw (left/right)
    transform.Rotate(Vector3.up * mouseX);

    // Clamp camera height relative to Beyblade
    Vector3 cameraPos = cameraTransform.localPosition;
    cameraPos.y = Mathf.Max(cameraPos.y, 0f); // never below 0
    cameraTransform.localPosition = cameraPos;
}

    void HandleSpin()
    {
        if (spinObject != null && Mathf.Abs(spinSpeed) > 0.01f)
        {
            spinObject.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}
